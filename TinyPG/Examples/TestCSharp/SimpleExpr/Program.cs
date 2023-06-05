﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TinyPG;

namespace TinyExprEval
{
	internal class Program
	{
		public static void Main(string[] Args)
		{
			Parser p = new Parser(new Scanner());
			string input = "5*3+12/(1+2*test)";
			var tree = p.Parse(input);
			Dictionary<string, int> context = new Dictionary<string, int>();
			context["test"]=2;
			tree.Context = context;
			Console.WriteLine(input + " = " + tree.Eval());
			Console.ReadLine();
		}
	}
}
