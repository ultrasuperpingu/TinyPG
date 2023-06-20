// Automatically generated from source file: <%SourceFilename%>
// By TinyPG v1.5 available at https://github.com/ultrasuperpingu/TinyPG


using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Globalization;
using System.Linq;
<%HeaderCode%>

namespace <%Namespace%>
{
	#region ParseTree
	[Serializable]
	public class ParseErrors : <%ParseErrors%>
	{
		public bool ContainsErrors
		{
			get { return Find(e => e.IsWarning == false) != null; }
		}
		public bool ContainsWarnings
	{
			get { return Find(e => e.IsWarning == true) != null; }
		}
		public <%ParseErrors%> Warnings
		{
			get { return FindAll(e => e.IsWarning == true); }
		}
		public <%ParseErrors%> BlockingErrors
		{
			get { return FindAll(e => e.IsWarning == false); }
		}
	}

	[Serializable]
	public class ParseError<%ParseError%>
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
	public partial class ParseTree : ParseNode<%IParseTree%>
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
	public partial class ParseNode<%IParseNode%>
	{
		protected string text;
		protected List<ParseNode> nodes;
		<%ITokenGet%>
		public List<ParseNode> Nodes { get {return nodes;} }
		<%INodesGet%>
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
<%EvalSymbols%>
				default:
					Value = Token.Text;
					break;
			}
			return Value;
		}

<%VirtualEvalMethods%>

<%CustomCode%>
	}
	#endregion ParseTree
}
