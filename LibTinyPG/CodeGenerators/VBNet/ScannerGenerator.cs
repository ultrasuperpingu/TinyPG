using System;
using System.Text;
using System.IO;
using TinyPG.Parsing;
using System.Collections.Generic;

namespace TinyPG.CodeGenerators.VBNet
{
	public class ScannerGenerator : BaseGenerator, ICodeGenerator
	{
		internal ScannerGenerator() : base("Scanner.vb")
		{
		}

		public Dictionary<string, string> Generate(Grammar Grammar, GenerateDebugMode Debug)
		{
			Dictionary<string, string> templateFilesPath = GetTemplateFilesPath(Grammar, "Scanner");

			int counter = 2;
			StringBuilder tokentype = new StringBuilder();
			StringBuilder regexps = new StringBuilder();
			StringBuilder skiplist = new StringBuilder();

			foreach (TerminalSymbol s in Grammar.SkipSymbols)
			{
				skiplist.AppendLine("			SkipList.Add(TokenType." + s.Name + ")");
			}

			// build system tokens
			tokentype.AppendLine(Environment.NewLine + "		'Non terminal tokens:");
			tokentype.AppendLine(Helper.Outline("_NONE_", 2, "= 0", 5));
			tokentype.AppendLine(Helper.Outline("_UNDETERMINED_", 2, "= 1", 5));

			// build non terminal tokens
			tokentype.AppendLine(Environment.NewLine + "		'Non terminal tokens:");
			foreach (Symbol s in Grammar.GetNonTerminals())
			{
				tokentype.AppendLine(Helper.Outline(s.Name, 2, "= " + String.Format("{0:d}", counter), 5));
				counter++;
			}

			// build terminal tokens
			tokentype.AppendLine(Environment.NewLine + "		'Terminal tokens:");
			bool first = true;
			foreach (TerminalSymbol s in Grammar.GetTerminals())
			{
				string RegexCompiled = null;
				Grammar.Directives.Find("TinyPG").TryGetValue("RegexCompiled", out RegexCompiled);
				string expr = s.Expression;
				if (expr.StartsWith("@"))
					expr = expr.Substring(1);
				

				regexps.Append("			regex = new Regex(" + expr + ", RegexOptions.None");

				if (RegexCompiled == null || RegexCompiled.ToLower().Equals("true"))
					regexps.Append(" & RegexOptions.Compiled");

				if (s.Attributes.ContainsKey("IgnoreCase"))
					regexps.Append(" & RegexOptions.IgnoreCase");

				regexps.Append(")" + Environment.NewLine);

				regexps.Append("			Patterns.Add(TokenType." + s.Name + ", regex)" + Environment.NewLine);
				regexps.Append("			Tokens.Add(TokenType." + s.Name + ")" + Environment.NewLine + Environment.NewLine);

				if (first)
					first = false;
				else
					tokentype.AppendLine("");

				tokentype.Append(Helper.Outline(s.Name, 2, "= " + String.Format("{0:d}", counter), 5));
				counter++;
			}

			Dictionary<string, string> generated = new Dictionary<string, string>();
			foreach (var entry in templateFilesPath)
			{
				var templateFilePath = entry.Value;
				string fileContent = File.ReadAllText(templateFilePath);
				fileContent = fileContent.Replace(@"<%SourceFilename%>", Grammar.SourceFilename);
				fileContent = fileContent.Replace(@"<%SkipList%>", skiplist.ToString());
				fileContent = fileContent.Replace(@"<%RegExps%>", regexps.ToString());
				fileContent = fileContent.Replace(@"<%TokenType%>", tokentype.ToString());
				fileContent = fileContent.Replace(@"<%Namespace%>", Grammar.Directives["TinyPG"]["Namespace"]);
				if (Debug != GenerateDebugMode.None)
				{
					fileContent = fileContent.Replace(@"<%Imports%>", "Imports TinyPG.Debug");
					fileContent = fileContent.Replace(@"<%IToken%>", "\r\n        Implements TinyPG.Debug.IToken");
					fileContent = fileContent.Replace(@"<%ImplementsITokenStartPos%>", " Implements TinyPG.Debug.IToken.StartPos");
					fileContent = fileContent.Replace(@"<%ImplementsITokenEndPos%>", " Implements TinyPG.Debug.IToken.EndPos");
					fileContent = fileContent.Replace(@"<%ImplementsITokenLength%>", " Implements TinyPG.Debug.IToken.Length");
					fileContent = fileContent.Replace(@"<%ImplementsITokenText%>", " Implements TinyPG.Debug.IToken.Text");
					fileContent = fileContent.Replace(@"<%ImplementsITokenToString%>", " Implements TinyPG.Debug.IToken.ToString");
				}
				else
				{
					fileContent = fileContent.Replace(@"<%Imports%>", "");
					fileContent = fileContent.Replace(@"<%IToken%>", "");
					fileContent = fileContent.Replace(@"<%ImplementsITokenStartPos%>", "");
					fileContent = fileContent.Replace(@"<%ImplementsITokenEndPos%>", "");
					fileContent = fileContent.Replace(@"<%ImplementsITokenLength%>", "");
					fileContent = fileContent.Replace(@"<%ImplementsITokenText%>", "");
					fileContent = fileContent.Replace(@"<%ImplementsITokenToString%>", "");
				}
				fileContent = ReplaceDirectiveAttributes(fileContent, Grammar.Directives["Scanner"]);
				generated[entry.Key] = fileContent;
			}

			return generated;
		}
	}
}
