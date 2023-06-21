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
using System.IO;
using System.Windows.Forms;
using TinyPG.Parsing;
using TinyPG.Compiler;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace TinyPG
{
	public class Program
	{
		public enum ExitCode : int
		{
			Success = 0,
			InvalidFilename = 1,
			UnknownError = 10
		}

		[STAThread]
		public static int Main(string[] args)
		{
			if (args.Length > 0)
			{
				string GrammarFilePath = null;
				bool generate = false;
				for (int i = 0; i<args.Length; i++)
				{
					string key = args[i];
					if (!key.StartsWith("-"))
					{
						if (File.Exists(key))
						{
							if(string.IsNullOrEmpty(GrammarFilePath))
								GrammarFilePath = key;
							else
								Console.Error.WriteLine("You can only specify one file to open or generate. File " + key + " will be ignored.");
						}
						else
						{
							Console.Error.WriteLine("Specified file " + key + " does not exists.");
						}
					}
					else if(key.ToLower().StartsWith("--generate") || key.ToLower().StartsWith("-generate") || key.ToLower().StartsWith("-g"))
					{
						generate = true;
					}
					else
					{
						Console.Error.WriteLine("Specified argument unknown " + key);
					}
				}
				if (generate)
					GenerateFromGrammar(GrammarFilePath);
				else
					OpenMainForm(GrammarFilePath);
			}
			else
			{
				OpenMainForm();
			}
			return (int)ExitCode.Success;
		}

		private static void GenerateFromGrammar(string GrammarFilePath)
		{
			if (!File.Exists(GrammarFilePath))
			{
				Console.Error.WriteLine("Specified file " + GrammarFilePath + " does not exists");
				return;
			}
			StringBuilder output = new StringBuilder(string.Empty);
			//As stated in documentation current directory is the one of the TPG file.
			Directory.SetCurrentDirectory(Path.GetDirectoryName(GrammarFilePath));

			DateTime starttimer = DateTime.Now;

			Program prog = new Program(ManageParseError, output);
			Grammar grammar = prog.ParseGrammar(File.ReadAllText(GrammarFilePath));

			if (grammar != null)
			{
				grammar.Filename = GrammarFilePath;
				if (prog.BuildCode(grammar, new TinyPG.Compiler.Compiler()))
				{
					TimeSpan span = DateTime.Now.Subtract(starttimer);
					output.AppendLine("Compilation successful in " + span.TotalMilliseconds + "ms.");
				}
			}

			Console.WriteLine(output.ToString());
		}

		private static void OpenMainForm(string file = null)
		{
			Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			var form = new MainForm();
			if (!string.IsNullOrEmpty(file))
			{
				// grammar will be loaded on form load (via LoadConfig)
				form.GrammarFile = file;
			}
			Application.Run(form);
		}

		static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
		{
			MessageBox.Show("An unhandled exception occured: " + e.Exception.Message);
		}

		public delegate void OnParseErrorDelegate(ParseTree tree, StringBuilder output);
		private OnParseErrorDelegate parseErrorDelegate;
		private StringBuilder output;
		public StringBuilder Output { get { return this.output; } }

		public Program(OnParseErrorDelegate parseErrorDelegate, StringBuilder output)
		{
			this.parseErrorDelegate = parseErrorDelegate;
			this.output = output;
		}

		public Grammar ParseGrammar(string input)
		{
			Grammar grammar = null;
			Scanner scanner = new Scanner();
			Parser parser = new Parser(scanner);

			ParseTree tree = parser.Parse(input, new GrammarTree());

			if (tree.Errors.Count > 0)
			{
				this.parseErrorDelegate(tree, this.output);
			}
			else
			{
				grammar = (Grammar)tree.Eval();
				grammar.Preprocess();

				if (tree.Errors.Count == 0)
				{
					this.output.AppendLine(grammar.PrintGrammar());
					this.output.AppendLine(grammar.PrintFirsts());

					this.output.AppendLine("Parse successful!\r\n");
				}
			}
			return grammar;
		}


		public bool BuildCode(Grammar grammar, TinyPG.Compiler.Compiler compiler)
		{
			this.output.AppendLine("Building code...");
			compiler.Compile(grammar);
			foreach (string err in compiler.Errors)
				this.output.AppendLine(err);
			if (!compiler.IsCompiled)
			{
				this.output.AppendLine("Compilation contains errors, could not compile.");
			}

			new GeneratedFilesWriter(grammar).Generate();

			return compiler.IsCompiled;
		}

		protected static void ManageParseError(ParseTree tree, StringBuilder output)
		{
			foreach (ParseError error in tree.Errors)
				output.AppendLine(string.Format("({0},{1}): {2}", error.Line, error.Column, error.Message));

			output.AppendLine("Semantic errors in grammar found.");
		}

	}
}
