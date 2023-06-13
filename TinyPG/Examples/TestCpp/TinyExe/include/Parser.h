// Automatically generated from source file: TinyExpEval_cpp.tpg
// By TinyPG v1.5 available at https://github.com/ultrasuperpingu/TinyPG

#pragma once
#include <string>
#include "Scanner.h"
#include "ParseTree.h"

namespace TinyExe
{
	class Parser
	{
	private:
		Scanner& scanner;
		ParseTree* tree;
		ParseTree* instanciatedTree;
		void DeleteTree();
	public:
		Parser(Scanner& scanner);
		virtual ~Parser();

		ParseTree* Parse(const std::string& input);
		ParseTree* Parse(const std::string& input, ParseTree* tree);

	public:
		ParseTree* ParseStart(const std::string& input, ParseTree* tree);
	protected:
		void ParseStart(ParseNode* parent);
	public:
		ParseTree* ParseFunction(const std::string& input, ParseTree* tree);
	protected:
		void ParseFunction(ParseNode* parent);
	public:
		ParseTree* ParsePrimaryExpression(const std::string& input, ParseTree* tree);
	protected:
		void ParsePrimaryExpression(ParseNode* parent);
	public:
		ParseTree* ParseParenthesizedExpression(const std::string& input, ParseTree* tree);
	protected:
		void ParseParenthesizedExpression(ParseNode* parent);
	public:
		ParseTree* ParseUnaryExpression(const std::string& input, ParseTree* tree);
	protected:
		void ParseUnaryExpression(ParseNode* parent);
	public:
		ParseTree* ParsePowerExpression(const std::string& input, ParseTree* tree);
	protected:
		void ParsePowerExpression(ParseNode* parent);
	public:
		ParseTree* ParseMultiplicativeExpression(const std::string& input, ParseTree* tree);
	protected:
		void ParseMultiplicativeExpression(ParseNode* parent);
	public:
		ParseTree* ParseAdditiveExpression(const std::string& input, ParseTree* tree);
	protected:
		void ParseAdditiveExpression(ParseNode* parent);
	public:
		ParseTree* ParseConcatEpression(const std::string& input, ParseTree* tree);
	protected:
		void ParseConcatEpression(ParseNode* parent);
	public:
		ParseTree* ParseRelationalExpression(const std::string& input, ParseTree* tree);
	protected:
		void ParseRelationalExpression(ParseNode* parent);
	public:
		ParseTree* ParseEqualityExpression(const std::string& input, ParseTree* tree);
	protected:
		void ParseEqualityExpression(ParseNode* parent);
	public:
		ParseTree* ParseConditionalAndExpression(const std::string& input, ParseTree* tree);
	protected:
		void ParseConditionalAndExpression(ParseNode* parent);
	public:
		ParseTree* ParseConditionalOrExpression(const std::string& input, ParseTree* tree);
	protected:
		void ParseConditionalOrExpression(ParseNode* parent);
	public:
		ParseTree* ParseAssignmentExpression(const std::string& input, ParseTree* tree);
	protected:
		void ParseAssignmentExpression(ParseNode* parent);
	public:
		ParseTree* ParseExpression(const std::string& input, ParseTree* tree);
	protected:
		void ParseExpression(ParseNode* parent);
	public:
		ParseTree* ParseParams(const std::string& input, ParseTree* tree);
	protected:
		void ParseParams(ParseNode* parent);
	public:
		ParseTree* ParseLiteral(const std::string& input, ParseTree* tree);
	protected:
		void ParseLiteral(ParseNode* parent);
	public:
		ParseTree* ParseIntegerLiteral(const std::string& input, ParseTree* tree);
	protected:
		void ParseIntegerLiteral(ParseNode* parent);
	public:
		ParseTree* ParseRealLiteral(const std::string& input, ParseTree* tree);
	protected:
		void ParseRealLiteral(ParseNode* parent);
	public:
		ParseTree* ParseStringLiteral(const std::string& input, ParseTree* tree);
	protected:
		void ParseStringLiteral(ParseNode* parent);
	public:
		ParseTree* ParseVariable(const std::string& input, ParseTree* tree);
	protected:
		void ParseVariable(ParseNode* parent);



	};

	inline void Parser::DeleteTree()
	{
		if (instanciatedTree != NULL)
		{
			delete instanciatedTree;
			instanciatedTree = NULL;
		}
	}

	inline Parser::Parser(Scanner& scanner) : scanner(scanner), tree(NULL), instanciatedTree(NULL)
	{
	}

	inline Parser::~Parser()
	{
		DeleteTree();
	}

	inline ParseTree* Parser::Parse(const std::string& input)
	{
		DeleteTree();
		instanciatedTree = new ParseTree();
		return Parse(input, new ParseTree());
	}

	inline ParseTree* Parser::Parse(const std::string& input, ParseTree* tree)
	{
		scanner.Init(input);
		if (tree != instanciatedTree)
			DeleteTree();
		this->tree = tree;
		ParseStart(tree);
		tree->Skipped = scanner.Skipped;

		return tree;
	}

	inline ParseTree* Parser::ParseStart(const std::string& input, ParseTree* tree) // NonTerminalSymbol: Start
	{
		scanner.Init(input);
		this->tree = tree;
		ParseStart(tree);
		tree->Skipped = scanner.Skipped;
		return tree;
	}

	inline void Parser::ParseStart(ParseNode* parent) // NonTerminalSymbol: Start
	{
		Token tok;
		ParseNode* n;
		ParseNode* node = parent->CreateNode(scanner.GetToken(TokenType::Start), "Start");
		parent->Nodes.push_back(node);


		 // Concat Rule
		tok = scanner.LookAhead({TokenType::FUNCTION, TokenType::VARIABLE, TokenType::BOOLEANLITERAL, TokenType::DECIMALINTEGERLITERAL, TokenType::HEXINTEGERLITERAL, TokenType::REALLITERAL, TokenType::STRINGLITERAL, TokenType::BRACKETOPEN, TokenType::PLUS, TokenType::MINUS, TokenType::NOT, TokenType::ASSIGN}); // Option Rule
		if (tok.Type == TokenType::FUNCTION
		    || tok.Type == TokenType::VARIABLE
		    || tok.Type == TokenType::BOOLEANLITERAL
		    || tok.Type == TokenType::DECIMALINTEGERLITERAL
		    || tok.Type == TokenType::HEXINTEGERLITERAL
		    || tok.Type == TokenType::REALLITERAL
		    || tok.Type == TokenType::STRINGLITERAL
		    || tok.Type == TokenType::BRACKETOPEN
		    || tok.Type == TokenType::PLUS
		    || tok.Type == TokenType::MINUS
		    || tok.Type == TokenType::NOT
		    || tok.Type == TokenType::ASSIGN)
		{
			ParseExpression(node); // NonTerminal Rule: Expression
		}

		 // Concat Rule
		tok = scanner.Scan({TokenType::EOF_}); // Terminal Rule: EOF_
		n = node->CreateNode(tok, tok.ToString() );
		node->TokenVal.UpdateRange(tok);
		node->Nodes.push_back(n);
		if (tok.Type != TokenType::EOF_) {
			tree->Errors.push_back(ParseError("Unexpected token '" + replace(tok.Text, "\n", "") + "' found. Expected EOF_", 0x1001, tok));
			return;
		}

		parent->TokenVal.UpdateRange(node->TokenVal);
	} // NonTerminalSymbol: Start

	inline ParseTree* Parser::ParseFunction(const std::string& input, ParseTree* tree) // NonTerminalSymbol: Function
	{
		scanner.Init(input);
		this->tree = tree;
		ParseFunction(tree);
		tree->Skipped = scanner.Skipped;
		return tree;
	}

	inline void Parser::ParseFunction(ParseNode* parent) // NonTerminalSymbol: Function
	{
		Token tok;
		ParseNode* n;
		ParseNode* node = parent->CreateNode(scanner.GetToken(TokenType::Function), "Function");
		parent->Nodes.push_back(node);


		 // Concat Rule
		tok = scanner.Scan({TokenType::FUNCTION}); // Terminal Rule: FUNCTION
		n = node->CreateNode(tok, tok.ToString() );
		node->TokenVal.UpdateRange(tok);
		node->Nodes.push_back(n);
		if (tok.Type != TokenType::FUNCTION) {
			tree->Errors.push_back(ParseError("Unexpected token '" + replace(tok.Text, "\n", "") + "' found. Expected FUNCTION", 0x1001, tok));
			return;
		}

		 // Concat Rule
		tok = scanner.Scan({TokenType::BRACKETOPEN}); // Terminal Rule: BRACKETOPEN
		n = node->CreateNode(tok, tok.ToString() );
		node->TokenVal.UpdateRange(tok);
		node->Nodes.push_back(n);
		if (tok.Type != TokenType::BRACKETOPEN) {
			tree->Errors.push_back(ParseError("Unexpected token '" + replace(tok.Text, "\n", "") + "' found. Expected BRACKETOPEN", 0x1001, tok));
			return;
		}

		 // Concat Rule
		tok = scanner.LookAhead({TokenType::FUNCTION, TokenType::VARIABLE, TokenType::BOOLEANLITERAL, TokenType::DECIMALINTEGERLITERAL, TokenType::HEXINTEGERLITERAL, TokenType::REALLITERAL, TokenType::STRINGLITERAL, TokenType::BRACKETOPEN, TokenType::PLUS, TokenType::MINUS, TokenType::NOT, TokenType::ASSIGN, TokenType::SEMICOLON}); // Option Rule
		if (tok.Type == TokenType::FUNCTION
		    || tok.Type == TokenType::VARIABLE
		    || tok.Type == TokenType::BOOLEANLITERAL
		    || tok.Type == TokenType::DECIMALINTEGERLITERAL
		    || tok.Type == TokenType::HEXINTEGERLITERAL
		    || tok.Type == TokenType::REALLITERAL
		    || tok.Type == TokenType::STRINGLITERAL
		    || tok.Type == TokenType::BRACKETOPEN
		    || tok.Type == TokenType::PLUS
		    || tok.Type == TokenType::MINUS
		    || tok.Type == TokenType::NOT
		    || tok.Type == TokenType::ASSIGN
		    || tok.Type == TokenType::SEMICOLON)
		{
			ParseParams(node); // NonTerminal Rule: Params
		}

		 // Concat Rule
		tok = scanner.Scan({TokenType::BRACKETCLOSE}); // Terminal Rule: BRACKETCLOSE
		n = node->CreateNode(tok, tok.ToString() );
		node->TokenVal.UpdateRange(tok);
		node->Nodes.push_back(n);
		if (tok.Type != TokenType::BRACKETCLOSE) {
			tree->Errors.push_back(ParseError("Unexpected token '" + replace(tok.Text, "\n", "") + "' found. Expected BRACKETCLOSE", 0x1001, tok));
			return;
		}

		parent->TokenVal.UpdateRange(node->TokenVal);
	} // NonTerminalSymbol: Function

	inline ParseTree* Parser::ParsePrimaryExpression(const std::string& input, ParseTree* tree) // NonTerminalSymbol: PrimaryExpression
	{
		scanner.Init(input);
		this->tree = tree;
		ParsePrimaryExpression(tree);
		tree->Skipped = scanner.Skipped;
		return tree;
	}

	inline void Parser::ParsePrimaryExpression(ParseNode* parent) // NonTerminalSymbol: PrimaryExpression
	{
		Token tok;
		ParseNode* n;
		ParseNode* node = parent->CreateNode(scanner.GetToken(TokenType::PrimaryExpression), "PrimaryExpression");
		parent->Nodes.push_back(node);

		tok = scanner.LookAhead({TokenType::FUNCTION, TokenType::VARIABLE, TokenType::BOOLEANLITERAL, TokenType::DECIMALINTEGERLITERAL, TokenType::HEXINTEGERLITERAL, TokenType::REALLITERAL, TokenType::STRINGLITERAL, TokenType::BRACKETOPEN}); // Choice Rule
		switch (tok.Type)
		{ // Choice Rule
			case TokenType::FUNCTION:
				ParseFunction(node); // NonTerminal Rule: Function
				break;
			case TokenType::VARIABLE:
				ParseVariable(node); // NonTerminal Rule: Variable
				break;
			case TokenType::BOOLEANLITERAL:
			case TokenType::DECIMALINTEGERLITERAL:
			case TokenType::HEXINTEGERLITERAL:
			case TokenType::REALLITERAL:
			case TokenType::STRINGLITERAL:
				ParseLiteral(node); // NonTerminal Rule: Literal
				break;
			case TokenType::BRACKETOPEN:
				ParseParenthesizedExpression(node); // NonTerminal Rule: ParenthesizedExpression
				break;
			default:
				tree->Errors.push_back(ParseError("Unexpected token '" + replace(tok.Text, "\n", "") + "' found. Expected FUNCTION, VARIABLE, BOOLEANLITERAL, DECIMALINTEGERLITERAL, HEXINTEGERLITERAL, REALLITERAL, STRINGLITERAL, or BRACKETOPEN.", 0x0002, tok));
				break;
		} // Choice Rule

		parent->TokenVal.UpdateRange(node->TokenVal);
	} // NonTerminalSymbol: PrimaryExpression

	inline ParseTree* Parser::ParseParenthesizedExpression(const std::string& input, ParseTree* tree) // NonTerminalSymbol: ParenthesizedExpression
	{
		scanner.Init(input);
		this->tree = tree;
		ParseParenthesizedExpression(tree);
		tree->Skipped = scanner.Skipped;
		return tree;
	}

	inline void Parser::ParseParenthesizedExpression(ParseNode* parent) // NonTerminalSymbol: ParenthesizedExpression
	{
		Token tok;
		ParseNode* n;
		ParseNode* node = parent->CreateNode(scanner.GetToken(TokenType::ParenthesizedExpression), "ParenthesizedExpression");
		parent->Nodes.push_back(node);


		 // Concat Rule
		tok = scanner.Scan({TokenType::BRACKETOPEN}); // Terminal Rule: BRACKETOPEN
		n = node->CreateNode(tok, tok.ToString() );
		node->TokenVal.UpdateRange(tok);
		node->Nodes.push_back(n);
		if (tok.Type != TokenType::BRACKETOPEN) {
			tree->Errors.push_back(ParseError("Unexpected token '" + replace(tok.Text, "\n", "") + "' found. Expected BRACKETOPEN", 0x1001, tok));
			return;
		}

		 // Concat Rule
		ParseExpression(node); // NonTerminal Rule: Expression

		 // Concat Rule
		tok = scanner.Scan({TokenType::BRACKETCLOSE}); // Terminal Rule: BRACKETCLOSE
		n = node->CreateNode(tok, tok.ToString() );
		node->TokenVal.UpdateRange(tok);
		node->Nodes.push_back(n);
		if (tok.Type != TokenType::BRACKETCLOSE) {
			tree->Errors.push_back(ParseError("Unexpected token '" + replace(tok.Text, "\n", "") + "' found. Expected BRACKETCLOSE", 0x1001, tok));
			return;
		}

		parent->TokenVal.UpdateRange(node->TokenVal);
	} // NonTerminalSymbol: ParenthesizedExpression

	inline ParseTree* Parser::ParseUnaryExpression(const std::string& input, ParseTree* tree) // NonTerminalSymbol: UnaryExpression
	{
		scanner.Init(input);
		this->tree = tree;
		ParseUnaryExpression(tree);
		tree->Skipped = scanner.Skipped;
		return tree;
	}

	inline void Parser::ParseUnaryExpression(ParseNode* parent) // NonTerminalSymbol: UnaryExpression
	{
		Token tok;
		ParseNode* n;
		ParseNode* node = parent->CreateNode(scanner.GetToken(TokenType::UnaryExpression), "UnaryExpression");
		parent->Nodes.push_back(node);

		tok = scanner.LookAhead({TokenType::FUNCTION, TokenType::VARIABLE, TokenType::BOOLEANLITERAL, TokenType::DECIMALINTEGERLITERAL, TokenType::HEXINTEGERLITERAL, TokenType::REALLITERAL, TokenType::STRINGLITERAL, TokenType::BRACKETOPEN, TokenType::PLUS, TokenType::MINUS, TokenType::NOT}); // Choice Rule
		switch (tok.Type)
		{ // Choice Rule
			case TokenType::FUNCTION:
			case TokenType::VARIABLE:
			case TokenType::BOOLEANLITERAL:
			case TokenType::DECIMALINTEGERLITERAL:
			case TokenType::HEXINTEGERLITERAL:
			case TokenType::REALLITERAL:
			case TokenType::STRINGLITERAL:
			case TokenType::BRACKETOPEN:
				ParsePrimaryExpression(node); // NonTerminal Rule: PrimaryExpression
				break;
			case TokenType::PLUS:

				 // Concat Rule
				tok = scanner.Scan({TokenType::PLUS}); // Terminal Rule: PLUS
				n = node->CreateNode(tok, tok.ToString() );
				node->TokenVal.UpdateRange(tok);
				node->Nodes.push_back(n);
				if (tok.Type != TokenType::PLUS) {
					tree->Errors.push_back(ParseError("Unexpected token '" + replace(tok.Text, "\n", "") + "' found. Expected PLUS", 0x1001, tok));
					return;
				}

				 // Concat Rule
				ParseUnaryExpression(node); // NonTerminal Rule: UnaryExpression
				break;
			case TokenType::MINUS:

				 // Concat Rule
				tok = scanner.Scan({TokenType::MINUS}); // Terminal Rule: MINUS
				n = node->CreateNode(tok, tok.ToString() );
				node->TokenVal.UpdateRange(tok);
				node->Nodes.push_back(n);
				if (tok.Type != TokenType::MINUS) {
					tree->Errors.push_back(ParseError("Unexpected token '" + replace(tok.Text, "\n", "") + "' found. Expected MINUS", 0x1001, tok));
					return;
				}

				 // Concat Rule
				ParseUnaryExpression(node); // NonTerminal Rule: UnaryExpression
				break;
			case TokenType::NOT:

				 // Concat Rule
				tok = scanner.Scan({TokenType::NOT}); // Terminal Rule: NOT
				n = node->CreateNode(tok, tok.ToString() );
				node->TokenVal.UpdateRange(tok);
				node->Nodes.push_back(n);
				if (tok.Type != TokenType::NOT) {
					tree->Errors.push_back(ParseError("Unexpected token '" + replace(tok.Text, "\n", "") + "' found. Expected NOT", 0x1001, tok));
					return;
				}

				 // Concat Rule
				ParseUnaryExpression(node); // NonTerminal Rule: UnaryExpression
				break;
			default:
				tree->Errors.push_back(ParseError("Unexpected token '" + replace(tok.Text, "\n", "") + "' found. Expected FUNCTION, VARIABLE, BOOLEANLITERAL, DECIMALINTEGERLITERAL, HEXINTEGERLITERAL, REALLITERAL, STRINGLITERAL, BRACKETOPEN, PLUS, MINUS, or NOT.", 0x0002, tok));
				break;
		} // Choice Rule

		parent->TokenVal.UpdateRange(node->TokenVal);
	} // NonTerminalSymbol: UnaryExpression

	inline ParseTree* Parser::ParsePowerExpression(const std::string& input, ParseTree* tree) // NonTerminalSymbol: PowerExpression
	{
		scanner.Init(input);
		this->tree = tree;
		ParsePowerExpression(tree);
		tree->Skipped = scanner.Skipped;
		return tree;
	}

	inline void Parser::ParsePowerExpression(ParseNode* parent) // NonTerminalSymbol: PowerExpression
	{
		Token tok;
		ParseNode* n;
		ParseNode* node = parent->CreateNode(scanner.GetToken(TokenType::PowerExpression), "PowerExpression");
		parent->Nodes.push_back(node);


		 // Concat Rule
		ParseUnaryExpression(node); // NonTerminal Rule: UnaryExpression

		 // Concat Rule
		tok = scanner.LookAhead({TokenType::POWER}); // ZeroOrMore Rule
		while (tok.Type == TokenType::POWER)
		{

			 // Concat Rule
			tok = scanner.Scan({TokenType::POWER}); // Terminal Rule: POWER
			n = node->CreateNode(tok, tok.ToString() );
			node->TokenVal.UpdateRange(tok);
			node->Nodes.push_back(n);
			if (tok.Type != TokenType::POWER) {
				tree->Errors.push_back(ParseError("Unexpected token '" + replace(tok.Text, "\n", "") + "' found. Expected POWER", 0x1001, tok));
				return;
			}

			 // Concat Rule
			ParseUnaryExpression(node); // NonTerminal Rule: UnaryExpression
		tok = scanner.LookAhead({TokenType::POWER}); // ZeroOrMore Rule
		}

		parent->TokenVal.UpdateRange(node->TokenVal);
	} // NonTerminalSymbol: PowerExpression

	inline ParseTree* Parser::ParseMultiplicativeExpression(const std::string& input, ParseTree* tree) // NonTerminalSymbol: MultiplicativeExpression
	{
		scanner.Init(input);
		this->tree = tree;
		ParseMultiplicativeExpression(tree);
		tree->Skipped = scanner.Skipped;
		return tree;
	}

	inline void Parser::ParseMultiplicativeExpression(ParseNode* parent) // NonTerminalSymbol: MultiplicativeExpression
	{
		Token tok;
		ParseNode* n;
		ParseNode* node = parent->CreateNode(scanner.GetToken(TokenType::MultiplicativeExpression), "MultiplicativeExpression");
		parent->Nodes.push_back(node);


		 // Concat Rule
		ParsePowerExpression(node); // NonTerminal Rule: PowerExpression

		 // Concat Rule
		tok = scanner.LookAhead({TokenType::ASTERIKS, TokenType::SLASH, TokenType::PERCENT}); // ZeroOrMore Rule
		while (tok.Type == TokenType::ASTERIKS
		    || tok.Type == TokenType::SLASH
		    || tok.Type == TokenType::PERCENT)
		{

			 // Concat Rule
			tok = scanner.LookAhead({TokenType::ASTERIKS, TokenType::SLASH, TokenType::PERCENT}); // Choice Rule
			switch (tok.Type)
			{ // Choice Rule
				case TokenType::ASTERIKS:
					tok = scanner.Scan({TokenType::ASTERIKS}); // Terminal Rule: ASTERIKS
					n = node->CreateNode(tok, tok.ToString() );
					node->TokenVal.UpdateRange(tok);
					node->Nodes.push_back(n);
					if (tok.Type != TokenType::ASTERIKS) {
						tree->Errors.push_back(ParseError("Unexpected token '" + replace(tok.Text, "\n", "") + "' found. Expected ASTERIKS", 0x1001, tok));
						return;
					}
					break;
				case TokenType::SLASH:
					tok = scanner.Scan({TokenType::SLASH}); // Terminal Rule: SLASH
					n = node->CreateNode(tok, tok.ToString() );
					node->TokenVal.UpdateRange(tok);
					node->Nodes.push_back(n);
					if (tok.Type != TokenType::SLASH) {
						tree->Errors.push_back(ParseError("Unexpected token '" + replace(tok.Text, "\n", "") + "' found. Expected SLASH", 0x1001, tok));
						return;
					}
					break;
				case TokenType::PERCENT:
					tok = scanner.Scan({TokenType::PERCENT}); // Terminal Rule: PERCENT
					n = node->CreateNode(tok, tok.ToString() );
					node->TokenVal.UpdateRange(tok);
					node->Nodes.push_back(n);
					if (tok.Type != TokenType::PERCENT) {
						tree->Errors.push_back(ParseError("Unexpected token '" + replace(tok.Text, "\n", "") + "' found. Expected PERCENT", 0x1001, tok));
						return;
					}
					break;
				default:
					tree->Errors.push_back(ParseError("Unexpected token '" + replace(tok.Text, "\n", "") + "' found. Expected ASTERIKS, SLASH, or PERCENT.", 0x0002, tok));
					break;
			} // Choice Rule

			 // Concat Rule
			ParsePowerExpression(node); // NonTerminal Rule: PowerExpression
		tok = scanner.LookAhead({TokenType::ASTERIKS, TokenType::SLASH, TokenType::PERCENT}); // ZeroOrMore Rule
		}

		parent->TokenVal.UpdateRange(node->TokenVal);
	} // NonTerminalSymbol: MultiplicativeExpression

	inline ParseTree* Parser::ParseAdditiveExpression(const std::string& input, ParseTree* tree) // NonTerminalSymbol: AdditiveExpression
	{
		scanner.Init(input);
		this->tree = tree;
		ParseAdditiveExpression(tree);
		tree->Skipped = scanner.Skipped;
		return tree;
	}

	inline void Parser::ParseAdditiveExpression(ParseNode* parent) // NonTerminalSymbol: AdditiveExpression
	{
		Token tok;
		ParseNode* n;
		ParseNode* node = parent->CreateNode(scanner.GetToken(TokenType::AdditiveExpression), "AdditiveExpression");
		parent->Nodes.push_back(node);


		 // Concat Rule
		ParseMultiplicativeExpression(node); // NonTerminal Rule: MultiplicativeExpression

		 // Concat Rule
		tok = scanner.LookAhead({TokenType::PLUS, TokenType::MINUS}); // ZeroOrMore Rule
		while (tok.Type == TokenType::PLUS
		    || tok.Type == TokenType::MINUS)
		{

			 // Concat Rule
			tok = scanner.LookAhead({TokenType::PLUS, TokenType::MINUS}); // Choice Rule
			switch (tok.Type)
			{ // Choice Rule
				case TokenType::PLUS:
					tok = scanner.Scan({TokenType::PLUS}); // Terminal Rule: PLUS
					n = node->CreateNode(tok, tok.ToString() );
					node->TokenVal.UpdateRange(tok);
					node->Nodes.push_back(n);
					if (tok.Type != TokenType::PLUS) {
						tree->Errors.push_back(ParseError("Unexpected token '" + replace(tok.Text, "\n", "") + "' found. Expected PLUS", 0x1001, tok));
						return;
					}
					break;
				case TokenType::MINUS:
					tok = scanner.Scan({TokenType::MINUS}); // Terminal Rule: MINUS
					n = node->CreateNode(tok, tok.ToString() );
					node->TokenVal.UpdateRange(tok);
					node->Nodes.push_back(n);
					if (tok.Type != TokenType::MINUS) {
						tree->Errors.push_back(ParseError("Unexpected token '" + replace(tok.Text, "\n", "") + "' found. Expected MINUS", 0x1001, tok));
						return;
					}
					break;
				default:
					tree->Errors.push_back(ParseError("Unexpected token '" + replace(tok.Text, "\n", "") + "' found. Expected PLUS or MINUS.", 0x0002, tok));
					break;
			} // Choice Rule

			 // Concat Rule
			ParseMultiplicativeExpression(node); // NonTerminal Rule: MultiplicativeExpression
		tok = scanner.LookAhead({TokenType::PLUS, TokenType::MINUS}); // ZeroOrMore Rule
		}

		parent->TokenVal.UpdateRange(node->TokenVal);
	} // NonTerminalSymbol: AdditiveExpression

	inline ParseTree* Parser::ParseConcatEpression(const std::string& input, ParseTree* tree) // NonTerminalSymbol: ConcatEpression
	{
		scanner.Init(input);
		this->tree = tree;
		ParseConcatEpression(tree);
		tree->Skipped = scanner.Skipped;
		return tree;
	}

	inline void Parser::ParseConcatEpression(ParseNode* parent) // NonTerminalSymbol: ConcatEpression
	{
		Token tok;
		ParseNode* n;
		ParseNode* node = parent->CreateNode(scanner.GetToken(TokenType::ConcatEpression), "ConcatEpression");
		parent->Nodes.push_back(node);


		 // Concat Rule
		ParseAdditiveExpression(node); // NonTerminal Rule: AdditiveExpression

		 // Concat Rule
		tok = scanner.LookAhead({TokenType::AMP}); // ZeroOrMore Rule
		while (tok.Type == TokenType::AMP)
		{

			 // Concat Rule
			tok = scanner.Scan({TokenType::AMP}); // Terminal Rule: AMP
			n = node->CreateNode(tok, tok.ToString() );
			node->TokenVal.UpdateRange(tok);
			node->Nodes.push_back(n);
			if (tok.Type != TokenType::AMP) {
				tree->Errors.push_back(ParseError("Unexpected token '" + replace(tok.Text, "\n", "") + "' found. Expected AMP", 0x1001, tok));
				return;
			}

			 // Concat Rule
			ParseAdditiveExpression(node); // NonTerminal Rule: AdditiveExpression
		tok = scanner.LookAhead({TokenType::AMP}); // ZeroOrMore Rule
		}

		parent->TokenVal.UpdateRange(node->TokenVal);
	} // NonTerminalSymbol: ConcatEpression

	inline ParseTree* Parser::ParseRelationalExpression(const std::string& input, ParseTree* tree) // NonTerminalSymbol: RelationalExpression
	{
		scanner.Init(input);
		this->tree = tree;
		ParseRelationalExpression(tree);
		tree->Skipped = scanner.Skipped;
		return tree;
	}

	inline void Parser::ParseRelationalExpression(ParseNode* parent) // NonTerminalSymbol: RelationalExpression
	{
		Token tok;
		ParseNode* n;
		ParseNode* node = parent->CreateNode(scanner.GetToken(TokenType::RelationalExpression), "RelationalExpression");
		parent->Nodes.push_back(node);


		 // Concat Rule
		ParseConcatEpression(node); // NonTerminal Rule: ConcatEpression

		 // Concat Rule
		tok = scanner.LookAhead({TokenType::LESSTHAN, TokenType::LESSEQUAL, TokenType::GREATERTHAN, TokenType::GREATEREQUAL}); // Option Rule
		if (tok.Type == TokenType::LESSTHAN
		    || tok.Type == TokenType::LESSEQUAL
		    || tok.Type == TokenType::GREATERTHAN
		    || tok.Type == TokenType::GREATEREQUAL)
		{

			 // Concat Rule
			tok = scanner.LookAhead({TokenType::LESSTHAN, TokenType::LESSEQUAL, TokenType::GREATERTHAN, TokenType::GREATEREQUAL}); // Choice Rule
			switch (tok.Type)
			{ // Choice Rule
				case TokenType::LESSTHAN:
					tok = scanner.Scan({TokenType::LESSTHAN}); // Terminal Rule: LESSTHAN
					n = node->CreateNode(tok, tok.ToString() );
					node->TokenVal.UpdateRange(tok);
					node->Nodes.push_back(n);
					if (tok.Type != TokenType::LESSTHAN) {
						tree->Errors.push_back(ParseError("Unexpected token '" + replace(tok.Text, "\n", "") + "' found. Expected LESSTHAN", 0x1001, tok));
						return;
					}
					break;
				case TokenType::LESSEQUAL:
					tok = scanner.Scan({TokenType::LESSEQUAL}); // Terminal Rule: LESSEQUAL
					n = node->CreateNode(tok, tok.ToString() );
					node->TokenVal.UpdateRange(tok);
					node->Nodes.push_back(n);
					if (tok.Type != TokenType::LESSEQUAL) {
						tree->Errors.push_back(ParseError("Unexpected token '" + replace(tok.Text, "\n", "") + "' found. Expected LESSEQUAL", 0x1001, tok));
						return;
					}
					break;
				case TokenType::GREATERTHAN:
					tok = scanner.Scan({TokenType::GREATERTHAN}); // Terminal Rule: GREATERTHAN
					n = node->CreateNode(tok, tok.ToString() );
					node->TokenVal.UpdateRange(tok);
					node->Nodes.push_back(n);
					if (tok.Type != TokenType::GREATERTHAN) {
						tree->Errors.push_back(ParseError("Unexpected token '" + replace(tok.Text, "\n", "") + "' found. Expected GREATERTHAN", 0x1001, tok));
						return;
					}
					break;
				case TokenType::GREATEREQUAL:
					tok = scanner.Scan({TokenType::GREATEREQUAL}); // Terminal Rule: GREATEREQUAL
					n = node->CreateNode(tok, tok.ToString() );
					node->TokenVal.UpdateRange(tok);
					node->Nodes.push_back(n);
					if (tok.Type != TokenType::GREATEREQUAL) {
						tree->Errors.push_back(ParseError("Unexpected token '" + replace(tok.Text, "\n", "") + "' found. Expected GREATEREQUAL", 0x1001, tok));
						return;
					}
					break;
				default:
					tree->Errors.push_back(ParseError("Unexpected token '" + replace(tok.Text, "\n", "") + "' found. Expected LESSTHAN, LESSEQUAL, GREATERTHAN, or GREATEREQUAL.", 0x0002, tok));
					break;
			} // Choice Rule

			 // Concat Rule
			ParseConcatEpression(node); // NonTerminal Rule: ConcatEpression
		}

		parent->TokenVal.UpdateRange(node->TokenVal);
	} // NonTerminalSymbol: RelationalExpression

	inline ParseTree* Parser::ParseEqualityExpression(const std::string& input, ParseTree* tree) // NonTerminalSymbol: EqualityExpression
	{
		scanner.Init(input);
		this->tree = tree;
		ParseEqualityExpression(tree);
		tree->Skipped = scanner.Skipped;
		return tree;
	}

	inline void Parser::ParseEqualityExpression(ParseNode* parent) // NonTerminalSymbol: EqualityExpression
	{
		Token tok;
		ParseNode* n;
		ParseNode* node = parent->CreateNode(scanner.GetToken(TokenType::EqualityExpression), "EqualityExpression");
		parent->Nodes.push_back(node);


		 // Concat Rule
		ParseRelationalExpression(node); // NonTerminal Rule: RelationalExpression

		 // Concat Rule
		tok = scanner.LookAhead({TokenType::EQUAL, TokenType::NOTEQUAL}); // ZeroOrMore Rule
		while (tok.Type == TokenType::EQUAL
		    || tok.Type == TokenType::NOTEQUAL)
		{

			 // Concat Rule
			tok = scanner.LookAhead({TokenType::EQUAL, TokenType::NOTEQUAL}); // Choice Rule
			switch (tok.Type)
			{ // Choice Rule
				case TokenType::EQUAL:
					tok = scanner.Scan({TokenType::EQUAL}); // Terminal Rule: EQUAL
					n = node->CreateNode(tok, tok.ToString() );
					node->TokenVal.UpdateRange(tok);
					node->Nodes.push_back(n);
					if (tok.Type != TokenType::EQUAL) {
						tree->Errors.push_back(ParseError("Unexpected token '" + replace(tok.Text, "\n", "") + "' found. Expected EQUAL", 0x1001, tok));
						return;
					}
					break;
				case TokenType::NOTEQUAL:
					tok = scanner.Scan({TokenType::NOTEQUAL}); // Terminal Rule: NOTEQUAL
					n = node->CreateNode(tok, tok.ToString() );
					node->TokenVal.UpdateRange(tok);
					node->Nodes.push_back(n);
					if (tok.Type != TokenType::NOTEQUAL) {
						tree->Errors.push_back(ParseError("Unexpected token '" + replace(tok.Text, "\n", "") + "' found. Expected NOTEQUAL", 0x1001, tok));
						return;
					}
					break;
				default:
					tree->Errors.push_back(ParseError("Unexpected token '" + replace(tok.Text, "\n", "") + "' found. Expected EQUAL or NOTEQUAL.", 0x0002, tok));
					break;
			} // Choice Rule

			 // Concat Rule
			ParseRelationalExpression(node); // NonTerminal Rule: RelationalExpression
		tok = scanner.LookAhead({TokenType::EQUAL, TokenType::NOTEQUAL}); // ZeroOrMore Rule
		}

		parent->TokenVal.UpdateRange(node->TokenVal);
	} // NonTerminalSymbol: EqualityExpression

	inline ParseTree* Parser::ParseConditionalAndExpression(const std::string& input, ParseTree* tree) // NonTerminalSymbol: ConditionalAndExpression
	{
		scanner.Init(input);
		this->tree = tree;
		ParseConditionalAndExpression(tree);
		tree->Skipped = scanner.Skipped;
		return tree;
	}

	inline void Parser::ParseConditionalAndExpression(ParseNode* parent) // NonTerminalSymbol: ConditionalAndExpression
	{
		Token tok;
		ParseNode* n;
		ParseNode* node = parent->CreateNode(scanner.GetToken(TokenType::ConditionalAndExpression), "ConditionalAndExpression");
		parent->Nodes.push_back(node);


		 // Concat Rule
		ParseEqualityExpression(node); // NonTerminal Rule: EqualityExpression

		 // Concat Rule
		tok = scanner.LookAhead({TokenType::AMPAMP}); // ZeroOrMore Rule
		while (tok.Type == TokenType::AMPAMP)
		{

			 // Concat Rule
			tok = scanner.Scan({TokenType::AMPAMP}); // Terminal Rule: AMPAMP
			n = node->CreateNode(tok, tok.ToString() );
			node->TokenVal.UpdateRange(tok);
			node->Nodes.push_back(n);
			if (tok.Type != TokenType::AMPAMP) {
				tree->Errors.push_back(ParseError("Unexpected token '" + replace(tok.Text, "\n", "") + "' found. Expected AMPAMP", 0x1001, tok));
				return;
			}

			 // Concat Rule
			ParseEqualityExpression(node); // NonTerminal Rule: EqualityExpression
		tok = scanner.LookAhead({TokenType::AMPAMP}); // ZeroOrMore Rule
		}

		parent->TokenVal.UpdateRange(node->TokenVal);
	} // NonTerminalSymbol: ConditionalAndExpression

	inline ParseTree* Parser::ParseConditionalOrExpression(const std::string& input, ParseTree* tree) // NonTerminalSymbol: ConditionalOrExpression
	{
		scanner.Init(input);
		this->tree = tree;
		ParseConditionalOrExpression(tree);
		tree->Skipped = scanner.Skipped;
		return tree;
	}

	inline void Parser::ParseConditionalOrExpression(ParseNode* parent) // NonTerminalSymbol: ConditionalOrExpression
	{
		Token tok;
		ParseNode* n;
		ParseNode* node = parent->CreateNode(scanner.GetToken(TokenType::ConditionalOrExpression), "ConditionalOrExpression");
		parent->Nodes.push_back(node);


		 // Concat Rule
		ParseConditionalAndExpression(node); // NonTerminal Rule: ConditionalAndExpression

		 // Concat Rule
		tok = scanner.LookAhead({TokenType::PIPEPIPE}); // ZeroOrMore Rule
		while (tok.Type == TokenType::PIPEPIPE)
		{

			 // Concat Rule
			tok = scanner.Scan({TokenType::PIPEPIPE}); // Terminal Rule: PIPEPIPE
			n = node->CreateNode(tok, tok.ToString() );
			node->TokenVal.UpdateRange(tok);
			node->Nodes.push_back(n);
			if (tok.Type != TokenType::PIPEPIPE) {
				tree->Errors.push_back(ParseError("Unexpected token '" + replace(tok.Text, "\n", "") + "' found. Expected PIPEPIPE", 0x1001, tok));
				return;
			}

			 // Concat Rule
			ParseConditionalAndExpression(node); // NonTerminal Rule: ConditionalAndExpression
		tok = scanner.LookAhead({TokenType::PIPEPIPE}); // ZeroOrMore Rule
		}

		parent->TokenVal.UpdateRange(node->TokenVal);
	} // NonTerminalSymbol: ConditionalOrExpression

	inline ParseTree* Parser::ParseAssignmentExpression(const std::string& input, ParseTree* tree) // NonTerminalSymbol: AssignmentExpression
	{
		scanner.Init(input);
		this->tree = tree;
		ParseAssignmentExpression(tree);
		tree->Skipped = scanner.Skipped;
		return tree;
	}

	inline void Parser::ParseAssignmentExpression(ParseNode* parent) // NonTerminalSymbol: AssignmentExpression
	{
		Token tok;
		ParseNode* n;
		ParseNode* node = parent->CreateNode(scanner.GetToken(TokenType::AssignmentExpression), "AssignmentExpression");
		parent->Nodes.push_back(node);


		 // Concat Rule
		ParseConditionalOrExpression(node); // NonTerminal Rule: ConditionalOrExpression

		 // Concat Rule
		tok = scanner.LookAhead({TokenType::QUESTIONMARK}); // Option Rule
		if (tok.Type == TokenType::QUESTIONMARK)
		{

			 // Concat Rule
			tok = scanner.Scan({TokenType::QUESTIONMARK}); // Terminal Rule: QUESTIONMARK
			n = node->CreateNode(tok, tok.ToString() );
			node->TokenVal.UpdateRange(tok);
			node->Nodes.push_back(n);
			if (tok.Type != TokenType::QUESTIONMARK) {
				tree->Errors.push_back(ParseError("Unexpected token '" + replace(tok.Text, "\n", "") + "' found. Expected QUESTIONMARK", 0x1001, tok));
				return;
			}

			 // Concat Rule
			ParseAssignmentExpression(node); // NonTerminal Rule: AssignmentExpression

			 // Concat Rule
			tok = scanner.Scan({TokenType::COLON}); // Terminal Rule: COLON
			n = node->CreateNode(tok, tok.ToString() );
			node->TokenVal.UpdateRange(tok);
			node->Nodes.push_back(n);
			if (tok.Type != TokenType::COLON) {
				tree->Errors.push_back(ParseError("Unexpected token '" + replace(tok.Text, "\n", "") + "' found. Expected COLON", 0x1001, tok));
				return;
			}

			 // Concat Rule
			ParseAssignmentExpression(node); // NonTerminal Rule: AssignmentExpression
		}

		parent->TokenVal.UpdateRange(node->TokenVal);
	} // NonTerminalSymbol: AssignmentExpression

	inline ParseTree* Parser::ParseExpression(const std::string& input, ParseTree* tree) // NonTerminalSymbol: Expression
	{
		scanner.Init(input);
		this->tree = tree;
		ParseExpression(tree);
		tree->Skipped = scanner.Skipped;
		return tree;
	}

	inline void Parser::ParseExpression(ParseNode* parent) // NonTerminalSymbol: Expression
	{
		Token tok;
		ParseNode* n;
		ParseNode* node = parent->CreateNode(scanner.GetToken(TokenType::Expression), "Expression");
		parent->Nodes.push_back(node);


		 // Concat Rule
		tok = scanner.LookAhead({TokenType::FUNCTION, TokenType::VARIABLE, TokenType::BOOLEANLITERAL, TokenType::DECIMALINTEGERLITERAL, TokenType::HEXINTEGERLITERAL, TokenType::REALLITERAL, TokenType::STRINGLITERAL, TokenType::BRACKETOPEN, TokenType::PLUS, TokenType::MINUS, TokenType::NOT}); // Option Rule
		if (tok.Type == TokenType::FUNCTION
		    || tok.Type == TokenType::VARIABLE
		    || tok.Type == TokenType::BOOLEANLITERAL
		    || tok.Type == TokenType::DECIMALINTEGERLITERAL
		    || tok.Type == TokenType::HEXINTEGERLITERAL
		    || tok.Type == TokenType::REALLITERAL
		    || tok.Type == TokenType::STRINGLITERAL
		    || tok.Type == TokenType::BRACKETOPEN
		    || tok.Type == TokenType::PLUS
		    || tok.Type == TokenType::MINUS
		    || tok.Type == TokenType::NOT)
		{
			ParseAssignmentExpression(node); // NonTerminal Rule: AssignmentExpression
		}

		 // Concat Rule
		tok = scanner.LookAhead({TokenType::ASSIGN}); // Option Rule
		if (tok.Type == TokenType::ASSIGN)
		{

			 // Concat Rule
			tok = scanner.Scan({TokenType::ASSIGN}); // Terminal Rule: ASSIGN
			n = node->CreateNode(tok, tok.ToString() );
			node->TokenVal.UpdateRange(tok);
			node->Nodes.push_back(n);
			if (tok.Type != TokenType::ASSIGN) {
				tree->Errors.push_back(ParseError("Unexpected token '" + replace(tok.Text, "\n", "") + "' found. Expected ASSIGN", 0x1001, tok));
				return;
			}

			 // Concat Rule
			ParseAssignmentExpression(node); // NonTerminal Rule: AssignmentExpression
		}

		parent->TokenVal.UpdateRange(node->TokenVal);
	} // NonTerminalSymbol: Expression

	inline ParseTree* Parser::ParseParams(const std::string& input, ParseTree* tree) // NonTerminalSymbol: Params
	{
		scanner.Init(input);
		this->tree = tree;
		ParseParams(tree);
		tree->Skipped = scanner.Skipped;
		return tree;
	}

	inline void Parser::ParseParams(ParseNode* parent) // NonTerminalSymbol: Params
	{
		Token tok;
		ParseNode* n;
		ParseNode* node = parent->CreateNode(scanner.GetToken(TokenType::Params), "Params");
		parent->Nodes.push_back(node);


		 // Concat Rule
		ParseExpression(node); // NonTerminal Rule: Expression

		 // Concat Rule
		tok = scanner.LookAhead({TokenType::SEMICOLON}); // ZeroOrMore Rule
		while (tok.Type == TokenType::SEMICOLON)
		{

			 // Concat Rule
			tok = scanner.Scan({TokenType::SEMICOLON}); // Terminal Rule: SEMICOLON
			n = node->CreateNode(tok, tok.ToString() );
			node->TokenVal.UpdateRange(tok);
			node->Nodes.push_back(n);
			if (tok.Type != TokenType::SEMICOLON) {
				tree->Errors.push_back(ParseError("Unexpected token '" + replace(tok.Text, "\n", "") + "' found. Expected SEMICOLON", 0x1001, tok));
				return;
			}

			 // Concat Rule
			ParseExpression(node); // NonTerminal Rule: Expression
		tok = scanner.LookAhead({TokenType::SEMICOLON}); // ZeroOrMore Rule
		}

		parent->TokenVal.UpdateRange(node->TokenVal);
	} // NonTerminalSymbol: Params

	inline ParseTree* Parser::ParseLiteral(const std::string& input, ParseTree* tree) // NonTerminalSymbol: Literal
	{
		scanner.Init(input);
		this->tree = tree;
		ParseLiteral(tree);
		tree->Skipped = scanner.Skipped;
		return tree;
	}

	inline void Parser::ParseLiteral(ParseNode* parent) // NonTerminalSymbol: Literal
	{
		Token tok;
		ParseNode* n;
		ParseNode* node = parent->CreateNode(scanner.GetToken(TokenType::Literal), "Literal");
		parent->Nodes.push_back(node);

		tok = scanner.LookAhead({TokenType::BOOLEANLITERAL, TokenType::DECIMALINTEGERLITERAL, TokenType::HEXINTEGERLITERAL, TokenType::REALLITERAL, TokenType::STRINGLITERAL}); // Choice Rule
		switch (tok.Type)
		{ // Choice Rule
			case TokenType::BOOLEANLITERAL:
				tok = scanner.Scan({TokenType::BOOLEANLITERAL}); // Terminal Rule: BOOLEANLITERAL
				n = node->CreateNode(tok, tok.ToString() );
				node->TokenVal.UpdateRange(tok);
				node->Nodes.push_back(n);
				if (tok.Type != TokenType::BOOLEANLITERAL) {
					tree->Errors.push_back(ParseError("Unexpected token '" + replace(tok.Text, "\n", "") + "' found. Expected BOOLEANLITERAL", 0x1001, tok));
					return;
				}
				break;
			case TokenType::DECIMALINTEGERLITERAL:
			case TokenType::HEXINTEGERLITERAL:
				ParseIntegerLiteral(node); // NonTerminal Rule: IntegerLiteral
				break;
			case TokenType::REALLITERAL:
				ParseRealLiteral(node); // NonTerminal Rule: RealLiteral
				break;
			case TokenType::STRINGLITERAL:
				ParseStringLiteral(node); // NonTerminal Rule: StringLiteral
				break;
			default:
				tree->Errors.push_back(ParseError("Unexpected token '" + replace(tok.Text, "\n", "") + "' found. Expected BOOLEANLITERAL, DECIMALINTEGERLITERAL, HEXINTEGERLITERAL, REALLITERAL, or STRINGLITERAL.", 0x0002, tok));
				break;
		} // Choice Rule

		parent->TokenVal.UpdateRange(node->TokenVal);
	} // NonTerminalSymbol: Literal

	inline ParseTree* Parser::ParseIntegerLiteral(const std::string& input, ParseTree* tree) // NonTerminalSymbol: IntegerLiteral
	{
		scanner.Init(input);
		this->tree = tree;
		ParseIntegerLiteral(tree);
		tree->Skipped = scanner.Skipped;
		return tree;
	}

	inline void Parser::ParseIntegerLiteral(ParseNode* parent) // NonTerminalSymbol: IntegerLiteral
	{
		Token tok;
		ParseNode* n;
		ParseNode* node = parent->CreateNode(scanner.GetToken(TokenType::IntegerLiteral), "IntegerLiteral");
		parent->Nodes.push_back(node);

		tok = scanner.LookAhead({TokenType::DECIMALINTEGERLITERAL, TokenType::HEXINTEGERLITERAL}); // Choice Rule
		switch (tok.Type)
		{ // Choice Rule
			case TokenType::DECIMALINTEGERLITERAL:
				tok = scanner.Scan({TokenType::DECIMALINTEGERLITERAL}); // Terminal Rule: DECIMALINTEGERLITERAL
				n = node->CreateNode(tok, tok.ToString() );
				node->TokenVal.UpdateRange(tok);
				node->Nodes.push_back(n);
				if (tok.Type != TokenType::DECIMALINTEGERLITERAL) {
					tree->Errors.push_back(ParseError("Unexpected token '" + replace(tok.Text, "\n", "") + "' found. Expected DECIMALINTEGERLITERAL", 0x1001, tok));
					return;
				}
				break;
			case TokenType::HEXINTEGERLITERAL:
				tok = scanner.Scan({TokenType::HEXINTEGERLITERAL}); // Terminal Rule: HEXINTEGERLITERAL
				n = node->CreateNode(tok, tok.ToString() );
				node->TokenVal.UpdateRange(tok);
				node->Nodes.push_back(n);
				if (tok.Type != TokenType::HEXINTEGERLITERAL) {
					tree->Errors.push_back(ParseError("Unexpected token '" + replace(tok.Text, "\n", "") + "' found. Expected HEXINTEGERLITERAL", 0x1001, tok));
					return;
				}
				break;
			default:
				tree->Errors.push_back(ParseError("Unexpected token '" + replace(tok.Text, "\n", "") + "' found. Expected DECIMALINTEGERLITERAL or HEXINTEGERLITERAL.", 0x0002, tok));
				break;
		} // Choice Rule

		parent->TokenVal.UpdateRange(node->TokenVal);
	} // NonTerminalSymbol: IntegerLiteral

	inline ParseTree* Parser::ParseRealLiteral(const std::string& input, ParseTree* tree) // NonTerminalSymbol: RealLiteral
	{
		scanner.Init(input);
		this->tree = tree;
		ParseRealLiteral(tree);
		tree->Skipped = scanner.Skipped;
		return tree;
	}

	inline void Parser::ParseRealLiteral(ParseNode* parent) // NonTerminalSymbol: RealLiteral
	{
		Token tok;
		ParseNode* n;
		ParseNode* node = parent->CreateNode(scanner.GetToken(TokenType::RealLiteral), "RealLiteral");
		parent->Nodes.push_back(node);

		tok = scanner.Scan({TokenType::REALLITERAL}); // Terminal Rule: REALLITERAL
		n = node->CreateNode(tok, tok.ToString() );
		node->TokenVal.UpdateRange(tok);
		node->Nodes.push_back(n);
		if (tok.Type != TokenType::REALLITERAL) {
			tree->Errors.push_back(ParseError("Unexpected token '" + replace(tok.Text, "\n", "") + "' found. Expected REALLITERAL", 0x1001, tok));
			return;
		}

		parent->TokenVal.UpdateRange(node->TokenVal);
	} // NonTerminalSymbol: RealLiteral

	inline ParseTree* Parser::ParseStringLiteral(const std::string& input, ParseTree* tree) // NonTerminalSymbol: StringLiteral
	{
		scanner.Init(input);
		this->tree = tree;
		ParseStringLiteral(tree);
		tree->Skipped = scanner.Skipped;
		return tree;
	}

	inline void Parser::ParseStringLiteral(ParseNode* parent) // NonTerminalSymbol: StringLiteral
	{
		Token tok;
		ParseNode* n;
		ParseNode* node = parent->CreateNode(scanner.GetToken(TokenType::StringLiteral), "StringLiteral");
		parent->Nodes.push_back(node);

		tok = scanner.Scan({TokenType::STRINGLITERAL}); // Terminal Rule: STRINGLITERAL
		n = node->CreateNode(tok, tok.ToString() );
		node->TokenVal.UpdateRange(tok);
		node->Nodes.push_back(n);
		if (tok.Type != TokenType::STRINGLITERAL) {
			tree->Errors.push_back(ParseError("Unexpected token '" + replace(tok.Text, "\n", "") + "' found. Expected STRINGLITERAL", 0x1001, tok));
			return;
		}

		parent->TokenVal.UpdateRange(node->TokenVal);
	} // NonTerminalSymbol: StringLiteral

	inline ParseTree* Parser::ParseVariable(const std::string& input, ParseTree* tree) // NonTerminalSymbol: Variable
	{
		scanner.Init(input);
		this->tree = tree;
		ParseVariable(tree);
		tree->Skipped = scanner.Skipped;
		return tree;
	}

	inline void Parser::ParseVariable(ParseNode* parent) // NonTerminalSymbol: Variable
	{
		Token tok;
		ParseNode* n;
		ParseNode* node = parent->CreateNode(scanner.GetToken(TokenType::Variable), "Variable");
		parent->Nodes.push_back(node);

		tok = scanner.Scan({TokenType::VARIABLE}); // Terminal Rule: VARIABLE
		n = node->CreateNode(tok, tok.ToString() );
		node->TokenVal.UpdateRange(tok);
		node->Nodes.push_back(n);
		if (tok.Type != TokenType::VARIABLE) {
			tree->Errors.push_back(ParseError("Unexpected token '" + replace(tok.Text, "\n", "") + "' found. Expected VARIABLE", 0x1001, tok));
			return;
		}

		parent->TokenVal.UpdateRange(node->TokenVal);
	} // NonTerminalSymbol: Variable



}
