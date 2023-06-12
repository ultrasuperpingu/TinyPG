#pragma once
#include <string>
#include <algorithm>

inline std::string str_tolower(std::string s)
{
	std::transform(s.begin(), s.end(), s.begin(),
		[](unsigned char c) { return std::tolower(c); }
	);
	return s;
}
inline std::string str_toupper(std::string s)
{
	std::transform(s.begin(), s.end(), s.begin(),
		[](unsigned char c) { return std::toupper(c); }
	);
	return s;
}
inline double ConvertToDouble(std::any val)
{
	if (val.has_value())
	{
		if (auto x = std::any_cast<int>(&val))
		{
			return *x;
		}
		if (auto x = std::any_cast<double>(&val))
		{
			return *x;
		}
		if (auto x = std::any_cast<bool>(&val))
		{
			return *x ? 1.0 : 0.0;
		}
		if (auto x = std::any_cast<std::string>(&val))
		{
			return std::stod(*x);
		}
	}
	return 0.0;
}
inline bool ConvertToBoolean(std::any val)
{
	if (val.has_value())
	{
		if (auto x = std::any_cast<int>(&val))
		{
			return *x;
		}
		if (auto x = std::any_cast<double>(&val))
		{
			return (int)(*x);
		}
		if (auto x = std::any_cast<bool>(&val))
		{
			return *x;
		}
		if (auto x = std::any_cast<std::string>(&val))
		{
			return std::stoi(*x);
		}
	}
	return false;
}
inline std::string ConvertToString(std::any val)
{
	if (val.has_value())
	{
		if (auto x = std::any_cast<int>(&val))
		{
			return std::to_string(*x);
		}
		if (auto x = std::any_cast<double>(&val))
		{
			return std::to_string(*x);
		}
		if (auto x = std::any_cast<bool>(&val))
		{
			return std::to_string(*x);
		}
		if (auto x = std::any_cast<std::string>(&val))
		{
			return *x;
		}
	}
	return "";
}
inline int ConvertToInt32(std::any val)
{
	if (val.has_value())
	{
		if (auto x = std::any_cast<int>(&val))
		{
			return *x;
		}
		if (auto x = std::any_cast<double>(&val))
		{
			return (int)(*x);
		}
		if (auto x = std::any_cast<bool>(&val))
		{
			return (bool)(*x);
		}
		if (auto x = std::any_cast<std::string>(&val))
		{
			return std::stoi(*x);
		}
	}
	return 0;
}

template <typename T>
struct identity { typedef T type; };

template <typename A, typename B>
inline bool containsKey(const std::map<A, B>& m
	, const typename identity<A>::type& str)
{
	return m.find(str) != m.end();
}