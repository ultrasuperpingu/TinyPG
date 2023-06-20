#pragma once

#include <climits>
#include <cfloat>
#include "Functions.h"
#include "Util.h"
#include "StaticFunction.h"

namespace TinyExe
{
	Functions Functions::defaultFunctions;
	Functions Functions::getDefaults()
	{
		defaultFunctions.InitDefaults();
		return defaultFunctions;
	}

	void Functions::InitDefaults()
	{
		FunctionDelegate test = std::bind(&Functions::Help, this, std::placeholders::_1);
		this->insert(std::make_pair(std::string("about"), new StaticFunction("About", [](std::vector<std::any>) { return std::string("@TinyExe - a Tiny Expression Evaluator v1.0\r\nby Herre Kuijpers - Copyright © 2011 under the CPOL license"); }, 0, 0)));
		this->insert(std::make_pair(std::string("help") , new StaticFunction("Help", test, 0, 0)));
		
		// high precision functions
		this->insert(std::make_pair(std::string("abs") , new StaticFunction("Abs",  [](std::vector<std::any> ps) { return abs(ConvertToDouble(ps[0])); }, 1, 1)));
		this->insert(std::make_pair(std::string("acos"), new StaticFunction("Acos", [](std::vector<std::any> ps) { return acos(ConvertToDouble(ps[0])); }, 1, 1)));
		this->insert(std::make_pair(std::string("asin"), new StaticFunction("Asin", [](std::vector<std::any> ps) { return asin(ConvertToDouble(ps[0])); }, 1, 1)));

		this->insert(std::make_pair(std::string("atan")   , new StaticFunction("Atan",    [](std::vector<std::any> ps) { return	atan(ConvertToDouble(ps[0])); }, 1, 1)));
		this->insert(std::make_pair(std::string("atan2")  , new StaticFunction("Atan2",   [](std::vector<std::any> ps) { return atan2(ConvertToDouble(ps[0]), ConvertToDouble(ps[1])); }, 2, 2)));
		this->insert(std::make_pair(std::string("ceiling"), new StaticFunction("Ceiling", [](std::vector<std::any> ps) { return ceil(ConvertToDouble(ps[0])); }, 1, 1)));
		this->insert(std::make_pair(std::string("cos")    , new StaticFunction("Cos",     [](std::vector<std::any> ps) { return cos(ConvertToDouble(ps[0])); }, 1, 1)));
		this->insert(std::make_pair(std::string("cosh")   , new StaticFunction("Cosh",    [](std::vector<std::any> ps) { return cosh(ConvertToDouble(ps[0])); }, 1, 1)));
		this->insert(std::make_pair(std::string("exp")    , new StaticFunction("Exp",     [](std::vector<std::any> ps) { return exp(ConvertToDouble(ps[0])); }, 1, 1)));
		this->insert(std::make_pair(std::string("int")    , new StaticFunction("int",     [](std::vector<std::any> ps) { return (int)floor(ConvertToDouble(ps[0])); }, 1, 1)));
		this->insert(std::make_pair(std::string("fact")   , new StaticFunction("Fact", Fact, 1, 1))); // factorials 1*2*3*4...
		this->insert(std::make_pair(std::string("floor")  , new StaticFunction("Floor",   [](std::vector<std::any> ps) { return floor(ConvertToDouble(ps[0])); }, 1, 1)));
		this->insert(std::make_pair(std::string("log")    , new StaticFunction("Log", Log, 1, 2))); // log allows 1 or 2 parameters
		this->insert(std::make_pair(std::string("ln")     , new StaticFunction("Ln",      [](std::vector<std::any> ps) { return log(ConvertToDouble(ps[0])); }, 1, 1)));
		this->insert(std::make_pair(std::string("pow")    , new StaticFunction("Pow",     [](std::vector<std::any> ps) { return pow(ConvertToDouble(ps[0]), ConvertToDouble(ps[1])); }, 2, 2)));
		this->insert(std::make_pair(std::string("round")  , new StaticFunction("Round",   [](std::vector<std::any> ps) { return round(ConvertToDouble(ps[0])); }, 1, 1)));
		//this->insert(std::make_pair(std::string("rand")   , new StaticFunction("Rand",    [](std::vector<std::any> ps) { return crand.Next(ConvertToInt32(ps[0])); }, 1, 1)));
		this->insert(std::make_pair(std::string("sign")   , new StaticFunction("Sign",    [](std::vector<std::any> ps) { return (0.0 < ConvertToDouble(ps[0])) - (ConvertToDouble(ps[0]) < 0.0); }, 1, 1)));
		this->insert(std::make_pair(std::string("sin")    , new StaticFunction("Sin",     [](std::vector<std::any> ps) { return sin(ConvertToDouble(ps[0])); }, 1, 1)));
		this->insert(std::make_pair(std::string("sinh")   , new StaticFunction("Sinh",    [](std::vector<std::any> ps) { return sinh(ConvertToDouble(ps[0])); }, 1, 1)));
		this->insert(std::make_pair(std::string("sqr")    , new StaticFunction("Sqr",     [](std::vector<std::any> ps) { return ConvertToDouble(ps[0]) * ConvertToDouble(ps[0]); }, 1, 1)));
		this->insert(std::make_pair(std::string("sqrt")   , new StaticFunction("Sqrt",    [](std::vector<std::any> ps) { return sqrt(ConvertToDouble(ps[0])); }, 1, 1)));
		//this->insert(std::make_pair(std::string("trunc")  , new StaticFunction("Trunc",   [](std::vector<std::any> ps) { return truncate(ConvertToDouble(ps[0])); }, 1, 1)));

		// array functions
		this->insert(std::make_pair(std::string("avg")    , new StaticFunction("Avg", Avg, 1, INT_MAX)));
		this->insert(std::make_pair(std::string("stdev")  , new StaticFunction("StDev", StDev, 1, INT_MAX)));
		this->insert(std::make_pair(std::string("var")    , new StaticFunction("Var", Var, 1, INT_MAX)));
		this->insert(std::make_pair(std::string("max")    , new StaticFunction("Max", Max, 1, INT_MAX)));
		this->insert(std::make_pair(std::string("median") , new StaticFunction("Median", Median, 1, INT_MAX)));
		this->insert(std::make_pair(std::string("min")    , new StaticFunction("Min", Min, 1, INT_MAX)));

		//boolean functions
		this->insert(std::make_pair(std::string("not")   , new StaticFunction("Not", [](std::vector<std::any> ps) { return !ConvertToBoolean(ps[0]); }, 1, 1)));
		this->insert(std::make_pair(std::string("if")    , new StaticFunction("If" , [](std::vector<std::any> ps) { return ConvertToBoolean(ps[0]) ? ps[1] : ps[2]; }, 3, 3)));
		this->insert(std::make_pair(std::string("and")   , new StaticFunction("And", [](std::vector<std::any> ps) { return ConvertToBoolean(ps[0]) && ConvertToBoolean(ps[1]); }, 2, 2)));
		this->insert(std::make_pair(std::string("or")    , new StaticFunction("Or" , [](std::vector<std::any> ps) { return ConvertToBoolean(ps[0]) || ConvertToBoolean(ps[1]); }, 2, 2)));

		// string functions
		this->insert(std::make_pair(std::string("left"), new StaticFunction("Left", [](std::vector<std::any> ps)
		{
			size_t len = ConvertToInt32(ps[1]) < ConvertToString(ps[0]).length() ? ConvertToInt32(ps[1]) : ConvertToString(ps[0]).length();
			return ConvertToString(ps[0]).substr(0, len);
		}, 2, 2)));

		this->insert(std::make_pair(std::string("right"), new StaticFunction("Right", [](std::vector<std::any> ps)
		{
			size_t len = ConvertToInt32(ps[1]) < ConvertToString(ps[0]).length() ? ConvertToInt32(ps[1]) : ConvertToString(ps[0]).length();
			return ConvertToString(ps[0]).substr(ConvertToString(ps[0]).length() - len, len);
		}, 2, 2)));

		this->insert(std::make_pair(std::string("mid"), new StaticFunction("Mid", [](std::vector<std::any> ps)
		{
			size_t idx = ConvertToInt32(ps[1]) < ConvertToString(ps[0]).length() ? ConvertToInt32(ps[1]) : ConvertToString(ps[0]).length();
			size_t len = ConvertToInt32(ps[2]) < ConvertToString(ps[0]).length() - idx ? ConvertToInt32(ps[2]) : ConvertToString(ps[0]).length() - idx;
			return ConvertToString(ps[0]).substr(idx, len);
		}, 3, 3)));
		
		//this->insert(std::make_pair(std::string("hex")   , new StaticFunction("Hex", [](std::vector<std::any> ps) { return String.Format("0x{0:X}", ConvertToInt32(ps[0].ToString())); }, 1, 1)));
		//this->insert(std::make_pair(std::string("format"), new StaticFunction("Format", [](std::vector<std::any> ps) { return string.Format(ps[0].ToString(), ps[1]); }, 2, 2)));
		this->insert(std::make_pair(std::string("len")   , new StaticFunction("Len", [](std::vector<std::any> ps) { return ConvertToString(ps[0]).length(); }, 1, 1)));
		this->insert(std::make_pair(std::string("lower") , new StaticFunction("Lower", [](std::vector<std::any> ps) { return str_tolower(ConvertToString(ps[0])); }, 1, 1)));
		this->insert(std::make_pair(std::string("upper") , new StaticFunction("Upper", [](std::vector<std::any> ps) { return str_toupper(ConvertToString(ps[0])); }, 1, 1)));
		this->insert(std::make_pair(std::string("val")   , new StaticFunction("Val", [](std::vector<std::any> ps) { return ConvertToDouble(ps[0]); }, 1, 1)));
		
		this->insert(std::make_pair(std::string("rshift"), new StaticFunction("rshift", [](std::vector<std::any> ps) {
			return ConvertToInt32(ps[0]) >> ConvertToInt32(ps[1]);
		}, 2, 2)));
		this->insert(std::make_pair(std::string("lshift"), new StaticFunction("lshift", [](std::vector<std::any> ps) {
			return ConvertToInt32(ps[0]) << ConvertToInt32(ps[1]);
		}, 2, 2)));
		this->insert(std::make_pair(std::string("bitand"), new StaticFunction("bitand", [](std::vector<std::any> ps) {
			return ConvertToInt32(ps[0]) & ConvertToInt32(ps[1]);
		}, 2, 2)));
		this->insert(std::make_pair(std::string("bitor"), new StaticFunction("bitor", [](std::vector<std::any> ps) {
			return ConvertToInt32(ps[0]) | ConvertToInt32(ps[1]);
		}, 2, 2)));
		this->insert(std::make_pair(std::string("bitxor"), new StaticFunction("bitxor", [](std::vector<std::any> ps) {
			return ConvertToInt32(ps[0]) ^ ConvertToInt32(ps[1]);
		}, 2, 2)));

	}

	std::any Functions::Avg(std::vector<std::any> ps)
	{
		double total = 0;
		for (auto o : ps)
			total += ConvertToDouble(o);

		return total / ps.size();
	}

	std::any Functions::Median(std::vector<std::any> ps)
	{
		std::vector<std::any> ordered = ps;
		sort(ordered.begin(), ordered.end(), [](std::any o1, std::any o2) { return ConvertToDouble(o1) < ConvertToDouble(o2); });

		if (ordered.size() % 2 == 1)
			return ordered[ordered.size() / 2];
		else
			return (ConvertToDouble(ordered[ordered.size() / 2]) + ConvertToDouble(ordered[ordered.size() / 2 - 1])) / 2;
			
		return 0.0;
	}

	std::any Functions::Var(std::vector<std::any> ps)
	{
		double avg = ConvertToDouble(Avg(ps));
		double total = 0;
		for (std::any o : ps)
			total += (ConvertToDouble(o) - avg) * (ConvertToDouble(o) - avg);

		return total / (ps.size() - 1);
		return 0.0;
	}

	std::any Functions::StDev(std::vector<std::any> ps)
	{
		double var = ConvertToDouble(Var(ps));
		return sqrt(var);
	}

	std::any Functions::Log(std::vector<std::any> ps)
	{
		if (ps.size() == 1)
			return log10(ConvertToDouble(ps[0]));

		if (ps.size() == 2)
			return log(ConvertToDouble(ps[0])) / log(ConvertToDouble(ps[1]));

		return std::any();
	}

	std::any Functions::Fact(std::vector<std::any> ps)
	{
		double total = 1;

		for (int i = ConvertToInt32(ps[0]); i > 1; i--)
			total *= i;

		return total;
	}

	std::any Functions::Max(std::vector<std::any> ps)
	{
		double max = DBL_MIN;

		for (std::any o : ps)
		{
			double val = ConvertToDouble(o);
			if (val > max)
				max = val;
		}
		return max;
	}

	std::any Functions::Min(std::vector<std::any> ps)
	{
		double min = DBL_MIN;

		for (std::any o : ps)
		{
			double val = ConvertToDouble(o);
			if (val < min)
				min = val;
		}
		return min;
	}

	std::any Functions::Help(std::vector<std::any> ps)
	{
		std::string help;
		help+="Tiny Expression Evalutator can evaluate expression containing the following functions:\n";
		for (auto it = begin(); it != end(); it++)
		{
			help+=(it->first + " ");
		}
		return help;
	}


}