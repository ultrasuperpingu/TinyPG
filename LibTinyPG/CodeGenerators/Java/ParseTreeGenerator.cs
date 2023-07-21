using System;
using System.Text;
using System.IO;
using TinyPG.Parsing;
using System.Text.RegularExpressions;
using System.Collections.Generic;


namespace TinyPG.CodeGenerators.Java
{
	public class ParseTreeGenerator : BaseGenerator, ICodeGenerator
	{
		internal ParseTreeGenerator() : base("ParseTree.java", "ParseNode.java", "ParseError.java", "ParseErrors.java")
		{
		}

		public Dictionary<string, string> Generate(Grammar Grammar, GenerateDebugMode Debug)
		{
			if (Debug != GenerateDebugMode.None)
				throw new Exception("Java cannot be generated in debug mode");
			Dictionary<string, string> templateFilesPath = GetTemplateFilesPath(Grammar, "ParseTree");

			StringBuilder evalsymbols = new StringBuilder();
			StringBuilder evalmethods = new StringBuilder();

			// build non terminal tokens
			foreach (NonTerminalSymbol s in Grammar.GetNonTerminals())
			{
				evalsymbols.AppendLine("			case " + s.Name + ":");
				evalsymbols.AppendLine("				Value = Eval" + s.Name + "(paramlist);");
				//evalsymbols.AppendLine("				Value = Token.Text;");
				evalsymbols.AppendLine("				break;");

				string returnType = "Object";
				if (!string.IsNullOrEmpty(s.ReturnType))
					returnType = s.ReturnType;
				string defaultReturnValue = "null";
				if (!string.IsNullOrEmpty(s.ReturnTypeDefault))
					defaultReturnValue = s.ReturnTypeDefault;
				if (s.Attributes.ContainsKey("EvalComment"))
				{
					evalmethods.AppendLine(GenerateComment(s.Attributes["EvalComment"], Helper.Indent2, true));
				}
				evalmethods.AppendLine("	protected " + returnType + " Eval" + s.Name + "(Object... paramlist)");
				evalmethods.AppendLine("	{");
				if (s.CodeBlock != null)
				{
					// paste user code here
					evalmethods.AppendLine(FormatCodeBlock(s));
				}
				else
				{
					// otherwise simply not implemented!
					evalmethods.AppendLine("		throw new UnsupportedOperationException(\"Could not interpret input; no semantics implemented.\");");
				}
				evalmethods.AppendLine("	}\r\n");
				evalmethods.AppendLine("	protected " + returnType + " Get" + s.Name + "Value(int index, Object... paramlist)");
				evalmethods.AppendLine("	{");
				evalmethods.AppendLine("		" + returnType + " o = "+defaultReturnValue+";");
				evalmethods.AppendLine("		ParseNode node = GetTokenNode(TokenType." + s.Name + ", index);");
				evalmethods.AppendLine("		if (node != null)");
				evalmethods.AppendLine("			o = node.Eval"+s.Name+"(paramlist);");
				evalmethods.AppendLine("		return o;");
				evalmethods.AppendLine("	}\r\n");
			}
			
			Dictionary<string, string> generated = new Dictionary<string, string>();
			foreach (var entry in templateFilesPath)
			{
				var templateFilePath = entry.Value;
				string fileContent = File.ReadAllText(templateFilePath);
				fileContent = fileContent.Replace(@"<%SourceFilename%>", Grammar.SourceFilename);
				fileContent = fileContent.Replace(@"<%Namespace%>", Grammar.Directives["TinyPG"]["Namespace"]);
				//fileContent = fileContent.Replace(@"<%CustomCode%>", Grammar.Directives["ParseTree"]["CustomCode"]);
				fileContent = fileContent.Replace(@"<%EvalSymbols%>", evalsymbols.ToString());
				fileContent = fileContent.Replace(@"<%VirtualEvalMethods%>", evalmethods.ToString());
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
						replacement = "this.GetTerminalValue(TokenType." + s.Name + ", " + indexer + ")";
					}
					else
					{
						replacement = "this.Get"+s.Name+"Value(" + indexer + ", paramlist)";
					}
				}
				else
				{
					replacement = "this.IsTokenPresent(TokenType." + s.Name + ", " + indexer + ")";
				}
				codeblock = codeblock.Substring(0, match.Captures[0].Index) + replacement + codeblock.Substring(match.Captures[0].Index + match.Captures[0].Length);
				startIndex =  match.Index + replacement.Length;
				match = var.Match(codeblock, startIndex);
			}

			codeblock = Helper.Indent3 + codeblock.Replace(Environment.NewLine, Environment.NewLine + Helper.Indent2);
			return codeblock;
		}
	}

}
