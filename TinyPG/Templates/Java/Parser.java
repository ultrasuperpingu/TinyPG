﻿// Automatically generated from source file: <%SourceFilename%>
// By TinyPG v1.3 available at http://github.com/SickheadGames/TinyPG

package <%Namespace%>;

    
public class Parser <%IParser%>
{
	private Scanner scanner;
	private ParseTree tree;
	
	public Parser(Scanner scanner)
	{
		this.scanner = scanner;
	}

	public <%IParseTree%> Parse(String input)
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

<%ParserCustomCode%>
}
