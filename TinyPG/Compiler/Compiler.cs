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
using CodeDom = System.CodeDom.Compiler;
using System.Reflection;

using TinyPG.CodeGenerators;
using TinyPG.Debug;

using System.Windows.Forms;


namespace TinyPG.Compiler
{

	public class Compiler
	{
		private Grammar Grammar;

		/// <summary>
		/// indicates if the grammar was parsed successfully
		/// </summary>
		public bool IsParsed { get; set; }

		/// <summary>
		/// indicates if the grammar was compiled successfully
		/// </summary>
		public bool IsCompiled { get; set; }

		/// <summary>
		/// a string containing the actual generated code for the scanner
		/// </summary>
		public string ScannerCode { get; set; }

		/// <summary>
		/// a string containing the actual generated code for the parser
		/// </summary>
		public string ParserCode { get; set; }

		/// <summary>
		/// a string containing the actual generated code for the Parse tree
		/// </summary>
		public string ParseTreeCode { get; set; }

		/// <summary>
		/// a list of errors that occured during parsing or compiling
		/// </summary>
		public List<string> Errors { get; set; }

		// the resulting compiled assembly
		private Assembly assembly;


		public Compiler()
		{
			IsCompiled = false;
			Errors = new List<string>();
		}

		public void Compile(Grammar grammar)
		{
			IsParsed = false;
			IsCompiled = false;
			Errors = new List<string>();
			if (grammar == null)
				throw new ArgumentNullException("grammar", "Grammar may not be null");

			Grammar = grammar;
			grammar.Preprocess();
			IsParsed = true;

			if (BuildCode())
				IsCompiled = true;
		}

		/// <summary>
		/// once the grammar compiles correctly, the code can be built.
		/// </summary>
		private bool BuildCode()
		{
			bool result = false;
			string language = Grammar.Directives["TinyPG"]["Language"];
			CodeDom.CompilerResults Result;
			CodeDom.CodeDomProvider provider = CodeGeneratorFactory.CreateCodeDomProvider(language);
			string templatePathSave = Grammar.Directives["TinyPG"]["TemplatePath"];
			GenerateDebugMode debugMode = GenerateDebugMode.DebugSelf;
			int ignoreError = 0;
			if (provider == null)
			{
				Errors.Add("Can't compile " + language + " project. " + Environment.NewLine +
					"Compiling with C# : Eval will not be performed and the parsing result is not " + Environment.NewLine +
					"garantee to be the same than the generated " + language + " sources.");
				language = "csharp";
				Grammar.Directives["TinyPG"]["TemplatePath"] = AppDomain.CurrentDomain.BaseDirectory +
							System.IO.Path.Combine("Templates", "C#") + System.IO.Path.DirectorySeparatorChar;
				provider = CodeGeneratorFactory.CreateCodeDomProvider(language);
				debugMode = GenerateDebugMode.DebugOther;
				ignoreError++;
				//return;
			}
			System.CodeDom.Compiler.CompilerParameters compilerparams = new System.CodeDom.Compiler.CompilerParameters();
			compilerparams.GenerateInMemory = true;
			compilerparams.GenerateExecutable = false;
			compilerparams.ReferencedAssemblies.Add("System.dll");
			compilerparams.ReferencedAssemblies.Add("System.Windows.Forms.dll");
			compilerparams.ReferencedAssemblies.Add("System.Drawing.dll");
			compilerparams.ReferencedAssemblies.Add("System.Xml.dll");
			compilerparams.ReferencedAssemblies.Add("System.Core.dll");
			compilerparams.ReferencedAssemblies.Add("System.Linq.dll"); 

			// reference this assembly to share interfaces (for debugging only)

			string tinypgfile = Assembly.GetExecutingAssembly().Location;
			compilerparams.ReferencedAssemblies.Add(tinypgfile);

			// generate the code with debug interface enabled
			List<string> sources = new List<string>();
			List<string> sourcesFile = new List<string>();
			ICodeGenerator generator;
			foreach (Directive d in Grammar.Directives)
			{
				generator = CodeGeneratorFactory.CreateGenerator(d.Name, language);
				if (generator != null && d.ContainsKey("FileName"))
					generator.FileName = d["FileName"];

				if (generator != null && (d.Name == "Compile" || d["Generate"].ToLower() == "true"))
				{
					sources.Add(generator.Generate(Grammar, debugMode));
					sourcesFile.Add(generator.FileName);
				}
			}

			if (sources.Count > 0)
			{
				Result = provider.CompileAssemblyFromSource(compilerparams, sources.ToArray());

				if (Result.Errors.Count > ignoreError)
				{
					foreach (CodeDom.CompilerError o in Result.Errors)
					{
						if (!o.IsWarning)
							result = false;
						int index = 0;
						string filename = "Unknown ("+System.IO.Path.GetFileName(o.FileName)+")";
						while (index < sources.Count)
						{
							var indexF = o.FileName.IndexOf("."+index+".cs");
							if(indexF >= 0)
								filename = sourcesFile[index];
							index++;
						}
						Errors.Add((o.IsWarning?"Warning ":"Error ") + o.ErrorNumber.ToString() +";"+ filename + " (" + o.Line.ToString()+"," + o.Column.ToString()+"): " + o.ErrorText);
					}
				}
				else
				{
					assembly = Result.CompiledAssembly;
					result = true;
				}
			}
			Grammar.Directives["TinyPG"]["TemplatePath"] = templatePathSave;
			return result;
		}

		/// <summary>
		/// evaluate the input expression
		/// </summary>
		/// <param name="input">the expression to evaluate with the parser</param>
		/// <returns>the output of the parser/compiler</returns>
		public CompilerResult Run(string input)
		{
			return Run(input, null);
		}

		public CompilerResult Run(string input, RichTextBox textHighlight)
		{
			CompilerResult compilerresult = new CompilerResult();
			string output = null;
			if (assembly == null)
				return null;

			object scannerinstance = assembly.CreateInstance(Grammar.Directives["TinyPG"]["Namespace"]+".Scanner");
			Type scanner = scannerinstance.GetType();

			object parserinstance = (IParser)assembly.CreateInstance(Grammar.Directives["TinyPG"]["Namespace"]+".Parser", true, BindingFlags.CreateInstance, null, new object[] { scannerinstance }, null, null);
			Type parsertype = parserinstance.GetType();

			object treeinstance = parsertype.InvokeMember("Parse", BindingFlags.InvokeMethod, null, parserinstance, new object[] { input });
			IParseTree itree = treeinstance as IParseTree;

			compilerresult.ParseTree = itree;
			Type treetype = treeinstance.GetType();

			List<IParseError> errors = (List<IParseError>)treetype.InvokeMember("Errors", BindingFlags.GetField, null, treeinstance, null);

			if (textHighlight != null && errors.Count == 0)
			{
				// try highlight the input text
				object highlighterinstance = assembly.CreateInstance(Grammar.Directives["TinyPG"]["Namespace"]+".TextHighlighter", true, BindingFlags.CreateInstance, null, new object[] { textHighlight, scannerinstance, parserinstance }, null, null);
				if (highlighterinstance != null)
				{
					output += "Highlighting input..." + "\r\n";
					Type highlightertype = highlighterinstance.GetType();
					// highlight the input text only once
					highlightertype.InvokeMember("HighlightText", BindingFlags.InvokeMethod, null, highlighterinstance, null);

					// let this thread sleep so background thread can highlight the text
					System.Threading.Thread.Sleep(20);

					// dispose of the highlighter object
					highlightertype.InvokeMember("Dispose", BindingFlags.InvokeMethod, null, highlighterinstance, null);
				}
			}
			if (errors.Count > 0)
			{
				foreach (IParseError err in errors)
					output += string.Format("({0},{1}): {2}\r\n", err.Line, err.Column, err.Message);
			}
			else
			{
				output += "Parse was successful." + "\r\n";
				output += "Evaluating...";

				// parsing was successful, now try to evaluate... this should really be done on a seperate thread.
				// e.g. if the thread hangs, it will hang the entire application (!)
				try
				{
					compilerresult.Value = itree.Eval(null);
					if (errors != null && errors.Count > 0)
					{
						output += "\r\nSemantics Errors: \r\n";
						foreach (IParseError err in errors)
							output += string.Format("({0},{1}): {2}\r\n", err.Line, err.Column, err.Message);
					}
					else
					{
						output += "\r\nResult: " + (compilerresult.Value == null ? "null" : compilerresult.Value.ToString());
					}
				}
				catch (NotImplementedException exc)
				{
					output += "\r\n" + exc.Message;
				}
				catch (Exception exc)
				{
					output += "\r\nException occurred: " + exc.Message;
					output += "\r\nStacktrace: " + exc.StackTrace;
				}

			}
			compilerresult.Output = output.ToString();
			return compilerresult;
		}
	}
}
