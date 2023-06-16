using System.Text;
using System.IO;
using TinyPG.Parsing;
using System.Collections.Generic;
using System;

namespace TinyPG.CodeGenerators.VBNet
{
	public class ParserGenerator : BaseGenerator, ICodeGenerator
	{
		internal ParserGenerator() : base("Parser.vb")
		{
		}

		public Dictionary<string, string> Generate(Grammar Grammar, GenerateDebugMode Debug)
		{
			string templatePath = Grammar.GetTemplatePath();
			if (string.IsNullOrEmpty(templatePath))
				throw new Exception("Template path not found:" + Grammar.Directives["TinyPG"]["TemplatePath"]);

			// generate the parser file
			StringBuilder parsers = new StringBuilder();
			
			// build non terminal tokens
			foreach (NonTerminalSymbol s in Grammar.GetNonTerminals())
			{
				string method = GenerateParseMethod(s);
				parsers.Append(method);
			}
			Dictionary<string, string> generated = new Dictionary<string, string>();
			foreach (var templateName in templateFiles)
			{
				string fileContent = File.ReadAllText(Path.Combine(templatePath, templateName));
				fileContent = fileContent.Replace(@"<%SourceFilename%>", Grammar.SourceFilename);
				fileContent = fileContent.Replace(@"<%Namespace%>", Grammar.Directives["TinyPG"]["Namespace"]);
				if (Debug != GenerateDebugMode.None)
				{
					fileContent = fileContent.Replace(@"<%Imports%>", "Imports TinyPG.Debug");
					fileContent = fileContent.Replace(@"<%IParser%>", "\r\n        Implements TinyPG.Debug.IParser\r\n");
					fileContent = fileContent.Replace(@"<%IParseTree%>", "TinyPG.Debug.IParseTree");
				}
				else
				{
					fileContent = fileContent.Replace(@"<%Imports%>", "");
					fileContent = fileContent.Replace(@"<%IParser%>", "");
					fileContent = fileContent.Replace(@"<%IParseTree%>", "ParseTree");
				}
				fileContent = ReplaceDirectiveAttributes(fileContent, Grammar.Directives["Parser"]);
				fileContent = fileContent.Replace(@"<%ParseNonTerminals%>", parsers.ToString());
				generated[templateName] = fileContent;
			}
			return generated;
		}

		// generates the method header and body
		private string GenerateParseMethod(NonTerminalSymbol s)
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("		Private Sub Parse" + s.Name + "(ByVal parent As ParseNode)" + Helper.AddComment("'", "NonTerminalSymbol: " + s.Name));
			sb.AppendLine("			Dim tok As Token");
			sb.AppendLine("			Dim n As ParseNode");
			sb.AppendLine("			Dim node As ParseNode = parent.CreateNode(m_scanner.GetToken(TokenType." + s.Name + "), \"" + s.Name + "\")");
			sb.AppendLine("			parent.Nodes.Add(node)");
			sb.AppendLine("");

			foreach (Rule rule in s.Rules)
			{
				sb.AppendLine(GenerateProductionRuleCode(s.Rules, 0, 3));
			}

			sb.AppendLine("			parent.Token.UpdateRange(node.Token)");
			sb.AppendLine("		End Sub" + Helper.AddComment("'", "NonTerminalSymbol: " + s.Name));
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
					sb.AppendLine(Indent + "tok = m_scanner.Scan(TokenType." + r.Symbol.Name + ")" + Helper.AddComment("'", "Terminal Rule: " + r.Symbol.Name));
					sb.AppendLine(Indent + "n = node.CreateNode(tok, tok.ToString() )");
					sb.AppendLine(Indent + "node.Token.UpdateRange(tok)");
					sb.AppendLine(Indent + "node.Nodes.Add(n)");
					sb.AppendLine(Indent + "If tok.Type <> TokenType." + r.Symbol.Name + " Then");
					sb.AppendLine(Indent + "	m_tree.Errors.Add(New ParseError(\"Unexpected token '\" + tok.Text.Replace(\"\\n\", \"\") + \"' found. Expected \" + TokenType." + r.Symbol.Name + ".ToString(), &H1001, tok))");
					sb.AppendLine(Indent + "	Return\r\n");
					sb.AppendLine(Indent + "End If\r\n");
					break;
				case RuleType.NonTerminal:
					sb.AppendLine(Indent + "Parse" + r.Symbol.Name + "(node)" + Helper.AddComment("'", "NonTerminal Rule: " + r.Symbol.Name));
					break;
				case RuleType.Concat:
					for (int i = 0; i < r.Rules.Count; i++)
					{
						sb.AppendLine();
						sb.AppendLine(Indent + Helper.AddComment("'", "Concat Rule"));
						sb.Append(GenerateProductionRuleCode(r.Rules, i, indent));
					}
					break;
				case RuleType.ZeroOrMore:
					firsts = r.GetFirstTerminals();
					sb.Append(Indent + "tok = m_scanner.LookAhead(");
					AppendTokenList(firsts, sb);
					sb.AppendLine(")" + Helper.AddComment("'", "ZeroOrMore Rule"));

					sb.Append(Indent + "While ");
					AppendTokenCondition(firsts, sb, Indent);
					sb.AppendLine("");


					for (int i = 0; i < r.Rules.Count; i++)
					{
						sb.Append(GenerateProductionRuleCode(r.Rules, i, indent + 1));
					}

					sb.Append(Indent + "tok = m_scanner.LookAhead(");
					AppendTokenList(firsts, sb);
					sb.AppendLine(")" + Helper.AddComment("'", "ZeroOrMore Rule"));
					sb.AppendLine(Indent + "End While");
					break;
				case RuleType.OneOrMore:
					sb.AppendLine(Indent + "Do" + Helper.AddComment("'", "OneOrMore Rule"));

					for (int i = 0; i < r.Rules.Count; i++)
					{
						sb.Append(GenerateProductionRuleCode(r.Rules, i, indent + 1));
					}

					firsts = r.GetFirstTerminals();
					sb.Append(Indent + "	tok = m_scanner.LookAhead(");
					AppendTokenList(firsts, sb);
					sb.AppendLine(")" + Helper.AddComment("'", "OneOrMore Rule"));
					sb.Append(Indent + "Loop While ");
					AppendTokenCondition(firsts, sb, Indent);
					sb.AppendLine("" + Helper.AddComment("'", "OneOrMore Rule"));
					break;
				case RuleType.Option:
					firsts = r.GetFirstTerminals();
					sb.Append(Indent + "tok = m_scanner.LookAhead(");
					AppendTokenList(firsts, sb);
					sb.AppendLine(")" + Helper.AddComment("'", "Option Rule"));

					sb.Append(Indent + "If ");
					AppendTokenCondition(firsts, sb, Indent);
					sb.AppendLine(" Then");

					for (int i = 0; i < r.Rules.Count; i++)
					{
						sb.Append(GenerateProductionRuleCode(r.Rules, i, indent + 1));
					}
					sb.AppendLine(Indent + "End If");
					break;
				case RuleType.Choice:
					firsts = r.GetFirstTerminals();
					sb.Append(Indent + "tok = m_scanner.LookAhead(");
					var tokens = new List<string>();
					AppendTokenList(firsts, sb, tokens);
					sb.AppendLine(")" + Helper.AddComment("'", "Choice Rule"));

					sb.AppendLine(Indent + "Select Case tok.Type");
					sb.AppendLine(Indent + "" + Helper.AddComment("'", "Choice Rule"));
					for (int i = 0; i < r.Rules.Count; i++)
					{
						foreach (TerminalSymbol s in r.Rules[i].GetFirstTerminals())
						{
							sb.AppendLine(Indent + "	Case TokenType." + s.Name + "");
							sb.Append(GenerateProductionRuleCode(r.Rules, i, indent + 2));
						}
					}
					sb.AppendLine(Indent + "	Case Else");
					string expectedTokens = BuildExpectedTokensStringForErrorMessage(tokens);
					sb.AppendLine(Indent + "		m_tree.Errors.Add(new ParseError(\"Unexpected token '\" + tok.Text.Replace(\"\\n\", \"\") + \"' found.  Expected " + expectedTokens + ".\", &H0002, tok))");
					sb.AppendLine(Indent + "		Exit Select");
					sb.AppendLine(Indent + "End Select" + Helper.AddComment("'", "Choice Rule"));
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
					sb.Append("tok.Type = TokenType." + s.Name);
				else
					sb.Append(" Or tok.Type = TokenType." + s.Name);
			}

		}
	}
}
