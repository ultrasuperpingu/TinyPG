#pragma once
#include <map>
#include <any>
#include <string>

namespace TinyExe
{
	class Variables : public std::map<std::string, std::any>
	{
	public:
		/// <summary>
		/// clones this set of variables
		/// this is required in order to support local scope and recursion
		/// a copy of the set of variables (arguments in a function) will be pushed on the scope stack
		/// </summary>
		/// <returns></returns>
		Variables* Clone();
	};
}