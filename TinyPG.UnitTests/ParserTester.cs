﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TinyPG.Parsing;
using TinyPG.Compiler;
using System.IO;

namespace TinyPG.UnitTests
{
	/// <summary>
	/// Summary description for UnitTest1
	/// </summary>
	[TestClass]
	public class ParserTester
	{
		// TODO: set the correct paths to be able to run the unittests succesfully
		private const string OUTPUTPATH = @"";
		private const string TESTFILESPATH = @"Examples\";

		public ParserTester()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		private TestContext testContextInstance;

		/// <summary>
		///Gets or sets the test context which provides
		///information about and functionality for the current test run.
		///</summary>
		public TestContext TestContext
		{
			get
			{
				return testContextInstance;
			}
			set
			{
				testContextInstance = value;
			}
		}

		#region Additional test attributes
		//
		// You can use the following additional attributes as you write your tests:
		//
		// Use ClassInitialize to run code before running the first test in the class
		// [ClassInitialize()]
		// public static void MyClassInitialize(TestContext testContext) { }
		//
		// Use ClassCleanup to run code after all tests in a class have run
		// [ClassCleanup()]
		// public static void MyClassCleanup() { }
		//
		// Use TestInitialize to run code before running each test 
		// [TestInitialize()]
		// public void MyTestInitialize() { }
		//
		// Use TestCleanup to run code after each test has run
		// [TestCleanup()]
		// public void MyTestCleanup() { }
		//
		#endregion


		private GrammarTree LoadGrammar(string filename)
		{
			string grammarfile = System.IO.File.ReadAllText(filename);
			Scanner scanner = new Scanner();
			Parser parser = new Parser(scanner);
			GrammarTree tree = (GrammarTree)parser.Parse(grammarfile, new GrammarTree());
			return tree;
		}

		[TestMethod]
		public void SimpleExpression1_Test()
		{
			var file = Path.Combine(TESTFILESPATH, @"simple expression1.tpg");
			Grammar G = Grammar.FromFile(file);

			G.Directives["TinyPG"]["OutputPath"] = OUTPUTPATH;

			// basic checks
			string temp = G.PrintFirsts();
			Assert.IsTrue(!String.IsNullOrEmpty(temp));
			temp = G.GetOutputPath();
			Assert.IsTrue(!String.IsNullOrEmpty(temp));
			temp = G.PrintGrammar();
			Assert.IsTrue(!String.IsNullOrEmpty(temp));

			Compiler.Compiler compiler = new Compiler.Compiler();

			compiler.Compile(G);

			Assert.IsTrue(compiler.Errors.Count == 0, "compilation contains errors");

			CompilerResult result = compiler.Run("5+7/3*2+(4*2)");



			Assert.IsTrue(result.Output.StartsWith("Parse was successful."));
		}

		[TestMethod]
		public void SimpleExpression2_Test()
		{
			var file = Path.Combine(TESTFILESPATH, @"simple expression2.tpg");
			Grammar G = Grammar.FromFile(file);

			G.Directives.Add(new Directive("TinyPG"));
			
			Compiler.Compiler compiler = new Compiler.Compiler();

			compiler.Compile(G);
			Assert.IsTrue(compiler.Errors.Count == 0, "compilation contains errors");

			CompilerResult result = compiler.Run("5+8/4*2+(4*2)");

			Assert.IsTrue(Convert.ToInt32(result.Value) == 17);
		}

		[TestMethod]
		public void SimpleExpression2_VB_Test()
		{
			var file = Path.Combine(TESTFILESPATH, @"simple expression2_vb.tpg");
			Grammar G = Grammar.FromFile(file);

			Compiler.Compiler compiler = new Compiler.Compiler();

			compiler.Compile(G);
			Assert.IsTrue(compiler.Errors.Count == 0, "compilation contains errors");

			CompilerResult result = compiler.Run("5+8/4*2+(4*2)");

			Assert.IsTrue(Convert.ToInt32(result.Value) == 17);
		}

		[TestMethod]
		public void SimpleExpression3_Test()
		{
			var file = Path.Combine(TESTFILESPATH, @"BNFGrammar 1.5.tpg");
			Grammar G = Grammar.FromFile(file);

			Compiler.Compiler compiler = new Compiler.Compiler();

			compiler.Compile(G);
			Assert.IsTrue(compiler.Errors.Count == 0, "compilation contains errors");

			CompilerResult result = compiler.Run("");

			Assert.IsTrue(result.Output.StartsWith("Parse was successful."));
		}


		[TestMethod]
		public void SimpleExpression4_Test()
		{
			var file = Path.Combine(TESTFILESPATH, @"GrammarHighlighter v1.3.tpg");
			Grammar G = Grammar.FromFile(file);
			
			Compiler.Compiler compiler = new Compiler.Compiler();

			compiler.Compile(G);
			Assert.IsTrue(compiler.Errors.Count == 0, "compilation contains errors");

			CompilerResult result = compiler.Run("using System.IO;\r\n");

			Assert.IsTrue(result.Output.StartsWith("Parse was successful."));
		}

		[TestMethod]
		public void SimpleExpression4_VB_Test()
		{
			var file = Path.Combine(TESTFILESPATH, @"GrammarHighlighter_vb.tpg");
			Grammar G = Grammar.FromFile(file);
			
			Compiler.Compiler compiler = new Compiler.Compiler();

			compiler.Compile(G);
			Assert.IsTrue(compiler.Errors.Count == 0, "compilation contains errors");

			CompilerResult result = compiler.Run("using System.IO;\r\n");

			Assert.IsTrue(result.Output.StartsWith("Parse was successful."));
		}

		[TestMethod]
		public void TinyExe_Test()
		{
			var file = Path.Combine(TESTFILESPATH, @"TinyExpEval.tpg");
			Grammar G = Grammar.FromFile(file);

			G.Directives.Add(new Directive("TinyPG"));
			
			Compiler.Compiler compiler = new Compiler.Compiler();

			compiler.Compile(G);
			Assert.IsTrue(compiler.Errors.Count == 0, "compilation contains errors");

			CompilerResult result = compiler.Run("f(x) := cos(x)^x\r\n");

			Assert.IsTrue(result.Output.StartsWith("Parse was successful."));
		}
	}
}
