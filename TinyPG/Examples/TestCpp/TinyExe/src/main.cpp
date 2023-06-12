#include <iostream>
#include <any>
#include "Parser.h"
#include "Context.h"

using namespace TinyExe;

int main()
{
	Context* context = new Context();
	context->Globals.insert(std::make_pair("testDouble", 2.5));
	context->Globals.insert(std::make_pair("testInt", 5));
	std::cout << "Enter an expression (empty to exit):" << std::endl;
	std::string expression = "5*3+(testInt / testDouble)/2";
	std::cout << "> " << expression << std::endl;
	Scanner s = Scanner();
	while (expression != "")
	{
		Parser* p = new Parser(s);
		ParseTree* tree = p->Parse(expression);
		if (tree->Errors.size() > 0)
		{
			for(auto e : tree->Errors)
			{
				std::cout << "Col " << e.Column << ": " << e.Message << std::endl;
			}
		}
		else
		{
			tree->Context = context;
			std::string res = ConvertToString(tree->Eval<std::any>({tree}));
			std::cout << "< " << res << std::endl;
		}
		std::cout << "> ";
		std::getline(std::cin, expression);
	}
	return 0;
}
