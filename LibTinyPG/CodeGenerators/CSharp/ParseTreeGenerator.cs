using System.Text;
using System.IO;
using TinyPG.Compiler;
using System.Text.RegularExpressions;
using System;
using System.Collections.Generic;

namespace TinyPG.CodeGenerators.CSharp
{
	public class ParseTreeGenerator : BaseGenerator, ICodeGenerator
	{
		internal ParseTreeGenerator() : base("ParseTree.cs")
		{
		}
		private bool isDebugOther;
		public Dictionary<string, string> Generate(Grammar Grammar, GenerateDebugMode Debug)
		{
			if (string.IsNullOrEmpty(Grammar.GetTemplatePath()))
				return null;
			isDebugOther = Debug == GenerateDebugMode.DebugOther;

			StringBuilder evalsymbols = new StringBuilder();
			StringBuilder evalmethods = new StringBuilder();

			// build non terminal tokens
			foreach (NonTerminalSymbol s in Grammar.GetNonTerminals())
			{
				evalsymbols.AppendLine("				case TokenType." + s.Name + ":");
				evalsymbols.AppendLine("					Value = Eval" + s.Name + "(paramlist);");
				//evalsymbols.AppendLine("					Value = Token.Text;");
				evalsymbols.AppendLine("				break;");

				string returnType = "object";
				if (!string.IsNullOrEmpty(s.ReturnType) && !isDebugOther)
					returnType = s.ReturnType;
				string defaultReturnValue = "default("+returnType+")";
				if (!string.IsNullOrEmpty(s.ReturnTypeDefault) && !isDebugOther)
					defaultReturnValue = s.ReturnTypeDefault;
				evalmethods.AppendLine("		protected virtual " + returnType + " Eval" + s.Name + "(params object[] paramlist)");
				evalmethods.AppendLine("		{");
				if (s.CodeBlock != null && !isDebugOther)
				{
					// paste user code here
					evalmethods.AppendLine(FormatCodeBlock(s));
				}
				else
				{
					// otherwise simply not implemented!
					evalmethods.AppendLine("			throw new NotImplementedException(\"Could not interpret input; no semantics implemented.\");");
				}
				evalmethods.AppendLine("		}\r\n");
				evalmethods.AppendLine("		protected virtual " + returnType + " Get" + s.Name + "Value(int index, params object[] paramlist )");
				evalmethods.AppendLine("		{");
				evalmethods.AppendLine("			" + returnType + " o = "+defaultReturnValue+";");
				evalmethods.AppendLine("			ParseNode node = GetTokenNode(TokenType." + s.Name + ", index);");
				evalmethods.AppendLine("			if (node != null)");
				evalmethods.AppendLine("				o = node.Eval"+s.Name+"(paramlist);");
				evalmethods.AppendLine("			return o;");
				evalmethods.AppendLine("		}\r\n");
			}

			Dictionary<string, string> generated = new Dictionary<string, string>();
			foreach (var templateName in TemplateFiles)
			{
				string fileContent = File.ReadAllText(Path.Combine(Grammar.GetTemplatePath(), templateName));
				fileContent = fileContent.Replace(@"<%SourceFilename%>", Grammar.SourceFilename);
				fileContent = fileContent.Replace(@"<%Namespace%>", Grammar.Directives["TinyPG"]["Namespace"]);
				if (Debug != GenerateDebugMode.None)
				{
					fileContent = fileContent.Replace(@"<%ParseError%>", " : TinyPG.Debug.IParseError");
					fileContent = fileContent.Replace(@"<%ParseErrors%>", "List<TinyPG.Debug.IParseError>");
					fileContent = fileContent.Replace(@"<%IParseTree%>", ", TinyPG.Debug.IParseTree");
					fileContent = fileContent.Replace(@"<%IParseNode%>", " : TinyPG.Debug.IParseNode");
					fileContent = fileContent.Replace(@"<%ITokenGet%>", "public TinyPG.Debug.IToken IToken { get {return (TinyPG.Debug.IToken)Token;} }");

					string inodes = "public List<TinyPG.Debug.IParseNode> INodes {get { return nodes.ConvertAll<TinyPG.Debug.IParseNode>( new Converter<ParseNode, TinyPG.Debug.IParseNode>( delegate(ParseNode n) { return (TinyPG.Debug.IParseNode)n; })); }}\r\n\r\n";
					fileContent = fileContent.Replace(@"<%INodesGet%>", inodes);
					if (Debug == GenerateDebugMode.DebugSelf)
					{
						fileContent = fileContent.Replace(@"<%CustomCode%>", Grammar.Directives["ParseTree"]["CustomCode"]);
					}
					else
					{
						fileContent = fileContent.Replace(@"<%CustomCode%>", "");
					}
				}
				else
				{
					fileContent = fileContent.Replace(@"<%ParseError%>", "");
					fileContent = fileContent.Replace(@"<%ParseErrors%>", "List<ParseError>");
					fileContent = fileContent.Replace(@"<%IParseTree%>", "");
					fileContent = fileContent.Replace(@"<%IParseNode%>", "");
					fileContent = fileContent.Replace(@"<%ITokenGet%>", "");
					fileContent = fileContent.Replace(@"<%INodesGet%>", "");
					fileContent = fileContent.Replace(@"<%CustomCode%>", Grammar.Directives["ParseTree"]["CustomCode"]);
				}

				fileContent = fileContent.Replace(@"<%EvalSymbols%>", evalsymbols.ToString());
				fileContent = fileContent.Replace(@"<%VirtualEvalMethods%>", evalmethods.ToString());
				fileContent = ReplaceDirectiveAttributes(fileContent, Grammar.Directives["ParseTree"]);
				generated[templateName] = fileContent;
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
			if (nts == null) return "";

			Regex var = new Regex(@"(?<eval>\$|\?)(?<var>[a-zA-Z_0-9]+)(\[(?<index>[^]]+)\])?", RegexOptions.Compiled);

			Symbols symbols = nts.DetermineProductionSymbols();


			Match match = var.Match(codeblock);
			while (match.Success)
			{
				Symbol s = symbols.Find(match.Groups["var"].Value);
				if (s == null)
				{
					continue; // error situation
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
				match = var.Match(codeblock);
			}

			codeblock = Helper.Indent3 + codeblock.Replace("\n", "\r\n" + Helper.Indent2);
			return codeblock;
		}
	}

}
