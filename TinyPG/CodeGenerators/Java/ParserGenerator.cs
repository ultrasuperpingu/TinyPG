using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using TinyPG;
using TinyPG.Compiler;

namespace TinyPG.CodeGenerators.Java
{
	public class ParserGenerator : BaseGenerator, ICodeGenerator
	{
		internal ParserGenerator() : base("Parser.java")
		{
		}

		public Dictionary<string, string> Generate(Grammar Grammar, GenerateDebugMode Debug)
		{
			if (Debug != GenerateDebugMode.None)
				throw new Exception("Java cannot be generated in debug mode");

			if (string.IsNullOrEmpty(Grammar.GetTemplatePath()))
				return null;

			// generate the parser file
			StringBuilder parsers = new StringBuilder();
			// build non terminal tokens
			foreach (NonTerminalSymbol s in Grammar.GetNonTerminals())
			{
				string method = GenerateParseMethod(s);
				parsers.Append(method);
			}

			Dictionary<string, string> generated = new Dictionary<string, string>();
			foreach (var templateName in TemplateFiles)
			{
				string fileContent = File.ReadAllText(Path.Combine(Grammar.GetTemplatePath(), templateName));
				fileContent = fileContent.Replace(@"<%SourceFilename%>", Grammar.SourceFilename);
				fileContent = fileContent.Replace(@"<%Namespace%>", Grammar.Directives["TinyPG"]["Namespace"]);
				fileContent = fileContent.Replace(@"<%IParseTree%>", "ParseTree");
				fileContent = fileContent.Replace(@"<%ParseNonTerminals%>", parsers.ToString());
				generated[templateName] = fileContent;
			}

			return generated;
		}

		// generates the method header and body
		private string GenerateParseMethod(NonTerminalSymbol s)
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("	public ParseTree Parse" + s.Name + "(String input, ParseTree tree)" + Helper.AddComment("NonTerminalSymbol: " + s.Name));
			sb.AppendLine("	{");
			sb.AppendLine("		scanner.Init(input);");
			sb.AppendLine("		this.tree = tree;");
			sb.AppendLine("		Parse" + s.Name + "(tree);");
			sb.AppendLine("		tree.Skipped = scanner.Skipped;");
			sb.AppendLine("		return tree;");
			sb.AppendLine("	}");
			sb.AppendLine();
			sb.AppendLine("	private void Parse" + s.Name + "(ParseNode parent)" + Helper.AddComment("NonTerminalSymbol: " + s.Name));
			sb.AppendLine("	{");
			sb.AppendLine("		Token tok;");
			sb.AppendLine("		ParseNode n;");
			sb.AppendLine("		ParseNode node = parent.CreateNode(scanner.GetToken(TokenType." + s.Name + "), \"" + s.Name + "\");");
			sb.AppendLine("		parent.getNodes().add(node);");
			sb.AppendLine("");

			foreach (Rule rule in s.Rules)
			{
				sb.AppendLine(GenerateProductionRuleCode(s.Rules, 0, 2));
			}

			sb.AppendLine("		parent.Token.UpdateRange(node.Token);");
			sb.AppendLine("	}" + Helper.AddComment("NonTerminalSymbol: " + s.Name));
			sb.AppendLine();
			return sb.ToString();
		}

		// generates the rule logic inside the method body
		private string GenerateProductionRuleCode(Rules rules, int index, int indent)
		{
			Rule r = rules[index];
			Symbols firsts = null;
			StringBuilder sb = new StringBuilder();
			string Indent = Helper.Indent(indent);

			switch (r.Type)
			{
				case RuleType.Terminal:
					// expecting terminal, so scan it.
					sb.AppendLine(Indent + "tok = scanner.Scan(TokenType." + r.Symbol.Name + ");" + Helper.AddComment("Terminal Rule: " + r.Symbol.Name));
					sb.AppendLine(Indent + "n = node.CreateNode(tok, tok.toString() );");
					sb.AppendLine(Indent + "node.Token.UpdateRange(tok);");
					sb.AppendLine(Indent + "node.getNodes().add(n);");
					sb.AppendLine(Indent + "if (tok.Type != TokenType." + r.Symbol.Name + ") {");
					sb.AppendLine(Indent + "	tree.Errors.add(new ParseError(\"Unexpected token '\" + tok.getText().replace(\"\\n\", \"\") + \"' found. Expected \" + TokenType." + r.Symbol.Name + ".toString(), 0x1001, tok));");
					sb.AppendLine(Indent + "	return;");
					sb.AppendLine(Indent + "}");
					break;
				case RuleType.NonTerminal:
					sb.AppendLine(Indent + "Parse" + r.Symbol.Name + "(node);" + Helper.AddComment("NonTerminal Rule: " + r.Symbol.Name));
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
					sb.Append(Indent + "tok = scanner.LookAhead(");
					AppendTokenList(firsts, sb);
					sb.AppendLine(");" + Helper.AddComment("ZeroOrMore Rule"));

					sb.Append(Indent + "while (");
					AppendTokenCondition(firsts, sb, Indent);
					sb.AppendLine(")");
					sb.AppendLine(Indent + "{");

					for (int i = 0; i < r.Rules.Count; i++)
					{
						sb.Append(GenerateProductionRuleCode(r.Rules, i, indent + 1));
					}

					sb.Append(Indent + "tok = scanner.LookAhead(");
					AppendTokenList(firsts, sb);
					sb.AppendLine(");" + Helper.AddComment("ZeroOrMore Rule"));
					sb.AppendLine(Indent + "}");
					break;
				case RuleType.OneOrMore:
					sb.AppendLine(Indent + "do {" + Helper.AddComment("OneOrMore Rule"));

					for (int i = 0; i < r.Rules.Count; i++)
					{
						sb.Append(GenerateProductionRuleCode(r.Rules, i, indent + 1));
					}

					firsts = r.GetFirstTerminals();
					sb.Append(Indent + "    tok = scanner.LookAhead(");
					AppendTokenList(firsts, sb);
					sb.AppendLine(");" + Helper.AddComment("OneOrMore Rule"));
					sb.Append(    Indent + "} while (");
					AppendTokenCondition(firsts, sb, Indent);
					sb.AppendLine(");" + Helper.AddComment("OneOrMore Rule"));
					break;
				case RuleType.Option:
					firsts = r.GetFirstTerminals();
					sb.Append(Indent + "tok = scanner.LookAhead(");
					AppendTokenList(firsts, sb);
					sb.AppendLine(");" + Helper.AddComment("Option Rule"));

					sb.Append(Indent + "if (");
					AppendTokenCondition(firsts, sb, Indent);
					sb.AppendLine(")");
					sb.AppendLine(Indent + "{");

					for (int i = 0; i < r.Rules.Count; i++)
					{
						sb.Append(GenerateProductionRuleCode(r.Rules, i, indent + 1));
					}
					sb.AppendLine(Indent + "}");
					break;
				case RuleType.Choice:
					firsts = r.GetFirstTerminals();
					sb.Append(Indent + "tok = scanner.LookAhead(");
					var tokens = new List<string>();
					AppendTokenList(firsts, sb, tokens);
					sb.AppendLine(");" + Helper.AddComment("Choice Rule"));

					sb.AppendLine(Indent + "switch (tok.Type)");
					sb.AppendLine(Indent + "{" + Helper.AddComment("Choice Rule"));
					for (int i = 0; i < r.Rules.Count; i++)
					{
						foreach (TerminalSymbol s in r.Rules[i].GetFirstTerminals())
						{
							sb.AppendLine(Indent + "	case " + s.Name + ":");
						}
						sb.Append(GenerateProductionRuleCode(r.Rules, i, indent + 2));
						sb.AppendLine(Indent + "		break;");
					}
					sb.AppendLine(Indent + "	default:");
					string expectedTokens = BuildExpectedTokensStringForErrorMessage(tokens);
					sb.AppendLine(Indent + "		tree.Errors.add(new ParseError(\"Unexpected token '\" + tok.getText().replace(\"\\n\", \"\") + \"' found. Expected " + expectedTokens + ".\", 0x0002, tok));");
					sb.AppendLine(Indent + "		break;");
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
					sb.Append(Environment.NewLine + indent + "    || tok.Type == TokenType." + s.Name);
			}

		}
	}
}
