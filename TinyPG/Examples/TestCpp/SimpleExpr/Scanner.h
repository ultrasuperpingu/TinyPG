// Automatically generated from source file: simple expression2_cpp.tpg
// By TinyPG v1.5 available at https://github.com/ultrasuperpingu/TinyPG

#pragma once
#include <string>
#include <vector>
#include <map>
#include <regex>

namespace TinyPG
{
	// Check if vector contains an element
	template <typename T>
	bool contains(
		const std::vector<T>& vecObj,
		const T& element)
	{
		// Get the iterator of first occurrence
		// of given element in vector
		auto it = std::find(
			vecObj.begin(),
			vecObj.end(),
			element);
		return it != vecObj.end();
	}
	inline std::string replace(const std::string& str, const std::string& from, const std::string& to) {
		size_t start_pos = str.find(from);
		if (start_pos == std::string::npos)
			return str;
		std::string val = str;
		val.replace(start_pos, from.length(), to);
		return val;
	}

	enum TokenType
	{

			//Non terminal tokens:
			_NONE_           = 0,
			_UNDETERMINED_   = 1,

			//Non terminal tokens:
			Start            = 2,
			AddExpr          = 3,
			MultExpr         = 4,
			Atom             = 5,

			//Terminal tokens:
			EOF_             = 6,
			NUMBER           = 7,
			ID               = 8,
			PLUSMINUS        = 9,
			MULTDIV          = 10,
			BROPEN           = 11,
			BRCLOSE          = 12,
			WHITESPACE       = 13
	};
	

	class Token
	{

	public:
		const static Token Empty;
		std::string File;
		int Line;
		int Column;
		int StartPos;
		int Length;
		int EndPos;
		std::string Text;

		std::vector<Token> Skipped;

		TokenType Type;

		Token()
			: Token(0, 0)
		{
		}

		inline Token(int start, int end)
		{
			Type = TokenType::_UNDETERMINED_;
			StartPos = start;
			EndPos = end;
			Text = "";
		}

		inline void UpdateRange(Token token)
		{
			if (token.StartPos < StartPos) StartPos = token.StartPos;
			if (token.EndPos > EndPos) EndPos = token.EndPos;
		}

		inline std::string ToString()
		{
			if (Text.empty())
				return std::to_string(Type) + " '" + Text + "'";
			else
				return std::to_string(Type) + "";
		}


	};
	const Token Token::Empty = Token();

	class Scanner
	{
	public:
		std::string Input;
		int StartPos = 0;
		int EndPos = 0;
		std::string CurrentFile;
		int CurrentLine;
		int CurrentColumn;
		int CurrentPosition;
		std::vector<Token> Skipped; // tokens that were skipped
		std::map<TokenType, std::regex> Patterns;
   
	private:
		Token LookAheadToken;
		std::vector<TokenType> Tokens;
		std::vector<TokenType> SkipList; // tokens to be skipped

	public:
		inline Scanner()
		{
			std::regex regex;
			//Patterns = new Dictionary<TokenType, Regex>();
			//Tokens = new List<TokenType>();
			LookAheadToken = Token::Empty;
			//Skipped = new List<Token>();

			//SkipList = new List<TokenType>();
			SkipList.push_back(TokenType::WHITESPACE);

			regex = std::regex("^\\s*$", std::regex_constants::ECMAScript);
			Patterns.insert(std::pair<TokenType,std::regex>(TokenType::EOF_, regex));
			Tokens.push_back(TokenType::EOF_);

			regex = std::regex("[0-9]+", std::regex_constants::ECMAScript);
			Patterns.insert(std::pair<TokenType,std::regex>(TokenType::NUMBER, regex));
			Tokens.push_back(TokenType::NUMBER);

			regex = std::regex("[a-zA-Z_][a-zA-Z0-9_]*", std::regex_constants::ECMAScript);
			Patterns.insert(std::pair<TokenType,std::regex>(TokenType::ID, regex));
			Tokens.push_back(TokenType::ID);

			regex = std::regex("(\\+|-)", std::regex_constants::ECMAScript);
			Patterns.insert(std::pair<TokenType,std::regex>(TokenType::PLUSMINUS, regex));
			Tokens.push_back(TokenType::PLUSMINUS);

			regex = std::regex("\\*|/", std::regex_constants::ECMAScript);
			Patterns.insert(std::pair<TokenType,std::regex>(TokenType::MULTDIV, regex));
			Tokens.push_back(TokenType::MULTDIV);

			regex = std::regex("\\(", std::regex_constants::ECMAScript);
			Patterns.insert(std::pair<TokenType,std::regex>(TokenType::BROPEN, regex));
			Tokens.push_back(TokenType::BROPEN);

			regex = std::regex("\\)", std::regex_constants::ECMAScript);
			Patterns.insert(std::pair<TokenType,std::regex>(TokenType::BRCLOSE, regex));
			Tokens.push_back(TokenType::BRCLOSE);

			regex = std::regex("\\s+", std::regex_constants::ECMAScript);
			Patterns.insert(std::pair<TokenType,std::regex>(TokenType::WHITESPACE, regex));
			Tokens.push_back(TokenType::WHITESPACE);


		}

		inline void Init(const std::string& input)
		{
			Init(input, "");
		}

		inline void Init(const std::string& input, const std::string& fileName)
		{
			this->Input = input;
			StartPos = 0;
			EndPos = 0;
			CurrentFile = fileName;
			CurrentLine = 1;
			CurrentColumn = 1;
			CurrentPosition = 0;
			LookAheadToken = Token::Empty;
		}

		inline Token GetToken(TokenType type)
		{
			Token t = Token(this->StartPos, this->EndPos);
			t.Type = type;
			return t;
		}

		/// <summary>
		/// executes a lookahead of the next token
		/// and will advance the scan on the input string
		/// </summary>
		/// <returns></returns>
		inline Token Scan(const std::vector<TokenType>& expectedtokens)
		{
			Token tok = LookAhead(expectedtokens); // temporarely retrieve the lookahead
			LookAheadToken = Token::Empty; // reset lookahead token, so scanning will continue
			StartPos = tok.EndPos;
			EndPos = tok.EndPos; // set the tokenizer to the new scan position
			CurrentLine = tok.Line + (int) (tok.Text.length() - replace(tok.Text,"\n", "").length());
			CurrentFile = tok.File;
			return tok;
		}

		/// <summary>
		/// returns token with longest best match
		/// </summary>
		/// <returns></returns>
		inline Token LookAhead(const std::vector<TokenType>& expectedtokens)
		{
			int i;
			int startpos = StartPos;
			int endpos = EndPos;
			int currentline = CurrentLine;
			std::string currentFile = CurrentFile;
			Token tok = Token::Empty;
			std::vector<TokenType> scantokens;


			// this prevents double scanning and matching
			// increased performance
			if (&LookAheadToken != &Token::Empty
				&& LookAheadToken.Type != TokenType::_UNDETERMINED_
				&& LookAheadToken.Type != TokenType::_NONE_)
				return LookAheadToken;

			// if no scantokens specified, then scan for all of them (= backward compatible)
			if (expectedtokens.size() == 0)
				scantokens = Tokens;
			else
			{
				scantokens = std::vector<TokenType>(expectedtokens);
				scantokens.insert(scantokens.end(), expectedtokens.begin(), expectedtokens.end());
				scantokens.insert(scantokens.end(), SkipList.begin(), SkipList.end());
			}

			do
			{

				int len = -1;
				TokenType index = (TokenType)INT_MAX;
				std::string input = Input.substr(startpos);

				tok = Token(startpos, endpos);

				for (i = 0; i < scantokens.size(); i++)
				{
					std::regex r = Patterns[scantokens[i]];
					std::smatch m;
					std::regex_search(input, m, r);
					
					int matchLen = (int) std::distance(m[0].first, m[0].second);
					if (!m.empty() && m[0].first == input.begin() && ((matchLen > len) || (scantokens[i] < index && matchLen == len)))
					{
						len = matchLen;
						index = scantokens[i];
					}
				}

				if (index >= 0 && len >= 0)
				{
					tok.EndPos = startpos + len;
					tok.Text = Input.substr(tok.StartPos, len);
					tok.Type = index;
				}
				else if (tok.StartPos == tok.EndPos)
				{
					if (tok.StartPos < Input.length())
						tok.Text = Input.substr(tok.StartPos, 1);
					else
						tok.Text = "EOF";
				}

				// Update the line and column count for error reporting.
				tok.File = currentFile;
				tok.Line = currentline;
				if (tok.StartPos < Input.length())
					tok.Column = tok.StartPos - (int) Input.find_last_of('\n', tok.StartPos);

				if (contains(SkipList, tok.Type))
				{
					startpos = tok.EndPos;
					endpos = tok.EndPos;
					currentline = tok.Line + (int) (tok.Text.length() - replace(tok.Text, "\n", "").length());
					currentFile = tok.File;
					Skipped.push_back(tok);
				}
				else
				{
					// only assign to non-skipped tokens
					tok.Skipped = Skipped; // assign prior skips to this token
					Skipped = std::vector<Token>(); //reset skips
				}
			}
			while (contains(SkipList, tok.Type));

			LookAheadToken = tok;
			return tok;
		}
	};

}
