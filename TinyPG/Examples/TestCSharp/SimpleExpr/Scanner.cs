// Automatically generated from source file: simple expression2.tpg
// By TinyPG v1.6 available at https://github.com/ultrasuperpingu/TinyPG

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace SimpleExpr
{
	#region Scanner

	public partial class Scanner
	{
		public string Input;
		public int StartPos = 0;
		public int EndPos = 0;
		public int CurrentLine;
		public int CurrentColumn;
		public int CurrentPosition;
		public List<Token> Skipped; // tokens that were skipped
		public Dictionary<TokenType, Regex> Patterns;

		private Token LookAheadToken;
		private List<TokenType> Tokens;
		private List<TokenType> SkipList; // tokens to be skipped

	public Scanner()
		{
			Regex regex;
			Patterns = new Dictionary<TokenType, Regex>();
			Tokens = new List<TokenType>();
			LookAheadToken = null;
			Skipped = new List<Token>();

			SkipList = new List<TokenType>();
			SkipList.Add(TokenType.WHITESPACE);

			regex = new Regex(@"\G(?:\s*$)", RegexOptions.None | RegexOptions.Compiled);
			Patterns.Add(TokenType.EOF, regex);
			Tokens.Add(TokenType.EOF);

			regex = new Regex(@"\G(?:[0-9]+)", RegexOptions.None | RegexOptions.Compiled);
			Patterns.Add(TokenType.NUMBER, regex);
			Tokens.Add(TokenType.NUMBER);

			regex = new Regex(@"\G(?:[a-zA-Z_][a-zA-Z0-9_]*)", RegexOptions.None | RegexOptions.Compiled);
			Patterns.Add(TokenType.ID, regex);
			Tokens.Add(TokenType.ID);

			regex = new Regex(@"\G(?:\+|-)", RegexOptions.None | RegexOptions.Compiled);
			Patterns.Add(TokenType.PLUSMINUS, regex);
			Tokens.Add(TokenType.PLUSMINUS);

			regex = new Regex(@"\G(?:\*|/)", RegexOptions.None | RegexOptions.Compiled);
			Patterns.Add(TokenType.MULTDIV, regex);
			Tokens.Add(TokenType.MULTDIV);

			regex = new Regex(@"\G(?:\()", RegexOptions.None | RegexOptions.Compiled);
			Patterns.Add(TokenType.BROPEN, regex);
			Tokens.Add(TokenType.BROPEN);

			regex = new Regex(@"\G(?:\))", RegexOptions.None | RegexOptions.Compiled);
			Patterns.Add(TokenType.BRCLOSE, regex);
			Tokens.Add(TokenType.BRCLOSE);

			regex = new Regex(@"\G(?:\s+)", RegexOptions.None | RegexOptions.Compiled);
			Patterns.Add(TokenType.WHITESPACE, regex);
			Tokens.Add(TokenType.WHITESPACE);


		}

		public void Init(string input)
		{
			this.Input = input;
			StartPos = 0;
			EndPos = 0;
			CurrentLine = 1;
			CurrentColumn = 1;
			CurrentPosition = 0;
			LookAheadToken = null;
		}

		public Token GetToken(TokenType type)
		{
			Token t = new Token(this.StartPos, this.EndPos);
			t.Type = type;
			return t;
		}

			/// <summary>
		/// executes a lookahead of the next token
		/// and will advance the scan on the input string
		/// </summary>
		/// <returns></returns>
		public Token Scan(params TokenType[] expectedtokens)
		{
			Token tok = LookAhead(expectedtokens); // temporarely retrieve the lookahead
			LookAheadToken = null; // reset lookahead token, so scanning will continue
			StartPos = tok.EndPos;
			EndPos = tok.EndPos; // set the tokenizer to the new scan position
			CurrentLine = tok.Line + (tok.Text.Length - tok.Text.Replace("\n", "").Length);
			return tok;
		}

		/// <summary>
		/// returns token with longest best match
		/// </summary>
		/// <returns></returns>
		public Token LookAhead(params TokenType[] expectedtokens)
		{
			int i;
			int startpos = StartPos;
			int endpos = EndPos;
			int currentline = CurrentLine;
			Token tok = null;
			List<TokenType> scantokens;


			// this prevents double scanning and matching
			// increased performance
			// TODO: check this, what if the expected token are different since last call?
			// Check at least that LookAheadToken is part of the expected tokens
			if (LookAheadToken != null
				&& LookAheadToken.Type != TokenType._UNDETERMINED_
				&& LookAheadToken.Type != TokenType._NONE_)
			{
				return LookAheadToken;
			}

			// if no scantokens specified, then scan for all of them (= backward compatible)
			if (expectedtokens.Length == 0)
			{
				scantokens = Tokens;
			}
			else
			{
				scantokens = new List<TokenType>(expectedtokens);
				scantokens.AddRange(SkipList);
			}

			do
			{
				int len = -1;
				TokenType index = (TokenType)int.MaxValue;
				//string input = Input.Substring(startpos);

				tok = new Token(startpos, endpos);

				for (i = 0; i < scantokens.Count; i++)
				{
					Regex r = Patterns[scantokens[i]];
					Match m = r.Match(Input, startpos);
					if (m.Success && m.Index == startpos && ((m.Length > len) || (scantokens[i] < index && m.Length == len)))
					{
						len = m.Length;
						index = scantokens[i];
					}
				}

				if (index >= 0 && len >= 0)
				{
					tok.EndPos = startpos + len;
					tok.Text = Input.Substring(tok.StartPos, len);
					tok.Type = index;
				}
				else if (tok.StartPos == tok.EndPos)
				{
					if (tok.StartPos < Input.Length)
						tok.Text = Input.Substring(tok.StartPos, 1);
					else
						tok.Text = "EOF";
				}

				// Update the line and column count for error reporting.
				tok.Line = currentline;
				if (tok.StartPos < Input.Length)
					tok.Column = tok.StartPos - Input.LastIndexOf('\n', tok.StartPos);

				if (SkipList.Contains(tok.Type))
				{
					startpos = tok.EndPos;
					endpos = tok.EndPos;
					currentline = tok.Line + (tok.Text.Length - tok.Text.Replace("\n", "").Length);
					Skipped.Add(tok);
				}
				else
				{
					// only assign to non-skipped tokens
					tok.Skipped = Skipped; // assign prior skips to this token
					Skipped = new List<Token>(); //reset skips
				}
			}
			while (SkipList.Contains(tok.Type));

			LookAheadToken = tok;
			return tok;
		}
	}

	#endregion

	#region Token

	public enum TokenType
	{

		//Non terminal tokens:
		_NONE_            = 0,
		_UNDETERMINED_    = 1,

		//Non terminal tokens:
		Start             = 2,
		AddExpr           = 3,
		MultExpr          = 4,
		Atom              = 5,

			//Terminal tokens:
		EOF               = 6,
		NUMBER            = 7,
		ID                = 8,
		PLUSMINUS         = 9,
		MULTDIV           = 10,
		BROPEN            = 11,
		BRCLOSE           = 12,
		WHITESPACE        = 13
	}

	public class Token
	{
		private int line;
		private int column;
		private int startpos;
		private int endpos;
		private string text;

		// contains all prior skipped symbols
		private List<Token> skipped;

		public int Line { 
			get { return line; } 
			set { line = value; }
		}

		public int Column {
			get { return column; } 
			set { column = value; }
		}

		public int StartPos { 
			get { return startpos;} 
			set { startpos = value; }
		}

		public int Length { 
			get { return endpos - startpos;} 
		}

		public int EndPos { 
			get { return endpos;} 
			set { endpos = value; }
		}

		public string Text { 
			get { return text;} 
			set { text = value; }
		}

		public List<Token> Skipped { 
			get { return skipped;} 
			set { skipped = value; }
		}
		
		[XmlAttribute]
		public TokenType Type;

		public Token() : this(0, 0)
		{
		}

		public Token(int start, int end)
		{
			Type = TokenType._UNDETERMINED_;
			startpos = start;
			endpos = end;
			Text = "";
		}

		public void UpdateRange(Token token)
		{
			if (token.StartPos < startpos) startpos = token.StartPos;
			if (token.EndPos > endpos) endpos = token.EndPos;
		}

		public override string ToString()
		{
			if (Text != null)
				return Type.ToString() + " '" + Text + "'";
			else
				return Type.ToString();
		}

	}

	#endregion
}
