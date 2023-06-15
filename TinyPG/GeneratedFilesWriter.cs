using System;
using System.Collections.Generic;
using System.Text;
using TinyPG.CodeGenerators;
using TinyPG.Compiler;
using System.IO;

namespace TinyPG
	{
	public class GeneratedFilesWriter
	{

		private Grammar grammar = null;

		public GeneratedFilesWriter(Grammar grammar)
		{
			this.grammar = grammar;
		}

		public void Generate(bool debug = false)
		{
			ICodeGenerator generator;

			string language = grammar.Directives["TinyPG"]["Language"];
			foreach (Directive d in grammar.Directives)
			{
				generator = CodeGeneratorFactory.CreateGenerator(d.Name, language);

				if (generator != null && d["Generate"].ToLower() == "true")
				{
					foreach (var entry in generator.Generate(grammar, debug ? GenerateDebugMode.DebugSelf : GenerateDebugMode.None))
					{
						File.WriteAllText(
							Path.Combine(grammar.GetOutputPath(), entry.Key),
							entry.Value
						);
					}
				}
			}
		}
	}
}
