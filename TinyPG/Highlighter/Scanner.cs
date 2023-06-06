// Automatically generated from source file: GrammarHighlighter v1.5.tpg
// By TinyPG v1.5 available at https://github.com/ultrasuperpingu/TinyPG

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace TinyPG.Highlighter
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

	public Scanner()
		{
			Regex regex;
			Patterns = new Dictionary<TokenType, Regex>();
			Tokens = new List<TokenType>();
			LookAheadToken = null;
			Skipped = new List<Token>();

			SkipList = new List<TokenType>();
			SkipList.Add(TokenType.WHITESPACE);

			regex = new Regex(@"\s+", RegexOptions.None | RegexOptions.Compiled);
			Patterns.Add(TokenType.WHITESPACE, regex);
			Tokens.Add(TokenType.WHITESPACE);

			regex = new Regex(@"^$", RegexOptions.None | RegexOptions.Compiled);
			Patterns.Add(TokenType.EOF, regex);
			Tokens.Add(TokenType.EOF);

			regex = new Regex(@"//[^\n]*\n?", RegexOptions.None | RegexOptions.Compiled);
			Patterns.Add(TokenType.GRAMMARCOMMENTLINE, regex);
			Tokens.Add(TokenType.GRAMMARCOMMENTLINE);

			regex = new Regex(@"/\*([^*]+|\*[^/])+(\*/)?", RegexOptions.None | RegexOptions.Compiled);
			Patterns.Add(TokenType.GRAMMARCOMMENTBLOCK, regex);
			Tokens.Add(TokenType.GRAMMARCOMMENTBLOCK);

			regex = new Regex(@"@?\""(\""\""|[^\""])*(""|\n)", RegexOptions.None | RegexOptions.Compiled);
			Patterns.Add(TokenType.DIRECTIVESTRING, regex);
			Tokens.Add(TokenType.DIRECTIVESTRING);

			regex = new Regex(@"^(@TinyPG|@Parser|@Scanner|@Grammar|@ParseTree|@TextHighlighter|@Compile)", RegexOptions.None | RegexOptions.Compiled);
			Patterns.Add(TokenType.DIRECTIVEKEYWORD, regex);
			Tokens.Add(TokenType.DIRECTIVEKEYWORD);

			regex = new Regex(@"^(@|(%[^>])|=|"")+?", RegexOptions.None | RegexOptions.Compiled);
			Patterns.Add(TokenType.DIRECTIVESYMBOL, regex);
			Tokens.Add(TokenType.DIRECTIVESYMBOL);

			regex = new Regex(@"[^%@=""]+", RegexOptions.None | RegexOptions.Compiled);
			Patterns.Add(TokenType.DIRECTIVENONKEYWORD, regex);
			Tokens.Add(TokenType.DIRECTIVENONKEYWORD);

			regex = new Regex(@"<%", RegexOptions.None | RegexOptions.Compiled);
			Patterns.Add(TokenType.DIRECTIVEOPEN, regex);
			Tokens.Add(TokenType.DIRECTIVEOPEN);

			regex = new Regex(@"%>", RegexOptions.None | RegexOptions.Compiled);
			Patterns.Add(TokenType.DIRECTIVECLOSE, regex);
			Tokens.Add(TokenType.DIRECTIVECLOSE);

			regex = new Regex(@"[^\[\]]", RegexOptions.None | RegexOptions.Compiled);
			Patterns.Add(TokenType.ATTRIBUTESYMBOL, regex);
			Tokens.Add(TokenType.ATTRIBUTESYMBOL);

			regex = new Regex(@"^(Skip|Color|IgnoreCase|FileAndLine)", RegexOptions.None | RegexOptions.Compiled);
			Patterns.Add(TokenType.ATTRIBUTEKEYWORD, regex);
			Tokens.Add(TokenType.ATTRIBUTEKEYWORD);

			regex = new Regex(@"[^\(\)\]\n\s]+", RegexOptions.None | RegexOptions.Compiled);
			Patterns.Add(TokenType.ATTRIBUTENONKEYWORD, regex);
			Tokens.Add(TokenType.ATTRIBUTENONKEYWORD);

			regex = new Regex(@"\[\s*", RegexOptions.None | RegexOptions.Compiled);
			Patterns.Add(TokenType.ATTRIBUTEOPEN, regex);
			Tokens.Add(TokenType.ATTRIBUTEOPEN);

			regex = new Regex(@"\s*\]\s*", RegexOptions.None | RegexOptions.Compiled);
			Patterns.Add(TokenType.ATTRIBUTECLOSE, regex);
			Tokens.Add(TokenType.ATTRIBUTECLOSE);

			regex = new Regex(@"^(abstract|as|base|break|case|catch|checked|class|const|continue|decimal|default|delegate|double|do|else|enum|event|explicit|extern|false|finally|fixed|float|foreach|for|get|goto|if|implicit|interface|internal|int|in|is|lock|namespace|new|null|object|operator|out|override|params|partial|private|protected|public|readonly|ref|return|sealed|set|sizeof|stackalloc|static|struct|switch|throw|this|true|try|typeof|unchecked|unsafe|ushort|using|var|virtual|void|volatile|while)", RegexOptions.None | RegexOptions.Compiled);
			Patterns.Add(TokenType.CS_KEYWORD, regex);
			Tokens.Add(TokenType.CS_KEYWORD);

			regex = new Regex(@"^(AddHandler|AddressOf|Alias|AndAlso|And|Ansi|Assembly|As|Auto|Boolean|ByRef|Byte|ByVal|Call|Case|Catch|CBool|CByte|CChar|CDate|CDec|CDbl|Char|CInt|Class|CLng|CObj|Const|CShort|CSng|CStr|CType|Date|Decimal|Declare|Default|Delegate|Dim|DirectCast|Double|Do|Each|ElseIf|Else|End|Enum|Erase|Error|Event|Exit|False|Finally|For|Friend|Function|GetType|Get|GoSub|GoTo|Handles|If|Implements|Imports|Inherits|Integer|Interface|In|Is|Let|Lib|Like|Long|Loop|Me|Mod|Module|MustInherit|MustOverride|MyBase|MyClass|Namespace|New|Next|Nothing|NotInheritable|NotOverridable|Not|Object|On|Optional|Option|OrElse|Or|Overloads|Overridable|Overrides|ParamArray|Preserve|Private|Property|Protected|Public|RaiseEvent|ReadOnly|ReDim|REM|RemoveHandler|Resume|Return|Select|Set|Shadows|Shared|Short|Single|Static|Step|Stop|String|Structure|Sub|SyncLock|Then|Throw|To|True|Try|TypeOf|Unicode|Until|Variant|When|While|With|WithEvents|WriteOnly|Xor|Source)", RegexOptions.None | RegexOptions.Compiled);
			Patterns.Add(TokenType.VB_KEYWORD, regex);
			Tokens.Add(TokenType.VB_KEYWORD);

			regex = new Regex(@"^(abstract|as|base|break|case|catch|checked|class|const|continue|decimal|default|delegate|double|do|else|enum|event|explicit|extern|false|finally|fixed|float|foreach|for|get|goto|if|implicit|interface|internal|int|in|is|lock|namespace|new|null|object|operator|out|override|params|partial|private|protected|public|readonly|ref|return|sealed|set|sizeof|stackalloc|static|struct|switch|throw|this|true|try|typeof|unchecked|unsafe|ushort|using|var|virtual|void|volatile|while)", RegexOptions.None | RegexOptions.Compiled);
			Patterns.Add(TokenType.DOTNET_KEYWORD, regex);
			Tokens.Add(TokenType.DOTNET_KEYWORD);

			regex = new Regex(@"^(Array|AttributeTargets|AttributeUsageAttribute|Attribute|BitConverter|Boolean|Buffer|Byte|Char|CharEnumerator|CLSCompliantAttribute|ConsoleColor|ConsoleKey|ConsoleKeyInfo|ConsoleModifiers|ConsoleSpecialKey|Console|ContextBoundObject|ContextStaticAttribute|Converter|Convert|DateTimeKind|DateTimeOffset|DateTime|DayOfWeek|DBNull|Decimal|Delegate|Double|Enum|Environment.SpecialFolder|EnvironmentVariableTarget|Environment|EventArgs|EventHandler|Exception|FlagsAttribute|GCCollectionMode|GC|Guid|ICloneable|IComparable|IConvertible|ICustomFormatter|IDisposable|IEquatable|IFormatProvider|IFormattable|IndexOutOfRangeException|InsufficientMemoryException|Int16|Int32|Int64|IntPtr|InvalidCastException|InvalidOperationException|InvalidProgramException|MarshalByRefObject|Math|MidpointRounding|NotFiniteNumberException|NotImplementedException|NotSupportedException|Nullable|NullReferenceException|ObjectDisposedException|Object|ObsoleteAttribute|OperatingSystem|OutOfMemoryException|OverflowException|ParamArrayAttribute|PlatformID|PlatformNotSupportedException|Predicate|Random|SByte|SerializableAttribute|Single|StackOverflowException|StringComparer|StringComparison|StringSplitOptions|String|SystemException|TimeSpan|TimeZone|TypeCode|TypedReference|TypeInitializationException|Type|UInt16|UInt32|UInt64|UIntPtr|UnauthorizedAccessException|UnhandledExceptionEventArgs|UnhandledExceptionEventHandler|ValueType|Void|WeakReference|Comparer|Dictionary|EqualityComparer|ICollection|IComparer|IDictionary|IEnumerable|IEnumerator|IEqualityComparer|IList|KeyNotFoundException|KeyValuePair|List|ASCIIEncoding|Decoder|DecoderExceptionFallback|DecoderExceptionFallbackBuffer|DecoderFallback|DecoderFallbackBuffer|DecoderFallbackException|DecoderReplacementFallback|DecoderReplacementFallbackBuffer|EncoderExceptionFallback|EncoderExceptionFallbackBuffer|EncoderFallback|EncoderFallbackBuffer|EncoderFallbackException|EncoderReplacementFallback|EncoderReplacementFallbackBuffer|Encoder|EncodingInfo|Encoding|NormalizationForm|StringBuilder|UnicodeEncoding|UTF32Encoding|UTF7Encoding|UTF8Encoding)", RegexOptions.None | RegexOptions.Compiled);
			Patterns.Add(TokenType.DOTNET_TYPES, regex);
			Tokens.Add(TokenType.DOTNET_TYPES);

			regex = new Regex(@"//[^\n]*\n?", RegexOptions.None | RegexOptions.Compiled);
			Patterns.Add(TokenType.CS_COMMENTLINE, regex);
			Tokens.Add(TokenType.CS_COMMENTLINE);

			regex = new Regex(@"/\*([^*]+|\*[^/])+(\*/)?", RegexOptions.None | RegexOptions.Compiled);
			Patterns.Add(TokenType.CS_COMMENTBLOCK, regex);
			Tokens.Add(TokenType.CS_COMMENTBLOCK);

			regex = new Regex(@"[^}]", RegexOptions.None | RegexOptions.Compiled);
			Patterns.Add(TokenType.CS_SYMBOL, regex);
			Tokens.Add(TokenType.CS_SYMBOL);

			regex = new Regex(@"([^""\n\s/;.}\(\)\[\]]|/[^/*]|}[^;])+", RegexOptions.None | RegexOptions.Compiled);
			Patterns.Add(TokenType.CS_NONKEYWORD, regex);
			Tokens.Add(TokenType.CS_NONKEYWORD);

			regex = new Regex(@"@?[""]([""][""]|[^\""\n])*[""]?", RegexOptions.None | RegexOptions.Compiled);
			Patterns.Add(TokenType.CS_STRING, regex);
			Tokens.Add(TokenType.CS_STRING);

			regex = new Regex(@"'[^\n]*\n?", RegexOptions.None | RegexOptions.Compiled);
			Patterns.Add(TokenType.VB_COMMENTLINE, regex);
			Tokens.Add(TokenType.VB_COMMENTLINE);

			regex = new Regex(@"REM[^\n]*\n?", RegexOptions.None | RegexOptions.Compiled);
			Patterns.Add(TokenType.VB_COMMENTBLOCK, regex);
			Tokens.Add(TokenType.VB_COMMENTBLOCK);

			regex = new Regex(@"[^}]", RegexOptions.None | RegexOptions.Compiled);
			Patterns.Add(TokenType.VB_SYMBOL, regex);
			Tokens.Add(TokenType.VB_SYMBOL);

			regex = new Regex(@"([^""\n\s/;.}\(\)\[\]]|/[^/*]|}[^;])+", RegexOptions.None | RegexOptions.Compiled);
			Patterns.Add(TokenType.VB_NONKEYWORD, regex);
			Tokens.Add(TokenType.VB_NONKEYWORD);

			regex = new Regex(@"@?[""]([""][""]|[^\""\n])*[""]?", RegexOptions.None | RegexOptions.Compiled);
			Patterns.Add(TokenType.VB_STRING, regex);
			Tokens.Add(TokenType.VB_STRING);

			regex = new Regex(@"//[^\n]*\n?", RegexOptions.None | RegexOptions.Compiled);
			Patterns.Add(TokenType.DOTNET_COMMENTLINE, regex);
			Tokens.Add(TokenType.DOTNET_COMMENTLINE);

			regex = new Regex(@"/\*([^*]+|\*[^/])+(\*/)?", RegexOptions.None | RegexOptions.Compiled);
			Patterns.Add(TokenType.DOTNET_COMMENTBLOCK, regex);
			Tokens.Add(TokenType.DOTNET_COMMENTBLOCK);

			regex = new Regex(@"[^}]", RegexOptions.None | RegexOptions.Compiled);
			Patterns.Add(TokenType.DOTNET_SYMBOL, regex);
			Tokens.Add(TokenType.DOTNET_SYMBOL);

			regex = new Regex(@"([^""\n\s/;.}\[\]\(\)]|/[^/*]|}[^;])+", RegexOptions.None | RegexOptions.Compiled);
			Patterns.Add(TokenType.DOTNET_NONKEYWORD, regex);
			Tokens.Add(TokenType.DOTNET_NONKEYWORD);

			regex = new Regex(@"@?[""]([""][""]|[^\""\n])*[""]?", RegexOptions.None | RegexOptions.Compiled);
			Patterns.Add(TokenType.DOTNET_STRING, regex);
			Tokens.Add(TokenType.DOTNET_STRING);

			regex = new Regex(@"\{", RegexOptions.None | RegexOptions.Compiled);
			Patterns.Add(TokenType.CODEBLOCKOPEN, regex);
			Tokens.Add(TokenType.CODEBLOCKOPEN);

			regex = new Regex(@"\};", RegexOptions.None | RegexOptions.Compiled);
			Patterns.Add(TokenType.CODEBLOCKCLOSE, regex);
			Tokens.Add(TokenType.CODEBLOCKCLOSE);

			regex = new Regex(@"(Start)", RegexOptions.None | RegexOptions.Compiled);
			Patterns.Add(TokenType.GRAMMARKEYWORD, regex);
			Tokens.Add(TokenType.GRAMMARKEYWORD);

			regex = new Regex(@"->", RegexOptions.None | RegexOptions.Compiled);
			Patterns.Add(TokenType.GRAMMARARROW, regex);
			Tokens.Add(TokenType.GRAMMARARROW);

			regex = new Regex(@"[^{}\[\]/<>]|[</]$", RegexOptions.None | RegexOptions.Compiled);
			Patterns.Add(TokenType.GRAMMARSYMBOL, regex);
			Tokens.Add(TokenType.GRAMMARSYMBOL);

			regex = new Regex(@"([^;""\[\n\s/<{\(\)]|/[^/*]|<[^%])+", RegexOptions.None | RegexOptions.Compiled);
			Patterns.Add(TokenType.GRAMMARNONKEYWORD, regex);
			Tokens.Add(TokenType.GRAMMARNONKEYWORD);

			regex = new Regex(@"@?[""]([""][""]|[^\""\n])*[""]?", RegexOptions.None | RegexOptions.Compiled);
			Patterns.Add(TokenType.GRAMMARSTRING, regex);
			Tokens.Add(TokenType.GRAMMARSTRING);


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
					if (m.Success && m.Index == 0 && ((m.Length > len) || (scantokens[i] < index && m.Length == len)))
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
		CommentBlock      = 3,
		DirectiveBlock    = 4,
		GrammarBlock      = 5,
		AttributeBlock    = 6,
		CodeBlock         = 7,

			//Terminal tokens:
		WHITESPACE        = 8,
		EOF               = 9,
		GRAMMARCOMMENTLINE= 10,
		GRAMMARCOMMENTBLOCK= 11,
		DIRECTIVESTRING   = 12,
		DIRECTIVEKEYWORD  = 13,
		DIRECTIVESYMBOL   = 14,
		DIRECTIVENONKEYWORD= 15,
		DIRECTIVEOPEN     = 16,
		DIRECTIVECLOSE    = 17,
		ATTRIBUTESYMBOL   = 18,
		ATTRIBUTEKEYWORD  = 19,
		ATTRIBUTENONKEYWORD= 20,
		ATTRIBUTEOPEN     = 21,
		ATTRIBUTECLOSE    = 22,
		CS_KEYWORD        = 23,
		VB_KEYWORD        = 24,
		DOTNET_KEYWORD    = 25,
		DOTNET_TYPES      = 26,
		CS_COMMENTLINE    = 27,
		CS_COMMENTBLOCK   = 28,
		CS_SYMBOL         = 29,
		CS_NONKEYWORD     = 30,
		CS_STRING         = 31,
		VB_COMMENTLINE    = 32,
		VB_COMMENTBLOCK   = 33,
		VB_SYMBOL         = 34,
		VB_NONKEYWORD     = 35,
		VB_STRING         = 36,
		DOTNET_COMMENTLINE= 37,
		DOTNET_COMMENTBLOCK= 38,
		DOTNET_SYMBOL     = 39,
		DOTNET_NONKEYWORD = 40,
		DOTNET_STRING     = 41,
		CODEBLOCKOPEN     = 42,
		CODEBLOCKCLOSE    = 43,
		GRAMMARKEYWORD    = 44,
		GRAMMARARROW      = 45,
		GRAMMARSYMBOL     = 46,
		GRAMMARNONKEYWORD = 47,
		GRAMMARSTRING     = 48
	}

	public class Token
	{
		private string file;
		private int line;
		private int column;
		private int startpos;
		private int endpos;
		private string text;
		private object value; // is it really used??

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
