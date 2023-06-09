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
	return 0.0;
}
inline bool ConvertToBoolean(std::any val)
{
	return false;
}
inline std::string ConvertToString(std::any val)
{
	return "";
}
inline int ConvertToInt32(std::any val)
{
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