// Automatically generated from source file: simple expression2_cpp.tpg
// By TinyPG v1.5 available at https://github.com/ultrasuperpingu/TinyPG

#pragma once
#include <string>
#include "Scanner.h"
#include "ParseTree.h"

namespace TinyPG
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

	protected:
		void ParseStart(ParseNode* parent);
		void ParseAddExpr(ParseNode* parent);
		void ParseMultExpr(ParseNode* parent);
		void ParseAtom(ParseNode* parent);



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

	inline void Parser::ParseStart(ParseNode* parent) // NonTerminalSymbol: Start
	{
		Token tok;
		ParseNode* n;
		ParseNode* node = parent->CreateNode(scanner.GetToken(TokenType::Start), "Start");
		parent->Nodes.push_back(node);


		 // Concat Rule
		tok = scanner.LookAhead({TokenType::NUMBER, TokenType::BROPEN, TokenType::ID}); // Option Rule
		if (tok.Type == TokenType::NUMBER
		    || tok.Type == TokenType::BROPEN
		    || tok.Type == TokenType::ID)
		{
			ParseAddExpr(node); // NonTerminal Rule: AddExpr
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

	inline void Parser::ParseAddExpr(ParseNode* parent) // NonTerminalSymbol: AddExpr
	{
		Token tok;
		ParseNode* n;
		ParseNode* node = parent->CreateNode(scanner.GetToken(TokenType::AddExpr), "AddExpr");
		parent->Nodes.push_back(node);


		 // Concat Rule
		ParseMultExpr(node); // NonTerminal Rule: MultExpr

		 // Concat Rule
		tok = scanner.LookAhead({TokenType::PLUSMINUS}); // ZeroOrMore Rule
		while (tok.Type == TokenType::PLUSMINUS)
		{

			 // Concat Rule
			tok = scanner.Scan({TokenType::PLUSMINUS}); // Terminal Rule: PLUSMINUS
			n = node->CreateNode(tok, tok.ToString() );
			node->TokenVal.UpdateRange(tok);
			node->Nodes.push_back(n);
			if (tok.Type != TokenType::PLUSMINUS) {
				tree->Errors.push_back(ParseError("Unexpected token '" + replace(tok.Text, "\n", "") + "' found. Expected PLUSMINUS", 0x1001, tok));
				return;
			}

			 // Concat Rule
			ParseMultExpr(node); // NonTerminal Rule: MultExpr
		tok = scanner.LookAhead({TokenType::PLUSMINUS}); // ZeroOrMore Rule
		}

		parent->TokenVal.UpdateRange(node->TokenVal);
	} // NonTerminalSymbol: AddExpr

	inline void Parser::ParseMultExpr(ParseNode* parent) // NonTerminalSymbol: MultExpr
	{
		Token tok;
		ParseNode* n;
		ParseNode* node = parent->CreateNode(scanner.GetToken(TokenType::MultExpr), "MultExpr");
		parent->Nodes.push_back(node);


		 // Concat Rule
		ParseAtom(node); // NonTerminal Rule: Atom

		 // Concat Rule
		tok = scanner.LookAhead({TokenType::MULTDIV}); // ZeroOrMore Rule
		while (tok.Type == TokenType::MULTDIV)
		{

			 // Concat Rule
			tok = scanner.Scan({TokenType::MULTDIV}); // Terminal Rule: MULTDIV
			n = node->CreateNode(tok, tok.ToString() );
			node->TokenVal.UpdateRange(tok);
			node->Nodes.push_back(n);
			if (tok.Type != TokenType::MULTDIV) {
				tree->Errors.push_back(ParseError("Unexpected token '" + replace(tok.Text, "\n", "") + "' found. Expected MULTDIV", 0x1001, tok));
				return;
			}

			 // Concat Rule
			ParseAtom(node); // NonTerminal Rule: Atom
		tok = scanner.LookAhead({TokenType::MULTDIV}); // ZeroOrMore Rule
		}

		parent->TokenVal.UpdateRange(node->TokenVal);
	} // NonTerminalSymbol: MultExpr

	inline void Parser::ParseAtom(ParseNode* parent) // NonTerminalSymbol: Atom
	{
		Token tok;
		ParseNode* n;
		ParseNode* node = parent->CreateNode(scanner.GetToken(TokenType::Atom), "Atom");
		parent->Nodes.push_back(node);

		tok = scanner.LookAhead({TokenType::NUMBER, TokenType::BROPEN, TokenType::ID}); // Choice Rule
		switch (tok.Type)
		{ // Choice Rule
			case TokenType::NUMBER:
				tok = scanner.Scan({TokenType::NUMBER}); // Terminal Rule: NUMBER
				n = node->CreateNode(tok, tok.ToString() );
				node->TokenVal.UpdateRange(tok);
				node->Nodes.push_back(n);
				if (tok.Type != TokenType::NUMBER) {
					tree->Errors.push_back(ParseError("Unexpected token '" + replace(tok.Text, "\n", "") + "' found. Expected NUMBER", 0x1001, tok));
					return;
				}
				break;
			case TokenType::BROPEN:

				 // Concat Rule
				tok = scanner.Scan({TokenType::BROPEN}); // Terminal Rule: BROPEN
				n = node->CreateNode(tok, tok.ToString() );
				node->TokenVal.UpdateRange(tok);
				node->Nodes.push_back(n);
				if (tok.Type != TokenType::BROPEN) {
					tree->Errors.push_back(ParseError("Unexpected token '" + replace(tok.Text, "\n", "") + "' found. Expected BROPEN", 0x1001, tok));
					return;
				}

				 // Concat Rule
				ParseAddExpr(node); // NonTerminal Rule: AddExpr

				 // Concat Rule
				tok = scanner.Scan({TokenType::BRCLOSE}); // Terminal Rule: BRCLOSE
				n = node->CreateNode(tok, tok.ToString() );
				node->TokenVal.UpdateRange(tok);
				node->Nodes.push_back(n);
				if (tok.Type != TokenType::BRCLOSE) {
					tree->Errors.push_back(ParseError("Unexpected token '" + replace(tok.Text, "\n", "") + "' found. Expected BRCLOSE", 0x1001, tok));
					return;
				}
				break;
			case TokenType::ID:
				tok = scanner.Scan({TokenType::ID}); // Terminal Rule: ID
				n = node->CreateNode(tok, tok.ToString() );
				node->TokenVal.UpdateRange(tok);
				node->Nodes.push_back(n);
				if (tok.Type != TokenType::ID) {
					tree->Errors.push_back(ParseError("Unexpected token '" + replace(tok.Text, "\n", "") + "' found. Expected ID", 0x1001, tok));
					return;
				}
				break;
			default:
				tree->Errors.push_back(ParseError("Unexpected token '" + replace(tok.Text, "\n", "") + "' found. Expected NUMBER, BROPEN, or ID.", 0x0002, tok));
				break;
		} // Choice Rule

		parent->TokenVal.UpdateRange(node->TokenVal);
	} // NonTerminalSymbol: Atom



}
