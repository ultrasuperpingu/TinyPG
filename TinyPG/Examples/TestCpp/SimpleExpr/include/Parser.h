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

	public:
		ParseTree* ParseStart(const std::string& input, ParseTree* tree);
	protected:
		void ParseStart(ParseNode* parent);
	public:
		ParseTree* ParseAddExpr(const std::string& input, ParseTree* tree);
	protected:
		void ParseAddExpr(ParseNode* parent);
	public:
		ParseTree* ParseMultExpr(const std::string& input, ParseTree* tree);
	protected:
		void ParseMultExpr(ParseNode* parent);
	public:
		ParseTree* ParseAtom(const std::string& input, ParseTree* tree);
	protected:
		void ParseAtom(ParseNode* parent);


	};

}
