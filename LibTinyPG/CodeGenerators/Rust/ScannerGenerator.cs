﻿using System;
using System.Text;
using System.IO;
using TinyPG.Parsing;
using System.Collections.Generic;

namespace TinyPG.CodeGenerators.Rust
{
	public class ScannerGenerator : BaseGenerator, ICodeGenerator
	{
		internal ScannerGenerator() : base("scanner.rs")
		{
		}

		public Dictionary<string, string> Generate(Grammar Grammar, GenerateDebugMode Debug)
		{
			if (Debug != GenerateDebugMode.None)
				throw new Exception("Rust cannot be generated in debug mode");
			Dictionary<string, string> templateFilesPath = GetTemplateFilesPath(Grammar, "Scanner");

			int counter = 2;
			StringBuilder tokentype = new StringBuilder();
			StringBuilder regexps = new StringBuilder();
			StringBuilder skiplist = new StringBuilder();

			foreach (TerminalSymbol s in Grammar.SkipSymbols)
			{
				skiplist.AppendLine("		ret.skip_list.push(TokenType::" + s.Name + ");");
			}

			// build system tokens
			tokentype.AppendLine();
			tokentype.AppendLine("		//Non terminal tokens:");
			tokentype.AppendLine(Helper.Outline("_NONE_", 1, "= 0,", 5));
			tokentype.AppendLine(Helper.Outline("_UNDETERMINED_", 1, "= 1,", 5));

			// build non terminal tokens
			tokentype.AppendLine(Environment.NewLine + "		//Non terminal tokens:");
			foreach (Symbol s in Grammar.GetNonTerminals())
			{
				tokentype.AppendLine(Helper.Outline(s.Name, 1, "= " + string.Format("{0:d},", counter), 5));
				counter++;
			}

			// build terminal tokens
			tokentype.AppendLine();
			tokentype.AppendLine("		//Terminal tokens:");
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
				regexps.Append("		regex = RegexBuilder::new(" + Helper.Unverbatim(expr) + ")");
				if (s.Attributes.ContainsKey("IgnoreCase")) {
					regexps.Append(".case_insensitive(true)");
				}
				regexps.AppendLine(".build().expect(\"Invalid regex\");");

				regexps.AppendLine("		ret.patterns.insert(TokenType::" + s.Name + ", regex);");
				regexps.AppendLine("		ret.tokens.push(TokenType::" + s.Name + ");");
				regexps.AppendLine();

				tokentype.Append(Helper.Outline(s.Name, 1, "= " + string.Format("{0:d}", counter), 5)).AppendLine(",");
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
