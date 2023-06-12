#pragma once
#include <vector>
#include <string>
#include "Variables.h"
#include "Functions.h"
#include "Util.h"

namespace TinyExe
{
	class Context
	{
	public:
		static Context Default;

		// list of functions currently in scope during an evaluation
		// note that this typically is NOT thread safe.

		// contains a list of variables that is in scope. Scope is used only for DynamicFunctions (for now)
	private:
		std::vector<Variables*> inScope;

	public:
		Functions Functions;
		Variables Globals;

		/// <summary>
		/// check current stacksize
		/// is used for debugging purposes and error handling
		/// to prevent stackoverflows
		/// </summary>
		int CurrentStackSize;

		Context();

		Variables* getCurrentScope();

		/// <summary>
		/// Traverse all the local scopes and searches for the specified variable
		/// </summary>
		/// <returns>The scoped variable.</returns>
		/// <param name="key">Key.</param>
		std::any GetScopeVariable(std::string key);

		void PushScope(Variables* vars);

		Variables* PopScope();

		/// <summary>
		/// resets the context to its defaults
		/// </summary> 
		void Reset();

		/// <summary>
		/// Thats not a Deep clone!
		/// Functions and Globals are shared!
		/// Delete is not managed...
		/// </summary>
		Context* Clone();
	};
}