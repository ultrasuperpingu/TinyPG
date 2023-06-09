#include <iostream>
#include <any>
#include "../include/Parser.h"
#include "../include/Context.h"

namespace TinyExe
{
	void main()
	{
		Context* context = new Context();
		context->Globals.insert(std::make_pair("testDouble", 0.1));
		context->Globals.insert(std::make_pair("testInt", 5));
		std::cout << "Enter an expression (empty to exit):" << std::endl;
		std::string expression = "5*3+(testInt * testDouble)/2";
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
				double res = tree->Eval<double>({});
				std::cout << "< " << res;
			}
			std::cout << "> ";
			std::cin >> expression;
		}
	}
	
}
