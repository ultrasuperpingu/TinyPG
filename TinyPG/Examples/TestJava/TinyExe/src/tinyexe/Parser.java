// Automatically generated from source file: TinyExpEval_java.tpg
// By TinyPG v1.5 available at https://github.com/ultrasuperpingu/TinyPG

package tinyexe;


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
		tok = scanner.LookAhead(TokenType.FUNCTION, TokenType.VARIABLE, TokenType.BOOLEANLITERAL, TokenType.DECIMALINTEGERLITERAL, TokenType.HEXINTEGERLITERAL, TokenType.REALLITERAL, TokenType.STRINGLITERAL, TokenType.BRACKETOPEN, TokenType.PLUS, TokenType.MINUS, TokenType.NOT, TokenType.ASSIGN, TokenType.EOF_); // Option Rule
		if (tok.Type == TokenType.FUNCTION
		    || tok.Type == TokenType.VARIABLE
		    || tok.Type == TokenType.BOOLEANLITERAL
		    || tok.Type == TokenType.DECIMALINTEGERLITERAL
		    || tok.Type == TokenType.HEXINTEGERLITERAL
		    || tok.Type == TokenType.REALLITERAL
		    || tok.Type == TokenType.STRINGLITERAL
		    || tok.Type == TokenType.BRACKETOPEN
		    || tok.Type == TokenType.PLUS
		    || tok.Type == TokenType.MINUS
		    || tok.Type == TokenType.NOT
		    || tok.Type == TokenType.ASSIGN)
		{
			ParseExpression(node); // NonTerminal Rule: Expression
		}

		 // Concat Rule
		tok = scanner.Scan(TokenType.EOF_); // Terminal Rule: EOF_
		n = node.CreateNode(tok, tok.toString() );
		node.Token.UpdateRange(tok);
		node.getNodes().add(n);
		if (tok.Type != TokenType.EOF_) {
			tree.Errors.add(new ParseError("Unexpected token '" + tok.getText().replace("\n", "") + "' found. Expected 'EOF_'.", 0x1001, tok));
			return;
		}

		parent.Token.UpdateRange(node.Token);
	} // NonTerminalSymbol: Start

	public ParseTree ParseFunction(String input, ParseTree tree) // NonTerminalSymbol: Function
	{
		scanner.Init(input);
		this.tree = tree;
		ParseFunction(tree);
		tree.Skipped = scanner.Skipped;
		return tree;
	}

	private void ParseFunction(ParseNode parent) // NonTerminalSymbol: Function
	{
		Token tok;
		ParseNode n;
		ParseNode node = parent.CreateNode(scanner.GetToken(TokenType.Function), "Function");
		parent.getNodes().add(node);


		 // Concat Rule
		tok = scanner.Scan(TokenType.FUNCTION); // Terminal Rule: FUNCTION
		n = node.CreateNode(tok, tok.toString() );
		node.Token.UpdateRange(tok);
		node.getNodes().add(n);
		if (tok.Type != TokenType.FUNCTION) {
			tree.Errors.add(new ParseError("Unexpected token '" + tok.getText().replace("\n", "") + "' found. Expected 'FUNCTION'.", 0x1001, tok));
			return;
		}

		 // Concat Rule
		tok = scanner.Scan(TokenType.BRACKETOPEN); // Terminal Rule: BRACKETOPEN
		n = node.CreateNode(tok, tok.toString() );
		node.Token.UpdateRange(tok);
		node.getNodes().add(n);
		if (tok.Type != TokenType.BRACKETOPEN) {
			tree.Errors.add(new ParseError("Unexpected token '" + tok.getText().replace("\n", "") + "' found. Expected 'BRACKETOPEN'.", 0x1001, tok));
			return;
		}

		 // Concat Rule
		tok = scanner.LookAhead(TokenType.FUNCTION, TokenType.VARIABLE, TokenType.BOOLEANLITERAL, TokenType.DECIMALINTEGERLITERAL, TokenType.HEXINTEGERLITERAL, TokenType.REALLITERAL, TokenType.STRINGLITERAL, TokenType.BRACKETOPEN, TokenType.PLUS, TokenType.MINUS, TokenType.NOT, TokenType.ASSIGN, TokenType.SEMICOLON, TokenType.BRACKETCLOSE); // Option Rule
		if (tok.Type == TokenType.FUNCTION
		    || tok.Type == TokenType.VARIABLE
		    || tok.Type == TokenType.BOOLEANLITERAL
		    || tok.Type == TokenType.DECIMALINTEGERLITERAL
		    || tok.Type == TokenType.HEXINTEGERLITERAL
		    || tok.Type == TokenType.REALLITERAL
		    || tok.Type == TokenType.STRINGLITERAL
		    || tok.Type == TokenType.BRACKETOPEN
		    || tok.Type == TokenType.PLUS
		    || tok.Type == TokenType.MINUS
		    || tok.Type == TokenType.NOT
		    || tok.Type == TokenType.ASSIGN
		    || tok.Type == TokenType.SEMICOLON)
		{
			ParseParams(node); // NonTerminal Rule: Params
		}

		 // Concat Rule
		tok = scanner.Scan(TokenType.BRACKETCLOSE); // Terminal Rule: BRACKETCLOSE
		n = node.CreateNode(tok, tok.toString() );
		node.Token.UpdateRange(tok);
		node.getNodes().add(n);
		if (tok.Type != TokenType.BRACKETCLOSE) {
			tree.Errors.add(new ParseError("Unexpected token '" + tok.getText().replace("\n", "") + "' found. Expected 'BRACKETCLOSE'.", 0x1001, tok));
			return;
		}

		parent.Token.UpdateRange(node.Token);
	} // NonTerminalSymbol: Function

	public ParseTree ParsePrimaryExpression(String input, ParseTree tree) // NonTerminalSymbol: PrimaryExpression
	{
		scanner.Init(input);
		this.tree = tree;
		ParsePrimaryExpression(tree);
		tree.Skipped = scanner.Skipped;
		return tree;
	}

	private void ParsePrimaryExpression(ParseNode parent) // NonTerminalSymbol: PrimaryExpression
	{
		Token tok;
		ParseNode n;
		ParseNode node = parent.CreateNode(scanner.GetToken(TokenType.PrimaryExpression), "PrimaryExpression");
		parent.getNodes().add(node);

		tok = scanner.LookAhead(TokenType.FUNCTION, TokenType.VARIABLE, TokenType.BOOLEANLITERAL, TokenType.DECIMALINTEGERLITERAL, TokenType.HEXINTEGERLITERAL, TokenType.REALLITERAL, TokenType.STRINGLITERAL, TokenType.BRACKETOPEN);
		switch (tok.Type)
		{ // Choice Rule
			case FUNCTION:
				ParseFunction(node); // NonTerminal Rule: Function
				break;
			case VARIABLE:
				ParseVariable(node); // NonTerminal Rule: Variable
				break;
			case BOOLEANLITERAL:
			case DECIMALINTEGERLITERAL:
			case HEXINTEGERLITERAL:
			case REALLITERAL:
			case STRINGLITERAL:
				ParseLiteral(node); // NonTerminal Rule: Literal
				break;
			case BRACKETOPEN:
				ParseParenthesizedExpression(node); // NonTerminal Rule: ParenthesizedExpression
				break;
			default:
				tree.Errors.add(new ParseError("Unexpected token '" + tok.getText().replace("\n", "") + "' found. Expected FUNCTION, VARIABLE, BOOLEANLITERAL, DECIMALINTEGERLITERAL, HEXINTEGERLITERAL, REALLITERAL, STRINGLITERAL, or BRACKETOPEN.", 0x0002, tok));
				break;
		} // Choice Rule

		parent.Token.UpdateRange(node.Token);
	} // NonTerminalSymbol: PrimaryExpression

	public ParseTree ParseParenthesizedExpression(String input, ParseTree tree) // NonTerminalSymbol: ParenthesizedExpression
	{
		scanner.Init(input);
		this.tree = tree;
		ParseParenthesizedExpression(tree);
		tree.Skipped = scanner.Skipped;
		return tree;
	}

	private void ParseParenthesizedExpression(ParseNode parent) // NonTerminalSymbol: ParenthesizedExpression
	{
		Token tok;
		ParseNode n;
		ParseNode node = parent.CreateNode(scanner.GetToken(TokenType.ParenthesizedExpression), "ParenthesizedExpression");
		parent.getNodes().add(node);


		 // Concat Rule
		tok = scanner.Scan(TokenType.BRACKETOPEN); // Terminal Rule: BRACKETOPEN
		n = node.CreateNode(tok, tok.toString() );
		node.Token.UpdateRange(tok);
		node.getNodes().add(n);
		if (tok.Type != TokenType.BRACKETOPEN) {
			tree.Errors.add(new ParseError("Unexpected token '" + tok.getText().replace("\n", "") + "' found. Expected 'BRACKETOPEN'.", 0x1001, tok));
			return;
		}

		 // Concat Rule
		ParseExpression(node); // NonTerminal Rule: Expression

		 // Concat Rule
		tok = scanner.Scan(TokenType.BRACKETCLOSE); // Terminal Rule: BRACKETCLOSE
		n = node.CreateNode(tok, tok.toString() );
		node.Token.UpdateRange(tok);
		node.getNodes().add(n);
		if (tok.Type != TokenType.BRACKETCLOSE) {
			tree.Errors.add(new ParseError("Unexpected token '" + tok.getText().replace("\n", "") + "' found. Expected 'BRACKETCLOSE'.", 0x1001, tok));
			return;
		}

		parent.Token.UpdateRange(node.Token);
	} // NonTerminalSymbol: ParenthesizedExpression

	public ParseTree ParseUnaryExpression(String input, ParseTree tree) // NonTerminalSymbol: UnaryExpression
	{
		scanner.Init(input);
		this.tree = tree;
		ParseUnaryExpression(tree);
		tree.Skipped = scanner.Skipped;
		return tree;
	}

	private void ParseUnaryExpression(ParseNode parent) // NonTerminalSymbol: UnaryExpression
	{
		Token tok;
		ParseNode n;
		ParseNode node = parent.CreateNode(scanner.GetToken(TokenType.UnaryExpression), "UnaryExpression");
		parent.getNodes().add(node);

		tok = scanner.LookAhead(TokenType.FUNCTION, TokenType.VARIABLE, TokenType.BOOLEANLITERAL, TokenType.DECIMALINTEGERLITERAL, TokenType.HEXINTEGERLITERAL, TokenType.REALLITERAL, TokenType.STRINGLITERAL, TokenType.BRACKETOPEN, TokenType.PLUS, TokenType.MINUS, TokenType.NOT);
		switch (tok.Type)
		{ // Choice Rule
			case FUNCTION:
			case VARIABLE:
			case BOOLEANLITERAL:
			case DECIMALINTEGERLITERAL:
			case HEXINTEGERLITERAL:
			case REALLITERAL:
			case STRINGLITERAL:
			case BRACKETOPEN:
				ParsePrimaryExpression(node); // NonTerminal Rule: PrimaryExpression
				break;
			case PLUS:

				 // Concat Rule
				tok = scanner.Scan(TokenType.PLUS); // Terminal Rule: PLUS
				n = node.CreateNode(tok, tok.toString() );
				node.Token.UpdateRange(tok);
				node.getNodes().add(n);
				if (tok.Type != TokenType.PLUS) {
					tree.Errors.add(new ParseError("Unexpected token '" + tok.getText().replace("\n", "") + "' found. Expected 'PLUS'.", 0x1001, tok));
					return;
				}

				 // Concat Rule
				ParseUnaryExpression(node); // NonTerminal Rule: UnaryExpression
				break;
			case MINUS:

				 // Concat Rule
				tok = scanner.Scan(TokenType.MINUS); // Terminal Rule: MINUS
				n = node.CreateNode(tok, tok.toString() );
				node.Token.UpdateRange(tok);
				node.getNodes().add(n);
				if (tok.Type != TokenType.MINUS) {
					tree.Errors.add(new ParseError("Unexpected token '" + tok.getText().replace("\n", "") + "' found. Expected 'MINUS'.", 0x1001, tok));
					return;
				}

				 // Concat Rule
				ParseUnaryExpression(node); // NonTerminal Rule: UnaryExpression
				break;
			case NOT:

				 // Concat Rule
				tok = scanner.Scan(TokenType.NOT); // Terminal Rule: NOT
				n = node.CreateNode(tok, tok.toString() );
				node.Token.UpdateRange(tok);
				node.getNodes().add(n);
				if (tok.Type != TokenType.NOT) {
					tree.Errors.add(new ParseError("Unexpected token '" + tok.getText().replace("\n", "") + "' found. Expected 'NOT'.", 0x1001, tok));
					return;
				}

				 // Concat Rule
				ParseUnaryExpression(node); // NonTerminal Rule: UnaryExpression
				break;
			default:
				tree.Errors.add(new ParseError("Unexpected token '" + tok.getText().replace("\n", "") + "' found. Expected FUNCTION, VARIABLE, BOOLEANLITERAL, DECIMALINTEGERLITERAL, HEXINTEGERLITERAL, REALLITERAL, STRINGLITERAL, BRACKETOPEN, PLUS, MINUS, or NOT.", 0x0002, tok));
				break;
		} // Choice Rule

		parent.Token.UpdateRange(node.Token);
	} // NonTerminalSymbol: UnaryExpression

	public ParseTree ParsePowerExpression(String input, ParseTree tree) // NonTerminalSymbol: PowerExpression
	{
		scanner.Init(input);
		this.tree = tree;
		ParsePowerExpression(tree);
		tree.Skipped = scanner.Skipped;
		return tree;
	}

	private void ParsePowerExpression(ParseNode parent) // NonTerminalSymbol: PowerExpression
	{
		Token tok;
		ParseNode n;
		ParseNode node = parent.CreateNode(scanner.GetToken(TokenType.PowerExpression), "PowerExpression");
		parent.getNodes().add(node);


		 // Concat Rule
		ParseUnaryExpression(node); // NonTerminal Rule: UnaryExpression

		 // Concat Rule
		tok = scanner.LookAhead(TokenType.POWER); // ZeroOrMore Rule
		while (tok.Type == TokenType.POWER)
		{

			 // Concat Rule
			tok = scanner.Scan(TokenType.POWER); // Terminal Rule: POWER
			n = node.CreateNode(tok, tok.toString() );
			node.Token.UpdateRange(tok);
			node.getNodes().add(n);
			if (tok.Type != TokenType.POWER) {
				tree.Errors.add(new ParseError("Unexpected token '" + tok.getText().replace("\n", "") + "' found. Expected 'POWER'.", 0x1001, tok));
				return;
			}

			 // Concat Rule
			ParseUnaryExpression(node); // NonTerminal Rule: UnaryExpression
		tok = scanner.LookAhead(TokenType.POWER); // ZeroOrMore Rule
		}

		parent.Token.UpdateRange(node.Token);
	} // NonTerminalSymbol: PowerExpression

	public ParseTree ParseMultiplicativeExpression(String input, ParseTree tree) // NonTerminalSymbol: MultiplicativeExpression
	{
		scanner.Init(input);
		this.tree = tree;
		ParseMultiplicativeExpression(tree);
		tree.Skipped = scanner.Skipped;
		return tree;
	}

	private void ParseMultiplicativeExpression(ParseNode parent) // NonTerminalSymbol: MultiplicativeExpression
	{
		Token tok;
		ParseNode n;
		ParseNode node = parent.CreateNode(scanner.GetToken(TokenType.MultiplicativeExpression), "MultiplicativeExpression");
		parent.getNodes().add(node);


		 // Concat Rule
		ParsePowerExpression(node); // NonTerminal Rule: PowerExpression

		 // Concat Rule
		tok = scanner.LookAhead(TokenType.ASTERIKS, TokenType.SLASH, TokenType.PERCENT); // ZeroOrMore Rule
		while (tok.Type == TokenType.ASTERIKS
		    || tok.Type == TokenType.SLASH
		    || tok.Type == TokenType.PERCENT)
		{

			 // Concat Rule
			tok = scanner.LookAhead(TokenType.ASTERIKS, TokenType.SLASH, TokenType.PERCENT);
			switch (tok.Type)
			{ // Choice Rule
				case ASTERIKS:
					tok = scanner.Scan(TokenType.ASTERIKS); // Terminal Rule: ASTERIKS
					n = node.CreateNode(tok, tok.toString() );
					node.Token.UpdateRange(tok);
					node.getNodes().add(n);
					if (tok.Type != TokenType.ASTERIKS) {
						tree.Errors.add(new ParseError("Unexpected token '" + tok.getText().replace("\n", "") + "' found. Expected 'ASTERIKS'.", 0x1001, tok));
						return;
					}
					break;
				case SLASH:
					tok = scanner.Scan(TokenType.SLASH); // Terminal Rule: SLASH
					n = node.CreateNode(tok, tok.toString() );
					node.Token.UpdateRange(tok);
					node.getNodes().add(n);
					if (tok.Type != TokenType.SLASH) {
						tree.Errors.add(new ParseError("Unexpected token '" + tok.getText().replace("\n", "") + "' found. Expected 'SLASH'.", 0x1001, tok));
						return;
					}
					break;
				case PERCENT:
					tok = scanner.Scan(TokenType.PERCENT); // Terminal Rule: PERCENT
					n = node.CreateNode(tok, tok.toString() );
					node.Token.UpdateRange(tok);
					node.getNodes().add(n);
					if (tok.Type != TokenType.PERCENT) {
						tree.Errors.add(new ParseError("Unexpected token '" + tok.getText().replace("\n", "") + "' found. Expected 'PERCENT'.", 0x1001, tok));
						return;
					}
					break;
				default:
					tree.Errors.add(new ParseError("Unexpected token '" + tok.getText().replace("\n", "") + "' found. Expected ASTERIKS, SLASH, or PERCENT.", 0x0002, tok));
					break;
			} // Choice Rule

			 // Concat Rule
			ParsePowerExpression(node); // NonTerminal Rule: PowerExpression
		tok = scanner.LookAhead(TokenType.ASTERIKS, TokenType.SLASH, TokenType.PERCENT); // ZeroOrMore Rule
		}

		parent.Token.UpdateRange(node.Token);
	} // NonTerminalSymbol: MultiplicativeExpression

	public ParseTree ParseAdditiveExpression(String input, ParseTree tree) // NonTerminalSymbol: AdditiveExpression
	{
		scanner.Init(input);
		this.tree = tree;
		ParseAdditiveExpression(tree);
		tree.Skipped = scanner.Skipped;
		return tree;
	}

	private void ParseAdditiveExpression(ParseNode parent) // NonTerminalSymbol: AdditiveExpression
	{
		Token tok;
		ParseNode n;
		ParseNode node = parent.CreateNode(scanner.GetToken(TokenType.AdditiveExpression), "AdditiveExpression");
		parent.getNodes().add(node);


		 // Concat Rule
		ParseMultiplicativeExpression(node); // NonTerminal Rule: MultiplicativeExpression

		 // Concat Rule
		tok = scanner.LookAhead(TokenType.PLUS, TokenType.MINUS); // ZeroOrMore Rule
		while (tok.Type == TokenType.PLUS
		    || tok.Type == TokenType.MINUS)
		{

			 // Concat Rule
			tok = scanner.LookAhead(TokenType.PLUS, TokenType.MINUS);
			switch (tok.Type)
			{ // Choice Rule
				case PLUS:
					tok = scanner.Scan(TokenType.PLUS); // Terminal Rule: PLUS
					n = node.CreateNode(tok, tok.toString() );
					node.Token.UpdateRange(tok);
					node.getNodes().add(n);
					if (tok.Type != TokenType.PLUS) {
						tree.Errors.add(new ParseError("Unexpected token '" + tok.getText().replace("\n", "") + "' found. Expected 'PLUS'.", 0x1001, tok));
						return;
					}
					break;
				case MINUS:
					tok = scanner.Scan(TokenType.MINUS); // Terminal Rule: MINUS
					n = node.CreateNode(tok, tok.toString() );
					node.Token.UpdateRange(tok);
					node.getNodes().add(n);
					if (tok.Type != TokenType.MINUS) {
						tree.Errors.add(new ParseError("Unexpected token '" + tok.getText().replace("\n", "") + "' found. Expected 'MINUS'.", 0x1001, tok));
						return;
					}
					break;
				default:
					tree.Errors.add(new ParseError("Unexpected token '" + tok.getText().replace("\n", "") + "' found. Expected PLUS or MINUS.", 0x0002, tok));
					break;
			} // Choice Rule

			 // Concat Rule
			ParseMultiplicativeExpression(node); // NonTerminal Rule: MultiplicativeExpression
		tok = scanner.LookAhead(TokenType.PLUS, TokenType.MINUS); // ZeroOrMore Rule
		}

		parent.Token.UpdateRange(node.Token);
	} // NonTerminalSymbol: AdditiveExpression

	public ParseTree ParseConcatEpression(String input, ParseTree tree) // NonTerminalSymbol: ConcatEpression
	{
		scanner.Init(input);
		this.tree = tree;
		ParseConcatEpression(tree);
		tree.Skipped = scanner.Skipped;
		return tree;
	}

	private void ParseConcatEpression(ParseNode parent) // NonTerminalSymbol: ConcatEpression
	{
		Token tok;
		ParseNode n;
		ParseNode node = parent.CreateNode(scanner.GetToken(TokenType.ConcatEpression), "ConcatEpression");
		parent.getNodes().add(node);


		 // Concat Rule
		ParseAdditiveExpression(node); // NonTerminal Rule: AdditiveExpression

		 // Concat Rule
		tok = scanner.LookAhead(TokenType.AMP); // ZeroOrMore Rule
		while (tok.Type == TokenType.AMP)
		{

			 // Concat Rule
			tok = scanner.Scan(TokenType.AMP); // Terminal Rule: AMP
			n = node.CreateNode(tok, tok.toString() );
			node.Token.UpdateRange(tok);
			node.getNodes().add(n);
			if (tok.Type != TokenType.AMP) {
				tree.Errors.add(new ParseError("Unexpected token '" + tok.getText().replace("\n", "") + "' found. Expected 'AMP'.", 0x1001, tok));
				return;
			}

			 // Concat Rule
			ParseAdditiveExpression(node); // NonTerminal Rule: AdditiveExpression
		tok = scanner.LookAhead(TokenType.AMP); // ZeroOrMore Rule
		}

		parent.Token.UpdateRange(node.Token);
	} // NonTerminalSymbol: ConcatEpression

	public ParseTree ParseRelationalExpression(String input, ParseTree tree) // NonTerminalSymbol: RelationalExpression
	{
		scanner.Init(input);
		this.tree = tree;
		ParseRelationalExpression(tree);
		tree.Skipped = scanner.Skipped;
		return tree;
	}

	private void ParseRelationalExpression(ParseNode parent) // NonTerminalSymbol: RelationalExpression
	{
		Token tok;
		ParseNode n;
		ParseNode node = parent.CreateNode(scanner.GetToken(TokenType.RelationalExpression), "RelationalExpression");
		parent.getNodes().add(node);


		 // Concat Rule
		ParseConcatEpression(node); // NonTerminal Rule: ConcatEpression

		 // Concat Rule
		tok = scanner.LookAhead(TokenType.LESSTHAN, TokenType.LESSEQUAL, TokenType.GREATERTHAN, TokenType.GREATEREQUAL); // Option Rule
		if (tok.Type == TokenType.LESSTHAN
		    || tok.Type == TokenType.LESSEQUAL
		    || tok.Type == TokenType.GREATERTHAN
		    || tok.Type == TokenType.GREATEREQUAL)
		{

			 // Concat Rule
			tok = scanner.LookAhead(TokenType.LESSTHAN, TokenType.LESSEQUAL, TokenType.GREATERTHAN, TokenType.GREATEREQUAL);
			switch (tok.Type)
			{ // Choice Rule
				case LESSTHAN:
					tok = scanner.Scan(TokenType.LESSTHAN); // Terminal Rule: LESSTHAN
					n = node.CreateNode(tok, tok.toString() );
					node.Token.UpdateRange(tok);
					node.getNodes().add(n);
					if (tok.Type != TokenType.LESSTHAN) {
						tree.Errors.add(new ParseError("Unexpected token '" + tok.getText().replace("\n", "") + "' found. Expected 'LESSTHAN'.", 0x1001, tok));
						return;
					}
					break;
				case LESSEQUAL:
					tok = scanner.Scan(TokenType.LESSEQUAL); // Terminal Rule: LESSEQUAL
					n = node.CreateNode(tok, tok.toString() );
					node.Token.UpdateRange(tok);
					node.getNodes().add(n);
					if (tok.Type != TokenType.LESSEQUAL) {
						tree.Errors.add(new ParseError("Unexpected token '" + tok.getText().replace("\n", "") + "' found. Expected 'LESSEQUAL'.", 0x1001, tok));
						return;
					}
					break;
				case GREATERTHAN:
					tok = scanner.Scan(TokenType.GREATERTHAN); // Terminal Rule: GREATERTHAN
					n = node.CreateNode(tok, tok.toString() );
					node.Token.UpdateRange(tok);
					node.getNodes().add(n);
					if (tok.Type != TokenType.GREATERTHAN) {
						tree.Errors.add(new ParseError("Unexpected token '" + tok.getText().replace("\n", "") + "' found. Expected 'GREATERTHAN'.", 0x1001, tok));
						return;
					}
					break;
				case GREATEREQUAL:
					tok = scanner.Scan(TokenType.GREATEREQUAL); // Terminal Rule: GREATEREQUAL
					n = node.CreateNode(tok, tok.toString() );
					node.Token.UpdateRange(tok);
					node.getNodes().add(n);
					if (tok.Type != TokenType.GREATEREQUAL) {
						tree.Errors.add(new ParseError("Unexpected token '" + tok.getText().replace("\n", "") + "' found. Expected 'GREATEREQUAL'.", 0x1001, tok));
						return;
					}
					break;
				default:
					tree.Errors.add(new ParseError("Unexpected token '" + tok.getText().replace("\n", "") + "' found. Expected LESSTHAN, LESSEQUAL, GREATERTHAN, or GREATEREQUAL.", 0x0002, tok));
					break;
			} // Choice Rule

			 // Concat Rule
			ParseConcatEpression(node); // NonTerminal Rule: ConcatEpression
		}

		parent.Token.UpdateRange(node.Token);
	} // NonTerminalSymbol: RelationalExpression

	public ParseTree ParseEqualityExpression(String input, ParseTree tree) // NonTerminalSymbol: EqualityExpression
	{
		scanner.Init(input);
		this.tree = tree;
		ParseEqualityExpression(tree);
		tree.Skipped = scanner.Skipped;
		return tree;
	}

	private void ParseEqualityExpression(ParseNode parent) // NonTerminalSymbol: EqualityExpression
	{
		Token tok;
		ParseNode n;
		ParseNode node = parent.CreateNode(scanner.GetToken(TokenType.EqualityExpression), "EqualityExpression");
		parent.getNodes().add(node);


		 // Concat Rule
		ParseRelationalExpression(node); // NonTerminal Rule: RelationalExpression

		 // Concat Rule
		tok = scanner.LookAhead(TokenType.EQUAL, TokenType.NOTEQUAL); // ZeroOrMore Rule
		while (tok.Type == TokenType.EQUAL
		    || tok.Type == TokenType.NOTEQUAL)
		{

			 // Concat Rule
			tok = scanner.LookAhead(TokenType.EQUAL, TokenType.NOTEQUAL);
			switch (tok.Type)
			{ // Choice Rule
				case EQUAL:
					tok = scanner.Scan(TokenType.EQUAL); // Terminal Rule: EQUAL
					n = node.CreateNode(tok, tok.toString() );
					node.Token.UpdateRange(tok);
					node.getNodes().add(n);
					if (tok.Type != TokenType.EQUAL) {
						tree.Errors.add(new ParseError("Unexpected token '" + tok.getText().replace("\n", "") + "' found. Expected 'EQUAL'.", 0x1001, tok));
						return;
					}
					break;
				case NOTEQUAL:
					tok = scanner.Scan(TokenType.NOTEQUAL); // Terminal Rule: NOTEQUAL
					n = node.CreateNode(tok, tok.toString() );
					node.Token.UpdateRange(tok);
					node.getNodes().add(n);
					if (tok.Type != TokenType.NOTEQUAL) {
						tree.Errors.add(new ParseError("Unexpected token '" + tok.getText().replace("\n", "") + "' found. Expected 'NOTEQUAL'.", 0x1001, tok));
						return;
					}
					break;
				default:
					tree.Errors.add(new ParseError("Unexpected token '" + tok.getText().replace("\n", "") + "' found. Expected EQUAL or NOTEQUAL.", 0x0002, tok));
					break;
			} // Choice Rule

			 // Concat Rule
			ParseRelationalExpression(node); // NonTerminal Rule: RelationalExpression
		tok = scanner.LookAhead(TokenType.EQUAL, TokenType.NOTEQUAL); // ZeroOrMore Rule
		}

		parent.Token.UpdateRange(node.Token);
	} // NonTerminalSymbol: EqualityExpression

	public ParseTree ParseConditionalAndExpression(String input, ParseTree tree) // NonTerminalSymbol: ConditionalAndExpression
	{
		scanner.Init(input);
		this.tree = tree;
		ParseConditionalAndExpression(tree);
		tree.Skipped = scanner.Skipped;
		return tree;
	}

	private void ParseConditionalAndExpression(ParseNode parent) // NonTerminalSymbol: ConditionalAndExpression
	{
		Token tok;
		ParseNode n;
		ParseNode node = parent.CreateNode(scanner.GetToken(TokenType.ConditionalAndExpression), "ConditionalAndExpression");
		parent.getNodes().add(node);


		 // Concat Rule
		ParseEqualityExpression(node); // NonTerminal Rule: EqualityExpression

		 // Concat Rule
		tok = scanner.LookAhead(TokenType.AMPAMP); // ZeroOrMore Rule
		while (tok.Type == TokenType.AMPAMP)
		{

			 // Concat Rule
			tok = scanner.Scan(TokenType.AMPAMP); // Terminal Rule: AMPAMP
			n = node.CreateNode(tok, tok.toString() );
			node.Token.UpdateRange(tok);
			node.getNodes().add(n);
			if (tok.Type != TokenType.AMPAMP) {
				tree.Errors.add(new ParseError("Unexpected token '" + tok.getText().replace("\n", "") + "' found. Expected 'AMPAMP'.", 0x1001, tok));
				return;
			}

			 // Concat Rule
			ParseEqualityExpression(node); // NonTerminal Rule: EqualityExpression
		tok = scanner.LookAhead(TokenType.AMPAMP); // ZeroOrMore Rule
		}

		parent.Token.UpdateRange(node.Token);
	} // NonTerminalSymbol: ConditionalAndExpression

	public ParseTree ParseConditionalOrExpression(String input, ParseTree tree) // NonTerminalSymbol: ConditionalOrExpression
	{
		scanner.Init(input);
		this.tree = tree;
		ParseConditionalOrExpression(tree);
		tree.Skipped = scanner.Skipped;
		return tree;
	}

	private void ParseConditionalOrExpression(ParseNode parent) // NonTerminalSymbol: ConditionalOrExpression
	{
		Token tok;
		ParseNode n;
		ParseNode node = parent.CreateNode(scanner.GetToken(TokenType.ConditionalOrExpression), "ConditionalOrExpression");
		parent.getNodes().add(node);


		 // Concat Rule
		ParseConditionalAndExpression(node); // NonTerminal Rule: ConditionalAndExpression

		 // Concat Rule
		tok = scanner.LookAhead(TokenType.PIPEPIPE); // ZeroOrMore Rule
		while (tok.Type == TokenType.PIPEPIPE)
		{

			 // Concat Rule
			tok = scanner.Scan(TokenType.PIPEPIPE); // Terminal Rule: PIPEPIPE
			n = node.CreateNode(tok, tok.toString() );
			node.Token.UpdateRange(tok);
			node.getNodes().add(n);
			if (tok.Type != TokenType.PIPEPIPE) {
				tree.Errors.add(new ParseError("Unexpected token '" + tok.getText().replace("\n", "") + "' found. Expected 'PIPEPIPE'.", 0x1001, tok));
				return;
			}

			 // Concat Rule
			ParseConditionalAndExpression(node); // NonTerminal Rule: ConditionalAndExpression
		tok = scanner.LookAhead(TokenType.PIPEPIPE); // ZeroOrMore Rule
		}

		parent.Token.UpdateRange(node.Token);
	} // NonTerminalSymbol: ConditionalOrExpression

	public ParseTree ParseAssignmentExpression(String input, ParseTree tree) // NonTerminalSymbol: AssignmentExpression
	{
		scanner.Init(input);
		this.tree = tree;
		ParseAssignmentExpression(tree);
		tree.Skipped = scanner.Skipped;
		return tree;
	}

	private void ParseAssignmentExpression(ParseNode parent) // NonTerminalSymbol: AssignmentExpression
	{
		Token tok;
		ParseNode n;
		ParseNode node = parent.CreateNode(scanner.GetToken(TokenType.AssignmentExpression), "AssignmentExpression");
		parent.getNodes().add(node);


		 // Concat Rule
		ParseConditionalOrExpression(node); // NonTerminal Rule: ConditionalOrExpression

		 // Concat Rule
		tok = scanner.LookAhead(TokenType.QUESTIONMARK); // Option Rule
		if (tok.Type == TokenType.QUESTIONMARK)
		{

			 // Concat Rule
			tok = scanner.Scan(TokenType.QUESTIONMARK); // Terminal Rule: QUESTIONMARK
			n = node.CreateNode(tok, tok.toString() );
			node.Token.UpdateRange(tok);
			node.getNodes().add(n);
			if (tok.Type != TokenType.QUESTIONMARK) {
				tree.Errors.add(new ParseError("Unexpected token '" + tok.getText().replace("\n", "") + "' found. Expected 'QUESTIONMARK'.", 0x1001, tok));
				return;
			}

			 // Concat Rule
			ParseAssignmentExpression(node); // NonTerminal Rule: AssignmentExpression

			 // Concat Rule
			tok = scanner.Scan(TokenType.COLON); // Terminal Rule: COLON
			n = node.CreateNode(tok, tok.toString() );
			node.Token.UpdateRange(tok);
			node.getNodes().add(n);
			if (tok.Type != TokenType.COLON) {
				tree.Errors.add(new ParseError("Unexpected token '" + tok.getText().replace("\n", "") + "' found. Expected 'COLON'.", 0x1001, tok));
				return;
			}

			 // Concat Rule
			ParseAssignmentExpression(node); // NonTerminal Rule: AssignmentExpression
		}

		parent.Token.UpdateRange(node.Token);
	} // NonTerminalSymbol: AssignmentExpression

	public ParseTree ParseExpression(String input, ParseTree tree) // NonTerminalSymbol: Expression
	{
		scanner.Init(input);
		this.tree = tree;
		ParseExpression(tree);
		tree.Skipped = scanner.Skipped;
		return tree;
	}

	private void ParseExpression(ParseNode parent) // NonTerminalSymbol: Expression
	{
		Token tok;
		ParseNode n;
		ParseNode node = parent.CreateNode(scanner.GetToken(TokenType.Expression), "Expression");
		parent.getNodes().add(node);


		 // Concat Rule
		tok = scanner.LookAhead(TokenType.FUNCTION, TokenType.VARIABLE, TokenType.BOOLEANLITERAL, TokenType.DECIMALINTEGERLITERAL, TokenType.HEXINTEGERLITERAL, TokenType.REALLITERAL, TokenType.STRINGLITERAL, TokenType.BRACKETOPEN, TokenType.PLUS, TokenType.MINUS, TokenType.NOT, TokenType.ASSIGN); // Option Rule
		if (tok.Type == TokenType.FUNCTION
		    || tok.Type == TokenType.VARIABLE
		    || tok.Type == TokenType.BOOLEANLITERAL
		    || tok.Type == TokenType.DECIMALINTEGERLITERAL
		    || tok.Type == TokenType.HEXINTEGERLITERAL
		    || tok.Type == TokenType.REALLITERAL
		    || tok.Type == TokenType.STRINGLITERAL
		    || tok.Type == TokenType.BRACKETOPEN
		    || tok.Type == TokenType.PLUS
		    || tok.Type == TokenType.MINUS
		    || tok.Type == TokenType.NOT)
		{
			ParseAssignmentExpression(node); // NonTerminal Rule: AssignmentExpression
		}

		 // Concat Rule
		tok = scanner.LookAhead(TokenType.ASSIGN); // Option Rule
		if (tok.Type == TokenType.ASSIGN)
		{

			 // Concat Rule
			tok = scanner.Scan(TokenType.ASSIGN); // Terminal Rule: ASSIGN
			n = node.CreateNode(tok, tok.toString() );
			node.Token.UpdateRange(tok);
			node.getNodes().add(n);
			if (tok.Type != TokenType.ASSIGN) {
				tree.Errors.add(new ParseError("Unexpected token '" + tok.getText().replace("\n", "") + "' found. Expected 'ASSIGN'.", 0x1001, tok));
				return;
			}

			 // Concat Rule
			ParseAssignmentExpression(node); // NonTerminal Rule: AssignmentExpression
		}

		parent.Token.UpdateRange(node.Token);
	} // NonTerminalSymbol: Expression

	public ParseTree ParseParams(String input, ParseTree tree) // NonTerminalSymbol: Params
	{
		scanner.Init(input);
		this.tree = tree;
		ParseParams(tree);
		tree.Skipped = scanner.Skipped;
		return tree;
	}

	private void ParseParams(ParseNode parent) // NonTerminalSymbol: Params
	{
		Token tok;
		ParseNode n;
		ParseNode node = parent.CreateNode(scanner.GetToken(TokenType.Params), "Params");
		parent.getNodes().add(node);


		 // Concat Rule
		ParseExpression(node); // NonTerminal Rule: Expression

		 // Concat Rule
		tok = scanner.LookAhead(TokenType.SEMICOLON); // ZeroOrMore Rule
		while (tok.Type == TokenType.SEMICOLON)
		{

			 // Concat Rule
			tok = scanner.Scan(TokenType.SEMICOLON); // Terminal Rule: SEMICOLON
			n = node.CreateNode(tok, tok.toString() );
			node.Token.UpdateRange(tok);
			node.getNodes().add(n);
			if (tok.Type != TokenType.SEMICOLON) {
				tree.Errors.add(new ParseError("Unexpected token '" + tok.getText().replace("\n", "") + "' found. Expected 'SEMICOLON'.", 0x1001, tok));
				return;
			}

			 // Concat Rule
			ParseExpression(node); // NonTerminal Rule: Expression
		tok = scanner.LookAhead(TokenType.SEMICOLON); // ZeroOrMore Rule
		}

		parent.Token.UpdateRange(node.Token);
	} // NonTerminalSymbol: Params

	public ParseTree ParseLiteral(String input, ParseTree tree) // NonTerminalSymbol: Literal
	{
		scanner.Init(input);
		this.tree = tree;
		ParseLiteral(tree);
		tree.Skipped = scanner.Skipped;
		return tree;
	}

	private void ParseLiteral(ParseNode parent) // NonTerminalSymbol: Literal
	{
		Token tok;
		ParseNode n;
		ParseNode node = parent.CreateNode(scanner.GetToken(TokenType.Literal), "Literal");
		parent.getNodes().add(node);

		tok = scanner.LookAhead(TokenType.BOOLEANLITERAL, TokenType.DECIMALINTEGERLITERAL, TokenType.HEXINTEGERLITERAL, TokenType.REALLITERAL, TokenType.STRINGLITERAL);
		switch (tok.Type)
		{ // Choice Rule
			case BOOLEANLITERAL:
				tok = scanner.Scan(TokenType.BOOLEANLITERAL); // Terminal Rule: BOOLEANLITERAL
				n = node.CreateNode(tok, tok.toString() );
				node.Token.UpdateRange(tok);
				node.getNodes().add(n);
				if (tok.Type != TokenType.BOOLEANLITERAL) {
					tree.Errors.add(new ParseError("Unexpected token '" + tok.getText().replace("\n", "") + "' found. Expected 'BOOLEANLITERAL'.", 0x1001, tok));
					return;
				}
				break;
			case DECIMALINTEGERLITERAL:
			case HEXINTEGERLITERAL:
				ParseIntegerLiteral(node); // NonTerminal Rule: IntegerLiteral
				break;
			case REALLITERAL:
				ParseRealLiteral(node); // NonTerminal Rule: RealLiteral
				break;
			case STRINGLITERAL:
				ParseStringLiteral(node); // NonTerminal Rule: StringLiteral
				break;
			default:
				tree.Errors.add(new ParseError("Unexpected token '" + tok.getText().replace("\n", "") + "' found. Expected BOOLEANLITERAL, DECIMALINTEGERLITERAL, HEXINTEGERLITERAL, REALLITERAL, or STRINGLITERAL.", 0x0002, tok));
				break;
		} // Choice Rule

		parent.Token.UpdateRange(node.Token);
	} // NonTerminalSymbol: Literal

	public ParseTree ParseIntegerLiteral(String input, ParseTree tree) // NonTerminalSymbol: IntegerLiteral
	{
		scanner.Init(input);
		this.tree = tree;
		ParseIntegerLiteral(tree);
		tree.Skipped = scanner.Skipped;
		return tree;
	}

	private void ParseIntegerLiteral(ParseNode parent) // NonTerminalSymbol: IntegerLiteral
	{
		Token tok;
		ParseNode n;
		ParseNode node = parent.CreateNode(scanner.GetToken(TokenType.IntegerLiteral), "IntegerLiteral");
		parent.getNodes().add(node);

		tok = scanner.LookAhead(TokenType.DECIMALINTEGERLITERAL, TokenType.HEXINTEGERLITERAL);
		switch (tok.Type)
		{ // Choice Rule
			case DECIMALINTEGERLITERAL:
				tok = scanner.Scan(TokenType.DECIMALINTEGERLITERAL); // Terminal Rule: DECIMALINTEGERLITERAL
				n = node.CreateNode(tok, tok.toString() );
				node.Token.UpdateRange(tok);
				node.getNodes().add(n);
				if (tok.Type != TokenType.DECIMALINTEGERLITERAL) {
					tree.Errors.add(new ParseError("Unexpected token '" + tok.getText().replace("\n", "") + "' found. Expected 'DECIMALINTEGERLITERAL'.", 0x1001, tok));
					return;
				}
				break;
			case HEXINTEGERLITERAL:
				tok = scanner.Scan(TokenType.HEXINTEGERLITERAL); // Terminal Rule: HEXINTEGERLITERAL
				n = node.CreateNode(tok, tok.toString() );
				node.Token.UpdateRange(tok);
				node.getNodes().add(n);
				if (tok.Type != TokenType.HEXINTEGERLITERAL) {
					tree.Errors.add(new ParseError("Unexpected token '" + tok.getText().replace("\n", "") + "' found. Expected 'HEXINTEGERLITERAL'.", 0x1001, tok));
					return;
				}
				break;
			default:
				tree.Errors.add(new ParseError("Unexpected token '" + tok.getText().replace("\n", "") + "' found. Expected DECIMALINTEGERLITERAL or HEXINTEGERLITERAL.", 0x0002, tok));
				break;
		} // Choice Rule

		parent.Token.UpdateRange(node.Token);
	} // NonTerminalSymbol: IntegerLiteral

	public ParseTree ParseRealLiteral(String input, ParseTree tree) // NonTerminalSymbol: RealLiteral
	{
		scanner.Init(input);
		this.tree = tree;
		ParseRealLiteral(tree);
		tree.Skipped = scanner.Skipped;
		return tree;
	}

	private void ParseRealLiteral(ParseNode parent) // NonTerminalSymbol: RealLiteral
	{
		Token tok;
		ParseNode n;
		ParseNode node = parent.CreateNode(scanner.GetToken(TokenType.RealLiteral), "RealLiteral");
		parent.getNodes().add(node);

		tok = scanner.Scan(TokenType.REALLITERAL); // Terminal Rule: REALLITERAL
		n = node.CreateNode(tok, tok.toString() );
		node.Token.UpdateRange(tok);
		node.getNodes().add(n);
		if (tok.Type != TokenType.REALLITERAL) {
			tree.Errors.add(new ParseError("Unexpected token '" + tok.getText().replace("\n", "") + "' found. Expected 'REALLITERAL'.", 0x1001, tok));
			return;
		}

		parent.Token.UpdateRange(node.Token);
	} // NonTerminalSymbol: RealLiteral

	public ParseTree ParseStringLiteral(String input, ParseTree tree) // NonTerminalSymbol: StringLiteral
	{
		scanner.Init(input);
		this.tree = tree;
		ParseStringLiteral(tree);
		tree.Skipped = scanner.Skipped;
		return tree;
	}

	private void ParseStringLiteral(ParseNode parent) // NonTerminalSymbol: StringLiteral
	{
		Token tok;
		ParseNode n;
		ParseNode node = parent.CreateNode(scanner.GetToken(TokenType.StringLiteral), "StringLiteral");
		parent.getNodes().add(node);

		tok = scanner.Scan(TokenType.STRINGLITERAL); // Terminal Rule: STRINGLITERAL
		n = node.CreateNode(tok, tok.toString() );
		node.Token.UpdateRange(tok);
		node.getNodes().add(n);
		if (tok.Type != TokenType.STRINGLITERAL) {
			tree.Errors.add(new ParseError("Unexpected token '" + tok.getText().replace("\n", "") + "' found. Expected 'STRINGLITERAL'.", 0x1001, tok));
			return;
		}

		parent.Token.UpdateRange(node.Token);
	} // NonTerminalSymbol: StringLiteral

	public ParseTree ParseVariable(String input, ParseTree tree) // NonTerminalSymbol: Variable
	{
		scanner.Init(input);
		this.tree = tree;
		ParseVariable(tree);
		tree.Skipped = scanner.Skipped;
		return tree;
	}

	private void ParseVariable(ParseNode parent) // NonTerminalSymbol: Variable
	{
		Token tok;
		ParseNode n;
		ParseNode node = parent.CreateNode(scanner.GetToken(TokenType.Variable), "Variable");
		parent.getNodes().add(node);

		tok = scanner.Scan(TokenType.VARIABLE); // Terminal Rule: VARIABLE
		n = node.CreateNode(tok, tok.toString() );
		node.Token.UpdateRange(tok);
		node.getNodes().add(n);
		if (tok.Type != TokenType.VARIABLE) {
			tree.Errors.add(new ParseError("Unexpected token '" + tok.getText().replace("\n", "") + "' found. Expected 'VARIABLE'.", 0x1001, tok));
			return;
		}

		parent.Token.UpdateRange(node.Token);
	} // NonTerminalSymbol: Variable



}
