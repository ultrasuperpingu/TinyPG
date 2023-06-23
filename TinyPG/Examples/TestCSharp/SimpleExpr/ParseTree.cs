// Automatically generated from source file: simple expression2.tpg
// By TinyPG v1.5 available at https://github.com/ultrasuperpingu/TinyPG


using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Globalization;
using System.Linq;


namespace SimpleExpr
{
	#region ParseTree
	[Serializable]
	public class ParseErrors : List<ParseError>
	{
		public bool ContainsErrors
		{
			get { return Find(e => e.IsWarning == false) != null; }
		}
		public bool ContainsWarnings
	{
			get { return Find(e => e.IsWarning == true) != null; }
		}
		public List<ParseError> Warnings
		{
			get { return FindAll(e => e.IsWarning == true); }
		}
		public List<ParseError> BlockingErrors
		{
			get { return FindAll(e => e.IsWarning == false); }
		}
	}

	[Serializable]
	public class ParseError
	{
		private string message;
		private int code;
		private int line;
		private int col;
		private int pos;
		private int length;
		private bool isWarning;

		public int Code { get { return code; } }
		public int Line { get { return line; } }
		public int Column { get { return col; } }
		public int Position { get { return pos; } }
		public int Length { get { return length; } }
		public string Message { get { return message; } }
		public bool IsWarning { get { return isWarning; } }

		// just for the sake of serialization
		public ParseError()
		{
		}

		public ParseError(string message, int code, ParseNode node, bool isWarning = false) : this(message, code, node.Token)
		{
		}

		public ParseError(string message, int code, Token token, bool isWarning = false) : this(message, code, token.Line, token.Column, token.StartPos, token.Length, isWarning)
		{
		}

		public ParseError(string message, int code, bool isWarning = false) : this(message, code, 0, 0, 0, 0, isWarning)
		{
		}

		public ParseError(string message, int code, int line, int col, int pos, int length, bool isWarning = false)
		{
			this.message = message;
			this.code = code;
			this.line = line;
			this.col = col;
			this.pos = pos;
			this.length = length;
			this.isWarning = isWarning;
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
			return Nodes[0].EvalNode(this, paramlist);
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

		protected virtual ParseNode GetTokenNode(TokenType type, int index)
		{
			if (index < 0)
				return null;
			// left to right
			foreach (ParseNode node in nodes)
			{
				if (node.Token.Type == type)
				{
					index--;
					if (index < 0)
					{
						return node;
					}
				}
			}
			return null;
		}

		protected virtual bool IsTokenPresent(TokenType type, int index)
		{
			ParseNode node = GetTokenNode(type, index);
			return node != null;
		}

		protected virtual string GetTerminalValue(TokenType type, int index)
		{
			ParseNode node = GetTokenNode(type, index);
			if (node != null)
				return node.Token.Text;
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
						o = node.EvalNode(paramlist);
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
		internal object EvalNode(params object[] paramlist)
		{
			object Value = null;

			switch (Token.Type)
			{
				case TokenType.Start:
					Value = EvalStart(paramlist);
				break;
				case TokenType.AddExpr:
					Value = EvalAddExpr(paramlist);
				break;
				case TokenType.MultExpr:
					Value = EvalMultExpr(paramlist);
				break;
				case TokenType.Atom:
					Value = EvalAtom(paramlist);
				break;

				default:
					Value = Token.Text;
					break;
			}
			return Value;
		}

		protected virtual int EvalStart(params object[] paramlist)
		{
			return this.GetAddExprValue(0, paramlist);
		}

		protected virtual int GetStartValue(int index, params object[] paramlist )
		{
			int o = default(int);
			ParseNode node = GetTokenNode(TokenType.Start, index);
			if (node != null)
				o = node.EvalStart(paramlist);
			return o;
		}

		protected virtual int EvalAddExpr(params object[] paramlist)
		{
			int Value = this.GetMultExprValue(0, paramlist);
	int i = 1;
	while (this.IsTokenPresent(TokenType.MultExpr, i))
	{
		string sign = this.GetTerminalValue(TokenType.PLUSMINUS, i-1);
		if (sign == "+")
			Value += this.GetMultExprValue(i++, paramlist);
		else 
			Value -= this.GetMultExprValue(i++, paramlist);
	}

	return Value;
		}

		protected virtual int GetAddExprValue(int index, params object[] paramlist )
		{
			int o = default(int);
			ParseNode node = GetTokenNode(TokenType.AddExpr, index);
			if (node != null)
				o = node.EvalAddExpr(paramlist);
			return o;
		}

		protected virtual int EvalMultExpr(params object[] paramlist)
		{
			int Value = this.GetAtomValue(0, paramlist);
	int i = 1;
	while (this.IsTokenPresent(TokenType.Atom, i))
	{
		string sign = this.GetTerminalValue(TokenType.MULTDIV, i-1);
		if (sign == "*")
			Value *= this.GetAtomValue(i++, paramlist);
		else 
			Value /= this.GetAtomValue(i++, paramlist);
	}
	return Value;
		}

		protected virtual int GetMultExprValue(int index, params object[] paramlist )
		{
			int o = default(int);
			ParseNode node = GetTokenNode(TokenType.MultExpr, index);
			if (node != null)
				o = node.EvalMultExpr(paramlist);
			return o;
		}

		protected virtual int EvalAtom(params object[] paramlist)
		{
			if (this.IsTokenPresent(TokenType.NUMBER, 0))
		return Convert.ToInt32(this.GetTerminalValue(TokenType.NUMBER, 0));
	if (this.IsTokenPresent(TokenType.ID, 0))
		return GetVarValue(this.GetTerminalValue(TokenType.ID, 0));
	return this.GetAddExprValue(0, paramlist);
		}

		protected virtual int GetAtomValue(int index, params object[] paramlist )
		{
			int o = default(int);
			ParseNode node = GetTokenNode(TokenType.Atom, index);
			if (node != null)
				o = node.EvalAtom(paramlist);
			return o;
		}




		protected System.Collections.Generic.Dictionary<string,int> context;
		public System.Collections.Generic.Dictionary<string,int> Context
		{
			get
			{
				if(context == null && this.Parent != null) {
					return Parent.Context;
				}
				return context;
			}
			set
			{
				context = value;
			}
		}

		public int GetVarValue(string id)
		{
			return Context == null?0:Context[id];
		}

	}
	#endregion ParseTree
}
