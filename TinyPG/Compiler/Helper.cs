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
	/// some handy to use static helper functions
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
		/// will add a comment that can be used for debugging problems in generated code
		/// comment will only be added if profile is set to Debug mode
		/// </summary>
		/// <param name="comment">the comment to write to the file</param>
		/// <returns></returns>
		public static string AddComment(string comment)
		{
			return AddComment("//", comment);
		}

		public static string AddComment(string commenter, string comment)
		{
#if DEBUG
			return " " + commenter + " " + comment;
#else
			return "";
#endif
		}
		public static string Unverbatim(this string v)
		{
			if (v[0] == '@')
			{
				v = v.Substring(2, v.Length - 3);
				v = v.Replace(@"\", @"\\");
				v = "\"" + v.Replace(@"""", "\\\"") + "\"";
			}
			return v;
		}
		public static string Unescape(string v)
		{
			if (v[0] == '@')
			{
				v = v.Substring(2, v.Length - 3);
				v = v.Replace(@"""""", @"""");
			}
			else
			{
				//TODO: change regex for string in grammar
				// it seams to be verbatim string event if
				// leading @ is not present...
				v = v.Substring(1, v.Length - 2);
				v = v.Replace(@"\r\n", "\r\n");
				v = v.Replace(@"\n", "\n");
				v = v.Replace(@"\r", "\r");
				v = v.Replace(@"\t", "\t");
				v = v.Replace(@"\""", "\"");
				//TODO: other escape
				v = v.Replace(@"\\", @"\");
			}
			return v;
		}
		public const string IndentString = "\t";
		public const string Indent1 = IndentString;
		public const string Indent2 = IndentString + IndentString;
		public const string Indent3 = Indent2 + IndentString;
		public static string Tabify(this string input, byte spacesPerIndent = 4)
		{
			Regex r = new Regex(@"(\G|^)(\t*) {"+spacesPerIndent+"}", RegexOptions.Multiline);
			input = r.Replace(input, "$1$2\t");
			r = new Regex("^(\t*)[ ]+", RegexOptions.Multiline); 
			return r.Replace(input, "$1"); ;
		}

		public static string Untabify(this string input, byte spacesPerIndent = 4, bool cleanup = true)
		{
			if(cleanup)
				input=Tabify(input, spacesPerIndent);
			Regex r = new Regex(@"(\G|^)(\t)", RegexOptions.Multiline);
			return r.Replace(input, Indent(spacesPerIndent, " "));
		}
		public static string FixNewLines(this string input)
		{
			// Inefficient way to do it...
			return input.Replace("\r\n", "\n").Replace('\r','\n').Replace("\n", Environment.NewLine); ;
		}
	}
}
