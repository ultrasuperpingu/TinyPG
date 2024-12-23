using System;
using System.Text;
using System.IO;
using TinyPG.Parsing;
using System.Text.RegularExpressions;
using System.Collections.Generic;


namespace TinyPG.CodeGenerators.Rust
{
	public class ParseTreeGenerator : BaseGenerator, ICodeGenerator
	{
		internal ParseTreeGenerator() : base("parse_tree.rs")
		{
		}

		public Dictionary<string, string> Generate(Grammar Grammar, GenerateDebugMode Debug)
		{
			if (Debug != GenerateDebugMode.None)
				throw new Exception("Rust cannot be generated in debug mode");
			Dictionary<string, string> templateFilesPath = GetTemplateFilesPath(Grammar, "ParseTree");


			StringBuilder evalsymbols = new StringBuilder();
			StringBuilder evalMethodsDecl = new StringBuilder();
			StringBuilder evalMethodsImpl = new StringBuilder();

			// build non terminal tokens
			foreach (NonTerminalSymbol s in Grammar.GetNonTerminals())
			{
				evalsymbols.AppendLine("				TokenType::" + s.Name + "=> {");
				evalsymbols.AppendLine("					Value = self.Eval" + s.Name + "(/*paramlist*/);");
				evalsymbols.AppendLine("				},");

				string returnType = "std::any";
				if (!string.IsNullOrEmpty(s.ReturnType))
					returnType = s.ReturnType;
				string defaultReturnValue = "std::any()";
				if (!string.IsNullOrEmpty(s.ReturnTypeDefault))
					defaultReturnValue = s.ReturnTypeDefault;
				if (s.Attributes.ContainsKey("EvalComment"))
				{
					evalMethodsImpl.AppendLine(GenerateComment(s.Attributes["EvalComment"], Helper.Indent2));
					evalMethodsDecl.AppendLine(GenerateComment(s.Attributes["EvalComment"], Helper.Indent2));
				}
				evalMethodsDecl.AppendLine("	fn Eval" + s.Name + "(&self/*, paramlist:&Vec<std::any>*/) -> " + returnType + ";");
				evalMethodsDecl.AppendLine("	fn Get" + s.Name + "Value(&self, index : i32/*, paramlist:&Vec<std::any>*/) -> " + returnType + ";");

				evalMethodsImpl.AppendLine("	fn Eval" + s.Name + "(&self/*, paramlist:&Vec<std::any>*/) -> " + returnType);
				evalMethodsImpl.AppendLine("	{");
				if (s.CodeBlock != null)
				{
					// paste user code here
					evalMethodsImpl.AppendLine(FormatCodeBlock(s));
				}
				else
				{
					evalMethodsImpl.AppendLine("		panic!(\"Could not interpret input; no semantics implemented.\");");
					// otherwise simply not implemented!
				}
				evalMethodsImpl.AppendLine("	}\r\n");


				evalMethodsImpl.AppendLine("	fn Get" + s.Name + "Value(&self, index : i32/*, paramlist:&Vec<std::any>*/) -> " + returnType);
				evalMethodsImpl.AppendLine("	{");
				evalMethodsImpl.AppendLine("		let node = self.GetTokenNode(TokenType::" + s.Name + ", index);");
				evalMethodsImpl.AppendLine("		if node.is_some() {");
				evalMethodsImpl.AppendLine("			return node.unwrap().Eval"+s.Name+"(/*paramlist*/);");
				evalMethodsImpl.AppendLine("		}");
				evalMethodsImpl.AppendLine("		panic!(\"No "+ s.Name+"[index] found.\");");
				evalMethodsImpl.AppendLine("	}");
				evalMethodsImpl.AppendLine();
			}

			Dictionary<string, string> generated = new Dictionary<string, string>();
			foreach (var entry in templateFilesPath)
			{
				var templateFilePath = entry.Value;
				string fileContent = File.ReadAllText(templateFilePath);
				fileContent = fileContent.Replace(@"<%SourceFilename%>", Grammar.SourceFilename);
				fileContent = fileContent.Replace(@"<%Namespace%>", Grammar.Directives["TinyPG"]["Namespace"]);
				
				fileContent = fileContent.Replace(@"<%EvalSymbols%>", evalsymbols.ToString());
				fileContent = fileContent.Replace(@"<%VirtualEvalMethods%>", evalMethodsImpl.ToString());
				fileContent = fileContent.Replace(@"<%VirtualEvalMethodsDecl%>", evalMethodsDecl.ToString());
				fileContent = fileContent.Replace(@"<%GeneratorVersion%>", TinyPGInfos.Version);
				fileContent = ReplaceDirectiveAttributes(fileContent, Grammar.Directives["ParseTree"]);
				generated[entry.Key] = fileContent;
			}
			return generated;
		}

		/// <summary>
		/// replaces $ variables with a c# statement
		/// the routine also implements some checks to see if $variables are matching with production symbols
		/// errors are added to the Error object.
		/// </summary>
		/// <param name="nts">non terminal and its production rule</param>
		/// <returns>a formated codeblock</returns>
		private string FormatCodeBlock(NonTerminalSymbol nts)
		{
			string codeblock = nts.CodeBlock;
			if (nts == null)
				return "";

			Regex var = new Regex(@"(?<eval>\$|\?)(?<var>[a-zA-Z_0-9]+)(\[(?<index>[^]]+)\])?", RegexOptions.Compiled);

			Symbols symbols = nts.DetermineProductionSymbols();


			int startIndex = 0;
			Match match = var.Match(codeblock, startIndex);
			while (match.Success)
			{
				Symbol s = symbols.Find(match.Groups["var"].Value);
				if (s == null)
				{
					// error situation
					startIndex =  match.Index + match.Length;
					match = var.Match(codeblock, startIndex);
					continue;
				}
				string indexer = "0";
				if (match.Groups["index"].Value.Length > 0)
				{
					indexer = match.Groups["index"].Value;
				}
				bool eval = match.Groups["eval"].Value == "$";
				string replacement;
				if (eval)
				{
					if(s is TerminalSymbol)
					{
						replacement = "self.GetTerminalValue(TokenType::" + s.Name + ", " + indexer + ")";
					}
					else
					{
						replacement = "self.Get"+s.Name+"Value(" + indexer + "/*, paramlist*/)";
					}
				}
				else
				{
					replacement = "self.IsTokenPresent(TokenType::" + s.Name + ", " + indexer + ")";
				}
				codeblock = codeblock.Substring(0, match.Captures[0].Index) + replacement + codeblock.Substring(match.Captures[0].Index + match.Captures[0].Length);
				startIndex =  match.Index + replacement.Length;
				match = var.Match(codeblock, startIndex);
			}

			codeblock = Helper.Indent2 + codeblock.FixNewLines().Replace(Environment.NewLine, Environment.NewLine + Helper.Indent1);
			return codeblock;
		}
	}

}
