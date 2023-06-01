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

	public:
		inline Parser(Scanner& scanner) : scanner(scanner), tree(NULL)
		{
		}

		virtual inline ~Parser()
		{
			if (tree != NULL)
			{
				delete tree;
				tree = NULL;
			}
		}


		inline <%IParseTree%> Parse(std::string input)
		{
			return Parse(input, "", new ParseTree());
		}

		inline <%IParseTree%> Parse(std::string input, std::string fileName)
		{
			return Parse(input, fileName, new ParseTree());
		}

		inline <%IParseTree%>& Parse(std::string input, std::string fileName, ParseTree* tree)
		{
			scanner.Init(input, fileName);

			this->tree = tree;
			ParseStart(tree);
			tree->Skipped = scanner.Skipped;

			return *tree;
		}

<%ParseNonTerminals%>

<%ParserCustomCode%>
	};

}
