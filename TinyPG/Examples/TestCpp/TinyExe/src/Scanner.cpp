// Automatically generated from source file: TinyExpEval_cpp.tpg
// By TinyPG v1.5 available at https://github.com/ultrasuperpingu/TinyPG
#include <climits>
#include "Scanner.h"

namespace TinyExe
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

		regex = std::regex("^(?:true|false)");
		Patterns.insert(std::pair<TokenType,std::regex>(TokenType::BOOLEANLITERAL, regex));
		Tokens.push_back(TokenType::BOOLEANLITERAL);

		regex = std::regex("^(?:[0-9]+(UL|Ul|uL|ul|LU|Lu|lU|lu|U|u|L|l)?)");
		Patterns.insert(std::pair<TokenType,std::regex>(TokenType::DECIMALINTEGERLITERAL, regex));
		Tokens.push_back(TokenType::DECIMALINTEGERLITERAL);

		regex = std::regex("^(?:([0-9]+\\.[0-9]+([eE][+-]?[0-9]+)?([fFdDMm]?)?)|(\\.[0-9]+([eE][+-]?[0-9]+)?([fFdDMm]?)?)|([0-9]+([eE][+-]?[0-9]+)([fFdDMm]?)?)|([0-9]+([fFdDMm]?)))");
		Patterns.insert(std::pair<TokenType,std::regex>(TokenType::REALLITERAL, regex));
		Tokens.push_back(TokenType::REALLITERAL);

		regex = std::regex("^(?:0(x|X)[0-9a-fA-F]+)");
		Patterns.insert(std::pair<TokenType,std::regex>(TokenType::HEXINTEGERLITERAL, regex));
		Tokens.push_back(TokenType::HEXINTEGERLITERAL);

		regex = std::regex("^(?:\\\"\"(\\\"\"\\\"\"|[^\\\"\"])*\\\"\")");
		Patterns.insert(std::pair<TokenType,std::regex>(TokenType::STRINGLITERAL, regex));
		Tokens.push_back(TokenType::STRINGLITERAL);

		regex = std::regex("^(?:[a-zA-Z_][a-zA-Z0-9_]*(?=\\s*\\())");
		Patterns.insert(std::pair<TokenType,std::regex>(TokenType::FUNCTION, regex));
		Tokens.push_back(TokenType::FUNCTION);

		regex = std::regex("^(?:[a-zA-Z_][a-zA-Z0-9_]*(?!\\s*\\())");
		Patterns.insert(std::pair<TokenType,std::regex>(TokenType::VARIABLE, regex));
		Tokens.push_back(TokenType::VARIABLE);

		regex = std::regex("^(?:pi|e)");
		Patterns.insert(std::pair<TokenType,std::regex>(TokenType::CONSTANT, regex));
		Tokens.push_back(TokenType::CONSTANT);

		regex = std::regex("^(?:\\{\\s*)");
		Patterns.insert(std::pair<TokenType,std::regex>(TokenType::BRACEOPEN, regex));
		Tokens.push_back(TokenType::BRACEOPEN);

		regex = std::regex("^(?:\\s*\\})");
		Patterns.insert(std::pair<TokenType,std::regex>(TokenType::BRACECLOSE, regex));
		Tokens.push_back(TokenType::BRACECLOSE);

		regex = std::regex("^(?:\\(\\s*)");
		Patterns.insert(std::pair<TokenType,std::regex>(TokenType::BRACKETOPEN, regex));
		Tokens.push_back(TokenType::BRACKETOPEN);

		regex = std::regex("^(?:\\s*\\))");
		Patterns.insert(std::pair<TokenType,std::regex>(TokenType::BRACKETCLOSE, regex));
		Tokens.push_back(TokenType::BRACKETCLOSE);

		regex = std::regex("^(?:;)");
		Patterns.insert(std::pair<TokenType,std::regex>(TokenType::SEMICOLON, regex));
		Tokens.push_back(TokenType::SEMICOLON);

		regex = std::regex("^(?:\\+\\+)");
		Patterns.insert(std::pair<TokenType,std::regex>(TokenType::PLUSPLUS, regex));
		Tokens.push_back(TokenType::PLUSPLUS);

		regex = std::regex("^(?:--)");
		Patterns.insert(std::pair<TokenType,std::regex>(TokenType::MINUSMINUS, regex));
		Tokens.push_back(TokenType::MINUSMINUS);

		regex = std::regex("^(?:\\|\\||or)");
		Patterns.insert(std::pair<TokenType,std::regex>(TokenType::PIPEPIPE, regex));
		Tokens.push_back(TokenType::PIPEPIPE);

		regex = std::regex("^(?:&&|and)");
		Patterns.insert(std::pair<TokenType,std::regex>(TokenType::AMPAMP, regex));
		Tokens.push_back(TokenType::AMPAMP);

		regex = std::regex("^(?:&(?!&))");
		Patterns.insert(std::pair<TokenType,std::regex>(TokenType::AMP, regex));
		Tokens.push_back(TokenType::AMP);

		regex = std::regex("^(?:\\^)");
		Patterns.insert(std::pair<TokenType,std::regex>(TokenType::POWER, regex));
		Tokens.push_back(TokenType::POWER);

		regex = std::regex("^(?:\\+)");
		Patterns.insert(std::pair<TokenType,std::regex>(TokenType::PLUS, regex));
		Tokens.push_back(TokenType::PLUS);

		regex = std::regex("^(?:-)");
		Patterns.insert(std::pair<TokenType,std::regex>(TokenType::MINUS, regex));
		Tokens.push_back(TokenType::MINUS);

		regex = std::regex("^(?:=)");
		Patterns.insert(std::pair<TokenType,std::regex>(TokenType::EQUAL, regex));
		Tokens.push_back(TokenType::EQUAL);

		regex = std::regex("^(?::=)");
		Patterns.insert(std::pair<TokenType,std::regex>(TokenType::ASSIGN, regex));
		Tokens.push_back(TokenType::ASSIGN);

		regex = std::regex("^(?:!=|<>)");
		Patterns.insert(std::pair<TokenType,std::regex>(TokenType::NOTEQUAL, regex));
		Tokens.push_back(TokenType::NOTEQUAL);

		regex = std::regex("^(?:!)");
		Patterns.insert(std::pair<TokenType,std::regex>(TokenType::NOT, regex));
		Tokens.push_back(TokenType::NOT);

		regex = std::regex("^(?:\\*)");
		Patterns.insert(std::pair<TokenType,std::regex>(TokenType::ASTERIKS, regex));
		Tokens.push_back(TokenType::ASTERIKS);

		regex = std::regex("^(?:/)");
		Patterns.insert(std::pair<TokenType,std::regex>(TokenType::SLASH, regex));
		Tokens.push_back(TokenType::SLASH);

		regex = std::regex("^(?:%)");
		Patterns.insert(std::pair<TokenType,std::regex>(TokenType::PERCENT, regex));
		Tokens.push_back(TokenType::PERCENT);

		regex = std::regex("^(?:\\?)");
		Patterns.insert(std::pair<TokenType,std::regex>(TokenType::QUESTIONMARK, regex));
		Tokens.push_back(TokenType::QUESTIONMARK);

		regex = std::regex("^(?:,)");
		Patterns.insert(std::pair<TokenType,std::regex>(TokenType::COMMA, regex));
		Tokens.push_back(TokenType::COMMA);

		regex = std::regex("^(?:<=)");
		Patterns.insert(std::pair<TokenType,std::regex>(TokenType::LESSEQUAL, regex));
		Tokens.push_back(TokenType::LESSEQUAL);

		regex = std::regex("^(?:>=)");
		Patterns.insert(std::pair<TokenType,std::regex>(TokenType::GREATEREQUAL, regex));
		Tokens.push_back(TokenType::GREATEREQUAL);

		regex = std::regex("^(?:<(?!>))");
		Patterns.insert(std::pair<TokenType,std::regex>(TokenType::LESSTHAN, regex));
		Tokens.push_back(TokenType::LESSTHAN);

		regex = std::regex("^(?:>)");
		Patterns.insert(std::pair<TokenType,std::regex>(TokenType::GREATERTHAN, regex));
		Tokens.push_back(TokenType::GREATERTHAN);

		regex = std::regex("^(?::)");
		Patterns.insert(std::pair<TokenType,std::regex>(TokenType::COLON, regex));
		Tokens.push_back(TokenType::COLON);

		regex = std::regex("^(?:$)");
		Patterns.insert(std::pair<TokenType,std::regex>(TokenType::EOF_, regex));
		Tokens.push_back(TokenType::EOF_);

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
		// TODO: check this, what if the expected token are different since last call?
		// Check at least that LookAheadToken is part of the expected tokens
		if (&LookAheadToken != &Token::Empty
			&& LookAheadToken.Type != TokenType::_UNDETERMINED_
			&& LookAheadToken.Type != TokenType::_NONE_)
		{
			return LookAheadToken;
		}

		// if no scantokens specified, then scan for all of them (= backward compatible)
		if (expectedtokens.size() == 0)
			scantokens = Tokens;
		else
		{
			scantokens = std::vector<TokenType>(expectedtokens);
			// TODO: Check this line (useless at first look, too late to figure out now) 
			scantokens.insert(scantokens.end(), expectedtokens.begin(), expectedtokens.end());
			scantokens.insert(scantokens.end(), SkipList.begin(), SkipList.end());
		}

		do
		{

			int len = -1;
			TokenType index = (TokenType)INT_MAX;
			std::string input = Input.substr(startpos);

			tok = Token(startpos, endpos);

			for (i = 0; i < (int)scantokens.size(); i++)
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
				if (tok.StartPos < (int)Input.length())
					tok.Text = Input.substr(tok.StartPos, 1);
				else
					tok.Text = "EOF";
			}

			// Update the line and column count for error reporting.
			tok.Line = currentline;
			if (tok.StartPos < (int)Input.length())
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
