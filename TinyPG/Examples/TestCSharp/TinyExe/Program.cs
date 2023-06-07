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
			Context context = new Context();
			context.Globals.Add("testDouble", 0.1);
			context.Globals.Add("testInt", 5);
			Console.WriteLine("Enter an expression (empty to exit):");
			string expression = "5*3+(testInt * testDouble)/2";
			Console.WriteLine("> " + expression);
			while (!string.IsNullOrEmpty(expression))
			{
				Parser p = new Parser(new Scanner());
				var tree = p.Parse(expression);
				if (tree.Errors.Count > 0)
				{
					foreach(var e in tree.Errors)
					{
						Console.WriteLine("Col " + e.Column + ": " + e.Message);
					}
				}
				else
				{
					tree.Context = context;
					var res = tree.Eval();
					Console.WriteLine("< "+res);
				}
				Console.Write("> ");
				expression = Console.ReadLine();
			}
		}
	}
}
