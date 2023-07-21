// Automatically generated from source file: simple expression2_cpp.tpg
// By TinyPG v1.6 available at https://github.com/ultrasuperpingu/TinyPG

#pragma once
#include <string>
#include <vector>
#include <map>
#include <regex>

namespace TinyPG
{

	enum TokenType
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
		EOF_              = 6,
		NUMBER            = 7,
		ID                = 8,
		PLUSMINUS         = 9,
		MULTDIV           = 10,
		BROPEN            = 11,
		BRCLOSE           = 12,
		WHITESPACE        = 13
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
