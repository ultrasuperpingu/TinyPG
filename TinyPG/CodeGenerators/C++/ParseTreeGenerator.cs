﻿using System.Text;
using System.IO;
using TinyPG.Compiler;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Security.Cryptography;
using System;

namespace TinyPG.CodeGenerators.Cpp
{
	public class ParseTreeGenerator : BaseGenerator, ICodeGenerator
	{
		internal ParseTreeGenerator()
			: base("ParseTree.h")
		{
		}

		public string Generate(Grammar Grammar, GenerateDebugMode Debug)
		{
			if (Debug != GenerateDebugMode.None)
				throw new Exception("Cpp cannot be generated in debug mode");
			if (string.IsNullOrEmpty(Grammar.GetTemplatePath()))
				return null;

			// copy the parse tree file (optionally)
			string parsetree = File.ReadAllText(Grammar.GetTemplatePath() + templateName);

			StringBuilder evalsymbols = new StringBuilder();
			StringBuilder evalmethods = new StringBuilder();

			foreach (TerminalSymbol s in Grammar.GetTerminals())
			{
				/*evalmethods.AppendLine("		inline virtual bool IsToken" + s.Name + "Present(int index)");
				evalmethods.AppendLine("		{");
				evalmethods.AppendLine("			if (index < 0) return false;");
				evalmethods.AppendLine("			// left to right");
				evalmethods.AppendLine("			for (ParseNode* node : Nodes)");
				evalmethods.AppendLine("			{");
				evalmethods.AppendLine("				if (node->TokenVal.Type == TokenType::"+s.Name+")");
				evalmethods.AppendLine("			    {");
				evalmethods.AppendLine("			        index--;");
				evalmethods.AppendLine("			        if (index < 0)");
				evalmethods.AppendLine("			        {");
				evalmethods.AppendLine("						return true;");
				evalmethods.AppendLine("			        }");
				evalmethods.AppendLine("			    }");
				evalmethods.AppendLine("			}");
				evalmethods.AppendLine("			return false;");
				evalmethods.AppendLine("		}\r\n");*/

				string returnType = "std::string";
				evalmethods.AppendLine("		inline virtual " + returnType + " Get" + s.Name + "Value(const ParseTree& tree, int index)");
				evalmethods.AppendLine("		{");
				evalmethods.AppendLine("			" + returnType + " o = \"\";");
				evalmethods.AppendLine("			if (index < 0) return o;");
				evalmethods.AppendLine("			// left to right");
				evalmethods.AppendLine("			for (ParseNode* node : Nodes)");
				evalmethods.AppendLine("			{");
				evalmethods.AppendLine("				if (node->TokenVal.Type == TokenType::"+s.Name+")");
				evalmethods.AppendLine("				{");
				evalmethods.AppendLine("					index--;");
				evalmethods.AppendLine("					if (index < 0)");
				evalmethods.AppendLine("					{");
				evalmethods.AppendLine("						o =  node->TokenVal.Text;");
				evalmethods.AppendLine("						break;");
				evalmethods.AppendLine("					}");
				evalmethods.AppendLine("				}");
				evalmethods.AppendLine("			}");
				evalmethods.AppendLine("			return o;");
				evalmethods.AppendLine("		}\r\n");
			}

			// build non terminal tokens
			foreach (NonTerminalSymbol s in Grammar.GetNonTerminals())
			{
				evalsymbols.AppendLine("				case TokenType::" + s.Name + ":");
				evalsymbols.AppendLine("					Value = Eval" + s.Name + "(tree, paramlist);");
				//evalsymbols.AppendLine("					Value = Token.Text;");
				evalsymbols.AppendLine("					break;");

				string returnType = "void*";
				if (!string.IsNullOrEmpty(s.ReturnType))
					returnType = s.ReturnType;
				string defaultReturnValue = "NULL";
				if (!string.IsNullOrEmpty(s.ReturnTypeDefault))
					defaultReturnValue = s.ReturnTypeDefault;
				evalmethods.AppendLine("		inline virtual " + returnType + " Eval" + s.Name + "(const ParseTree& tree, std::vector<void*> paramlist)");
				evalmethods.AppendLine("		{");
				if (s.CodeBlock != null)
				{
					// paste user code here
					evalmethods.AppendLine(FormatCodeBlock(s));
				}
				else
				{
					if (s.Name == "Start") // return a nice warning message from root object.
						evalmethods.AppendLine("			return "+defaultReturnValue+"; //\"Could not interpret input; no semantics implemented.\";");
					else
						evalmethods.AppendLine("			for (ParseNode* node : Nodes)\r\n" +
											   "				node->Eval(tree, paramlist);\r\n" +
											   "			return "+defaultReturnValue+";");

					// otherwise simply not implemented!
				}
				evalmethods.AppendLine("		}\r\n");
				
				
				evalmethods.AppendLine("		inline virtual " + returnType + " Get" + s.Name + "Value(const ParseTree& tree, int index)");
				evalmethods.AppendLine("		{");
				evalmethods.AppendLine("			" + returnType + " o = "+defaultReturnValue+";");
				evalmethods.AppendLine("			if (index < 0) return o;");
				evalmethods.AppendLine("			// left to right");
				evalmethods.AppendLine("			for (ParseNode* node : Nodes)");
				evalmethods.AppendLine("			{");
				evalmethods.AppendLine("				if (node->TokenVal.Type == TokenType::"+s.Name+")");
				evalmethods.AppendLine("				{");
				evalmethods.AppendLine("					index--;");
				evalmethods.AppendLine("					if (index < 0)");
				evalmethods.AppendLine("					{");
				evalmethods.AppendLine("						o = node->Eval"+s.Name+"(tree, { });");
				evalmethods.AppendLine("						break;");
				evalmethods.AppendLine("					}");
				evalmethods.AppendLine("				}");
				evalmethods.AppendLine("			}");
				evalmethods.AppendLine("			return o;");
				evalmethods.AppendLine("		}\r\n");
			}

			parsetree = parsetree.Replace(@"<%SourceFilename%>", Grammar.SourceFilename);
			parsetree = parsetree.Replace(@"<%Namespace%>", Grammar.Directives["TinyPG"]["Namespace"]);
			parsetree = parsetree.Replace(@"<%ParseError%>", "");
			parsetree = parsetree.Replace(@"<%ParseErrors%>", "std::vector<ParseError>");
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
				if(eval)
					replacement = "this->Get"+s.Name+"Value(tree, " + indexer + ")";
				else
					replacement = "this->IsTokenPresent(TokenType::" + s.Name + ", " + indexer + ")";
				codeblock = codeblock.Substring(0, match.Captures[0].Index) + replacement + codeblock.Substring(match.Captures[0].Index + match.Captures[0].Length);
				match = var.Match(codeblock);
			}

			codeblock = Helper.Indent3 + codeblock.Replace("\n", "\r\n" + Helper.Indent2);
			return codeblock;
		}
	}

}
