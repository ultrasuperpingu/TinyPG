// Automatically generated from source file: simple expression2_cpp.tpg
// By TinyPG v1.5 available at https://github.com/ultrasuperpingu/TinyPG

#include "Scanner.h"
#include <climits>
namespace TinyPG
{
	// Token implementation
	const Token Token::Empty = Token();

	Token::Token()
		: Token(0, 0)
	{
	}

	Token::Token(int start, int end)
	{
		Type = TokenType::_UNDETERMINED_;
		StartPos = start;
		EndPos = end;
		Text = "";
	}

	void Token::UpdateRange(Token token)
	{
		if (token.StartPos < StartPos) StartPos = token.StartPos;
		if (token.EndPos > EndPos) EndPos = token.EndPos;
	}

	std::string Token::ToString()
	{
		if (Text.empty())
			return std::to_string(Type) + " '" + Text + "'";
		else
			return std::to_string(Type) + "";
	}

	// Scanner implementation
	Scanner::Scanner()
	{
		std::regex regex;
		LookAheadToken = Token::Empty;
		
		SkipList.push_back(TokenType::WHITESPACE);

		regex = std::regex("^\\s*$");
		Patterns.insert(std::pair<TokenType,std::regex>(TokenType::EOF_, regex));
		Tokens.push_back(TokenType::EOF_);

		regex = std::regex("^(?:[0-9]+)");
		Patterns.insert(std::pair<TokenType,std::regex>(TokenType::NUMBER, regex));
		Tokens.push_back(TokenType::NUMBER);

		regex = std::regex("^(?:[a-zA-Z_][a-zA-Z0-9_]*)");
		Patterns.insert(std::pair<TokenType,std::regex>(TokenType::ID, regex));
		Tokens.push_back(TokenType::ID);

		regex = std::regex("^(?:(\\+|-))");
		Patterns.insert(std::pair<TokenType,std::regex>(TokenType::PLUSMINUS, regex));
		Tokens.push_back(TokenType::PLUSMINUS);

		regex = std::regex("^(?:\\*|/)");
		Patterns.insert(std::pair<TokenType,std::regex>(TokenType::MULTDIV, regex));
		Tokens.push_back(TokenType::MULTDIV);

		regex = std::regex("^(?:\\()");
		Patterns.insert(std::pair<TokenType,std::regex>(TokenType::BROPEN, regex));
		Tokens.push_back(TokenType::BROPEN);

		regex = std::regex("^(?:\\))");
		Patterns.insert(std::pair<TokenType,std::regex>(TokenType::BRCLOSE, regex));
		Tokens.push_back(TokenType::BRCLOSE);

		regex = std::regex("^(?:\\s+)");
		Patterns.insert(std::pair<TokenType,std::regex>(TokenType::WHITESPACE, regex));
		Tokens.push_back(TokenType::WHITESPACE);


	}

	void Scanner::Init(const std::string& input)
	{
		this->Input = input;
		StartPos = 0;
		EndPos = 0;
		CurrentLine = 1;
		CurrentColumn = 1;
		CurrentPosition = 0;
		LookAheadToken = Token::Empty;
	}

	Token Scanner::GetToken(TokenType type)
	{
		Token t = Token(this->StartPos, this->EndPos);
		t.Type = type;
		return t;
	}

	Token Scanner::Scan(const std::vector<TokenType>& expectedtokens)
	{
		Token tok = LookAhead(expectedtokens); // temporarely retrieve the lookahead
		LookAheadToken = Token::Empty; // reset lookahead token, so scanning will continue
		StartPos = tok.EndPos;
		EndPos = tok.EndPos; // set the tokenizer to the new scan position
		CurrentLine = tok.Line + (int)(tok.Text.length() - replace(tok.Text, "\n", "").length());
		return tok;
	}

	Token Scanner::LookAhead(const std::vector<TokenType>& expectedtokens)
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
