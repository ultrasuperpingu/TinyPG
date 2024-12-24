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
				evalsymbols.AppendLine("					value = self.eval_" + s.Name.ToLowerInvariant() + "(paramlist);");
				evalsymbols.AppendLine("				},");

				string returnType = "Option<Box<dyn std::any::Any>>";
				if (!string.IsNullOrEmpty(s.ReturnType))
					returnType = s.ReturnType;
				string defaultReturnValue = "None";
				if (!string.IsNullOrEmpty(s.ReturnTypeDefault))
					defaultReturnValue = s.ReturnTypeDefault;
				if (s.Attributes.ContainsKey("EvalComment"))
				{
					evalMethodsImpl.AppendLine(GenerateComment(s.Attributes["EvalComment"], Helper.Indent2));
					evalMethodsDecl.AppendLine(GenerateComment(s.Attributes["EvalComment"], Helper.Indent2));
				}
				evalMethodsDecl.AppendLine("	fn eval_" + s.Name.ToLowerInvariant() + "(&self, paramlist:&mut Vec<Box<dyn std::any::Any>>) -> " + returnType + ";");
				evalMethodsDecl.AppendLine("	fn get_" + s.Name.ToLowerInvariant() + "_value(&self, index : i32, paramlist:&mut Vec<Box<dyn std::any::Any>>) -> " + returnType + ";");

				evalMethodsImpl.AppendLine("	fn eval_" + s.Name.ToLowerInvariant() + "(&self, paramlist:&mut Vec<Box<dyn std::any::Any>>) -> " + returnType);
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


				evalMethodsImpl.AppendLine("	fn get_" + s.Name.ToLowerInvariant() + "_value(&self, index : i32, paramlist:&mut Vec<Box<dyn std::any::Any>>) -> " + returnType);
				evalMethodsImpl.AppendLine("	{");
				evalMethodsImpl.AppendLine("		let node = self.get_token_node(TokenType::" + s.Name + ", index);");
				evalMethodsImpl.AppendLine("		if let Some(n) = node {");
				evalMethodsImpl.AppendLine("			return n.eval_"+s.Name.ToLowerInvariant()+"(paramlist);");
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
						replacement = "self.get_terminal_value(TokenType::" + s.Name + ", " + indexer + ")";
					}
					else
					{
						replacement = "self.get_"+s.Name.ToLowerInvariant()+"_value(" + indexer + ", paramlist)";
					}
				}
				else
				{
					replacement = "self.is_token_present(TokenType::" + s.Name + ", " + indexer + ")";
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
