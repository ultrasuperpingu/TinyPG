#pragma once
#include "Function.h"
#include "ParseTree.h"


namespace TinyExe
{
	class StaticFunction : public Function
	{
	protected:
		/// <summary>
		/// the actual function implementation
		/// </summary>
		FunctionDelegate functionDelegate;

		FunctionContextDelegate functionContextDelegate;


	public:
		StaticFunction(const std::string& name, FunctionDelegate function, int minParameters, int maxParameters);
		StaticFunction(const std::string& name, FunctionContextDelegate function, int minParameters, int maxParameters);

		std::any Eval(std::vector<std::any> parameters, ParseTree* tree);

	};

}