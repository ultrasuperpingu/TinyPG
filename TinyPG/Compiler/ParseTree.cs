// Automatically generated from source file: BNFGrammar 1.5.tpg
// By TinyPG v1.5 available at https://github.com/ultrasuperpingu/TinyPG


using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Globalization;
using System.Linq;

namespace TinyPG
{
	#region ParseTree
	[Serializable]
	public class ParseErrors : List<ParseError>
	{
	}

	[Serializable]
	public class ParseError
	{
		private string file;
		private string message;
		private int code;
		private int line;
		private int col;
		private int pos;
		private int length;

		public string File { get { return file; } }
		public int Code { get { return code; } }
		public int Line { get { return line; } }
		public int Column { get { return col; } }
		public int Position { get { return pos; } }
		public int Length { get { return length; } }
		public string Message { get { return message; } }

		// just for the sake of serialization
		public ParseError()
		{
		}

		public ParseError(string message, int code, ParseNode node) : this(message, code, node.Token)
		{
		}

		public ParseError(string message, int code, Token token) : this(message, code, token.File, token.Line, token.Column, token.StartPos, token.Length)
		{
		}

		public ParseError(string message, int code) : this(message, code, string.Empty, 0, 0, 0, 0)
		{
		}

		public ParseError(string message, int code, string file, int line, int col, int pos, int length)
		{
			this.file = file;
			this.message = message;
			this.code = code;
			this.line = line;
			this.col = col;
			this.pos = pos;
			this.length = length;
		}
	}

	// rootlevel of the node tree
	[Serializable]
	public partial class ParseTree : ParseNode
	{
		public ParseErrors Errors;

		public List<Token> Skipped;

		public ParseTree() : base(new Token(), "ParseTree")
		{
			Token.Type = TokenType.Start;
			Token.Text = "Root";
			Errors = new ParseErrors();
		}

		public string PrintTree()
		{
			StringBuilder sb = new StringBuilder();
			int indent = 0;
			PrintNode(sb, this, indent);
			return sb.ToString();
		}

		private void PrintNode(StringBuilder sb, ParseNode node, int indent)
		{
			string space = "".PadLeft(indent, ' ');

			sb.Append(space);
			sb.AppendLine(node.Text);

			foreach (ParseNode n in node.Nodes)
				PrintNode(sb, n, indent + 2);
		}

		/// <summary>
		/// this is the entry point for executing and evaluating the parse tree.
		/// </summary>
		/// <param name="paramlist">additional optional input parameters</param>
		/// <returns>the output of the evaluation function</returns>
		public object Eval(params object[] paramlist)
		{
			return Nodes[0].Eval(this, paramlist);
		}
	}

	[Serializable]
	[XmlInclude(typeof(ParseTree))]
	public partial class ParseNode
	{
		protected string text;
		protected List<ParseNode> nodes;
		
		public List<ParseNode> Nodes { get {return nodes;} }
		
		[XmlIgnore] // avoid circular references when serializing
		public ParseNode Parent;
		public Token Token; // the token/rule

		[XmlIgnore] // skip redundant text (is part of Token)
		public string Text { // text to display in parse tree 
			get { return text;} 
			set { text = value; }
		} 

		public virtual ParseNode CreateNode(Token token, string text)
		{
			ParseNode node = new ParseNode(token, text);
			node.Parent = this;
			return node;
		}

		protected ParseNode(Token token, string text)
		{
			this.Token = token;
			this.text = text;
			this.nodes = new List<ParseNode>();
		}
		protected virtual bool IsTokenPresent(TokenType type, int index)
		{
			if (index < 0) return false;
			// left to right
			foreach (ParseNode node in nodes)
			{
				if (node.Token.Type == type)
				{
					index--;
					if (index < 0)
					{
						return true;
					}
				}
			}
			return false;
		}
		protected virtual string GetTerminalValue(TokenType type, int index)
		{
			if (index < 0)
				return "";
			// left to right
			foreach (ParseNode node in nodes)
			{
				if (node.Token.Type == type)
				{
					index--;
					if (index < 0)
					{
						return node.Token.Text;
					}
				}
			}
			return null;
		}
		protected object GetValue(TokenType type, int index, params object[] paramlist)
		{
			return GetValue(type, ref index, paramlist);
		}

		protected object GetValue(TokenType type, ref int index, params object[] paramlist)
		{
			object o = null;
			if (index < 0) return o;

			// left to right
			foreach (ParseNode node in nodes)
			{
				if (node.Token.Type == type)
				{
					index--;
					if (index < 0)
					{
						o = node.Eval(paramlist);
						break;
					}
				}
			}
			return o;
		}

		/// <summary>
		/// this implements the evaluation functionality, cannot be used directly
		/// </summary>
		/// <param name="tree">the parsetree itself</param>
		/// <param name="paramlist">optional input parameters</param>
		/// <returns>a partial result of the evaluation</returns>
		internal object Eval(params object[] paramlist)
		{
			object Value = null;

			switch (Token.Type)
			{
				case TokenType.Start:
					Value = EvalStart(paramlist);
				break;
				case TokenType.Directive:
					Value = EvalDirective(paramlist);
				break;
				case TokenType.NameValue:
					Value = EvalNameValue(paramlist);
				break;
				case TokenType.ExtProduction:
					Value = EvalExtProduction(paramlist);
				break;
				case TokenType.Attribute:
					Value = EvalAttribute(paramlist);
				break;
				case TokenType.Params:
					Value = EvalParams(paramlist);
				break;
				case TokenType.Param:
					Value = EvalParam(paramlist);
				break;
				case TokenType.Production:
					Value = EvalProduction(paramlist);
				break;
				case TokenType.Rule:
					Value = EvalRule(paramlist);
				break;
				case TokenType.Subrule:
					Value = EvalSubrule(paramlist);
				break;
				case TokenType.ConcatRule:
					Value = EvalConcatRule(paramlist);
				break;
				case TokenType.Symbol:
					Value = EvalSymbol(paramlist);
				break;

				default:
					Value = Token.Text;
					break;
			}
			return Value;
		}

		protected virtual object EvalStart(params object[] paramlist)
		{
			return default(object); //"Could not interpret input; no semantics implemented.";
		}

		protected virtual object GetStartValue(int index)
		{
			object o = default(object);
			if (index < 0)
				return o;
			// left to right
			foreach (ParseNode node in nodes)
			{
				if (node.Token.Type == TokenType.Start)
				{
					index--;
					if (index < 0)
					{
						o = node.EvalStart();
						break;
					}
				}
			}
			return o;
		}

		protected virtual object EvalDirective(params object[] paramlist)
		{
			foreach (ParseNode node in Nodes)
				node.Eval(paramlist);
			return default(object);
		}

		protected virtual object GetDirectiveValue(int index)
		{
			object o = default(object);
			if (index < 0)
				return o;
			// left to right
			foreach (ParseNode node in nodes)
			{
				if (node.Token.Type == TokenType.Directive)
				{
					index--;
					if (index < 0)
					{
						o = node.EvalDirective();
						break;
					}
				}
			}
			return o;
		}

		protected virtual object EvalNameValue(params object[] paramlist)
		{
			foreach (ParseNode node in Nodes)
				node.Eval(paramlist);
			return default(object);
		}

		protected virtual object GetNameValueValue(int index)
		{
			object o = default(object);
			if (index < 0)
				return o;
			// left to right
			foreach (ParseNode node in nodes)
			{
				if (node.Token.Type == TokenType.NameValue)
				{
					index--;
					if (index < 0)
					{
						o = node.EvalNameValue();
						break;
					}
				}
			}
			return o;
		}

		protected virtual object EvalExtProduction(params object[] paramlist)
		{
			foreach (ParseNode node in Nodes)
				node.Eval(paramlist);
			return default(object);
		}

		protected virtual object GetExtProductionValue(int index)
		{
			object o = default(object);
			if (index < 0)
				return o;
			// left to right
			foreach (ParseNode node in nodes)
			{
				if (node.Token.Type == TokenType.ExtProduction)
				{
					index--;
					if (index < 0)
					{
						o = node.EvalExtProduction();
						break;
					}
				}
			}
			return o;
		}

		protected virtual object EvalAttribute(params object[] paramlist)
		{
			foreach (ParseNode node in Nodes)
				node.Eval(paramlist);
			return default(object);
		}

		protected virtual object GetAttributeValue(int index)
		{
			object o = default(object);
			if (index < 0)
				return o;
			// left to right
			foreach (ParseNode node in nodes)
			{
				if (node.Token.Type == TokenType.Attribute)
				{
					index--;
					if (index < 0)
					{
						o = node.EvalAttribute();
						break;
					}
				}
			}
			return o;
		}

		protected virtual object EvalParams(params object[] paramlist)
		{
			foreach (ParseNode node in Nodes)
				node.Eval(paramlist);
			return default(object);
		}

		protected virtual object GetParamsValue(int index)
		{
			object o = default(object);
			if (index < 0)
				return o;
			// left to right
			foreach (ParseNode node in nodes)
			{
				if (node.Token.Type == TokenType.Params)
				{
					index--;
					if (index < 0)
					{
						o = node.EvalParams();
						break;
					}
				}
			}
			return o;
		}

		protected virtual object EvalParam(params object[] paramlist)
		{
			foreach (ParseNode node in Nodes)
				node.Eval(paramlist);
			return default(object);
		}

		protected virtual object GetParamValue(int index)
		{
			object o = default(object);
			if (index < 0)
				return o;
			// left to right
			foreach (ParseNode node in nodes)
			{
				if (node.Token.Type == TokenType.Param)
				{
					index--;
					if (index < 0)
					{
						o = node.EvalParam();
						break;
					}
				}
			}
			return o;
		}

		protected virtual object EvalProduction(params object[] paramlist)
		{
			foreach (ParseNode node in Nodes)
				node.Eval(paramlist);
			return default(object);
		}

		protected virtual object GetProductionValue(int index)
		{
			object o = default(object);
			if (index < 0)
				return o;
			// left to right
			foreach (ParseNode node in nodes)
			{
				if (node.Token.Type == TokenType.Production)
				{
					index--;
					if (index < 0)
					{
						o = node.EvalProduction();
						break;
					}
				}
			}
			return o;
		}

		protected virtual object EvalRule(params object[] paramlist)
		{
			foreach (ParseNode node in Nodes)
				node.Eval(paramlist);
			return default(object);
		}

		protected virtual object GetRuleValue(int index)
		{
			object o = default(object);
			if (index < 0)
				return o;
			// left to right
			foreach (ParseNode node in nodes)
			{
				if (node.Token.Type == TokenType.Rule)
				{
					index--;
					if (index < 0)
					{
						o = node.EvalRule();
						break;
					}
				}
			}
			return o;
		}

		protected virtual object EvalSubrule(params object[] paramlist)
		{
			foreach (ParseNode node in Nodes)
				node.Eval(paramlist);
			return default(object);
		}

		protected virtual object GetSubruleValue(int index)
		{
			object o = default(object);
			if (index < 0)
				return o;
			// left to right
			foreach (ParseNode node in nodes)
			{
				if (node.Token.Type == TokenType.Subrule)
				{
					index--;
					if (index < 0)
					{
						o = node.EvalSubrule();
						break;
					}
				}
			}
			return o;
		}

		protected virtual object EvalConcatRule(params object[] paramlist)
		{
			foreach (ParseNode node in Nodes)
				node.Eval(paramlist);
			return default(object);
		}

		protected virtual object GetConcatRuleValue(int index)
		{
			object o = default(object);
			if (index < 0)
				return o;
			// left to right
			foreach (ParseNode node in nodes)
			{
				if (node.Token.Type == TokenType.ConcatRule)
				{
					index--;
					if (index < 0)
					{
						o = node.EvalConcatRule();
						break;
					}
				}
			}
			return o;
		}

		protected virtual object EvalSymbol(params object[] paramlist)
		{
			foreach (ParseNode node in Nodes)
				node.Eval(paramlist);
			return default(object);
		}

		protected virtual object GetSymbolValue(int index)
		{
			object o = default(object);
			if (index < 0)
				return o;
			// left to right
			foreach (ParseNode node in nodes)
			{
				if (node.Token.Type == TokenType.Symbol)
				{
					index--;
					if (index < 0)
					{
						o = node.EvalSymbol();
						break;
					}
				}
			}
			return o;
		}




	}
	#endregion ParseTree
}
