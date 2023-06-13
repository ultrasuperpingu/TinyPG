// Copyright 2008 - 2010 Herre Kuijpers - <herre.kuijpers@gmail.com>
//
// This source file(s) may be redistributed, altered and customized
// by any means PROVIDING the authors name and all copyright
// notices remain intact.
// THIS SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED. USE IT AT YOUR OWN RISK. THE AUTHOR ACCEPTS NO
// LIABILITY FOR ANY DATA DAMAGE/LOSS THAT THIS PRODUCT MAY CAUSE.
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;
using TinyPG.Highlighter;

namespace TinyPG.Compiler
{

	public class GrammarTree : GrammarNode
	{
		public GrammarTree()
		{
		}
	}


	/// <summary>
	/// this class implements the semantics of the parsetree
	/// it will create a Grammar with production rules
	/// </summary>
	public partial class GrammarNode : ParseTree
	{

		public override ParseNode CreateNode(Token token, string text)
		{
			GrammarNode node = new GrammarNode(token, text);
			node.Parent = this;
			return node;
		}

		protected GrammarNode()
		{
		}

		protected GrammarNode(Token token, string text)
		{
			Token = token;
			this.text = text;
			nodes = new List<ParseNode>();
		}

		/// <summary>
		/// EvalStart will first do a semantic check to see if symbols are declared correctly
		/// then it will also check for attributes and parse the directives
		/// after that it will complete the transformation to the grammar tree.
		/// </summary>
		/// <param name="tree"></param>
		/// <param name="paramlist"></param>
		/// <returns></returns>
		protected override object EvalStart(params object[] paramlist)
		{
			TerminalSymbol terminal = null;
			bool StartFound = false;
			Grammar g = new Grammar();
			ParseTree tree = Parent as ParseTree;
			foreach (ParseNode n in Nodes)
			{
				if (n.Token.Type == TokenType.Directive)
				{
					EvalDirective(new object[] { tree, g, n });
				}
				if (n.Token.Type == TokenType.ExtProduction)
				{

					if (n.Nodes[n.Nodes.Count - 1].Nodes[2].Nodes[0].Token.Type == TokenType.STRING)
					{
						try
						{
							terminal = new TerminalSymbol(n.Nodes[n.Nodes.Count - 1].Nodes[0].Token.Text, (string)n.Nodes[n.Nodes.Count - 1].Nodes[2].Nodes[0].Token.Text);
							for (int i = 0; i < n.Nodes.Count - 1; i++)
							{
								if (n.Nodes[i].Token.Type == TokenType.Attribute)
									EvalAttribute(new object[] { tree, g, terminal, n.Nodes[i] });
							}

						}
						catch (Exception ex)
						{
							tree.Errors.Add(new ParseError("regular expression for '" + n.Nodes[n.Nodes.Count - 1].Nodes[0].Token.Text + "' results in error: " + ex.Message, 0x1020, n.Nodes[0]));
							continue;
						}

						if (terminal.Name == "Start")
							tree.Errors.Add(new ParseError("'Start' symbol cannot be a regular expression.", 0x1021, n.Nodes[0]));

						if (g.Symbols.Find(terminal.Name) == null)
							g.Symbols.Add(terminal);
						else
							tree.Errors.Add(new ParseError("Terminal already declared: " + terminal.Name, 0x1022, n.Nodes[0]));

					}
					else
					{
						NonTerminalSymbol nts = new NonTerminalSymbol(n.Nodes[n.Nodes.Count - 1].Nodes[0].Token.Text);
						if (g.Symbols.Find(nts.Name) == null)
							g.Symbols.Add(nts);
						else
							tree.Errors.Add(new ParseError("Non terminal already declared: " + nts.Name, 0x1023, n.Nodes[0]));

						for (int i = 0; i < n.Nodes.Count - 1; i++)
						{
							if (n.Nodes[i].Token.Type == TokenType.Attribute)
								EvalAttribute(new object[] { tree, g, nts, n.Nodes[i] });
						}

						if (nts.Name == "Start")
							StartFound = true;
					}
				}
			}

			if (!StartFound)
			{
				tree.Errors.Add(new ParseError("The grammar requires 'Start' to be a production rule.", 0x0024));
				return g;
			}

			foreach (ParseNode n in Nodes)
			{
				if (n.Token.Type == TokenType.ExtProduction)
				{
					n.EvalNode(tree, g);
				}
			}

			return g;
		}


		protected override object EvalDirective(params object[] paramlist)
		{
			ParseTree tree = (ParseTree)paramlist[0];
			Grammar g = (Grammar)paramlist[1];
			GrammarNode node = (GrammarNode)paramlist[2];
			string name = node.Nodes[1].Token.Text;

			switch (name)
			{
				case "TinyPG":
				case "Parser":
				case "Scanner":
				case "ParseTree":
				case "TextHighlighter":
					if (name == "TinyPG" && g.Directives.Count > 0)
					{
						tree.Errors.Add(new ParseError("Directive '" + name + "' should be the first directive declared", 0x1030, node.Nodes[1], true));
					}
					if (g.Directives.Find(name) != null)
					{
						tree.Errors.Add(new ParseError("Directive '" + name + "' is already defined", 0x1030, node.Nodes[1], true));
						return null;
					}
					if (name == "TextHighlighter")
					{
						var decl = g.Directives.Find("TinyPG");
						if (decl != null && decl.ContainsKey("Language"))
						{
							var lang=CodeGenerators.CodeGeneratorFactory.GetSupportedLanguage(decl["Language"]);
							if (lang != CodeGenerators.SupportedLanguage.CSharp && lang != CodeGenerators.SupportedLanguage.VBNet)
							{
								tree.Errors.Add(new ParseError("Directive '" + name + "' is not supported with language " + decl["Language"], 0x1030, node.Nodes[1], true));
								return null;
							}
						}
					}
					break;
				case "Compile":
					{
						var decl = g.Directives.Find("TinyPG");
						if (decl != null && decl.ContainsKey("Language"))
						{
							var lang = CodeGenerators.CodeGeneratorFactory.GetSupportedLanguage(decl["Language"]);
							if (lang != CodeGenerators.SupportedLanguage.CSharp && lang != CodeGenerators.SupportedLanguage.VBNet)
							{
								tree.Errors.Add(new ParseError("Directive '" + name + "' is not supported with language " + decl["Language"], 0x1030, node.Nodes[1], true));
								return null;
							}
						}
					}
					break;
				default:
					tree.Errors.Add(new ParseError("Directive '" + name + "' is not supported", 0x1031, node.Nodes[1], true));
					break;
			}

			Directive directive = new Directive(name);
			g.Directives.Add(directive);

			foreach (ParseNode n in node.Nodes)
			{
				if (n.Token.Type == TokenType.NameValue)
					EvalNameValue(new object[] { tree, g, directive, n });
			}

			return null;
		}

		protected override object EvalNameValue(params object[] paramlist)
		{
			ParseTree tree = (ParseTree)paramlist[0];
			Grammar grammar = (Grammar)paramlist[1];
			Directive directive = (Directive)paramlist[2];
			GrammarNode node = (GrammarNode)paramlist[3];

			string key = node.Nodes[0].Token.Text;
			
			string value = node.Nodes[2].Token.Text;
			if (node.Nodes[2].Token.Type == TokenType.STRING)
			{
				value = value.UnescapeVerbatim().FixNewLines();
			}
			else
			{
				value = value.UnescapeVerbatim().FixNewLines();
				value = value.Substring(0, value.Length-1);
			}

			directive[key] = value;

			List<string> names = new List<string>(new string[] { "Namespace", "OutputPath", "TemplatePath", });
			List<string> languages = new List<string>(new string[] { "c#", "cs", "csharp", "vb", "vb.net", "vbnet", "visualbasic", "java", "cpp", "c++" });
			switch (directive.Name)
			{
				case "TinyPG":
					names.Add("Namespace");
					names.Add("OutputPath");
					names.Add("TemplatePath");
					names.Add("Language");
					names.Add("RegexCompiled");

					if (key == "TemplatePath")
						if (grammar.GetTemplatePath() == null)
							tree.Errors.Add(new ParseError("Template path '" + value + "' does not exist", 0x1060, node.Nodes[2]));

					if (key == "OutputPath")
						if (grammar.GetOutputPath() == null)
							tree.Errors.Add(new ParseError("Output path '" + value + "' does not exist", 0x1061, node.Nodes[2]));

					if (key == "Language")
						if (!languages.Contains(value.ToLower(CultureInfo.InvariantCulture)))
							tree.Errors.Add(new ParseError("Language '" + value + "' is not supported", 0x1062, node.Nodes[2]));
					break;
				case "Parser":
				case "Scanner":
				case "ParseTree":
				case "TextHighlighter":
					names.Add("Generate");
					names.Add("FileName");
					names.Add("CustomCode");
					names.Add("HeaderCode");
					break;
				case "Compile":
					names.Add("FileName");
					break;
				default:
					// Unknow Directive already reported
					return null;
			}

			if (!names.Contains(key))
			{
				tree.Errors.Add(new ParseError("Directive attribute '" + key + "' is not supported", 0x1034, node.Nodes[0], true));
			}
			return null;
		}

		protected override object EvalExtProduction(params object[] paramlist)
		{
			return Nodes[Nodes.Count - 1].EvalNode(paramlist);
		}

		protected override object EvalAttribute(params object[] paramlist)
		{
			ParseTree tree = (ParseTree)paramlist[0];
			Grammar grammar = (Grammar)paramlist[1];
			Symbol symbol = (Symbol)paramlist[2];
			GrammarNode node = (GrammarNode)paramlist[3];

			if (symbol.Attributes.ContainsKey(node.Nodes[1].Token.Text))
			{
				tree.Errors.Add(new ParseError("Attribute already defined for this symbol: " + node.Nodes[1].Token.Text, 0x1039, node.Nodes[1], true));
				return null;
			}

			symbol.Attributes.Add(node.Nodes[1].Token.Text, (object[])EvalParams(new object[] { tree, node }));
			switch (node.Nodes[1].Token.Text)
			{
				case "Skip":
					if (symbol is TerminalSymbol)
						grammar.SkipSymbols.Add(symbol);
					else
						tree.Errors.Add(new ParseError("Attribute Skip for non-terminal rule not allowed: " + node.Nodes[1].Token.Text, 0x1035, node, true));
					break;
				case "Color":
					if (symbol is NonTerminalSymbol)
						tree.Errors.Add(new ParseError("Attribute Color for non-terminal rule not allowed: " + node.Nodes[1].Token.Text, 0x1035, node, true));

					if (symbol.Attributes["Color"] == null || symbol.Attributes["Color"].Length != 1 && symbol.Attributes["Color"].Length != 3)
						tree.Errors.Add(new ParseError("Attribute " + node.Nodes[1].Token.Text + " has too many or missing parameters", 0x103A, node.Nodes[1], true));

					for (int i = 0; symbol.Attributes["Color"] != null && i < symbol.Attributes["Color"].Length; i++)
					{
						if (symbol.Attributes["Color"][i] is string)
						{
							tree.Errors.Add(new ParseError("Parameter " + node.Nodes[3].Nodes[i * 2].Nodes[0].Token.Text + " is of incorrect type", 0x103A, node.Nodes[3].Nodes[i * 2].Nodes[0], true));
							break;
						}
					}
					break;
				case "IgnoreCase":
					if (!(symbol is TerminalSymbol))
						tree.Errors.Add(new ParseError("Attribute IgnoreCase for non-terminal rule not allowed: " + node.Nodes[1].Token.Text, 0x1035, node, true));
					break;
				default:
					tree.Errors.Add(new ParseError("Attribute not supported: " + node.Nodes[1].Token.Text, 0x1036, node.Nodes[1], true));
					break;
			}

			return symbol;
		}

		protected override object EvalParams(params object[] paramlist)
		{
			ParseTree tree = (ParseTree)paramlist[0];
			GrammarNode node = (GrammarNode)paramlist[1];
			if (node.Nodes.Count < 4) return null;
			if (node.Nodes[3].Token.Type != TokenType.Params) return null;

			GrammarNode parms = (GrammarNode)node.Nodes[3];
			List<object> objects = new List<object>();
			for (int i = 0; i < parms.Nodes.Count; i += 2)
			{
				objects.Add(EvalParam(new object[] { tree, parms.Nodes[i] }));
			}
			return objects.ToArray();
		}

		protected override object EvalParam(params object[] paramlist)
		{
			ParseTree tree = (ParseTree)paramlist[0];
			GrammarNode node = (GrammarNode)paramlist[1];
			try
			{
				switch (node.Nodes[0].Token.Type)
				{
					case TokenType.STRING:
						return node.Nodes[0].Token.Text;
					case TokenType.INTEGER:
						return Convert.ToInt32(node.Nodes[0].Token.Text);
					case TokenType.HEX:
						return long.Parse(node.Nodes[0].Token.Text.Substring(2), NumberStyles.HexNumber);
					default:
						tree.Errors.Add(new ParseError("Attribute parameter is not a valid value: " + node.Token.Text, 0x1037, node));
						break;
				}
			}
			catch (Exception)
			{
				tree.Errors.Add(new ParseError("Attribute parameter is not a valid value: " + node.Token.Text, 0x1038, node, true));
			}
			return null;
		}

		protected override object EvalProduction(params object[] paramlist)
		{
			ParseTree tree = (ParseTree)paramlist[0];
			Grammar g = (Grammar)paramlist[1];
			if (Nodes[2].Nodes[0].Token.Type == TokenType.STRING)
			{
				TerminalSymbol term = g.Symbols.Find(Nodes[0].Token.Text) as TerminalSymbol;
				if (term == null)
					tree.Errors.Add(new ParseError("Symbol '" + Nodes[0].Token.Text + "' is not declared. ", 0x1040, Nodes[0]));
				if (Nodes[3].Token.Type == TokenType.COLON)
					tree.Errors.Add(new ParseError("Terminal Symbol '" + Nodes[0].Token.Text + "' cannot declare a return type. ", 0x1040, Nodes[0]));

				if (Nodes[3].Token.Type == TokenType.CODEBLOCK || Nodes.Count > 5 && Nodes[5].Token.Type == TokenType.CODEBLOCK|| Nodes.Count > 9 && Nodes[9].Token.Type == TokenType.CODEBLOCK)
					tree.Errors.Add(new ParseError("Terminal Symbol '" + Nodes[0].Token.Text + "' cannot declare a code block. ", 0x1040, Nodes[0]));
			}
			else
			{
				NonTerminalSymbol nts = g.Symbols.Find(Nodes[0].Token.Text) as NonTerminalSymbol;
				if (nts == null)
					tree.Errors.Add(new ParseError("Symbol '" + Nodes[0].Token.Text + "' is not declared. ", 0x1041, Nodes[0]));
				Rule r = (Rule)Nodes[2].EvalNode(tree, g, nts);
				if (nts != null)
					nts.Rules.Add(r);

				if (Nodes[3].Token.Type == TokenType.COLON)
				{
					nts.ReturnType = Nodes[4].Token.Text.ToString();
					if (Nodes[5].Token.Type == TokenType.DEFAULT)
					{
						nts.ReturnTypeDefault = Nodes[7].Token.Text.ToString();
					}
				}


				if (Nodes[3].Token.Type == TokenType.CODEBLOCK || Nodes.Count > 5 && Nodes[5].Token.Type == TokenType.CODEBLOCK|| Nodes.Count > 9 && Nodes[9].Token.Type == TokenType.CODEBLOCK)
				{
					string codeblock = null;
					if (Nodes[3].Token.Type == TokenType.CODEBLOCK)
						codeblock = Nodes[3].Token.Text;
					else if(Nodes[5].Token.Type == TokenType.CODEBLOCK)
						codeblock = Nodes[5].Token.Text;
					else if (Nodes[9].Token.Type == TokenType.CODEBLOCK)
						codeblock = Nodes[9].Token.Text;
					nts.CodeBlock = codeblock;
					ValidateCodeBlock(tree, nts, Nodes[3]);

					// beautify the codeblock format
					codeblock = codeblock.Substring(1, codeblock.Length - 3).Trim();
					nts.CodeBlock = codeblock;
				}
			}
			return g;
		}

		protected override object EvalRule(params object[] paramlist)
		{
			return Nodes[0].EvalNode(paramlist);
		}

		protected override object EvalSubrule(params object[] paramlist)
		{
			if (Nodes.Count == 1) // single symbol
				return Nodes[0].EvalNode(paramlist);

			Rule choiceRule = new Rule(RuleType.Choice);
			// i+=2 to skip to the | symbols
			for (int i = 0; i < Nodes.Count; i += 2)
			{
				Rule rule = (Rule)Nodes[i].EvalNode(paramlist);
				choiceRule.Rules.Add(rule);
			}

			return choiceRule;
		}

		protected override object EvalConcatRule(params object[] paramlist)
		{
			if (Nodes.Count == 1) // single symbol
				return Nodes[0].EvalNode(paramlist);

			Rule concatRule = new Rule(RuleType.Concat);
			for (int i = 0; i < Nodes.Count; i++)
			{
				Rule rule = (Rule)Nodes[i].EvalNode(paramlist);
				concatRule.Rules.Add(rule);
			}

			return concatRule;
		}


		protected override object EvalSymbol(params object[] paramlist)
		{
			ParseNode last = Nodes[Nodes.Count - 1];
			if (last.Token.Type == TokenType.UNARYOPER)
			{
				Rule unaryRule;
				string oper = last.Token.Text.Trim();
				if (oper == "*")
					unaryRule = new Rule(RuleType.ZeroOrMore);
				else if (oper == "+")
					unaryRule = new Rule(RuleType.OneOrMore);
				else if (oper == "?")
					unaryRule = new Rule(RuleType.Option);
				else
					throw new NotImplementedException("Internal error: unknown unary operator " + oper);

				if (Nodes[0].Token.Type == TokenType.BRACKETOPEN)
				{
					Rule rule = (Rule)Nodes[1].EvalNode(paramlist);
					unaryRule.Rules.Add(rule);
				}
				else
				{
					ParseTree tree = (ParseTree)paramlist[0];
					Grammar g = (Grammar)paramlist[1];
					if (Nodes[0].Token.Type == TokenType.IDENTIFIER)
					{

						Symbol s = g.Symbols.Find(Nodes[0].Token.Text);
						if (s == null)
						{
							tree.Errors.Add(new ParseError("Symbol '" + Nodes[0].Token.Text + "' is not declared. ", 0x1042, Nodes[0]));
						}
						Rule r = new Rule(s);
						unaryRule.Rules.Add(r);
					}
				}
				return unaryRule;
			}

			if (Nodes[0].Token.Type == TokenType.BRACKETOPEN)
			{
				// create subrule syntax tree
				return Nodes[1].EvalNode(paramlist);
			}
			else
			{
				ParseTree tree = (ParseTree)paramlist[0];
				Grammar g = (Grammar)paramlist[1];
				Symbol s = (Symbol)g.Symbols.Find(Nodes[0].Token.Text);
				if (s == null)
				{
					tree.Errors.Add(new ParseError("Symbol '" + Nodes[0].Token.Text + "' is not declared.", 0x1043, Nodes[0]));
				}
				return new Rule(s);
			}
		}

		/// <summary>
		/// validates whether $ variables are corresponding to valid symbols
		/// errors are added to the tree Error object.
		/// </summary>
		/// <param name="nts">non terminal and its production rule</param>
		/// <returns>a formated codeblock</returns>
		private void ValidateCodeBlock(ParseTree tree, NonTerminalSymbol nts, ParseNode node)
		{
			// TODO: Check if this validation is really needed. An invalid Code block will result
			//       in code which doesn't compile but is it to the parser to check the code
			//       provided by the user...
			if (nts == null)
				return;
			string codeblock = nts.CodeBlock;

			Regex var = new Regex(@"\$(?<var>[a-zA-Z_0-9]+)(\[(?<index>[^]]+)\])?", RegexOptions.Compiled);

			Symbols symbols = nts.DetermineProductionSymbols();


			MatchCollection matches = var.Matches(codeblock);
			foreach (Match match in matches)
			{
				Symbol s = symbols.Find(match.Groups["var"].Value);
				if (s == null)
				{
					tree.Errors.Add(new ParseError("Variable $" + match.Groups["var"].Value + " cannot be matched.", 0x1016, node.Token.StartPos + match.Groups["var"].Index, node.Token.StartPos + match.Groups["var"].Index, node.Token.StartPos + match.Groups["var"].Index, match.Groups["var"].Length, true));
					//break; // error situation
					// just a warning, the code will probably fail to compile, but generation should work
				}
			}
		}
	}
}
