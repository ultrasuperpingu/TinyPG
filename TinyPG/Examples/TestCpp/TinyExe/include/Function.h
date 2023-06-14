#pragma once
#include <iostream>
#include <map>
#include <vector>
#include "Variables.h"
#include <functional>

namespace TinyExe
{
	class Context;
	typedef std::function<std::any(std::vector<std::any>)> FunctionDelegate;
	typedef std::function<std::any(std::vector<std::any>, Context*)> FunctionContextDelegate;

	class ParseTree;
	class ParseNode;
	class Function
	{
	public:
		Function();
		/// <summary>
		/// define the arguments of the dynamic function
		/// </summary>
		Variables* Arguments;

		/// <summary>
		/// name of the function
		/// </summary>
		std::string Name;

		/// <summary>
		/// minimum number of allowed parameters (default = 0)
		/// </summary>
		int MaxParameters;

		/// <summary>
		/// maximum number of allowed parameters (default = 0)
		/// </summary>
		int MinParameters;

		virtual std::any Eval(std::vector<std::any> parameters, ParseTree* tree) = 0;
		virtual bool IsDynamic();

	};

	inline Function::Function() : Arguments(NULL), Name(""), MaxParameters(0), MinParameters(0)
	{
	}

	inline bool Function::IsDynamic()
	{
		return false;
	}

}