using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using TinyPG;
using TinyPG.Parsing;

namespace TinyPG.CodeGenerators.Java
{
	public class ScannerGenerator : BaseGenerator, ICodeGenerator
	{
		internal ScannerGenerator() : base("Scanner.java", "Token.java", "TokenType.java")
		{
		}

		public Dictionary<string, string> Generate(Grammar Grammar, GenerateDebugMode Debug)
		{
			if (Debug != GenerateDebugMode.None)
				throw new Exception("Java cannot be generated in debug mode");
			if (string.IsNullOrEmpty(Grammar.GetTemplatePath()))
				return null;


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

			Dictionary<string, string> generated = new Dictionary<string, string>();
			foreach (var f in templateFiles)
			{
				string fileContent = File.ReadAllText(Path.Combine(Grammar.GetTemplatePath(), f));
				fileContent = fileContent.Replace(@"<%SourceFilename%>", Grammar.SourceFilename);
				fileContent = fileContent.Replace(@"<%SkipList%>", skiplist.ToString());
				fileContent = fileContent.Replace(@"<%RegExps%>", regexps.ToString());
				fileContent = fileContent.Replace(@"<%TokenType%>", tokentype.ToString());
				fileContent = fileContent.Replace(@"<%Namespace%>", Grammar.Directives["TinyPG"]["Namespace"]);
				fileContent = ReplaceDirectiveAttributes(fileContent, Grammar.Directives["Scanner"]);
				generated[f] = fileContent;
			}
			return generated;
		}
	}
}
