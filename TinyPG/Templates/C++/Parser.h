// Automatically generated from source file: <%SourceFilename%>
// By TinyPG v1.5 available at https://github.com/ultrasuperpingu/TinyPG

#pragma once
#include <string>
#include "Scanner.h"
#include "ParseTree.h"

namespace <%Namespace%>
{
	class Parser <%IParser%>
	{
	private:
		Scanner& scanner;
		ParseTree* tree;
		ParseTree* instanciatedTree;
		void DeleteTree();
	public:
		Parser(Scanner& scanner);
		virtual ~Parser();

		<%IParseTree%>* Parse(const std::string& input);
		<%IParseTree%>* Parse(const std::string& input, const std::string& fileName);
		<%IParseTree%>* Parse(const std::string& input, const std::string& fileName, ParseTree* tree);

	protected:
<%ParseNonTerminals%>

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

	inline <%IParseTree%>* Parser::Parse(const std::string& input)
	{
		DeleteTree();
		instanciatedTree = new ParseTree();
		return Parse(input, "", instanciatedTree);
	}

	inline <%IParseTree%>* Parser::Parse(const std::string& input, const std::string& fileName)
	{
		DeleteTree();
		instanciatedTree = new ParseTree();
		return Parse(input, fileName, new ParseTree());
	}

	inline <%IParseTree%>* Parser::Parse(const std::string& input, const std::string& fileName, ParseTree* tree)
	{
		scanner.Init(input, fileName);
		if (tree != instanciatedTree)
			DeleteTree();
		this->tree = tree;
		ParseStart(tree);
		tree->Skipped = scanner.Skipped;

		return tree;
	}
}
