using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TinyExe;
using static TinyExe.ParseNode;

namespace TinyExprEval
{
	internal class Program
	{
		public static void Main(string[] Args)
		{
			Parser p = new Parser(new Scanner());
			var tree = p.Parse("5*cos(testInt)+testDouble");
			tree.Context.Globals.Add("testDouble", 0.1);
			tree.Context.Globals.Add("testInt", 5);
			var res = tree.Eval();
			Console.WriteLine(res);
			Console.ReadLine();
		}
	}
}
