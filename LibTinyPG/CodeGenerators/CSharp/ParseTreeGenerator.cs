﻿using System;
using System.Text;
using System.IO;
using TinyPG.Parsing;
using System.Text.RegularExpressions;
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
			Dictionary<string, string> templateFilesPath = GetTemplateFilesPath(Grammar, "ParseTree");
			isDebugOther = Debug == GenerateDebugMode.DebugOther;

			StringBuilder evalsymbols = new StringBuilder();
			StringBuilder evalmethods = new StringBuilder();

			// build non terminal tokens
			foreach (NonTerminalSymbol s in Grammar.GetNonTerminals())
			{
				evalsymbols.AppendLine("				case TokenType." + s.Name + ":");
				evalsymbols.AppendLine("					Value = Eval" + s.Name + "(paramlist);");
				evalsymbols.AppendLine("				break;");

				string returnType = "object";
				if (!string.IsNullOrEmpty(s.ReturnType) && !isDebugOther)
					returnType = s.ReturnType;
				string defaultReturnValue = "default("+returnType+")";
				if (!string.IsNullOrEmpty(s.ReturnTypeDefault) && !isDebugOther)
					defaultReturnValue = s.ReturnTypeDefault;
				if (s.Attributes.ContainsKey("Comment"))
				{
					evalmethods.AppendLine(GenerateComment(s.Attributes["Comment"], Helper.Indent2));
				}
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
				evalmethods.AppendLine("		}");
				evalmethods.AppendLine();
				evalmethods.AppendLine("		protected virtual " + returnType + " Get" + s.Name + "Value(int index, params object[] paramlist )");
				evalmethods.AppendLine("		{");
				evalmethods.AppendLine("			" + returnType + " o = "+defaultReturnValue+";");
				evalmethods.AppendLine("			ParseNode node = GetTokenNode(TokenType." + s.Name + ", index);");
				evalmethods.AppendLine("			if (node != null)");
				evalmethods.AppendLine("				o = node.Eval"+s.Name+"(paramlist);");
				evalmethods.AppendLine("			return o;");
				evalmethods.AppendLine("		}");
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
					fileContent = fileContent.Replace(@"<%IParseTree%>", "");
					fileContent = fileContent.Replace(@"<%IParseNode%>", "");
					fileContent = fileContent.Replace(@"<%ITokenGet%>", "");
					fileContent = fileContent.Replace(@"<%INodesGet%>", "");
					fileContent = fileContent.Replace(@"<%CustomCode%>", Grammar.Directives["ParseTree"]["CustomCode"]);
				}

				fileContent = fileContent.Replace(@"<%EvalSymbols%>", evalsymbols.ToString());
				fileContent = fileContent.Replace(@"<%VirtualEvalMethods%>", evalmethods.ToString());
				fileContent = ReplaceDirectiveAttributes(fileContent, Grammar.Directives["ParseTree"]);
				generated[entry.Key] = fileContent;
			}
			return generated;
		}

		private Dictionary<string, string> GetTemplateFilesPath(Grammar Grammar, string directiveName)
		{
			Dictionary<string, string> _templateFiles = new Dictionary<string, string>();
			string templatePath = Grammar.GetTemplatePath();
			if (string.IsNullOrEmpty(templatePath))
				throw new Exception("Template path not found:" + Grammar.Directives["TinyPG"]["TemplatePath"]);
			List<string> files;
			if (Grammar.Directives["ParseTree"].ContainsKey("TemplateFiles"))
			{
				var templateFilesString = Grammar.Directives[directiveName]["TemplateFiles"];
				files = new List<string>(templateFilesString.Split(','));
				for (int i = 0; i < files.Count; i++)
				{
					if (string.IsNullOrWhiteSpace(files[i]))
					{
						files.RemoveAt(i);
						i--;
						continue;
					}
					files[i] = files[i].Trim();
				}
			}
			else
			{
				files=templateFiles;
			}
			for (int i = 0; i < files.Count; i++)
			{
				var f = Path.Combine(templatePath, files[i]);
				if (!File.Exists(f))
				{
					throw new Exception("Template file "+files[i]+" does not exist.");
				}
				_templateFiles.Add(files[i], f);
			}

			return _templateFiles;
		}

		protected string GenerateComment(object[] objects, string Indent)
		{
			StringBuilder sb = new StringBuilder();
			foreach(var o in objects)
			{
				sb.Append(Indent).Append("/// ").AppendLine(Helper.LiteralToUnescaped(o.ToString()));
			}
			return sb.ToString().TrimEnd();
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

			codeblock = Helper.Indent3 + codeblock.Replace(Environment.NewLine, Environment.NewLine + Helper.Indent2);
			return codeblock;
		}
	}

}
