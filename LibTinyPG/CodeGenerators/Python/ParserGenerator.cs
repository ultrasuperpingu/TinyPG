using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using TinyPG.Parsing;

namespace TinyPG.CodeGenerators.Python
{
	public class ParserGenerator : BaseGenerator, ICodeGenerator
	{
		internal ParserGenerator() : base("parser.py")
		{
		}

		public Dictionary<string, string> Generate(Grammar Grammar, GenerateDebugMode Debug)
		{
			Dictionary<string, string> templateFilesPath = GetTemplateFilesPath(Grammar, "Parser");

			// generate the parser file
			StringBuilder parsers = new StringBuilder();
			
			// build non terminal tokens
			foreach (NonTerminalSymbol s in Grammar.GetNonTerminals())
			{
				string method = GenerateParseMethod(s);
				parsers.Append(method);
			}

			Dictionary<string, string> generated = new Dictionary<string, string>();
			foreach (var entry in templateFilesPath)
			{
				var templateFilePath = entry.Value;
				string fileContent = File.ReadAllText(templateFilePath);
				fileContent = fileContent.Replace(@"<%SourceFilename%>", Grammar.SourceFilename);
				fileContent = fileContent.Replace(@"<%Namespace%>", Grammar.Directives["TinyPG"]["Namespace"]);
				if (Debug != GenerateDebugMode.None)
				{
					fileContent = fileContent.Replace(@"<%IParser%>", " : TinyPG.Debug.IParser");
					fileContent = fileContent.Replace(@"<%IParseTree%>", "TinyPG.Debug.IParseTree");
					fileContent = fileContent.Replace(@"<%ParserCustomCode%>", Grammar.Directives["Parser"]["CustomCode"]);
				}
				else
				{
					fileContent = fileContent.Replace(@"<%IParser%>", "");
					fileContent = fileContent.Replace(@"<%IParseTree%>", "ParseTree");
				}

				fileContent = fileContent.Replace(@"<%ParseNonTerminals%>", parsers.ToString());
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
			if (s.Attributes.ContainsKey("ParseComment"))
			{
				sb.AppendLine(GenerateComment(s.Attributes["ParseComment"], Helper.Indent2));
			}
			sb.AppendLine("	def Parse" + s.Name + "(self, input, tree):" + Helper.AddComment("#", "NonTerminalSymbol: " + s.Name));
			sb.AppendLine("		self.scanner.Init(input);");
			sb.AppendLine("		self.tree = tree;");
			sb.AppendLine("		self.Parse" + s.Name + "(tree);");
			sb.AppendLine("		tree.Skipped = scanner.Skipped;");
			sb.AppendLine("		return tree;");
			sb.AppendLine();
			sb.AppendLine("	def Parse" + s.Name + "(self, parent):" + Helper.AddComment("#", "NonTerminalSymbol: " + s.Name));
			sb.AppendLine("		node = parent.CreateNode(self.scanner.GetToken(TokenType." + s.Name + "), \"" + s.Name + "\");");
			sb.AppendLine("		parent.Nodes+=[node];");
			sb.AppendLine();

			if (s.Rules.Count == 1)
				sb.AppendLine(GenerateProductionRuleCode(s.Rules, 0, 2));
			else
				throw new Exception("Internal error");

			sb.AppendLine("		parent.Token.UpdateRange(node.Token);");
			sb.AppendLine("	" + Helper.AddComment("#", "NonTerminalSymbol: " + s.Name));
			sb.AppendLine();
			return sb.ToString();
		}

		// generates the rule logic inside the method body
		private string GenerateProductionRuleCode(Rules rules, int index, int indent)
		{
			Rule r = rules[index];
			Symbols firsts = null;
#if LOOKAHEAD_FOLLOWING_RULES_ON_OPTIONALS
			Symbols firstsExtended = null;
#endif
			StringBuilder sb = new StringBuilder();
			string Indent = Helper.Indent(indent);
			switch (r.Type)
			{
				case RuleType.Terminal:
					// expecting terminal, so scan it.
					sb.AppendLine(Indent + "tok = self.scanner.Scan([TokenType." + r.Symbol.Name + "]);" + Helper.AddComment("#", "Terminal Rule: " + r.Symbol.Name));
					sb.AppendLine(Indent + "n = node.CreateNode(tok, str(tok));");
					sb.AppendLine(Indent + "node.Token.UpdateRange(tok);");
					sb.AppendLine(Indent + "node.Nodes+=[n];");
					sb.AppendLine(Indent + "if tok.Type != TokenType." + r.Symbol.Name + ":");
					sb.AppendLine(Indent + "	self.tree.Errors += [ParseError.FromToken(\"Unexpected token '\" + tok.Text.replace(\"\\n\", \"\") + \"' found. Expected '" + r.Symbol.Name + "'.\", 0x1001, tok)];");
					sb.AppendLine(Indent + "	return;");
					break;
				case RuleType.NonTerminal:
					sb.AppendLine(Indent + "self.Parse" + r.Symbol.Name + "(node);" + Helper.AddComment("#", "NonTerminal Rule: " + r.Symbol.Name));
					break;
				case RuleType.Concat:
					for (int i = 0; i < r.Rules.Count; i++)
					{
						sb.AppendLine();
						sb.AppendLine(Indent + Helper.AddComment("#", "Concat Rule").TrimStart());
						sb.Append(GenerateProductionRuleCode(r.Rules, i, indent));
					}
					break;
				case RuleType.ZeroOrMore:
					firsts = r.GetFirstTerminals();
#if LOOKAHEAD_FOLLOWING_RULES_ON_OPTIONALS
					firstsExtended = CollectExpectedTokens(rules, index + 1);
					firstsExtended.InsertRange(0, firsts);
#endif
					sb.Append(Indent + "tok = self.scanner.LookAhead([");
#if LOOKAHEAD_FOLLOWING_RULES_ON_OPTIONALS
					AppendTokenList(firstsExtended, sb);
#else
					AppendTokenList(firsts, sb);
#endif
					sb.AppendLine("]);" + Helper.AddComment("#", "ZeroOrMore Rule"));

					sb.Append(Indent + "while (");
					AppendTokenCondition(firsts, sb, Indent);
					sb.AppendLine("):");
					
					for (int i = 0; i < r.Rules.Count; i++)
					{
						sb.Append(GenerateProductionRuleCode(r.Rules, i, indent + 1));
					}

					sb.Append(Indent + "	tok = self.scanner.LookAhead([");
#if LOOKAHEAD_FOLLOWING_RULES_ON_OPTIONALS
					AppendTokenList(firstsExtended, sb);
#else
					AppendTokenList(firsts, sb);
#endif
					sb.AppendLine("]);" + Helper.AddComment("#", "ZeroOrMore Rule"));
					break;
				case RuleType.OneOrMore:
					sb.AppendLine(Indent + "while True:" + Helper.AddComment("#", "OneOrMore Rule"));

					for (int i = 0; i < r.Rules.Count; i++)
					{
						sb.Append(GenerateProductionRuleCode(r.Rules, i, indent + 1));
					}

					firsts = r.GetFirstTerminals();
#if LOOKAHEAD_FOLLOWING_RULES_ON_OPTIONALS
					firstsExtended = CollectExpectedTokens(rules, index + 1);
					firstsExtended.InsertRange(0, firsts);
#endif

					sb.Append(Indent + "	tok = self.scanner.LookAhead([");
#if LOOKAHEAD_FOLLOWING_RULES_ON_OPTIONALS
					AppendTokenList(firstsExtended, sb);
#else
					AppendTokenList(firsts, sb);
#endif
					sb.AppendLine("]);");
					sb.Append(    Indent + "	if ");
					AppendTokenCondition(firsts, sb, Indent);
					sb.AppendLine(":" + Helper.AddComment("#", "OneOrMore Rule"));
					sb.AppendLine("	break;");
					break;
				case RuleType.Option:
					firsts = r.GetFirstTerminals();
#if LOOKAHEAD_FOLLOWING_RULES_ON_OPTIONALS
					firstsExtended = CollectExpectedTokens(rules, index + 1);
					firstsExtended.InsertRange(0, firsts);
#endif
					sb.Append(Indent + "tok = self.scanner.LookAhead([");
#if LOOKAHEAD_FOLLOWING_RULES_ON_OPTIONALS
					AppendTokenList(firstsExtended, sb);
#else
					AppendTokenList(firsts, sb);
#endif
					sb.AppendLine("]);" + Helper.AddComment("#", "Option Rule"));

					sb.Append(Indent + "if ");
					AppendTokenCondition(firsts, sb, Indent);
					sb.AppendLine(":");
					
					for (int i = 0; i < r.Rules.Count; i++)
					{
						sb.Append(GenerateProductionRuleCode(r.Rules, i, indent + 1));
					}
					break;
				case RuleType.Choice:
					firsts = r.GetFirstTerminals();
					sb.Append(Indent + "tok = self.scanner.LookAhead([");
					var tokens = new List<string>();
					AppendTokenList(firsts, sb, tokens);
					sb.AppendLine("]);");

					sb.AppendLine(Indent + Helper.AddComment("#", "Choice Rule"));
					for (int i = 0; i < r.Rules.Count; i++)
					{
						if(i == 0)
							sb.Append(Indent + "if ");
						else
							sb.Append(Indent + "elif ");
						var first = true;
						foreach (TerminalSymbol s in r.Rules[i].GetFirstTerminals())
						{
							if(!first)
								sb.Append(" | ");
							sb.Append("tok.Type == TokenType." + s.Name);
							first = false;
						}
						sb.AppendLine(":");

						sb.Append(GenerateProductionRuleCode(r.Rules, i, indent + 1));
					}
					sb.AppendLine(Indent + "else:");
					string expectedTokens = BuildExpectedTokensStringForErrorMessage(tokens);
					sb.AppendLine(Indent + "		self.tree.Errors+=[ParseError.FromToken(\"Unexpected token '\" + tok.Text.replace(\"\\n\", \"\") + \"' found. Expected " + expectedTokens + ".\", 0x0002, tok)];");
					sb.AppendLine(Indent + Helper.AddComment("#", "Choice Rule"));
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
					sb.Append("TokenType." + s.Name);
				else
					sb.Append(", TokenType." + s.Name);
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
					sb.Append("tok.Type == TokenType." + s.Name);
				else
					sb.Append("\\" + Environment.NewLine + indent + "    or tok.Type == TokenType." + s.Name);
			}

		}
	}
}
