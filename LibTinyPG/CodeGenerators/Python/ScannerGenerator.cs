using System;
using System.Text;
using System.IO;
using TinyPG.Parsing;
using System.Collections.Generic;

namespace TinyPG.CodeGenerators.Python
{
	public class ScannerGenerator : BaseGenerator, ICodeGenerator
	{
		internal ScannerGenerator() : base("scanner.py")
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
				skiplist.AppendLine("		self.SkipList += [TokenType." + s.Name + "];");
			}

			// build system tokens
			tokentype.AppendLine(Environment.NewLine + "	#Non terminal tokens:");
			tokentype.AppendLine(Helper.Outline("NONE_", 1, "= 0,", 5));
			tokentype.AppendLine(Helper.Outline("UNDETERMINED_", 1, "= 1,", 5));

			// build non terminal tokens
			tokentype.AppendLine(Environment.NewLine + "	#Non terminal tokens:");
			foreach (Symbol s in Grammar.GetNonTerminals())
			{
				tokentype.AppendLine(Helper.Outline(s.Name, 1, "= " + String.Format("{0:d},", counter), 5));
				counter++;
			}

			// build terminal tokens
			tokentype.AppendLine(Environment.NewLine + "	#Terminal tokens:");
			bool first = true;
			foreach (TerminalSymbol s in Grammar.GetTerminals())
			{
				string RegexCompiled = null;
				Grammar.Directives.Find("TinyPG").TryGetValue("RegexCompiled", out RegexCompiled);
				var expr = s.Expression;
				// Add begin anchor if not present (\G).
				// the whole regex specified by user is encapsulated by
				//  a non capturing group: (?:userRegex)
				// TODO: on expression starting with a begin anchor (\A, ^), Regex.Match(content, startat) will fail
				// Launch a warning to the user...
				if (!expr.StartsWith("@\"^")
					&& !expr.StartsWith("\"^"))
				{
					expr = expr.Insert(expr.IndexOf("\"")+1, @"^(?:");
					expr = expr.Insert(expr.Length-1, ")");
				}
				regexps.Append("		regex = re.compile(" + Helper.Unverbatim(expr) + ", 0");

				if (s.Attributes.ContainsKey("IgnoreCase"))
					regexps.Append(" | re.IGNORECASE");

				regexps.Append(");" + Environment.NewLine);

				regexps.Append("		self.Patterns[TokenType." + s.Name + "] = regex;" + Environment.NewLine);
				regexps.Append("		self.Tokens += [TokenType." + s.Name + "];" + Environment.NewLine + Environment.NewLine);

				if (first)
					first = false;
				else
					tokentype.AppendLine(",");

				tokentype.Append(Helper.Outline(s.Name, 1, "= " + String.Format("{0:d}", counter), 5));
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
				fileContent = fileContent.Replace(@"<%GeneratorVersion%>", TinyPGInfos.Version);
				fileContent = ReplaceDirectiveAttributes(fileContent, Grammar.Directives["Scanner"]);
				generated[entry.Key] = fileContent;
			}

			return generated;
		}
	}
}
