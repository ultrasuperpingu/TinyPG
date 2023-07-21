// Automatically generated from source file: simple expression2_java.tpg
// By TinyPG v1.6 available at https://github.com/ultrasuperpingu/TinyPG

package tinypg;


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

	public ParseTree ParseStart(String input, ParseTree tree) // NonTerminalSymbol: Start
	{
		scanner.Init(input);
		this.tree = tree;
		ParseStart(tree);
		tree.Skipped = scanner.Skipped;
		return tree;
	}

	private void ParseStart(ParseNode parent) // NonTerminalSymbol: Start
	{
		Token tok;
		ParseNode n;
		ParseNode node = parent.CreateNode(scanner.GetToken(TokenType.Start), "Start");
		parent.getNodes().add(node);


		 // Concat Rule
		tok = scanner.LookAhead(TokenType.NUMBER, TokenType.BROPEN, TokenType.ID, TokenType.EOF); // Option Rule
		if (tok.Type == TokenType.NUMBER
		    || tok.Type == TokenType.BROPEN
		    || tok.Type == TokenType.ID)
		{
			ParseAddExpr(node); // NonTerminal Rule: AddExpr
		}

		 // Concat Rule
		tok = scanner.Scan(TokenType.EOF); // Terminal Rule: EOF
		n = node.CreateNode(tok, tok.toString() );
		node.Token.UpdateRange(tok);
		node.getNodes().add(n);
		if (tok.Type != TokenType.EOF) {
			tree.Errors.add(new ParseError("Unexpected token '" + tok.getText().replace("\n", "") + "' found. Expected 'EOF'.", 0x1001, tok));
			return;
		}

		parent.Token.UpdateRange(node.Token);
	} // NonTerminalSymbol: Start

	public ParseTree ParseAddExpr(String input, ParseTree tree) // NonTerminalSymbol: AddExpr
	{
		scanner.Init(input);
		this.tree = tree;
		ParseAddExpr(tree);
		tree.Skipped = scanner.Skipped;
		return tree;
	}

	private void ParseAddExpr(ParseNode parent) // NonTerminalSymbol: AddExpr
	{
		Token tok;
		ParseNode n;
		ParseNode node = parent.CreateNode(scanner.GetToken(TokenType.AddExpr), "AddExpr");
		parent.getNodes().add(node);


		 // Concat Rule
		ParseMultExpr(node); // NonTerminal Rule: MultExpr

		 // Concat Rule
		tok = scanner.LookAhead(TokenType.PLUSMINUS); // ZeroOrMore Rule
		while (tok.Type == TokenType.PLUSMINUS)
		{

			 // Concat Rule
			tok = scanner.Scan(TokenType.PLUSMINUS); // Terminal Rule: PLUSMINUS
			n = node.CreateNode(tok, tok.toString() );
			node.Token.UpdateRange(tok);
			node.getNodes().add(n);
			if (tok.Type != TokenType.PLUSMINUS) {
				tree.Errors.add(new ParseError("Unexpected token '" + tok.getText().replace("\n", "") + "' found. Expected 'PLUSMINUS'.", 0x1001, tok));
				return;
			}

			 // Concat Rule
			ParseMultExpr(node); // NonTerminal Rule: MultExpr
		tok = scanner.LookAhead(TokenType.PLUSMINUS); // ZeroOrMore Rule
		}

		parent.Token.UpdateRange(node.Token);
	} // NonTerminalSymbol: AddExpr

	public ParseTree ParseMultExpr(String input, ParseTree tree) // NonTerminalSymbol: MultExpr
	{
		scanner.Init(input);
		this.tree = tree;
		ParseMultExpr(tree);
		tree.Skipped = scanner.Skipped;
		return tree;
	}

	private void ParseMultExpr(ParseNode parent) // NonTerminalSymbol: MultExpr
	{
		Token tok;
		ParseNode n;
		ParseNode node = parent.CreateNode(scanner.GetToken(TokenType.MultExpr), "MultExpr");
		parent.getNodes().add(node);


		 // Concat Rule
		ParseAtom(node); // NonTerminal Rule: Atom

		 // Concat Rule
		tok = scanner.LookAhead(TokenType.MULTDIV); // ZeroOrMore Rule
		while (tok.Type == TokenType.MULTDIV)
		{

			 // Concat Rule
			tok = scanner.Scan(TokenType.MULTDIV); // Terminal Rule: MULTDIV
			n = node.CreateNode(tok, tok.toString() );
			node.Token.UpdateRange(tok);
			node.getNodes().add(n);
			if (tok.Type != TokenType.MULTDIV) {
				tree.Errors.add(new ParseError("Unexpected token '" + tok.getText().replace("\n", "") + "' found. Expected 'MULTDIV'.", 0x1001, tok));
				return;
			}

			 // Concat Rule
			ParseAtom(node); // NonTerminal Rule: Atom
		tok = scanner.LookAhead(TokenType.MULTDIV); // ZeroOrMore Rule
		}

		parent.Token.UpdateRange(node.Token);
	} // NonTerminalSymbol: MultExpr

	public ParseTree ParseAtom(String input, ParseTree tree) // NonTerminalSymbol: Atom
	{
		scanner.Init(input);
		this.tree = tree;
		ParseAtom(tree);
		tree.Skipped = scanner.Skipped;
		return tree;
	}

	private void ParseAtom(ParseNode parent) // NonTerminalSymbol: Atom
	{
		Token tok;
		ParseNode n;
		ParseNode node = parent.CreateNode(scanner.GetToken(TokenType.Atom), "Atom");
		parent.getNodes().add(node);

		tok = scanner.LookAhead(TokenType.NUMBER, TokenType.BROPEN, TokenType.ID);
		switch (tok.Type)
		{ // Choice Rule
			case NUMBER:
				tok = scanner.Scan(TokenType.NUMBER); // Terminal Rule: NUMBER
				n = node.CreateNode(tok, tok.toString() );
				node.Token.UpdateRange(tok);
				node.getNodes().add(n);
				if (tok.Type != TokenType.NUMBER) {
					tree.Errors.add(new ParseError("Unexpected token '" + tok.getText().replace("\n", "") + "' found. Expected 'NUMBER'.", 0x1001, tok));
					return;
				}
				break;
			case BROPEN:

				 // Concat Rule
				tok = scanner.Scan(TokenType.BROPEN); // Terminal Rule: BROPEN
				n = node.CreateNode(tok, tok.toString() );
				node.Token.UpdateRange(tok);
				node.getNodes().add(n);
				if (tok.Type != TokenType.BROPEN) {
					tree.Errors.add(new ParseError("Unexpected token '" + tok.getText().replace("\n", "") + "' found. Expected 'BROPEN'.", 0x1001, tok));
					return;
				}

				 // Concat Rule
				ParseAddExpr(node); // NonTerminal Rule: AddExpr

				 // Concat Rule
				tok = scanner.Scan(TokenType.BRCLOSE); // Terminal Rule: BRCLOSE
				n = node.CreateNode(tok, tok.toString() );
				node.Token.UpdateRange(tok);
				node.getNodes().add(n);
				if (tok.Type != TokenType.BRCLOSE) {
					tree.Errors.add(new ParseError("Unexpected token '" + tok.getText().replace("\n", "") + "' found. Expected 'BRCLOSE'.", 0x1001, tok));
					return;
				}
				break;
			case ID:
				tok = scanner.Scan(TokenType.ID); // Terminal Rule: ID
				n = node.CreateNode(tok, tok.toString() );
				node.Token.UpdateRange(tok);
				node.getNodes().add(n);
				if (tok.Type != TokenType.ID) {
					tree.Errors.add(new ParseError("Unexpected token '" + tok.getText().replace("\n", "") + "' found. Expected 'ID'.", 0x1001, tok));
					return;
				}
				break;
			default:
				tree.Errors.add(new ParseError("Unexpected token '" + tok.getText().replace("\n", "") + "' found. Expected NUMBER, BROPEN, or ID.", 0x0002, tok));
				break;
		} // Choice Rule

		parent.Token.UpdateRange(node.Token);
	} // NonTerminalSymbol: Atom



}
