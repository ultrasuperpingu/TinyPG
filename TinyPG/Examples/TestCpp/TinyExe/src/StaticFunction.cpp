#include "../include/StaticFunction.h"

namespace TinyExe
{
	/// <summary>
	/// the actual function implementation
	/// </summary>
	//public FunctionDelegate FunctionDelegate{ get; private set; }

	//public FunctionContextDelegate FunctionContextDelegate{ get; private set; }

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

	StaticFunction::StaticFunction(std::string name, FunctionDelegate function, int minParameters, int maxParameters)
	{
		Name = name;
		functionDelegate = function;
		MinParameters = minParameters;
		MaxParameters = maxParameters;
		Arguments = new Variables();
	}

	StaticFunction::StaticFunction(std::string name, FunctionContextDelegate function, int minParameters, int maxParameters)
	{
		Name = name;
		functionContextDelegate = function;
		MinParameters = minParameters;
		MaxParameters = maxParameters;
		Arguments = new Variables();
	}
}