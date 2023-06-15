using System.Text;
using System.IO;
using TinyPG.Compiler;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace TinyPG.CodeGenerators.VBNet
{
	public class ParseTreeGenerator : BaseGenerator, ICodeGenerator
	{
		internal ParseTreeGenerator() : base("ParseTree.vb")
		{
		}
		private bool isDebugOther;
		public Dictionary<string, string> Generate(Grammar Grammar, GenerateDebugMode Debug)
		{
			if (string.IsNullOrEmpty(Grammar.GetTemplatePath()))
				return null;
			isDebugOther = Debug == GenerateDebugMode.DebugOther;
			// copy the parse tree file (optionally)
			
			StringBuilder evalsymbols = new StringBuilder();
			StringBuilder evalmethods = new StringBuilder();

			// build non terminal tokens
			foreach (NonTerminalSymbol s in Grammar.GetNonTerminals())
			{
				evalsymbols.AppendLine("				Case TokenType." + s.Name + "");
				evalsymbols.AppendLine("					Value = Eval" + s.Name + "(paramlist)");
				evalsymbols.AppendLine("					Exit Select");

				string returnType = "Object";
				if (!string.IsNullOrEmpty(s.ReturnType) && !isDebugOther)
					returnType = s.ReturnType;
				string defaultReturnValue = "Nothing";
				if (!string.IsNullOrEmpty(s.ReturnTypeDefault) && !isDebugOther)
					defaultReturnValue = s.ReturnTypeDefault;
				evalmethods.AppendLine("		Protected Overridable Function Eval" + s.Name + "(ByVal ParamArray paramlist As Object()) As " + returnType);
				if (s.CodeBlock != null && !isDebugOther)
				{
					// paste user code here
					evalmethods.AppendLine(FormatCodeBlock(s));
				}
				else
				{
					// otherwise simply not implemented!
					evalmethods.AppendLine("			Throw New NotImplementedException(\"Could not interpret input; no semantics implemented.\")");
				}
				evalmethods.AppendLine("		End Function\r\n");
				evalmethods.AppendLine("		Protected Overridable Function Get" + s.Name + "Value(index As Integer, ByVal ParamArray paramlist As Object()) As " + returnType);
				evalmethods.AppendLine("			Dim o As " + returnType + " = "+defaultReturnValue+"");
				evalmethods.AppendLine("			Dim node As ParseNode = GetTokenNode(TokenType." + s.Name + ", index)");
				evalmethods.AppendLine("			If node IsNot Nothing");
				evalmethods.AppendLine("				o = node.Eval"+s.Name+"(paramlist)");
				evalmethods.AppendLine("			End If");
				evalmethods.AppendLine("			Return o");
				evalmethods.AppendLine("		End Function\r\n");
			}

			Dictionary<string, string> generated = new Dictionary<string, string>();
			foreach (var templateName in templateFiles)
			{
				string fileContent = File.ReadAllText(Path.Combine(Grammar.GetTemplatePath(), templateName));
				fileContent = fileContent.Replace(@"<%SourceFilename%>", Grammar.SourceFilename);
				fileContent = fileContent.Replace(@"<%Namespace%>", Grammar.Directives["TinyPG"]["Namespace"]);
				if (Debug != GenerateDebugMode.None)
				{
					fileContent = fileContent.Replace(@"<%Imports%>", "Imports TinyPG.Debug");
					//parsetree = parsetree.Replace(@"<%Namespace%>", "TinyPG.Debug");
					fileContent = fileContent.Replace(@"<%IParseTree%>", "\r\n        Implements TinyPG.Debug.IParseTree");
					fileContent = fileContent.Replace(@"<%IParseNode%>", "\r\n        Implements TinyPG.Debug.IParseNode\r\n");
					fileContent = fileContent.Replace(@"<%ParseError%>", "\r\n        Implements TinyPG.Debug.IParseError\r\n");
					fileContent = fileContent.Replace(@"<%ParseErrors%>", "List(Of TinyPG.Debug.IParseError)");

					string itoken = "		Public ReadOnly Property IToken() As TinyPG.Debug.IToken Implements TinyPG.Debug.IParseNode.IToken\r\n"
									+ "			Get\r\n"
									+ "				Return DirectCast(Token, TinyPG.Debug.IToken)\r\n"
									+ "			End Get\r\n"
									+ "		End Property\r\n";

					fileContent = fileContent.Replace(@"<%ITokenGet%>", itoken);


					fileContent = fileContent.Replace(@"<%ImplementsIParseTreePrintTree%>", " Implements TinyPG.Debug.IParseTree.PrintTree");
					fileContent = fileContent.Replace(@"<%ImplementsIParseTreeEval%>", " Implements TinyPG.Debug.IParseTree.Eval");
					fileContent = fileContent.Replace(@"<%ImplementsIParseErrorCode%>", " Implements TinyPG.Debug.IParseError.Code");
					fileContent = fileContent.Replace(@"<%ImplementsIParseErrorLine%>", " Implements TinyPG.Debug.IParseError.Line");
					fileContent = fileContent.Replace(@"<%ImplementsIParseErrorColumn%>", " Implements TinyPG.Debug.IParseError.Column");
					fileContent = fileContent.Replace(@"<%ImplementsIParseErrorPosition%>", " Implements TinyPG.Debug.IParseError.Position");
					fileContent = fileContent.Replace(@"<%ImplementsIParseErrorLength%>", " Implements TinyPG.Debug.IParseError.Length");
					fileContent = fileContent.Replace(@"<%ImplementsIParseErrorMessage%>", " Implements TinyPG.Debug.IParseError.Message");
					fileContent = fileContent.Replace(@"<%ImplementsIParseErrorIsWarning%>", " Implements TinyPG.Debug.IParseError.IsWarning");
					string inodes = "		Public Shared Function Node2INode(ByVal node As ParseNode) As TinyPG.Debug.IParseNode\r\n"
										+ "			Return DirectCast(node, TinyPG.Debug.IParseNode)\r\n"
										+ "		End Function\r\n\r\n"
										+ "		Public ReadOnly Property INodes() As List(Of TinyPG.Debug.IParseNode) Implements TinyPG.Debug.IParseNode.INodes\r\n"
										+ "			Get\r\n"
										+ "				Return Nodes.ConvertAll(Of TinyPG.Debug.IParseNode)(New Converter(Of ParseNode, TinyPG.Debug.IParseNode)(AddressOf Node2INode))\r\n"
										+ "			End Get\r\n"
										+ "		End Property\r\n";
					fileContent = fileContent.Replace(@"<%INodesGet%>", inodes);
					fileContent = fileContent.Replace(@"<%ImplementsIParseNodeText%>", " Implements TinyPG.Debug.IParseNode.Text");
					fileContent = fileContent.Replace(@"<%CustomCode%>", Grammar.Directives["ParseTree"]["CustomCode"]);
				}
				else
				{
					fileContent = fileContent.Replace(@"<%Imports%>", "");
					//parsetree = parsetree.Replace(@"<%Namespace%>", Grammar.Directives["TinyPG"]["Namespace"]);
					fileContent = fileContent.Replace(@"<%ParseError%>", "");
					fileContent = fileContent.Replace(@"<%ParseErrors%>", "List(Of ParseError)");
					fileContent = fileContent.Replace(@"<%IParseTree%>", "");
					fileContent = fileContent.Replace(@"<%IParseNode%>", "");
					fileContent = fileContent.Replace(@"<%ITokenGet%>", "");
					fileContent = fileContent.Replace(@"<%INodesGet%>", "");

					fileContent = fileContent.Replace(@"<%ImplementsIParseTreePrintTree%>", "");
					fileContent = fileContent.Replace(@"<%ImplementsIParseTreeEval%>", "");
					fileContent = fileContent.Replace(@"<%ImplementsIParseErrorCode%>", "");
					fileContent = fileContent.Replace(@"<%ImplementsIParseErrorLine%>", "");
					fileContent = fileContent.Replace(@"<%ImplementsIParseErrorColumn%>", "");
					fileContent = fileContent.Replace(@"<%ImplementsIParseErrorPosition%>", "");
					fileContent = fileContent.Replace(@"<%ImplementsIParseErrorLength%>", "");
					fileContent = fileContent.Replace(@"<%ImplementsIParseErrorMessage%>", "");
					fileContent = fileContent.Replace(@"<%ImplementsIParseErrorIsWarning%>", "");
					fileContent = fileContent.Replace(@"<%ImplementsIParseNodeText%>", "");
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
					continue; // error situation
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
						replacement = "Me.GetTerminalValue(TokenType." + s.Name + ", " + indexer + ")";
					}
					else
					{
						replacement = "Me.Get"+ s.Name + "Value(" + indexer + ", paramlist)";
					}
				}
				else
				{
					replacement = "Me.IsTokenPresent(TokenType." + s.Name + ", " + indexer + ")";
				}
				codeblock = codeblock.Substring(0, match.Captures[0].Index) + replacement + codeblock.Substring(match.Captures[0].Index + match.Captures[0].Length);
				match = var.Match(codeblock);
			}

			codeblock = Helper.Indent3 + codeblock.Replace("\n", "\r\n" + Helper.Indent2);
			return codeblock;
		}
	}

}
