#pragma once

#include "Function.h"

namespace TinyExe
{
	class ParseTree;
	class ParseNode;
	class DynamicFunction : public Function
	{
	private:
		/// <summary>
		/// points to the RHS of the assignment of this function
		/// this branch will be evaluated each time this function is executed
		/// </summary>
		ParseNode* Node;
	public:
		DynamicFunction(const std::string& name, ParseNode* node, Variables* args, int minParameters, int maxParameters);
		/// <summary>
		/// the list of parameters must correspond the the required set of Arguments
		/// </summary>
		/// <param name="parameters"></param>
		/// <returns></returns>
		std::any Eval(std::vector<std::any> parameters, ParseTree* tree);

		virtual bool IsDynamic();
	};
	inline bool DynamicFunction::IsDynamic()
	{
		return true;
	}
}