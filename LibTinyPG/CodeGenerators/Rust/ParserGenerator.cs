using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using TinyPG.Parsing;
using TinyPG.Debug;

namespace TinyPG.CodeGenerators.Rust
{
	public class ParserGenerator : BaseGenerator, ICodeGenerator
	{
		internal ParserGenerator() : base("parser.rs")
		{
		}

		public Dictionary<string, string> Generate(Grammar Grammar, GenerateDebugMode Debug)
		{
			if (Debug != GenerateDebugMode.None)
				throw new Exception("Rust cannot be generated in debug mode");

			Dictionary<string, string> templateFilesPath = GetTemplateFilesPath(Grammar, "Parser");

			// generate the parser file
			StringBuilder parserMethodsImpl = new StringBuilder();
			
			// build non terminal tokens
			foreach (NonTerminalSymbol s in Grammar.GetNonTerminals())
			{
				string method = GenerateParseMethod(s);
				parserMethodsImpl.Append(method);
			}
			Dictionary<string, string> generated = new Dictionary<string, string>();
			foreach (var entry in templateFilesPath)
			{
				var templateFilePath = entry.Value;
				string fileContent = File.ReadAllText(templateFilePath);
				fileContent = fileContent.Replace(@"<%SourceFilename%>", Grammar.SourceFilename);
				fileContent = fileContent.Replace(@"<%Namespace%>", Grammar.Directives["TinyPG"]["Namespace"]);
				fileContent = fileContent.Replace(@"<%ParseNonTerminals%>", parserMethodsImpl.ToString());
				fileContent = fileContent.Replace(@"<%GeneratorVersion%>", TinyPGInfos.Version);
				fileContent = ReplaceDirectiveAttributes(fileContent, Grammar.Directives["Parser"]);
				generated[entry.Key] = fileContent;
			}
			return generated;
		}

		// generates the method header and body
		private string GenerateParseMethod(NonTerminalSymbol s)
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("	pub fn parse_" + s.Name.ToLowerInvariant() + "(&mut self, input: &str, mut tree : ParseTree) -> ParseTree" + Helper.AddComment("NonTerminalSymbol: " + s.Name));
			sb.AppendLine("	{");
			sb.AppendLine("		self.scanner.init(input);");
			sb.AppendLine("		let mut node = tree.root.take().unwrap();");
			sb.AppendLine("		self.parse_node_" + s.Name.ToLowerInvariant() + "(&mut tree, Some(&mut node));");
			sb.AppendLine("		tree.skipped = self.scanner.skipped.clone();");
			sb.AppendLine("		tree.root = Some(node);");
			sb.AppendLine("		tree");
			sb.AppendLine("	}");
			sb.AppendLine();
			sb.AppendLine("	fn parse_node_" + s.Name.ToLowerInvariant() + "(&mut self, tree:&mut ParseTree, parent : Option<&mut Box<dyn IParseNode>>)" + Helper.AddComment("NonTerminalSymbol: " + s.Name));
			sb.AppendLine("	{");
			sb.AppendLine("		#[allow(unused_variables, unused_mut)]");
			sb.AppendLine("		let mut tok: Token;");
			sb.AppendLine("		#[allow(unused_variables, unused_mut)]");
			sb.AppendLine("		let mut n: Box<dyn IParseNode>;");
			sb.AppendLine("		let mut node = tree.create_node(self.scanner.get_token(TokenType::" + s.Name + "), \"" + s.Name + "\".to_string());");
			sb.AppendLine("");

			if (s.Rules.Count == 1)
				sb.AppendLine(GenerateProductionRuleCode(s.Rules, 0, 2));
			else
				throw new Exception("Internal error");
			sb.AppendLine("		if let Some(p) = parent {");
			sb.AppendLine("			p.get_token_mut().update_range(node.get_token());");
			sb.AppendLine("			p.add_node(node);");
			sb.AppendLine("		} else {");
			sb.AppendLine("			tree.root = Some(node);");
			sb.AppendLine("		}");
			sb.AppendLine("	}" + Helper.AddComment("NonTerminalSymbol: " + s.Name));
			sb.AppendLine();
			return sb.ToString();
		}

		// generates the rule logic inside the method body
		private string GenerateProductionRuleCode(Rules rules, int index, int indent)
		{
			Rule r = rules[index];
			Symbols firsts = null;
#if look_ahead_FOLLOWING_RULES_ON_OPTIONALS
			Symbols firstsExtended = null;
#endif
			StringBuilder sb = new StringBuilder();
			string Indent = Helper.Indent(indent);
			switch (r.Type)
			{
				case RuleType.Terminal:
					// expecting terminal, so scan it.
					sb.AppendLine(Indent + "tok = self.scanner.scan(vec![TokenType::" + r.Symbol.Name + "]);" + Helper.AddComment("Terminal Rule: " + r.Symbol.Name));
					sb.AppendLine(Indent + "n = tree.create_node(tok.clone(), tok.to_string() );");
					sb.AppendLine(Indent + "node.get_token_mut().update_range(&tok);");
					sb.AppendLine(Indent + "node.add_node(n);");
					sb.AppendLine(Indent + "if tok._type != TokenType::" + r.Symbol.Name + " {");
					sb.AppendLine(Indent + "	tree.errors.push(ParseError::from_token(\"Unexpected token '\".to_string() + tok.text.replace(&\"\\n\".to_string(), \"\").as_str() + \"' found. Expected '" + r.Symbol.Name + "'.\", 0x1001, &tok, false));");
					sb.AppendLine(Indent + "	if let Some(p) = parent {");
					sb.AppendLine(Indent + "		p.add_node(node);");
					sb.AppendLine(Indent + "	} else {");
					sb.AppendLine(Indent + "		tree.root = Some(node);");
					sb.AppendLine(Indent + "	}");
					sb.AppendLine(Indent + "	return;");
					sb.AppendLine(Indent + "}");
					break;
				case RuleType.NonTerminal:
					sb.AppendLine(Indent + "self.parse_node_" + r.Symbol.Name.ToLowerInvariant() + "(tree, Some(&mut node));" + Helper.AddComment("NonTerminal Rule: " + r.Symbol.Name));
					break;
				case RuleType.Concat:
					for (int i = 0; i < r.Rules.Count; i++)
					{
						sb.AppendLine();
						sb.AppendLine(Indent + Helper.AddComment("Concat Rule"));
						sb.Append(GenerateProductionRuleCode(r.Rules, i, indent));
					}
					break;
				case RuleType.ZeroOrMore:
					firsts = r.GetFirstTerminals();
#if look_ahead_FOLLOWING_RULES_ON_OPTIONALS
					firstsExtended = CollectExpectedTokens(rules, index + 1);
					firstsExtended.InsertRange(0, firsts);
#endif
					sb.Append(Indent + "tok = self.scanner.look_ahead(vec![");
#if look_ahead_FOLLOWING_RULES_ON_OPTIONALS
					AppendTokenList(firstsExtended, sb);
#else
					AppendTokenList(firsts, sb);
#endif
					sb.AppendLine("]);" + Helper.AddComment("ZeroOrMore Rule"));

					sb.Append(Indent + "while ");
					AppendTokenCondition(firsts, sb, Indent);
					sb.AppendLine("");
					sb.AppendLine(Indent + "{");

					for (int i = 0; i < r.Rules.Count; i++)
					{
						sb.Append(GenerateProductionRuleCode(r.Rules, i, indent + 1));
					}

					sb.Append(Indent + "	tok = self.scanner.look_ahead(vec![");
#if look_ahead_FOLLOWING_RULES_ON_OPTIONALS
					AppendTokenList(firstsExtended, sb);
#else
					AppendTokenList(firsts, sb);
#endif
					sb.AppendLine("]);" + Helper.AddComment("ZeroOrMore Rule"));
					sb.AppendLine(Indent + "}");
					break;
				case RuleType.OneOrMore:
					sb.AppendLine(Indent + "loop {" + Helper.AddComment("OneOrMore Rule"));

					for (int i = 0; i < r.Rules.Count; i++)
					{
						sb.Append(GenerateProductionRuleCode(r.Rules, i, indent + 1));
					}

					firsts = r.GetFirstTerminals();
#if look_ahead_FOLLOWING_RULES_ON_OPTIONALS
					firstsExtended = CollectExpectedTokens(rules, index + 1);
					firstsExtended.InsertRange(0, firsts);
#endif

					sb.Append    (Indent + "	tok = self.scanner.look_ahead(vec![");
#if look_ahead_FOLLOWING_RULES_ON_OPTIONALS
					AppendTokenList(firstsExtended, sb);
#else
					AppendTokenList(firsts, sb);
#endif
					sb.AppendLine("]);");
					sb.Append(    Indent + "	if ");
					AppendTokenCondition(firsts, sb, Indent);
					sb.AppendLine(" {");
					sb.AppendLine(Indent + "		break;");
					sb.AppendLine(Indent + "	}");
					sb.AppendLine(Indent + "}" + Helper.AddComment("OneOrMore Rule"));
					break;
				case RuleType.Option:
					firsts = r.GetFirstTerminals();
#if look_ahead_FOLLOWING_RULES_ON_OPTIONALS
					firstsExtended = CollectExpectedTokens(rules, index + 1);
					firstsExtended.InsertRange(0, firsts);
#endif
					sb.Append(Indent + "tok = self.scanner.look_ahead(vec![");
#if look_ahead_FOLLOWING_RULES_ON_OPTIONALS
					AppendTokenList(firstsExtended, sb);
#else
					AppendTokenList(firsts, sb);
#endif
					sb.AppendLine("]);" + Helper.AddComment("Option Rule"));

					sb.Append(Indent + "if ");
					AppendTokenCondition(firsts, sb, Indent);
					sb.AppendLine("");
					sb.AppendLine(Indent + "{");

					for (int i = 0; i < r.Rules.Count; i++)
					{
						sb.Append(GenerateProductionRuleCode(r.Rules, i, indent + 1));
					}
					sb.AppendLine(Indent + "}");
					break;
				case RuleType.Choice:
					firsts = r.GetFirstTerminals();
					sb.Append(Indent + "tok = self.scanner.look_ahead(vec![");
					var tokens = new List<string>();
					AppendTokenList(firsts, sb, tokens);
					sb.AppendLine("]);");

					sb.AppendLine(Indent + "match tok._type");
					sb.AppendLine(Indent + "{" + Helper.AddComment("Choice Rule"));
					for (int i = 0; i < r.Rules.Count; i++)
					{
						var terminals = r.Rules[i].GetFirstTerminals();
						for (int j=0;j < terminals.Count;j++)
						{
							TerminalSymbol s = terminals[j] as TerminalSymbol;
							sb.Append(Indent + "	TokenType::" + s.Name);
							if (j == terminals.Count - 1)
								sb.AppendLine(" => {");
							else
								sb.AppendLine(" |");
						}
						sb.Append(GenerateProductionRuleCode(r.Rules, i, indent + 2));
						sb.AppendLine(Indent + "	},");
					}
					sb.AppendLine(Indent + "	_ => {");
					string expectedTokens = BuildExpectedTokensStringForErrorMessage(tokens);
					sb.AppendLine(Indent + "		tree.errors.push(ParseError::from_token(\"Unexpected token '\".to_string() + tok.text.replace(\"\\n\", \"\").as_str() + \"' found. Expected " + expectedTokens + ".\", 0x0002, &tok, false));");
					sb.AppendLine(Indent + "	}");
					sb.AppendLine(Indent + "}" + Helper.AddComment("Choice Rule"));
					break;
				default:
					break;
			}
			return sb.ToString();
		}

		private static string BuildExpectedTokensStringForErrorMessage(List<string> tokens)
		{
			string expectedTokens;
			if (tokens.Count == 1)
				expectedTokens = tokens[0];
			else if (tokens.Count == 2)
				expectedTokens = tokens[0] + " or " + tokens[1];
			else
			{
				expectedTokens = string.Join(", ", tokens.GetRange(0, tokens.Count - 1).ToArray());
				expectedTokens += ", or " + tokens[tokens.Count - 1];
			}

			return expectedTokens;
		}

		private static Symbols CollectExpectedTokens(Rules rules, int index)
		{
			var symbols = new Symbols();
			for (int i = index; i < rules.Count; i++)
			{
				rules[i].DetermineFirstTerminals(symbols);
				if (rules[i].Type != RuleType.ZeroOrMore &&
					rules[i].Type != RuleType.Option)
					break;
			}
			return symbols;
		}

		private void AppendTokenList(Symbols symbols, StringBuilder sb, List<string> tokenNames = null)
		{
			int i = 0;
			foreach (TerminalSymbol s in symbols)
			{
				if (i == 0)
					sb.Append("TokenType::" + s.Name);
				else
					sb.Append(", TokenType::" + s.Name);
				i++;
				if (tokenNames != null)
					tokenNames.Add(s.Name);
			}
		}

		private void AppendTokenCondition(Symbols symbols, StringBuilder sb, string indent)
		{
			for (int i = 0; i < symbols.Count; i++)
			{
				TerminalSymbol s = (TerminalSymbol)symbols[i];
				if (i == 0)
					sb.Append("tok._type == TokenType::" + s.Name);
				else
					sb.Append(Environment.NewLine + indent + "    || tok._type == TokenType::" + s.Name);
			}

		}
	}
}
