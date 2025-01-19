using System;
using System.Text;
using System.IO;
using TinyPG.Parsing;
using System.Text.RegularExpressions;
using System.Collections.Generic;


namespace TinyPG.CodeGenerators.Python
{
	public class ParseTreeGenerator : BaseGenerator, ICodeGenerator
	{
		internal ParseTreeGenerator() : base("parse_tree.py")
		{
		}
		private bool isDebugOther;
		public Dictionary<string, string> Generate(Grammar Grammar, GenerateDebugMode Debug)
		{
			isDebugOther = Debug == GenerateDebugMode.DebugOther;
			Dictionary<string, string> templateFilesPath = GetTemplateFilesPath(Grammar, "ParseTree");


			StringBuilder evalsymbols = new StringBuilder();
			StringBuilder evalmethods = new StringBuilder();
			bool first = true;
			// build non terminal tokens
			foreach (NonTerminalSymbol s in Grammar.GetNonTerminals())
			{
				if(first)
					evalsymbols.AppendLine("		if self.Token.Type == TokenType." + s.Name + ":");
				else
					evalsymbols.AppendLine("		elif self.Token.Type == TokenType." + s.Name + ":");
				evalsymbols.AppendLine("			Value = Eval" + s.Name + "(paramlist);");

				string returnType = "object";
				if (!string.IsNullOrEmpty(s.ReturnType) && !isDebugOther)
					returnType = s.ReturnType;
				string defaultReturnValue = "";
				if (!string.IsNullOrEmpty(s.ReturnTypeDefault) && !isDebugOther)
					defaultReturnValue = s.ReturnTypeDefault;
				if (s.Attributes.ContainsKey("EvalComment"))
				{
					evalmethods.AppendLine(GenerateComment(s.Attributes["EvalComment"], Helper.Indent2));
				}
				evalmethods.AppendLine("	def  Eval" + s.Name + "(paramlist):");
				if (s.CodeBlock != null && !isDebugOther)
				{
					// paste user code here
					evalmethods.AppendLine(FormatCodeBlock(s));
				}
				else
				{
					// otherwise simply not implemented!
					evalmethods.AppendLine("		print(\"Could not interpret input; no semantics implemented.\");");
				}
				evalmethods.AppendLine();
				evalmethods.AppendLine("	def Get" + s.Name + "Value(index, paramlist):");
				evalmethods.AppendLine("		o = "+defaultReturnValue+";");
				evalmethods.AppendLine("		node = self.GetTokenNode(TokenType." + s.Name + ", index);");
				evalmethods.AppendLine("		if (node != None):");
				evalmethods.AppendLine("			o = node.Eval"+s.Name+"(paramlist);");
				evalmethods.AppendLine("		return o;");
				evalmethods.AppendLine();
			}

			Dictionary<string, string> generated = new Dictionary<string, string>();
			foreach (var entry in templateFilesPath)
			{
				var templateFilePath = entry.Value;
				string fileContent = File.ReadAllText(templateFilePath);
				fileContent = fileContent.Replace(@"<%SourceFilename%>", Grammar.SourceFilename);
				fileContent = fileContent.Replace(@"<%Namespace%>", Grammar.Directives["TinyPG"]["Namespace"]);
				if (Debug != GenerateDebugMode.None)
				{
					if (Debug == GenerateDebugMode.DebugSelf)
					{
						fileContent = fileContent.Replace(@"<%CustomCode%>", Grammar.Directives["ParseTree"]["CustomCode"]);
						fileContent = fileContent.Replace(@"<%HeaderCode%>", Grammar.Directives["ParseTree"]["HeaderCode"]);
					}
					else
					{
						fileContent = fileContent.Replace(@"<%CustomCode%>", "");
						fileContent = fileContent.Replace(@"<%HeaderCode%>", "");
					}
				}
				else
				{
					fileContent = fileContent.Replace(@"<%ParseError%>", "");
					fileContent = fileContent.Replace(@"<%ParseErrors%>", "List<ParseError>");
					fileContent = fileContent.Replace(@"<%CustomCode%>", Grammar.Directives["ParseTree"]["CustomCode"]);
				}

				fileContent = fileContent.Replace(@"<%EvalSymbols%>", evalsymbols.ToString());
				fileContent = fileContent.Replace(@"<%VirtualEvalMethods%>", evalmethods.ToString());
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
						replacement = "self.GetTerminalValue(TokenType." + s.Name + ", " + indexer + ")";
					}
					else
					{
						replacement = "self.Get"+s.Name+"Value(" + indexer + ", paramlist)";
					}
				}
				else
				{
					replacement = "self.IsTokenPresent(TokenType." + s.Name + ", " + indexer + ")";
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
