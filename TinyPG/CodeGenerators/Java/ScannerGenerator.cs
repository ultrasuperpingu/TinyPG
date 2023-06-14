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

		public string Generate(Grammar Grammar, GenerateDebugMode Debug)
		{
			if (Debug != GenerateDebugMode.None)
				throw new Exception("Java cannot be generated in debug mode");
			if (string.IsNullOrEmpty(Grammar.GetTemplatePath()))
				return null;

			string scanner = File.ReadAllText(Grammar.GetTemplatePath() + templateName);

			int counter = 2;
			StringBuilder tokentype = new StringBuilder();
			StringBuilder regexps = new StringBuilder();
			StringBuilder skiplist = new StringBuilder();

			foreach (TerminalSymbol s in Grammar.SkipSymbols)
			{
				skiplist.AppendLine("		SkipList.add(TokenType." + s.Name + ");");
			}

			// build system tokens
			tokentype.AppendLine(Environment.NewLine + "	//Non terminal tokens:");
			tokentype.AppendLine(Helper.Outline("_NONE_", 1, ",", 4));
			tokentype.AppendLine(Helper.Outline("_UNDETERMINED_", 1, ",", 4));

			// build non terminal tokens
			tokentype.AppendLine(Environment.NewLine + "	//Non terminal tokens:");
			foreach (Symbol s in Grammar.GetNonTerminals())
			{
				tokentype.AppendLine(Helper.Outline(s.Name, 1, ",", 4));
				counter++;
			}

			// build terminal tokens
			tokentype.AppendLine(Environment.NewLine + "	//Terminal tokens:");
			bool first = true;
			foreach (TerminalSymbol s in Grammar.GetTerminals())
			{
				regexps.Append("		regex = Pattern.compile(" + Helper.Unverbatim(s.Expression));
				if (s.Attributes.ContainsKey("IgnoreCase"))
					regexps.Append(", Pattern.CASE_INSENSITIVE");
				regexps.Append(");" + Environment.NewLine);
				regexps.Append("		Patterns.put(TokenType." + s.Name + ", regex);" + Environment.NewLine);
				regexps.Append("		Tokens.add(TokenType." + s.Name + ");" + Environment.NewLine + Environment.NewLine);

				if (first)
					first = false;
				else
					tokentype.AppendLine(",");

				tokentype.Append(Helper.Outline(s.Name, 1, "", 4));
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
