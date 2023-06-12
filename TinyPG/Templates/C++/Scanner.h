// Automatically generated from source file: <%SourceFilename%>
// By TinyPG v1.5 available at https://github.com/ultrasuperpingu/TinyPG

#pragma once
#include <string>
#include <vector>
#include <map>
#include <regex>

namespace <%Namespace%>
{

	enum TokenType
	{
<%TokenType%>
	};
	

	class Token<%IToken%>
	{

	public:
		const static Token Empty;
		int Line;
		int Column;
		int StartPos;
		int Length;
		int EndPos;
		std::string Text;

		std::vector<Token> Skipped;
		TokenType Type;

		inline Token();

		Token(int start, int end);

		void UpdateRange(Token token);

		std::string ToString();

<%ScannerCustomCode%>
	};
	
	class Scanner
	{
	public:
		std::string Input;
		int StartPos = 0;
		int EndPos = 0;
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
		Scanner();

		void Init(const std::string& input);

		Token GetToken(TokenType type);

		/// <summary>
		/// executes a lookahead of the next token
		/// and will advance the scan on the input string
		/// </summary>
		/// <returns></returns>
		Token Scan(const std::vector<TokenType>& expectedtokens);

		/// <summary>
		/// returns token with longest best match
		/// </summary>
		/// <returns></returns>
		Token LookAhead(const std::vector<TokenType>& expectedtokens);
	};
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

	// Token implementation
	const inline Token Token::Empty = Token();

	inline Token::Token()
		: Token(0, 0)
	{
	}

	inline Token::Token(int start, int end)
	{
		Type = TokenType::_UNDETERMINED_;
		StartPos = start;
		EndPos = end;
		Text = "";
	}

	inline void Token::UpdateRange(Token token)
	{
		if (token.StartPos < StartPos) StartPos = token.StartPos;
		if (token.EndPos > EndPos) EndPos = token.EndPos;
	}

	inline std::string Token::ToString()
	{
		if (Text.empty())
			return std::to_string(Type) + " '" + Text + "'";
		else
			return std::to_string(Type) + "";
	}

	// Scanner implementation
	inline Scanner::Scanner()
	{
		std::regex regex;
		LookAheadToken = Token::Empty;
		
<%SkipList%>
<%RegExps%>
	}

	inline void Scanner::Init(const std::string& input)
	{
		this->Input = input;
		StartPos = 0;
		EndPos = 0;
		CurrentLine = 1;
		CurrentColumn = 1;
		CurrentPosition = 0;
		LookAheadToken = Token::Empty;
	}

	inline Token Scanner::GetToken(TokenType type)
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
	inline Token Scanner::Scan(const std::vector<TokenType>& expectedtokens)
	{
		Token tok = LookAhead(expectedtokens); // temporarely retrieve the lookahead
		LookAheadToken = Token::Empty; // reset lookahead token, so scanning will continue
		StartPos = tok.EndPos;
		EndPos = tok.EndPos; // set the tokenizer to the new scan position
		CurrentLine = tok.Line + (int)(tok.Text.length() - replace(tok.Text, "\n", "").length());
		return tok;
	}

	/// <summary>
	/// returns token with longest best match
	/// </summary>
	/// <returns></returns>
	inline Token Scanner::LookAhead(const std::vector<TokenType>& expectedtokens)
	{
		int i;
		int startpos = StartPos;
		int endpos = EndPos;
		int currentline = CurrentLine;
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

				int matchLen = (int)std::distance(m[0].first, m[0].second);
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
			tok.Line = currentline;
			if (tok.StartPos < Input.length())
				tok.Column = tok.StartPos - (int)Input.find_last_of('\n', tok.StartPos);

			if (contains(SkipList, tok.Type))
			{
				startpos = tok.EndPos;
				endpos = tok.EndPos;
				currentline = tok.Line + (int)(tok.Text.length() - replace(tok.Text, "\n", "").length());
				Skipped.push_back(tok);
			}
			else
			{
				// only assign to non-skipped tokens
				tok.Skipped = Skipped; // assign prior skips to this token
				Skipped = std::vector<Token>(); //reset skips
			}
		} while (contains(SkipList, tok.Type));

		LookAheadToken = tok;
		return tok;
	}
}
