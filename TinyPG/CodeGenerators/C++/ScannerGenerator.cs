using System;
using System.Text;
using System.IO;
using TinyPG.Compiler;
using System.Collections.Generic;

namespace TinyPG.CodeGenerators.Cpp
{
	public class ScannerGenerator : BaseGenerator, ICodeGenerator
	{
		internal ScannerGenerator() : base("include/Scanner.h", "src/Scanner.cpp")
		{
		}

		public Dictionary<string, string> Generate(Grammar Grammar, GenerateDebugMode Debug)
		{
			if (Debug != GenerateDebugMode.None)
				throw new Exception("Cpp cannot be generated in debug mode");
			if (string.IsNullOrEmpty(Grammar.GetTemplatePath()))
				return null;

			int counter = 2;
			StringBuilder tokentype = new StringBuilder();
			StringBuilder regexps = new StringBuilder();
			StringBuilder skiplist = new StringBuilder();

			foreach (TerminalSymbol s in Grammar.SkipSymbols)
			{
				skiplist.AppendLine("		SkipList.push_back(TokenType::" + s.Name + ");");
			}

			// build system tokens
			tokentype.AppendLine(Environment.NewLine + "		//Non terminal tokens:");
			tokentype.AppendLine(Helper.Outline("_NONE_", 2, "= 0,", 5));
			tokentype.AppendLine(Helper.Outline("_UNDETERMINED_", 2, "= 1,", 5));

			// build non terminal tokens
			tokentype.AppendLine(Environment.NewLine + "		//Non terminal tokens:");
			foreach (Symbol s in Grammar.GetNonTerminals())
			{
				tokentype.AppendLine(Helper.Outline(s.Name, 2, "= " + string.Format("{0:d},", counter), 5));
				counter++;
			}

			// build terminal tokens
			tokentype.AppendLine("\r\n		//Terminal tokens:");
			bool first = true;
			foreach (TerminalSymbol s in Grammar.GetTerminals())
			{
				var expr = s.Expression;
				// Add begin anchor if not present (^).
				// the whole regex specified by user is encapsulated by
				//  a non capturing group: (?:userRegex)
				if (!expr.StartsWith("@\"^")
					&& !expr.StartsWith("\"^"))
				{
					expr = expr.Insert(expr.IndexOf("\"")+1, @"^(?:");
					expr = expr.Insert(expr.Length-1, ")");
				}
				regexps.Append("		regex = std::regex(" + Helper.Unverbatim(expr) + "");

				if (s.Attributes.ContainsKey("IgnoreCase"))
					regexps.Append("std::regex_constants::icase");

				regexps.Append(");\r\n");

				regexps.Append("		Patterns.insert(std::pair<TokenType,std::regex>(TokenType::" + s.Name + ", regex));\r\n");
				regexps.Append("		Tokens.push_back(TokenType::" + s.Name + ");\r\n\r\n");

				if (first)
					first = false;
				else
					tokentype.AppendLine(",");

				tokentype.Append(Helper.Outline(s.Name, 2, "= " + string.Format("{0:d}", counter), 5));
				counter++;
			}
			Dictionary<string, string> generated = new Dictionary<string, string>();
			foreach (var templateName in templateFiles)
			{
				string fileContent = File.ReadAllText(Path.Combine(Grammar.GetTemplatePath(), templateName));
				fileContent = fileContent.Replace(@"<%SourceFilename%>", Grammar.SourceFilename);
				fileContent = fileContent.Replace(@"<%SkipList%>", skiplist.ToString());
				fileContent = fileContent.Replace(@"<%RegExps%>", regexps.ToString());
				fileContent = fileContent.Replace(@"<%TokenType%>", tokentype.ToString());
				fileContent = fileContent.Replace(@"<%Namespace%>", Grammar.Directives["TinyPG"]["Namespace"]);
				fileContent = ReplaceDirectiveAttributes(fileContent, Grammar.Directives["Scanner"]);
				generated[templateName] = fileContent;
			}
			return generated;
		}

	}
}
