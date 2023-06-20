using System;
using System.Collections.Generic;
using System.IO;
using TinyPG;
using TinyPG.Parsing;

namespace TinyPGCL
{
	internal class Program
	{
		static void Main(string[] args)
		{
			if(args.Length == 0)
			{
				ShowHelp();
				return;
			}
			Dictionary<string, string> parameters = new Dictionary<string, string>();
			List<string> grammarFiles = new List<string>();
			for(int i=0;i<args.Length;i++)
			{
				string key = args[i];
				if (!key.StartsWith("--"))
				{
					grammarFiles.Add(key);
				}
				else
				{
					if(args.Length <= i+1)
					{
						Console.Error.WriteLine("Error: No value specified for parameter " + key);
						return;
					}
					parameters.Add(key, args[i+1]);
					i++;
				}
			}
			if(grammarFiles.Count == 0)
			{
				Console.Error.WriteLine("Error: No grammar file specified");
				return;
			}
			foreach (var grammarFile in grammarFiles)
			{
				string grammarText = File.ReadAllText(grammarFile);
				Scanner scanner = new Scanner();
				Parser parser = new Parser(scanner);
				GrammarTree tree = (GrammarTree)parser.Parse(grammarText, new GrammarTree());
				foreach (var e in tree.Errors)
				{
					Console.Error.WriteLine(grammarFile + ":"+e.Line+":"+e.Column+": "+(e.IsWarning ? "warning: " : "error: ")+e.Message);
				}
				if (tree.Errors.ContainsErrors)
				{
					continue;
				}
				else
				{
					tree.Errors.Clear();
					Grammar grammar = (Grammar)tree.Eval();
					foreach (var e in tree.Errors)
					{
						Console.Error.WriteLine(grammarFile + ":"+e.Line+":"+e.Column+": "+(e.IsWarning ? "warning: " : "error: ")+e.Message);
					}
					if (grammar == null)
					{
						continue;
					}
					grammar.Filename = grammarFile;
					if (parameters.ContainsKey("--language"))
						grammar.Directives["TinyPG"]["Language"] = parameters["--language"];
					if (parameters.ContainsKey("--output-path"))
						grammar.Directives["TinyPG"]["OutputPath"] = Path.Combine(Environment.CurrentDirectory, parameters["--output-path"]);
					if (parameters.ContainsKey("--template-path path"))
						grammar.Directives["TinyPG"]["TemplatePath"] = Path.Combine(Environment.CurrentDirectory, parameters["--template-path"]);
					if (parameters.ContainsKey("--template-path path"))
						grammar.Directives["TinyPG"]["Namespace"] = parameters["--namespace"];
					grammar.Preprocess();
					if (tree.Errors.Count == 0)
					{
						Console.WriteLine(grammar.PrintGrammar());
						Console.WriteLine(grammar.PrintFirsts());

						Console.WriteLine("Parse successful!\r\n");
						new GeneratedFilesWriter(grammar).Generate();
					}
				}
			}
		}

		private static void ShowHelp()
		{
			Console.WriteLine(AppDomain.CurrentDomain.FriendlyName + ":");
			Console.WriteLine("\tParse a grammar file and generate the corresponding parser code.");
			Console.WriteLine("\n\tSyntax: "+AppDomain.CurrentDomain.FriendlyName + "grammarFile [options]");
			Console.WriteLine("\tOptions:");
			Console.WriteLine("\t\t--namespace namespace: namespace of the generated code");
			Console.WriteLine("\t\t--output-path path: output directory");
			Console.WriteLine("\t\t--template-path path: custom template directory containing the templates");
			Console.WriteLine("\t\t--language lang: language to generate (c#, vb, cpp or java)");
		}
	}
}
