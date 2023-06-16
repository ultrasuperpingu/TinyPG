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

	};

}
