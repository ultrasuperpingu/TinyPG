// Automatically generated from source file: <%SourceFilename%>
// By TinyPG v1.5 available at https://github.com/ultrasuperpingu/TinyPG

using System;
using System.Collections.Generic;

// Disable unused variable warnings which
// can happen during the parser generation.
//#pragma warning disable 168

namespace <%Namespace%>
{
	#region Parser

	public partial class Parser <%IParser%>
	{
		private Scanner scanner;
		private ParseTree tree;

		public Parser(Scanner scanner)
		{
			this.scanner = scanner;
		}

		public <%IParseTree%> Parse(string input)
		{
			return Parse(input, new ParseTree());
		}

		public <%IParseTree%> Parse(string input, ParseTree tree)
		{
			scanner.Init(input);

			this.tree = tree;
			ParseStart(tree);
			tree.Skipped = scanner.Skipped;

			return tree;
		}

<%ParseNonTerminals%>

	}

	#endregion Parser
}
