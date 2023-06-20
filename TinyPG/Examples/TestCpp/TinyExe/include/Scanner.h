// Automatically generated from source file: TinyExpEval_cpp.tpg
// By TinyPG v1.5 available at https://github.com/ultrasuperpingu/TinyPG

#pragma once
#include <string>
#include <vector>
#include <map>
#include <regex>

namespace TinyExe
{

	enum TokenType
	{

		//Non terminal tokens:
		_NONE_            = 0,
		_UNDETERMINED_    = 1,

		//Non terminal tokens:
		Start             = 2,
		Function          = 3,
		PrimaryExpression = 4,
		ParenthesizedExpression= 5,
		UnaryExpression   = 6,
		PowerExpression   = 7,
		MultiplicativeExpression= 8,
		AdditiveExpression= 9,
		ConcatEpression   = 10,
		RelationalExpression= 11,
		EqualityExpression= 12,
		ConditionalAndExpression= 13,
		ConditionalOrExpression= 14,
		AssignmentExpression= 15,
		Expression        = 16,
		Params            = 17,
		Literal           = 18,
		IntegerLiteral    = 19,
		RealLiteral       = 20,
		StringLiteral     = 21,
		Variable          = 22,

		//Terminal tokens:
		BOOLEANLITERAL    = 23,
		DECIMALINTEGERLITERAL= 24,
		REALLITERAL       = 25,
		HEXINTEGERLITERAL = 26,
		STRINGLITERAL     = 27,
		FUNCTION          = 28,
		VARIABLE          = 29,
		CONSTANT          = 30,
		BRACEOPEN         = 31,
		BRACECLOSE        = 32,
		BRACKETOPEN       = 33,
		BRACKETCLOSE      = 34,
		SEMICOLON         = 35,
		PLUSPLUS          = 36,
		MINUSMINUS        = 37,
		PIPEPIPE          = 38,
		AMPAMP            = 39,
		AMP               = 40,
		POWER             = 41,
		PLUS              = 42,
		MINUS             = 43,
		EQUAL             = 44,
		ASSIGN            = 45,
		NOTEQUAL          = 46,
		NOT               = 47,
		ASTERIKS          = 48,
		SLASH             = 49,
		PERCENT           = 50,
		QUESTIONMARK      = 51,
		COMMA             = 52,
		LESSEQUAL         = 53,
		GREATEREQUAL      = 54,
		LESSTHAN          = 55,
		GREATERTHAN       = 56,
		COLON             = 57,
		EOF_              = 58,
		WHITESPACE        = 59
	};
	

	class Token
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

		Token();

		Token(int start, int end);

		void UpdateRange(Token token);

		std::string ToString();

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

}
