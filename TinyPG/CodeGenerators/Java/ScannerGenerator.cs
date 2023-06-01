using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using TinyPG;
using TinyPG.Compiler;

namespace TinyPG.CodeGenerators.Java
{
	public class ScannerGenerator : BaseGenerator, ICodeGenerator
	{
		internal ScannerGenerator() : base("Scanner.java")
		{
		}

		public string Generate(Grammar Grammar, bool Debug)
		{
			if (string.IsNullOrEmpty(Grammar.GetTemplatePath()))
				return null;
			if (Debug)
				throw new Exception("Java cannot be generated in debug mode");

			string scanner = File.ReadAllText(Grammar.GetTemplatePath() + templateName);

			int counter = 2;
			StringBuilder tokentype = new StringBuilder();
			StringBuilder regexps = new StringBuilder();
			StringBuilder skiplist = new StringBuilder();

			foreach (TerminalSymbol s in Grammar.SkipSymbols)
			{
				skiplist.AppendLine(Helper.Indent3 + "SkipList.add(TokenType." + s.Name + ");");
			}

			if (Grammar.FileAndLine != null)
				skiplist.AppendLine(Helper.Indent3 + "FileAndLine = TokenType." + Grammar.FileAndLine.Name + ";");

			// build system token
			tokentype.AppendLine("\r\n" + Helper.Indent3 + "//Non terminal tokens:");
			tokentype.AppendLine(Helper.Outline("_NONE_", 3, ",", 5));
			tokentype.AppendLine(Helper.Outline("_UNDETERMINED_", 3, ",", 5));

			// build non terminal tokens
			tokentype.AppendLine("\r\n" + Helper.Indent3 + "//Non terminal tokens:");
			foreach (Symbol s in Grammar.GetNonTerminals())
			{
				tokentype.AppendLine(Helper.Outline(s.Name, 3, ",", 5));
				counter++;
			}

			// build terminal tokens
			tokentype.AppendLine("\r\n" + Helper.Indent3 + "//Terminal tokens:");
			bool first = true;
			foreach (TerminalSymbol s in Grammar.GetTerminals())
			{
				regexps.Append(Helper.Indent3 + "regex = Pattern.compile(" + Helper.Unverbatim(s.Expression.ToString()));
				if (s.Attributes.ContainsKey("IgnoreCase"))
					regexps.Append(", Pattern.CASE_INSENSITIVE");
				regexps.Append(");\r\n");
				regexps.Append(Helper.Indent3 + "Patterns.put(TokenType." + s.Name + ", regex);\r\n");
				regexps.Append(Helper.Indent3 + "Tokens.add(TokenType." + s.Name + ");\r\n\r\n");

				if (first) first = false;
				else tokentype.AppendLine(",");

				tokentype.Append(Helper.Outline(s.Name, 3, "", 5));
				counter++;
			}

			scanner = scanner.Replace(@"<%SourceFilename%>", Grammar.SourceFilename);
			scanner = scanner.Replace(@"<%SkipList%>", skiplist.ToString());
			scanner = scanner.Replace(@"<%RegExps%>", regexps.ToString());
			scanner = scanner.Replace(@"<%TokenType%>", tokentype.ToString());
			scanner = scanner.Replace(@"<%Namespace%>", Grammar.Directives["TinyPG"]["Namespace"]);
			scanner = scanner.Replace(@"<%IToken%>", "");
			scanner = scanner.Replace(@"<%ScannerCustomCode%>", Grammar.Directives["Scanner"]["CustomCode"]);

			return scanner;
		}
	}
}
