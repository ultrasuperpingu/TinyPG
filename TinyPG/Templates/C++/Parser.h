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
		inline void DeleteTree()
		{
			if (instanciatedTree != NULL)
			{
				delete instanciatedTree;
				instanciatedTree = NULL;
			}
		}
	public:
		inline Parser(Scanner& scanner) : scanner(scanner), tree(NULL), instanciatedTree(NULL)
		{
		}

		virtual inline ~Parser()
		{
			DeleteTree();
		}

		inline <%IParseTree%>* Parse(const std::string& input)
		{
			DeleteTree();
			instanciatedTree = new ParseTree();
			return Parse(input, "", instanciatedTree);
		}

		inline <%IParseTree%>* Parse(const std::string& input, const std::string& fileName)
		{
			DeleteTree();
			instanciatedTree = new ParseTree();
			return Parse(input, fileName, new ParseTree());
		}

		inline <%IParseTree%>* Parse(const std::string& input, const std::string& fileName, ParseTree* tree)
		{
			scanner.Init(input, fileName);
			if (tree != instanciatedTree)
				DeleteTree();
			this->tree = tree;
			ParseStart(tree);
			tree->Skipped = scanner.Skipped;

			return tree;
		}

	protected:
<%ParseNonTerminals%>

<%ParserCustomCode%>
	};

}
