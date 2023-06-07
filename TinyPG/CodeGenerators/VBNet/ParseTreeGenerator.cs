using System.Text;
using System.IO;
using TinyPG.Compiler;
using System.Text.RegularExpressions;

namespace TinyPG.CodeGenerators.VBNet
{
	public class ParseTreeGenerator : BaseGenerator, ICodeGenerator
	{
		internal ParseTreeGenerator() : base("ParseTree.vb")
		{
		}
		private bool isDebugOther;
		public string Generate(Grammar Grammar, GenerateDebugMode Debug)
		{
			if (string.IsNullOrEmpty(Grammar.GetTemplatePath()))
				return null;
			isDebugOther = Debug == GenerateDebugMode.DebugOther;
			// copy the parse tree file (optionally)
			string parsetree = File.ReadAllText(Grammar.GetTemplatePath() + templateName);

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

			parsetree = parsetree.Replace(@"<%SourceFilename%>", Grammar.SourceFilename);
			parsetree = parsetree.Replace(@"<%Namespace%>", Grammar.Directives["TinyPG"]["Namespace"]);
			if (Debug != GenerateDebugMode.None)
			{
				parsetree = parsetree.Replace(@"<%Imports%>", "Imports TinyPG.Debug");
				//parsetree = parsetree.Replace(@"<%Namespace%>", "TinyPG.Debug");
				parsetree = parsetree.Replace(@"<%IParseTree%>", "\r\n        Implements TinyPG.Debug.IParseTree");
				parsetree = parsetree.Replace(@"<%IParseNode%>", "\r\n        Implements TinyPG.Debug.IParseNode\r\n");
				parsetree = parsetree.Replace(@"<%ParseError%>", "\r\n        Implements TinyPG.Debug.IParseError\r\n");
				parsetree = parsetree.Replace(@"<%ParseErrors%>", "List(Of TinyPG.Debug.IParseError)");

				string itoken = "		Public ReadOnly Property IToken() As TinyPG.Debug.IToken Implements TinyPG.Debug.IParseNode.IToken\r\n"
								+ "			Get\r\n"
								+ "				Return DirectCast(Token, TinyPG.Debug.IToken)\r\n"
								+ "			End Get\r\n"
								+ "		End Property\r\n";

				parsetree = parsetree.Replace(@"<%ITokenGet%>", itoken);


				parsetree = parsetree.Replace(@"<%ImplementsIParseTreePrintTree%>", " Implements TinyPG.Debug.IParseTree.PrintTree");
				parsetree = parsetree.Replace(@"<%ImplementsIParseTreeEval%>", " Implements TinyPG.Debug.IParseTree.Eval");
				parsetree = parsetree.Replace(@"<%ImplementsIParseErrorCode%>", " Implements TinyPG.Debug.IParseError.Code");
				parsetree = parsetree.Replace(@"<%ImplementsIParseErrorLine%>", " Implements TinyPG.Debug.IParseError.Line");
				parsetree = parsetree.Replace(@"<%ImplementsIParseErrorColumn%>", " Implements TinyPG.Debug.IParseError.Column");
				parsetree = parsetree.Replace(@"<%ImplementsIParseErrorPosition%>", " Implements TinyPG.Debug.IParseError.Position");
				parsetree = parsetree.Replace(@"<%ImplementsIParseErrorLength%>", " Implements TinyPG.Debug.IParseError.Length");
				parsetree = parsetree.Replace(@"<%ImplementsIParseErrorMessage%>", " Implements TinyPG.Debug.IParseError.Message");
				parsetree = parsetree.Replace(@"<%ImplementsIParseErrorIsWarning%>", " Implements TinyPG.Debug.IParseError.IsWarning");
				string inodes = "		Public Shared Function Node2INode(ByVal node As ParseNode) As TinyPG.Debug.IParseNode\r\n"
									+ "			Return DirectCast(node, TinyPG.Debug.IParseNode)\r\n"
									+ "		End Function\r\n\r\n"
									+ "		Public ReadOnly Property INodes() As List(Of TinyPG.Debug.IParseNode) Implements TinyPG.Debug.IParseNode.INodes\r\n"
									+ "			Get\r\n"
									+ "				Return Nodes.ConvertAll(Of TinyPG.Debug.IParseNode)(New Converter(Of ParseNode, TinyPG.Debug.IParseNode)(AddressOf Node2INode))\r\n"
									+ "			End Get\r\n"
									+ "		End Property\r\n";
				parsetree = parsetree.Replace(@"<%INodesGet%>", inodes);
				parsetree = parsetree.Replace(@"<%ImplementsIParseNodeText%>", " Implements TinyPG.Debug.IParseNode.Text");
				parsetree = parsetree.Replace(@"<%ParseTreeCustomCode%>", Grammar.Directives["ParseTree"]["CustomCode"]);
			}
			else
			{
				parsetree = parsetree.Replace(@"<%Imports%>", "");
				//parsetree = parsetree.Replace(@"<%Namespace%>", Grammar.Directives["TinyPG"]["Namespace"]);
				parsetree = parsetree.Replace(@"<%ParseError%>", "");
				parsetree = parsetree.Replace(@"<%ParseErrors%>", "List(Of ParseError)");
				parsetree = parsetree.Replace(@"<%IParseTree%>", "");
				parsetree = parsetree.Replace(@"<%IParseNode%>", "");
				parsetree = parsetree.Replace(@"<%ITokenGet%>", "");
				parsetree = parsetree.Replace(@"<%INodesGet%>", "");

				parsetree = parsetree.Replace(@"<%ImplementsIParseTreePrintTree%>", "");
				parsetree = parsetree.Replace(@"<%ImplementsIParseTreeEval%>", "");
				parsetree = parsetree.Replace(@"<%ImplementsIParseErrorCode%>", "");
				parsetree = parsetree.Replace(@"<%ImplementsIParseErrorLine%>", "");
				parsetree = parsetree.Replace(@"<%ImplementsIParseErrorColumn%>", "");
				parsetree = parsetree.Replace(@"<%ImplementsIParseErrorPosition%>", "");
				parsetree = parsetree.Replace(@"<%ImplementsIParseErrorLength%>", "");
				parsetree = parsetree.Replace(@"<%ImplementsIParseErrorMessage%>", "");
				parsetree = parsetree.Replace(@"<%ImplementsIParseErrorIsWarning%>", "");
				parsetree = parsetree.Replace(@"<%ImplementsIParseNodeText%>", "");
				parsetree = parsetree.Replace(@"<%ParseTreeCustomCode%>", Grammar.Directives["ParseTree"]["CustomCode"]);
			}

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
