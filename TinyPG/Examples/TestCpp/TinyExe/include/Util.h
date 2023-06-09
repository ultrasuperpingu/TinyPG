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

inline double ConvertToDouble(std::any val)
{
	if (val.has_value())
	{
		if (auto x = std::any_cast<int>(&val))
		{
			return std::any_cast<int>(val);
		}
		if (auto x = std::any_cast<double>(&val))
		{
			return std::any_cast<double>(val);
		}
		if (auto x = std::any_cast<bool>(&val))
		{
			return std::any_cast<bool>(val) ? 1.0 : 0.0;
		}
		if (auto x = std::any_cast<std::string>(&val))
		{
			return std::stod(std::any_cast<std::string>(val));
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
			return std::any_cast<int>(val);
		}
		if (auto x = std::any_cast<double>(&val))
		{
			return (int)std::any_cast<double>(val);
		}
		if (auto x = std::any_cast<bool>(&val))
		{
			return std::any_cast<bool>(val);
		}
		if (auto x = std::any_cast<std::string>(&val))
		{
			return std::stoi(std::any_cast<std::string>(val));
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
			return std::to_string(std::any_cast<int>(val));
		}
		if (auto x = std::any_cast<double>(&val))
		{
			return std::to_string(std::any_cast<double>(val));
		}
		if (auto x = std::any_cast<bool>(&val))
		{
			return std::to_string(std::any_cast<bool>(val));
		}
		if (auto x = std::any_cast<std::string>(&val))
		{
			return std::any_cast<std::string>(val);
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
			return std::any_cast<int>(val);
		}
		if (auto x = std::any_cast<double>(&val))
		{
			return (int)std::any_cast<double>(val);
		}
		if (auto x = std::any_cast<bool>(&val))
		{
			return (bool)std::any_cast<bool>(val);
		}
		if (auto x = std::any_cast<std::string>(&val))
		{
			return std::stoi(std::any_cast<std::string>(val));
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