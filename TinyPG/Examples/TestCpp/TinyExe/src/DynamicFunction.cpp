#include "DynamicFunction.h"
#include "ParseTree.h"

namespace TinyExe
{
	std::any DynamicFunction::Eval(std::vector<std::any> parameters, ParseTree* tree)
	{
		// create a new scope for the arguments
		Variables* pars = Arguments->Clone();
		// now push a copy of the function arguments on the stack
		tree->Context->PushScope(pars);

		// assign the parameters to the current function scope variables
		int i = 0;
		
		for(auto it=pars->begin();it != pars->end();it++)
			it->second = parameters[i++];

		// execute the function here
		std::any result = Node->Eval({ tree });

		// clean up the stack
		tree->Context->PopScope();

		return result;
	}

	DynamicFunction::DynamicFunction(const std::string& name, ParseNode* node, Variables* args, int minParameters, int maxParameters)
	{
		Node = node;
		Arguments = args;
		MinParameters = minParameters;
		MaxParameters = maxParameters;
	}
}