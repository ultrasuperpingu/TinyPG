using System.Collections.Generic;
using System.Text;
using System.IO;
using TinyPG.Compiler;
using System;

namespace TinyPG.CodeGenerators
{
	public class CompileIncludeGenerator : BaseGenerator, ICodeGenerator
	{
		internal CompileIncludeGenerator() : base()
		{
		}
		
		public Dictionary<string, string> Generate(Grammar Grammar, GenerateDebugMode Debug)
		{
			// generate the parser file
			StringBuilder parsers = new StringBuilder();
			string fileContent = File.ReadAllText(TemplateFiles[0]);
			Dictionary<string, string> files = new Dictionary<string, string>();
			files[TemplateFiles[0]] = fileContent;
			return files;
		}
	}
}
