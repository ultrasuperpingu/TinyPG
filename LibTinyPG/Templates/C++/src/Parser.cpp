// Automatically generated from source file: <%SourceFilename%>
// By TinyPG v1.5 available at https://github.com/ultrasuperpingu/TinyPG

#include "Parser.h"

namespace <%Namespace%>
{

	void Parser::DeleteTree()
	{
		if (instanciatedTree != NULL)
		{
			delete instanciatedTree;
			instanciatedTree = NULL;
		}
	}

	Parser::Parser(Scanner& scanner) : scanner(scanner), tree(NULL), instanciatedTree(NULL)
	{
	}

	Parser::~Parser()
	{
		DeleteTree();
	}

	ParseTree* Parser::Parse(const std::string& input)
	{
		DeleteTree();
		instanciatedTree = new ParseTree();
		return Parse(input, new ParseTree());
	}

	ParseTree* Parser::Parse(const std::string& input, ParseTree* tree)
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
