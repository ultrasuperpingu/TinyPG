using System.Collections.Generic;
using System.Text;
using System.IO;
using TinyPG.Compiler;
using System;

namespace TinyPG.CodeGenerators
{
	public class CompileIncludeGenerator : BaseGenerator, ICodeGenerator
	{
		internal CompileIncludeGenerator() : base(null)
		{
		}
		
		public string Generate(Grammar Grammar, GenerateDebugMode Debug)
		{
			// generate the parser file
			StringBuilder parsers = new StringBuilder();
			string parser = File.ReadAllText(FileName);

			return parser;
		}
	}
}
