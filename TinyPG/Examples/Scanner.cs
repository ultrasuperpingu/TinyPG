// Automatically generated from source file: C:\Users\Ping\Documents\TinyPG\TinyPG\Examples\BNFGrammar 1.5.tpg
// By TinyPG v1.5 available at https://github.com/ultrasuperpingu/TinyPG

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace TinyPG
{
	#region Scanner

	public partial class Scanner
	{
		public string Input;
		public int StartPos = 0;
		public int EndPos = 0;
		public string CurrentFile;
		public int CurrentLine;
		public int CurrentColumn;
		public int CurrentPosition;
		public List<Token> Skipped; // tokens that were skipped
		public Dictionary<TokenType, Regex> Patterns;
   
		private Token LookAheadToken;
		private List<TokenType> Tokens;
		private List<TokenType> SkipList; // tokens to be skipped
		private readonly TokenType FileAndLine = TokenType._NONE_;

    public Scanner()
		{
			Regex regex;
			Patterns = new Dictionary<TokenType, Regex>();
			Tokens = new List<TokenType>();
			LookAheadToken = null;
			Skipped = new List<Token>();

			SkipList = new List<TokenType>();
            SkipList.Add(TokenType.WHITESPACE);
            SkipList.Add(TokenType.COMMENTLINE);
            SkipList.Add(TokenType.COMMENTBLOCK);

            regex = new Regex(@"\(", RegexOptions.None | RegexOptions.Compiled);
            Patterns.Add(TokenType.BRACKETOPEN, regex);
            Tokens.Add(TokenType.BRACKETOPEN);

            regex = new Regex(@"\)", RegexOptions.None | RegexOptions.Compiled);
            Patterns.Add(TokenType.BRACKETCLOSE, regex);
            Tokens.Add(TokenType.BRACKETCLOSE);

            regex = new Regex(@"\{[^\}]*\}([^};][^}]*\}+)*;", RegexOptions.None | RegexOptions.Compiled);
            Patterns.Add(TokenType.CODEBLOCK, regex);
            Tokens.Add(TokenType.CODEBLOCK);

            regex = new Regex(@",", RegexOptions.None | RegexOptions.Compiled);
            Patterns.Add(TokenType.COMMA, regex);
            Tokens.Add(TokenType.COMMA);

            regex = new Regex(@"\[", RegexOptions.None | RegexOptions.Compiled);
            Patterns.Add(TokenType.SQUAREOPEN, regex);
            Tokens.Add(TokenType.SQUAREOPEN);

            regex = new Regex(@"\]", RegexOptions.None | RegexOptions.Compiled);
            Patterns.Add(TokenType.SQUARECLOSE, regex);
            Tokens.Add(TokenType.SQUARECLOSE);

            regex = new Regex(@"=", RegexOptions.None | RegexOptions.Compiled);
            Patterns.Add(TokenType.ASSIGN, regex);
            Tokens.Add(TokenType.ASSIGN);

            regex = new Regex(@"\|", RegexOptions.None | RegexOptions.Compiled);
            Patterns.Add(TokenType.PIPE, regex);
            Tokens.Add(TokenType.PIPE);

            regex = new Regex(@";", RegexOptions.None | RegexOptions.Compiled);
            Patterns.Add(TokenType.SEMICOLON, regex);
            Tokens.Add(TokenType.SEMICOLON);

            regex = new Regex(@":", RegexOptions.None | RegexOptions.Compiled);
            Patterns.Add(TokenType.COLON, regex);
            Tokens.Add(TokenType.COLON);

            regex = new Regex(@"(\*|\+|\?)", RegexOptions.None | RegexOptions.Compiled);
            Patterns.Add(TokenType.UNARYOPER, regex);
            Tokens.Add(TokenType.UNARYOPER);

            regex = new Regex(@"[a-zA-Z_][a-zA-Z0-9_]*", RegexOptions.None | RegexOptions.Compiled);
            Patterns.Add(TokenType.IDENTIFIER, regex);
            Tokens.Add(TokenType.IDENTIFIER);

            regex = new Regex(@"[a-zA-Z_][a-zA-Z0-9_]*(::[a-zA-Z_][a-zA-Z0-9_]*)*(<[a-zA-Z_][a-zA-Z0-9_]*(::[a-zA-Z_][a-zA-Z0-9_]*)*>)?", RegexOptions.None | RegexOptions.Compiled);
            Patterns.Add(TokenType.TYPE, regex);
            Tokens.Add(TokenType.TYPE);

            regex = new Regex(@"[0-9]+", RegexOptions.None | RegexOptions.Compiled);
            Patterns.Add(TokenType.INTEGER, regex);
            Tokens.Add(TokenType.INTEGER);

            regex = new Regex(@"[0-9]*\.[0-9]+", RegexOptions.None | RegexOptions.Compiled);
            Patterns.Add(TokenType.DOUBLE, regex);
            Tokens.Add(TokenType.DOUBLE);

            regex = new Regex(@"(0x[0-9a-fA-F]{6})", RegexOptions.None | RegexOptions.Compiled);
            Patterns.Add(TokenType.HEX, regex);
            Tokens.Add(TokenType.HEX);

            regex = new Regex(@"->", RegexOptions.None | RegexOptions.Compiled);
            Patterns.Add(TokenType.ARROW, regex);
            Tokens.Add(TokenType.ARROW);

            regex = new Regex(@"<%\s*@", RegexOptions.None | RegexOptions.Compiled);
            Patterns.Add(TokenType.DIRECTIVEOPEN, regex);
            Tokens.Add(TokenType.DIRECTIVEOPEN);

            regex = new Regex(@"%>", RegexOptions.None | RegexOptions.Compiled);
            Patterns.Add(TokenType.DIRECTIVECLOSE, regex);
            Tokens.Add(TokenType.DIRECTIVECLOSE);

            regex = new Regex(@"^$", RegexOptions.None | RegexOptions.Compiled);
            Patterns.Add(TokenType.EOF, regex);
            Tokens.Add(TokenType.EOF);

            regex = new Regex(@"@?\""(\""\""|[^\""])*\""", RegexOptions.None | RegexOptions.Compiled);
            Patterns.Add(TokenType.STRING, regex);
            Tokens.Add(TokenType.STRING);

            regex = new Regex(@"\s+", RegexOptions.None | RegexOptions.Compiled);
            Patterns.Add(TokenType.WHITESPACE, regex);
            Tokens.Add(TokenType.WHITESPACE);

            regex = new Regex(@"//[^\n]*\n?", RegexOptions.None | RegexOptions.Compiled);
            Patterns.Add(TokenType.COMMENTLINE, regex);
            Tokens.Add(TokenType.COMMENTLINE);

            regex = new Regex(@"/\*[^*]*\*+(?:[^/*][^*]*\*+)*/", RegexOptions.None | RegexOptions.Compiled);
            Patterns.Add(TokenType.COMMENTBLOCK, regex);
            Tokens.Add(TokenType.COMMENTBLOCK);


		}

		public void Init(string input)
		{
			Init(input, "");
		}

		public void Init(string input, string fileName)
		{
			this.Input = input;
			StartPos = 0;
			EndPos = 0;
			CurrentFile = fileName;
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
			CurrentFile = tok.File;
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
			string currentFile = CurrentFile;
			Token tok = null;
			List<TokenType> scantokens;


			// this prevents double scanning and matching
			// increased performance
			if (LookAheadToken != null 
				&& LookAheadToken.Type != TokenType._UNDETERMINED_ 
				&& LookAheadToken.Type != TokenType._NONE_) return LookAheadToken;

			// if no scantokens specified, then scan for all of them (= backward compatible)
			if (expectedtokens.Length == 0)
				scantokens = Tokens;
			else
			{
				scantokens = new List<TokenType>(expectedtokens);
				scantokens.AddRange(SkipList);
			}

			do
			{

				int len = -1;
				TokenType index = (TokenType)int.MaxValue;
				string input = Input.Substring(startpos);

				tok = new Token(startpos, endpos);

				for (i = 0; i < scantokens.Count; i++)
				{
					Regex r = Patterns[scantokens[i]];
					Match m = r.Match(input);
					if (m.Success && m.Index == 0 && ((m.Length > len) || (scantokens[i] < index && m.Length == len )))
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
				tok.File = currentFile;
				tok.Line = currentline;
				if (tok.StartPos < Input.Length)
					tok.Column = tok.StartPos - Input.LastIndexOf('\n', tok.StartPos);

				if (SkipList.Contains(tok.Type))
				{
					startpos = tok.EndPos;
					endpos = tok.EndPos;
					currentline = tok.Line + (tok.Text.Length - tok.Text.Replace("\n", "").Length);
					currentFile = tok.File;
					Skipped.Add(tok);
				}
				else
				{
					// only assign to non-skipped tokens
					tok.Skipped = Skipped; // assign prior skips to this token
					Skipped = new List<Token>(); //reset skips
				}

				// Check to see if the parsed token wants to 
				// alter the file and line number.
				if (tok.Type == FileAndLine)
				{
					Match match = Patterns[tok.Type].Match(tok.Text);
					Group fileMatch = match.Groups["File"];
					if (fileMatch.Success)
						currentFile = fileMatch.Value.Replace("\\\\", "\\");
					Group lineMatch = match.Groups["Line"];
					if (lineMatch.Success)
						currentline = int.Parse(lineMatch.Value, NumberStyles.Integer, CultureInfo.InvariantCulture);
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
            _NONE_  = 0,
            _UNDETERMINED_= 1,

            //Non terminal tokens:
            Start   = 2,
            Directive= 3,
            NameValue= 4,
            ExtProduction= 5,
            Attribute= 6,
            Params  = 7,
            Param   = 8,
            Production= 9,
            Rule    = 10,
            Subrule = 11,
            ConcatRule= 12,
            Symbol  = 13,

            //Terminal tokens:
            BRACKETOPEN= 14,
            BRACKETCLOSE= 15,
            CODEBLOCK= 16,
            COMMA   = 17,
            SQUAREOPEN= 18,
            SQUARECLOSE= 19,
            ASSIGN  = 20,
            PIPE    = 21,
            SEMICOLON= 22,
            COLON   = 23,
            UNARYOPER= 24,
            IDENTIFIER= 25,
            TYPE    = 26,
            INTEGER = 27,
            DOUBLE  = 28,
            HEX     = 29,
            ARROW   = 30,
            DIRECTIVEOPEN= 31,
            DIRECTIVECLOSE= 32,
            EOF     = 33,
            STRING  = 34,
            WHITESPACE= 35,
            COMMENTLINE= 36,
            COMMENTBLOCK= 37
	}

	public class Token
	{
		private string file;
		private int line;
		private int column;
		private int startpos;
		private int endpos;
		private string text;
		private object value;

		// contains all prior skipped symbols
		private List<Token> skipped;

		public string File { 
			get { return file; } 
			set { file = value; }
		}

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
		public object Value { 
			get { return value;} 
			set { this.value = value; }
		}

		[XmlAttribute]
		public TokenType Type;

		public Token()
			: this(0, 0)
		{
		}

		public Token(int start, int end)
		{
			Type = TokenType._UNDETERMINED_;
			startpos = start;
			endpos = end;
			Text = ""; // must initialize with empty string, may cause null reference exceptions otherwise
			Value = null;
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
