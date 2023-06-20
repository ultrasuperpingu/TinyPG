#pragma once
#include "Context.h"

namespace TinyExe
{
	Variables* Context::getCurrentScope()
	{
		if (inScope.size() <= 0)
			return NULL;

		return inScope[inScope.size() - 1];
	}

	std::any Context::GetScopeVariable(std::string key)
	{
		for (auto it = inScope.rbegin(); it != inScope.rend(); it++)
		{
			if (containsKey(**it, key))
				return (**it)[key];
		}
		return std::any();
	}

	void Context::PushScope(Variables* vars)
	{
		inScope.push_back(vars);
	}

	Variables* Context::PopScope()
	{
		if (inScope.size() <= 0)
			return NULL;

		Variables* vars = inScope.back();
		inScope.pop_back();
		return vars;
	}

	Context::Context()
	{
		Reset();
	}

	void Context::Reset()
	{
		inScope = std::vector<Variables*>();
		functions = ::TinyExe::Functions();
		Globals = Variables();
		functions.InitDefaults();
		Globals["Pi"] = 3.1415926535897932384626433832795; // Math.Pi is not very precise
		Globals["E"] = 2.7182818284590452353602874713527;  // Math.E is not very precise either
	}

	/// <summary>
	/// Thats not a Deep clone!
	/// Functions and Globals are shared!
	/// </summary>
	Context* Context::Clone()
	{
		Context* c = new Context();
		c->Globals = this->Globals;
		c->functions = this->functions;
		return c;
	}
}