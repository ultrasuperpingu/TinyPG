#pragma once

#include <iostream>
#include <map>
#include "Function.h"

namespace TinyExe
{
	class Functions : public std::map<std::string, class Function*>
	{
	private:
		static Functions defaultFunctions;
		//Random crand = new Random();

	public:

		inline static Functions getDefaults();

		void InitDefaults();

		private:
			/// <summary>
			/// calculates the average over a list of numeric values
			/// </summary>
			/// <param name="ps">list of numeric values</param>
			/// <returns>the average value</returns>
			static std::any Avg(std::vector<std::any> ps);

			/// <summary>
			/// calculates the median over a list of numeric values
			/// </summary>
			/// <param name="ps">list of numeric values</param>
			/// <returns>the median value</returns>
			static std::any Median(std::vector<std::any> ps);


			/// <summary>
			/// calculates the statistical variance over a list of numeric values
			/// </summary>
			/// <param name="ps">list of numeric values</param>
			/// <returns>the variance</returns>
			static std::any Var(std::vector<std::any> ps);

			/// <summary>
			/// calculates the statistical standard deviation over a list of numeric values
			/// </summary>
			/// <param name="ps">list of numeric values</param>
			/// <returns>the standard deviation</returns>
			static std::any StDev(std::vector<std::any> ps);

			/// <summary>
			/// generic Log implementation, allows 1 or 2 parameters
			/// </summary>
			/// <param name="ps">numeric values</param>
			/// <returns>Log of the value</returns>
			static std::any Log(std::vector<std::any> ps);
			static std::any Fact(std::vector<std::any> ps);
			static std::any Max(std::vector<std::any> ps);
			static std::any Min(std::vector<std::any> ps);
			std::any Help(std::vector<std::any> ps);

	};

}