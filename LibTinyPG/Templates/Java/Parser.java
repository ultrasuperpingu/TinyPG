﻿// Automatically generated from source file: <%SourceFilename%>
// By TinyPG v<%GeneratorVersion%> available at https://github.com/ultrasuperpingu/TinyPG

package <%Namespace%>;


public class Parser
{
	private final Scanner scanner;
	private ParseTree tree;
	
	public Parser(Scanner scanner)
	{
		this.scanner = scanner;
	}

	public ParseTree Parse(String input)
	{
		tree = new ParseTree();
		return Parse(input, tree);
	}

	public ParseTree Parse(String input, ParseTree tree)
	{
		scanner.Init(input);

		this.tree = tree;
		ParseStart(tree);
		tree.Skipped = scanner.Skipped;

		return tree;
	}

<%ParseNonTerminals%>

}
