using System;
using System.Text;
using System.IO;
using TinyPG.Compiler;

namespace TinyPG.CodeGenerators.Cpp
{
	public class ScannerGenerator : BaseGenerator, ICodeGenerator
	{
		internal ScannerGenerator()
			: base("Scanner.h")
		{
		}

		public string Generate(Grammar Grammar, bool Debug)
		{
			if (string.IsNullOrEmpty(Grammar.GetTemplatePath()))
				return null;

			string scanner = File.ReadAllText(Grammar.GetTemplatePath() + templateName);

			int counter = 2;
			StringBuilder tokentype = new StringBuilder();
			StringBuilder regexps = new StringBuilder();
			StringBuilder skiplist = new StringBuilder();

			foreach (TerminalSymbol s in Grammar.SkipSymbols)
			{
				skiplist.AppendLine(Helper.Indent3 + "SkipList.push_back(TokenType::" + s.Name + ");");
			}

			if (Grammar.FileAndLine != null)
				skiplist.AppendLine(Helper.Indent3 + "FileAndLine = TokenType::" + Grammar.FileAndLine.Name + ";");

			// build system tokens
			tokentype.AppendLine("\r\n" + Helper.Indent3 + "//Non terminal tokens:");
			tokentype.AppendLine(Helper.Outline("_NONE_", 3, "= 0,", 5));
			tokentype.AppendLine(Helper.Outline("_UNDETERMINED_", 3, "= 1,", 5));

			// build non terminal tokens
			tokentype.AppendLine("\r\n" + Helper.Indent3 + "//Non terminal tokens:");
			foreach (Symbol s in Grammar.GetNonTerminals())
			{
				tokentype.AppendLine(Helper.Outline(s.Name, 3, "= " + string.Format("{0:d},", counter), 5));
				counter++;
			}

			// build terminal tokens
			tokentype.AppendLine("\r\n" + Helper.Indent3 + "//Terminal tokens:");
			bool first = true;
			foreach (TerminalSymbol s in Grammar.GetTerminals())
			{
				regexps.Append(Helper.Indent3 + "regex = std::regex(" + Helper.Unverbatim(s.Expression.ToString()) + ", std::regex_constants::ECMAScript");

				if (s.Attributes.ContainsKey("IgnoreCase"))
					regexps.Append(" | std::regex_constants::icase");

				regexps.Append(");\r\n");

				regexps.Append(Helper.Indent3 + "Patterns.insert(std::pair<TokenType,std::regex>(TokenType::" + s.Name + ", regex));\r\n");
				regexps.Append(Helper.Indent3 + "Tokens.push_back(TokenType::" + s.Name + ");\r\n\r\n");

				if (first) first = false;
				else tokentype.AppendLine(",");

				tokentype.Append(Helper.Outline(s.Name, 3, "= " + String.Format("{0:d}", counter), 5));
				counter++;
			}

			scanner = scanner.Replace(@"<%SourceFilename%>", Grammar.SourceFilename);
			scanner = scanner.Replace(@"<%SkipList%>", skiplist.ToString());
			scanner = scanner.Replace(@"<%RegExps%>", regexps.ToString());
			scanner = scanner.Replace(@"<%TokenType%>", tokentype.ToString());

			if (Debug)
			{
				scanner = scanner.Replace(@"<%Namespace%>", "TinyPG.Debug");
				scanner = scanner.Replace(@"<%IToken%>", " : TinyPG.Debug.IToken");
				scanner = scanner.Replace(@"<%ScannerCustomCode%>", Grammar.Directives["Scanner"]["CustomCode"]);
			}
			else
			{
				scanner = scanner.Replace(@"<%Namespace%>", Grammar.Directives["TinyPG"]["Namespace"]);
				scanner = scanner.Replace(@"<%IToken%>", "");
				scanner = scanner.Replace(@"<%ScannerCustomCode%>", Grammar.Directives["Scanner"]["CustomCode"]);
			}

			return scanner;
		}

	}
}
