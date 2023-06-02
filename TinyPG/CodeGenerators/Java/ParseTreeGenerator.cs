using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using TinyPG;
using TinyPG.Compiler;
using System.Text.RegularExpressions;

namespace TinyPG.CodeGenerators.Java
{
	public class ParseTreeGenerator : BaseGenerator, ICodeGenerator
	{
		internal ParseTreeGenerator() : base("ParseTree.java")
		{
		}

		public string Generate(Grammar Grammar, bool Debug)
		{
			if (Debug)
				throw new Exception("Java cannot be generated in debug mode");
			if (string.IsNullOrEmpty(Grammar.GetTemplatePath()))
				return null;

			// copy the parse tree file (optionally)
			string parsetree = File.ReadAllText(Grammar.GetTemplatePath() + templateName);

			StringBuilder evalsymbols = new StringBuilder();
			StringBuilder evalmethods = new StringBuilder();

			// build non terminal tokens
			foreach (NonTerminalSymbol s in Grammar.GetNonTerminals())
			{
				evalsymbols.AppendLine("			case " + s.Name + ":");
				evalsymbols.AppendLine("				Value = Eval" + s.Name + "(tree, paramlist);");
				//evalsymbols.AppendLine("				Value = Token.Text;");
				evalsymbols.AppendLine("				break;");

				string returnType = "object";
				if (!string.IsNullOrEmpty(s.ReturnType))
					returnType = s.ReturnType;
				string defaultReturnValue = "default("+returnType+")";
				if (!string.IsNullOrEmpty(s.ReturnTypeDefault))
					defaultReturnValue = s.ReturnTypeDefault;
				evalmethods.AppendLine("	protected " + returnType + " Eval" + s.Name + "(ParseTree tree, Object... paramlist)");
				evalmethods.AppendLine("	{");
				if (s.CodeBlock != null)
				{
					// paste user code here
					evalmethods.AppendLine(FormatCodeBlock(s));
				}
				else
				{
					if (s.Name == "Start") // return a nice warning message from root object.
						evalmethods.AppendLine("		return "+defaultReturnValue+"; //\"Could not interpret input; no semantics implemented.\";");
					else
						evalmethods.AppendLine("		for (ParseNode node : getNodes())\r\n" +
											   "			node.Eval(tree, paramlist);\r\n" +
											   "		return "+defaultReturnValue+";");

					// otherwise simply not implemented!
				}
				evalmethods.AppendLine("	}\r\n");
			}

			parsetree = parsetree.Replace(@"<%SourceFilename%>", Grammar.SourceFilename);
			parsetree = parsetree.Replace(@"<%Namespace%>", Grammar.Directives["TinyPG"]["Namespace"]);
			parsetree = parsetree.Replace(@"<%ParseError%>", "");
			parsetree = parsetree.Replace(@"<%ParseErrors%>", "ArrayList<ParseError>");
			parsetree = parsetree.Replace(@"<%IParseTree%>", "");
			parsetree = parsetree.Replace(@"<%IParseNode%>", "");
			parsetree = parsetree.Replace(@"<%ITokenGet%>", "");
			parsetree = parsetree.Replace(@"<%INodesGet%>", "");
			parsetree = parsetree.Replace(@"<%ParseTreeCustomCode%>", Grammar.Directives["ParseTree"]["CustomCode"]);
			parsetree = parsetree.Replace(@"<%EvalSymbols%>", evalsymbols.ToString());
			parsetree = parsetree.Replace(@"<%VirtualEvalMethods%>", evalmethods.ToString());

			return parsetree;
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
			if (nts == null) return "";

			Regex var = new Regex(@"\$(?<var>[a-zA-Z_0-9]+)(\[(?<index>[^]]+)\])?", RegexOptions.Compiled);

			Symbols symbols = nts.DetermineProductionSymbols();


			Match match = var.Match(codeblock);
			while (match.Success)
			{
				Symbol s = symbols.Find(match.Groups["var"].Value);
				if (s == null)
				{
					//TODO: handle error situation
					//Errors.Add("Variable $" + match.Groups["var"].Value + " cannot be matched.");
					break; // error situation
				}
				string indexer = "0";
				if (match.Groups["index"].Value.Length > 0)
				{
					indexer = match.Groups["index"].Value;
				}

				string replacement = "this.GetValue(tree, TokenType." + s.Name + ", " + indexer + ")";

				codeblock = codeblock.Substring(0, match.Captures[0].Index) + replacement + codeblock.Substring(match.Captures[0].Index + match.Captures[0].Length);
				match = var.Match(codeblock);
			}
			
			var = new Regex(@"\?(?<var>[a-zA-Z_0-9]+)(\[(?<index>[^]]+)\])?", RegexOptions.Compiled);
			match = var.Match(codeblock);
			while (match.Success)
			{
				Symbol s = symbols.Find(match.Groups["var"].Value);
				if (s == null)
				{
					//TODO: handle error situation
					//Errors.Add("Variable $" + match.Groups["var"].Value + " cannot be matched.");
					break; // error situation
				}
				string indexer = "0";
				if (match.Groups["index"].Value.Length > 0)
				{
					indexer = match.Groups["index"].Value;
				}

				string replacement = "this.IsTokenPresent(TokenType." + s.Name + ", " + indexer + ")";

				codeblock = codeblock.Substring(0, match.Captures[0].Index) + replacement + codeblock.Substring(match.Captures[0].Index + match.Captures[0].Length);
				match = var.Match(codeblock);
			}
			codeblock = Helper.Indent3 + codeblock.Replace("\n", "\r\n" + Helper.Indent2);
			return codeblock;
		}
	}

}
