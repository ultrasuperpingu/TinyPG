using System;
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
		}
	}
}
