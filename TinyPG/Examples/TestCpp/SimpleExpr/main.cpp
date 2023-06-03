#include <iostream>
#include "Parser.h"

int main()
{
	TinyPG::Scanner s = TinyPG::Scanner();
	TinyPG::Parser p = TinyPG::Parser(s);
	TinyPG::ParseTree* tree = p.Parse("(_5 + 3) + _15 / (4 - 2)");
	std::map<std::string, int> context;
	context.insert(std::pair<std::string, int>("_5", 5));
	context.insert(std::pair<std::string, int>("_15", 15));
	tree->setContext(&context);

	std::cerr << "errors: " << tree->Errors.size() << std::endl;
	for (int i = 0; i < tree->Errors.size(); i++)
	{
		std::cerr << "\terror: " << tree->Errors[i].Line << ","<< tree->Errors[i].Column<< " : " << tree->Errors[i].Message << std::endl;
	}
	std::cerr << tree->Nodes[0]->EvalStart(*tree, {}) << std::endl;
	return 0;
}