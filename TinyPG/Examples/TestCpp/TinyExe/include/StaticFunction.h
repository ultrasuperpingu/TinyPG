#pragma once
#include "Function.h"
#include "ParseTree.h"


namespace TinyExe
{
	class StaticFunction : Function
	{
	protected:
		/// <summary>
		/// the actual function implementation
		/// </summary>
		FunctionDelegate functionDelegate;

		FunctionContextDelegate functionContextDelegate;


	public:
		std::any Eval(std::vector<std::any> parameters, ParseTree* tree);

		StaticFunction(std::string name, FunctionDelegate function, int minParameters, int maxParameters);

		StaticFunction(std::string name, FunctionContextDelegate function, int minParameters, int maxParameters);
	};

}