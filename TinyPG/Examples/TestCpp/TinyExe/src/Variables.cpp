#include "Variables.h"

namespace TinyExe
{
	Variables* Variables::Clone()
	{
		Variables* vars = new Variables();
		for (std::map<std::string, std::any>::iterator it = begin(); it != end(); ++it)
			vars->insert(std::make_pair(it->first, it->second));

		return vars;
	}
}