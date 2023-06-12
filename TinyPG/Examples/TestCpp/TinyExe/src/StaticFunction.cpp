#include "StaticFunction.h"

namespace TinyExe
{
	StaticFunction::StaticFunction(const std::string& name, FunctionDelegate function, int minParameters, int maxParameters)
	{
		Name = name;
		functionDelegate = function;
		MinParameters = minParameters;
		MaxParameters = maxParameters;
		Arguments = new Variables();
	}

	StaticFunction::StaticFunction(const std::string& name, FunctionContextDelegate function, int minParameters, int maxParameters)
	{
		Name = name;
		functionContextDelegate = function;
		MinParameters = minParameters;
		MaxParameters = maxParameters;
		Arguments = new Variables();
	}

	std::any StaticFunction::Eval(std::vector<std::any> parameters, ParseTree* tree)
	{
		tree->Context->PushScope(NULL);

		std::any result;
		if (functionDelegate != NULL)
			result = functionDelegate(parameters);
		else if (functionContextDelegate != NULL)
			result = functionContextDelegate(parameters, tree->Context);
		tree->Context->PopScope();
		return result;
	}

}