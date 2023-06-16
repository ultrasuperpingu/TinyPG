// Copyright 2008 - 2010 Herre Kuijpers - <herre.kuijpers@gmail.com>
//
// This source file(s) may be redistributed, altered and customized
// by any means PROVIDING the authors name and all copyright
// notices remain intact.
// THIS SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED. USE IT AT YOUR OWN RISK. THE AUTHOR ACCEPTS NO
// LIABILITY FOR ANY DATA DAMAGE/LOSS THAT THIS PRODUCT MAY CAUSE.
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

// extends the System.Text namespace
namespace System.Text
{
	/// <summary>
	/// Some handy to use static helper functions
	/// Note that this class is used by the TinyTG.Compiler classes for string formatting
	/// </summary>
	public static class Helper
	{
		public static string Reverse(this string text)
		{
			char[] charArray = new char[text.Length];
			int len = text.Length - 1;
			for (int i = 0; i <= len; i++)
				charArray[i] = text[len - i];
			return new string(charArray);
		}

		public static string Outline(string text1, int indent1, string text2, int indent2)
		{
			string r = Indent(indent1);
			r += text1;
			r = r.PadRight((indent2 * 4) % 256, ' ');
			r += text2;
			return r;
		}

		public static string Indent(int indentcount, string indentString = IndentString)
		{
			string t = "";
			for (int i = 0; i < indentcount; i++)
				t += indentString;

			return t;
		}

		/// <summary>
		/// Will add a comment that can be used for debugging problems in generated code
		/// comment will only be added if profile is set to Debug mode.
		/// It will use '//' as comment line char sequence.
		/// </summary>
		/// <param name="comment">the comment to write to the file</param>
		/// <returns></returns>
		public static string AddComment(string comment)
		{
			return AddComment("//", comment);
		}

		/// <summary>
		/// Will add a comment that can be used for debugging problems in generated code
		/// comment will only be added if profile is set to Debug mode.
		/// </summary>
		/// <param name="commenter">the comment line char sequence to use</param>
		/// <param name="comment">the comment to write to the file</param>
		/// <returns></returns>
		public static string AddComment(string commenter, string comment)
		{
#if DEBUG
			return " " + commenter + " " + comment;
#else
			return "";
#endif
		}

		/// <summary>
		/// Convert a verbatim string literal to an regular string literal
		/// Does nothing if string literal does not starts with @ and force is false
		/// <para>
		/// This method presumes the string is a string literal (start with " or @" and ends with ")
		/// but no checking is done to ensure this
		/// </para>
		/// <example>
		/// Example (string content delimiter is { and } in this example):
		/// <list type="bullet">
		///   <item>v={@"\""test\"""} will returns {"\\\"test\\\""}</item>
		/// </list>
		/// </example>
		/// </summary>
		/// <param name="v">string literal to unverbatim</param>
		/// <param name="force">proceed event if literal does not start with @</param>
		/// <returns>regular string literal</returns>
		public static string Unverbatim(this string v, bool force=false)
		{
			if (v[0] == '@' || force)
			{
				v = v.Substring(v[0] == '@'?2:1, v.Length - (v[0] == '@'?3:2));
				v = v.Replace(@"\", @"\\");
				v = "\"" + v.Replace(@"""", "\\\"") + "\"";
			}
			return v;
		}

		/// <summary>
		/// Unescape string literal like <see cref="LiteralToUnescaped"></see> does but force the input to be threated as a verbatim literal
		/// <para>
		/// This method presumes the string is a string literal (start with " or @" and ends with ")
		/// but no checking is done to ensure this
		/// </para>
		/// </summary>
		/// <param name="v">string literal to proceed</param>
		/// <returns>unescaped string</returns>
		public static string VerbatimLiteralToUnescaped(this string v)
		{
			if (v[0] != '@')
				v='@' + v;
			return LiteralToUnescaped(v);
		}

		/// <summary>
		/// Get the unescaped string from a string literal
		/// <list type="bullet">
		///  <item>The double quote corresponding to the begin and the end of the string literal will be removed</item>
		///  <item>for verbatim string: "" will be replaced by "</item>
		///  <item>for regular string: all escape sequences (ie \x with x={\,n,t,r}) will be replaced by the corresponding char</item>
		/// </list>
		/// <para>
		/// This method presumes the string is a string literal (start with " or @" and ends with ")
		/// but no checking is done to ensure this.
		/// </para>
		/// <example>
		/// Examples (string content delimiter is { and } in those examples):
		/// <list type="bullet">
		/// <item>v={"\\\ttest\""} will return {\	test"}</item>
		/// <item>v={@"\\\ttest\"""} will return {\\\ttest\"}</item>
		///	</list>
		///	</example>
		/// </summary>
		/// <remarks>
		/// If escape sequence is unknown (for now, just \n, \r and \t are known),
		/// no replacement is done (so {"\T\R\e"} as input will result in {\T\R\e}
		/// </remarks>
		/// <param name="v">string literal to proceed</param>
		/// <param name="dontUnescapeSpecialChars">if true, only \" will be replaced by " if the string is not verbatim</param>
		/// <returns>unescaped string</returns>
		public static string LiteralToUnescaped(this string v, bool dontUnescapeSpecialChars = false)
		{
			if (v[0] == '@')
			{
				v = v.Substring(2, v.Length - 3);
				v = v.Replace(@"""""", @"""");
			}
			else
			{
				v = v.Substring(1, v.Length - 2);
				if (!dontUnescapeSpecialChars)
				{
					v = v.Replace(@"\r\n", "\r\n");
					v = v.Replace(@"\n", "\n");
					v = v.Replace(@"\r", "\r");
					v = v.Replace(@"\t", "\t");
					v = v.Replace(@"\\", @"\");                 
					//TODO: other escape
				}
				v = v.Replace(@"\""", "\"");
			}
			return v;
		}
		public const string IndentString = "\t";
		public const string Indent1 = IndentString;
		public const string Indent2 = IndentString + IndentString;
		public const string Indent3 = Indent2 + IndentString;

		/// <summary>
		/// Replace each <paramref name="spacesPerIndent"/> spaces at the beginning of
		/// each line in <paramref name="input"/> by a tab
		/// </summary>
		/// <param name="input">string to tabify</param>
		/// <param name="spacesPerIndent">nb space per indentation in <paramref name="input"/></param>
		/// <returns>a tabified string</returns>
		public static string Tabify(this string input, byte spacesPerIndent = 4)
		{
			Regex r = new Regex(@"(\G|^)(\t*) {"+spacesPerIndent+"}", RegexOptions.Multiline);
			input = r.Replace(input, "$1$2\t");
			r = new Regex("^(\t*)[ ]+", RegexOptions.Multiline); 
			return r.Replace(input, "$1"); ;
		}

		/// <summary>
		/// Replace each leading tabs in each line in <paramref name="input"/> by <paramref name="spacesPerIndent"/> spaces.
		/// </summary>
		/// <param name="input">string to untabify</param>
		/// <param name="spacesPerIndent">nb spaces per indentation</param>
		/// <param name="cleanup">if true, try to fix mixed leading tabs and spaces</param>
		/// <returns></returns>
		public static string Untabify(this string input, byte spacesPerIndent = 4, bool cleanup = true)
		{
			if(cleanup)
				input=Tabify(input, spacesPerIndent);
			Regex r = new Regex(@"(\G|^)(\t)", RegexOptions.Multiline);
			return r.Replace(input, Indent(spacesPerIndent, " "));
		}

		/// <summary>
		/// Make all \r, \n and \r\n be the correct new line (Environment.NewLine)
		/// </summary>
		/// <param name="input">string to fix</param>
		/// <returns>fixed string</returns>
		public static string FixNewLines(this string input)
		{
			// Inefficient way to do it...
			// Maybe try:
			// r = new Regex("(?<!\r)\n");
			// r = new Regex("\r(?!\n)");

			return input.Replace("\r\n", "\n").Replace('\r','\n').Replace("\n", Environment.NewLine); ;
		}

		/// <summary>
		/// Check if a string if a valid regex pattern.
		/// </summary>
		/// <param name="pattern">string to check</param>
		/// <returns>is the string a valid regex</returns>
		public static bool IsValidRegex(this string pattern)
		{
			string error;
			return IsValidRegex(pattern, out error);
		}

		/// <summary>
		/// Check if a string if a valid regex pattern. If not, <paramref name="errorMessage"/>
		/// will be filled with an error message describing the problem. Otherwise, it will be null.
		/// </summary>
		/// <param name="pattern">string to check</param>
		/// <param name="errorMessage">will be filled with an error message describing the problem if input is not a valid regex. Otherwise, it will be null.</param>
		/// <returns>is the string a valid regex</returns>
		public static bool IsValidRegex(this string pattern, out string errorMessage)
		{
			try
			{
				Regex.Match("", pattern);
			}
			catch (ArgumentException ex)
			{
				errorMessage = ex.Message;
				return false;
			}
			errorMessage = null;
			return true;
		}
	}
}
