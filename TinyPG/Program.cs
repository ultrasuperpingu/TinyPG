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
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using TinyPG.Compiler;
using TinyPG.Parsing;

namespace TinyPG
{
	public class Program
	{
		public enum ExitCode : int
		{
			Success = 0,
			InvalidFilename = 1,
			ParsingFails = 2,
			EvalGrammarFails = 3,
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
							if (string.IsNullOrEmpty(GrammarFilePath))
								GrammarFilePath = key;
							else
								Console.Error.WriteLine("You can only specify one file to open or generate. File " + key + " will be ignored.");
						}
						else
						{
							Console.Error.WriteLine("Specified file " + key + " does not exists.");
						}
					}
					else if (key.ToLower().StartsWith("--generate") || key.ToLower().StartsWith("-generate") || key.ToLower().StartsWith("-g"))
					{
						generate = true;
					}
					else
					{
						Console.Error.WriteLine("Specified argument unknown " + key);
					}
				}
				if (generate)
					return GenerateCodeFromGrammar(GrammarFilePath);
				else
					OpenMainForm(GrammarFilePath);
			}
			else
			{
				OpenMainForm();
			}
			return 0;
		}

		private static int GenerateCodeFromGrammar(string GrammarFilePath)
		{
			if (!File.Exists(GrammarFilePath))
			{
				Console.Error.WriteLine("Specified file " + GrammarFilePath + " does not exists");
				return (int)ExitCode.InvalidFilename;
			}
			//As stated in documentation current directory is the one of the TPG file.
			Directory.SetCurrentDirectory(Path.GetDirectoryName(GrammarFilePath));

			DateTime starttimer = DateTime.Now;
			ParseErrors errors;
			Grammar grammar = Grammar.FromSource(File.ReadAllText(GrammarFilePath), out errors);
			foreach (ParseError error in errors)
				Console.Error.WriteLine(string.Format((error.IsWarning ? "Warning: " : "Error: ")+"({0},{1}): {2}", error.Line, error.Column, error.Message));
			TimeSpan duration = DateTime.Now - starttimer;

			if (grammar != null && !errors.ContainsErrors)
			{
				Console.WriteLine("Grammar parsed successfully in " + duration.TotalMilliseconds.ToString(CultureInfo.InvariantCulture) + "ms.");

				grammar.Filename = GrammarFilePath;
				new GeneratedFilesWriter(grammar).Generate();

				Console.WriteLine(grammar.PrintGrammar());
				Console.WriteLine(grammar.PrintFirsts());
				Console.WriteLine("Parse successful!\r\n");
			}
			else
			{
				return (int)ExitCode.ParsingFails;
			}
			return (int)ExitCode.Success;
		}
		/*[DllImport("kernel32.dll", SetLastError = true)]
		internal static extern int AllocConsole();
		[DllImport("user32.dll")]
		static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
		[DllImport("kernel32.dll")]
		static extern IntPtr GetConsoleWindow();*/
		[DllImport("kernel32.dll")]
		static extern bool FreeConsole();
		private static void OpenMainForm(string file = null)
		{
			FreeConsole();
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

		private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
		{
			MessageBox.Show("An unhandled exception occured: " + e.Exception.Message);
		}

	}
}
