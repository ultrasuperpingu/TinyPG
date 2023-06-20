#include <iostream>
#include "Parser.h"

int main()
{
	TinyPG::Scanner s = TinyPG::Scanner();
	TinyPG::Parser p = TinyPG::Parser(s);
	std::string expr = "(_5 * 3) + _15 / (4 - 2)";
	TinyPG::ParseTree* tree = p.Parse(expr);
	std::map<std::string, int> context;
	context.insert(std::pair<std::string, int>("_5", 5));
	context.insert(std::pair<std::string, int>("_15", 15));
	tree->setContext(&context);

	for (int i = 0; i < (int)tree->Errors.size(); i++)
	{
		std::cerr << "error: " << tree->Errors[i].Line << ","<< tree->Errors[i].Column<< " : " << tree->Errors[i].Message << std::endl;
	}
	if(tree->Errors.size() == 0)
		std::cerr << expr << " = " << tree->Eval<int>({}) << std::endl;
	return 0;
}