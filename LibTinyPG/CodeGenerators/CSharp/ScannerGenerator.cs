using System;
using System.Text;
using System.IO;
using TinyPG.Parsing;
using System.Collections.Generic;

namespace TinyPG.CodeGenerators.CSharp
{
	public class ScannerGenerator : BaseGenerator, ICodeGenerator
	{
		internal ScannerGenerator() : base("Scanner.cs")
		{
		}

		public Dictionary<string, string> Generate(Grammar Grammar, GenerateDebugMode Debug)
		{
			string templatePath = Grammar.GetTemplatePath();
			if (string.IsNullOrEmpty(templatePath))
				throw new Exception("Template path not found:" + Grammar.Directives["TinyPG"]["TemplatePath"]);

			int counter = 2;
			StringBuilder tokentype = new StringBuilder();
			StringBuilder regexps = new StringBuilder();
			StringBuilder skiplist = new StringBuilder();

			foreach (TerminalSymbol s in Grammar.SkipSymbols)
			{
				skiplist.AppendLine("			SkipList.Add(TokenType." + s.Name + ");");
			}

			// build system tokens
			tokentype.AppendLine(Environment.NewLine + "		//Non terminal tokens:");
			tokentype.AppendLine(Helper.Outline("_NONE_", 2, "= 0,", 5));
			tokentype.AppendLine(Helper.Outline("_UNDETERMINED_", 2, "= 1,", 5));

			// build non terminal tokens
			tokentype.AppendLine(Environment.NewLine + "		//Non terminal tokens:");
			foreach (Symbol s in Grammar.GetNonTerminals())
			{
				tokentype.AppendLine(Helper.Outline(s.Name, 2, "= " + String.Format("{0:d},", counter), 5));
				counter++;
			}

			// build terminal tokens
			tokentype.AppendLine(Environment.NewLine + "			//Terminal tokens:");
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
				if (expr.StartsWith("@"))
				{
					if (!expr.StartsWith("@\"^") && !expr.StartsWith(@"@""\A") && !expr.StartsWith(@"@""\G"))
					{
						expr = expr.Insert(expr.IndexOf("\"")+1, @"\G(?:");
						expr = expr.Insert(expr.Length-1, ")");
					}
				}
				else if (expr.StartsWith("\""))
				{
					if (!expr.StartsWith("\"^") && !expr.StartsWith("\"\\A") && !expr.StartsWith("\"\\G"))
					{
						expr = expr.Insert(expr.IndexOf("\"")+1, @"\\G(?:");
						expr = expr.Insert(expr.Length-1, ")");
					}
				}
				else
				{
					throw new Exception("Internal Error: Termimal token not starting with \" or @\"");
				}
				regexps.Append("			regex = new Regex(" + expr + ", RegexOptions.None");

				if (RegexCompiled == null || RegexCompiled.ToLower().Equals("true"))
					regexps.Append(" | RegexOptions.Compiled");

				if (s.Attributes.ContainsKey("IgnoreCase"))
					regexps.Append(" | RegexOptions.IgnoreCase");

				regexps.Append(");" + Environment.NewLine);

				regexps.Append("			Patterns.Add(TokenType." + s.Name + ", regex);" + Environment.NewLine);
				regexps.Append("			Tokens.Add(TokenType." + s.Name + ");" + Environment.NewLine + Environment.NewLine);

				if (first)
					first = false;
				else
					tokentype.AppendLine(",");

				tokentype.Append(Helper.Outline(s.Name, 2, "= " + String.Format("{0:d}", counter), 5));
				counter++;
			}

			Dictionary<string, string> generated = new Dictionary<string, string>();
			foreach (var templateName in TemplateFiles)
			{
				string fileContent = File.ReadAllText(Path.Combine(templatePath, templateName));
				fileContent = fileContent.Replace(@"<%SourceFilename%>", Grammar.SourceFilename);
				fileContent = fileContent.Replace(@"<%SkipList%>", skiplist.ToString());
				fileContent = fileContent.Replace(@"<%RegExps%>", regexps.ToString());
				fileContent = fileContent.Replace(@"<%TokenType%>", tokentype.ToString());
				fileContent = fileContent.Replace(@"<%Namespace%>", Grammar.Directives["TinyPG"]["Namespace"]);
				if (Debug != GenerateDebugMode.None)
				{
					fileContent = fileContent.Replace(@"<%IToken%>", " : TinyPG.Debug.IToken");
				}
				else
				{
					fileContent = fileContent.Replace(@"<%IToken%>", "");
				}
				fileContent = ReplaceDirectiveAttributes(fileContent, Grammar.Directives["Scanner"]);
				generated[templateName] = fileContent;
			}

			return generated;
		}
	}
}
