using System;
using System.Collections.Generic;
using System.Text;

using TinyPG;
using TinyPG.Parsing;

namespace TinyPG.CodeGenerators
{
	public enum GenerateDebugMode
	{
		None,
		DebugSelf,
		DebugOther
	}
	public interface ICodeGenerator
	{
		List<string> TemplateFiles { get; }
		/// <summary>
		/// Generates an output file based on the grammar
		/// </summary>
		/// <param name="grammar">the grammar object model for the langauge</param>
		/// <param name="debug">a flag that indicates that the generated classes must implement the Debug intefaces (IParser, IParseTree or IToken). Default is false</param>
		/// <returns>returns the output classes to be stored in the output file</returns>
		Dictionary<string, string> Generate(Grammar grammar, GenerateDebugMode debug);
	}
}
