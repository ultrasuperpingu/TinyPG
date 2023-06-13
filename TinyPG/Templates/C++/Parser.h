// Automatically generated from source file: <%SourceFilename%>
// By TinyPG v1.5 available at https://github.com/ultrasuperpingu/TinyPG

#pragma once
#include <string>
#include "Scanner.h"
#include "ParseTree.h"

namespace <%Namespace%>
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

<%ParseNonTerminalsDecl%>

<%ParserCustomCode%>
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

<%ParseNonTerminalsImpl%>

}
