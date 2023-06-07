using System;
using System.Text;
using System.IO;
using TinyPG.Compiler;

namespace TinyPG.CodeGenerators.Cpp
{
	public class ScannerGenerator : BaseGenerator, ICodeGenerator
	{
		internal ScannerGenerator() : base("Scanner.h")
		{
		}

		public string Generate(Grammar Grammar, GenerateDebugMode Debug)
		{
			if (Debug != GenerateDebugMode.None)
				throw new Exception("Cpp cannot be generated in debug mode");
			if (string.IsNullOrEmpty(Grammar.GetTemplatePath()))
				return null;
			
			string scanner = File.ReadAllText(Grammar.GetTemplatePath() + templateName);

			int counter = 2;
			StringBuilder tokentype = new StringBuilder();
			StringBuilder regexps = new StringBuilder();
			StringBuilder skiplist = new StringBuilder();

			foreach (TerminalSymbol s in Grammar.SkipSymbols)
			{
				skiplist.AppendLine("			SkipList.push_back(TokenType::" + s.Name + ");");
			}

			// build system tokens
			tokentype.AppendLine("\r\n			//Non terminal tokens:");
			tokentype.AppendLine(Helper.Outline("_NONE_", 3, "= 0,", 5));
			tokentype.AppendLine(Helper.Outline("_UNDETERMINED_", 3, "= 1,", 5));

			// build non terminal tokens
			tokentype.AppendLine("\r\n			//Non terminal tokens:");
			foreach (Symbol s in Grammar.GetNonTerminals())
			{
				tokentype.AppendLine(Helper.Outline(s.Name, 3, "= " + string.Format("{0:d},", counter), 5));
				counter++;
			}

			// build terminal tokens
			tokentype.AppendLine("\r\n			//Terminal tokens:");
			bool first = true;
			foreach (TerminalSymbol s in Grammar.GetTerminals())
			{
				regexps.Append("			regex = std::regex(" + Helper.Unverbatim(s.Expression.ToString()) + ", std::regex_constants::ECMAScript");

				if (s.Attributes.ContainsKey("IgnoreCase"))
					regexps.Append(" | std::regex_constants::icase");

				regexps.Append(");\r\n");

				regexps.Append("			Patterns.insert(std::pair<TokenType,std::regex>(TokenType::" + s.Name + ", regex));\r\n");
				regexps.Append("			Tokens.push_back(TokenType::" + s.Name + ");\r\n\r\n");

				if (first)
					first = false;
				else
					tokentype.AppendLine(",");

				tokentype.Append(Helper.Outline(s.Name, 3, "= " + string.Format("{0:d}", counter), 5));
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
