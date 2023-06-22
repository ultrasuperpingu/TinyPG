// Automatically generated from source file: BNFGrammar.tpg
// By TinyPG v1.5 available at https://github.com/ultrasuperpingu/TinyPG

using System;
using System.Collections.Generic;

// Disable unused variable warnings which
// can happen during the parser generation.
//#pragma warning disable 168

namespace TinyPG.Parsing
{
	#region Parser

	#pragma warning disable 168 // unused variables
	public partial class Parser 
	{
		private Scanner scanner;
		private ParseTree tree;

		public Parser(Scanner scanner)
		{
			this.scanner = scanner;
		}

		public ParseTree Parse(string input)
		{
			return Parse(input, new ParseTree());
		}

		public ParseTree Parse(string input, ParseTree tree)
		{
			scanner.Init(input);

			this.tree = tree;
			ParseStart(tree);
			tree.Skipped = scanner.Skipped;

			return tree;
		}

		public ParseTree ParseStart(string input, ParseTree tree) // NonTerminalSymbol: Start
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
			parent.Nodes.Add(node);


			 // Concat Rule
			tok = scanner.LookAhead(TokenType.SQUAREOPEN, TokenType.IDENTIFIER, TokenType.EOF, TokenType.DIRECTIVEOPEN); // ZeroOrMore Rule
			while (tok.Type == TokenType.DIRECTIVEOPEN)
			{
				ParseDirective(node); // NonTerminal Rule: Directive
				tok = scanner.LookAhead(TokenType.SQUAREOPEN, TokenType.IDENTIFIER, TokenType.EOF, TokenType.DIRECTIVEOPEN); // ZeroOrMore Rule
			}

			 // Concat Rule
			tok = scanner.LookAhead(TokenType.EOF, TokenType.SQUAREOPEN, TokenType.IDENTIFIER); // ZeroOrMore Rule
			while (tok.Type == TokenType.SQUAREOPEN
			    || tok.Type == TokenType.IDENTIFIER)
			{
				ParseExtProduction(node); // NonTerminal Rule: ExtProduction
				tok = scanner.LookAhead(TokenType.EOF, TokenType.SQUAREOPEN, TokenType.IDENTIFIER); // ZeroOrMore Rule
			}

			 // Concat Rule
			tok = scanner.Scan(TokenType.EOF); // Terminal Rule: EOF
			n = node.CreateNode(tok, tok.ToString() );
			node.Token.UpdateRange(tok);
			node.Nodes.Add(n);
			if (tok.Type != TokenType.EOF) {
				tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.EOF.ToString(), 0x1001, tok));
				return;
			}

			parent.Token.UpdateRange(node.Token);
		} // NonTerminalSymbol: Start

		public ParseTree ParseDirective(string input, ParseTree tree) // NonTerminalSymbol: Directive
		{
			scanner.Init(input);
			this.tree = tree;
			ParseDirective(tree);
			tree.Skipped = scanner.Skipped;
			return tree;
		}

		private void ParseDirective(ParseNode parent) // NonTerminalSymbol: Directive
		{
			Token tok;
			ParseNode n;
			ParseNode node = parent.CreateNode(scanner.GetToken(TokenType.Directive), "Directive");
			parent.Nodes.Add(node);


			 // Concat Rule
			tok = scanner.Scan(TokenType.DIRECTIVEOPEN); // Terminal Rule: DIRECTIVEOPEN
			n = node.CreateNode(tok, tok.ToString() );
			node.Token.UpdateRange(tok);
			node.Nodes.Add(n);
			if (tok.Type != TokenType.DIRECTIVEOPEN) {
				tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.DIRECTIVEOPEN.ToString(), 0x1001, tok));
				return;
			}

			 // Concat Rule
			tok = scanner.Scan(TokenType.IDENTIFIER); // Terminal Rule: IDENTIFIER
			n = node.CreateNode(tok, tok.ToString() );
			node.Token.UpdateRange(tok);
			node.Nodes.Add(n);
			if (tok.Type != TokenType.IDENTIFIER) {
				tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.IDENTIFIER.ToString(), 0x1001, tok));
				return;
			}

			 // Concat Rule
			tok = scanner.LookAhead(TokenType.DIRECTIVECLOSE, TokenType.IDENTIFIER); // ZeroOrMore Rule
			while (tok.Type == TokenType.IDENTIFIER)
			{
				ParseNameValue(node); // NonTerminal Rule: NameValue
				tok = scanner.LookAhead(TokenType.DIRECTIVECLOSE, TokenType.IDENTIFIER); // ZeroOrMore Rule
			}

			 // Concat Rule
			tok = scanner.Scan(TokenType.DIRECTIVECLOSE); // Terminal Rule: DIRECTIVECLOSE
			n = node.CreateNode(tok, tok.ToString() );
			node.Token.UpdateRange(tok);
			node.Nodes.Add(n);
			if (tok.Type != TokenType.DIRECTIVECLOSE) {
				tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.DIRECTIVECLOSE.ToString(), 0x1001, tok));
				return;
			}

			parent.Token.UpdateRange(node.Token);
		} // NonTerminalSymbol: Directive

		public ParseTree ParseNameValue(string input, ParseTree tree) // NonTerminalSymbol: NameValue
		{
			scanner.Init(input);
			this.tree = tree;
			ParseNameValue(tree);
			tree.Skipped = scanner.Skipped;
			return tree;
		}

		private void ParseNameValue(ParseNode parent) // NonTerminalSymbol: NameValue
		{
			Token tok;
			ParseNode n;
			ParseNode node = parent.CreateNode(scanner.GetToken(TokenType.NameValue), "NameValue");
			parent.Nodes.Add(node);


			 // Concat Rule
			tok = scanner.Scan(TokenType.IDENTIFIER); // Terminal Rule: IDENTIFIER
			n = node.CreateNode(tok, tok.ToString() );
			node.Token.UpdateRange(tok);
			node.Nodes.Add(n);
			if (tok.Type != TokenType.IDENTIFIER) {
				tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.IDENTIFIER.ToString(), 0x1001, tok));
				return;
			}

			 // Concat Rule
			tok = scanner.Scan(TokenType.ASSIGN); // Terminal Rule: ASSIGN
			n = node.CreateNode(tok, tok.ToString() );
			node.Token.UpdateRange(tok);
			node.Nodes.Add(n);
			if (tok.Type != TokenType.ASSIGN) {
				tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.ASSIGN.ToString(), 0x1001, tok));
				return;
			}

			 // Concat Rule
			tok = scanner.LookAhead(TokenType.VERBATIM_STRING, TokenType.STRING, TokenType.CODEBLOCK);
			switch (tok.Type)
			{ // Choice Rule
				case TokenType.VERBATIM_STRING:
					tok = scanner.Scan(TokenType.VERBATIM_STRING); // Terminal Rule: VERBATIM_STRING
					n = node.CreateNode(tok, tok.ToString() );
					node.Token.UpdateRange(tok);
					node.Nodes.Add(n);
					if (tok.Type != TokenType.VERBATIM_STRING) {
						tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.VERBATIM_STRING.ToString(), 0x1001, tok));
						return;
					}
					break;
				case TokenType.STRING:
					tok = scanner.Scan(TokenType.STRING); // Terminal Rule: STRING
					n = node.CreateNode(tok, tok.ToString() );
					node.Token.UpdateRange(tok);
					node.Nodes.Add(n);
					if (tok.Type != TokenType.STRING) {
						tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.STRING.ToString(), 0x1001, tok));
						return;
					}
					break;
				case TokenType.CODEBLOCK:
					tok = scanner.Scan(TokenType.CODEBLOCK); // Terminal Rule: CODEBLOCK
					n = node.CreateNode(tok, tok.ToString() );
					node.Token.UpdateRange(tok);
					node.Nodes.Add(n);
					if (tok.Type != TokenType.CODEBLOCK) {
						tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.CODEBLOCK.ToString(), 0x1001, tok));
						return;
					}
					break;
				default:
					tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected VERBATIM_STRING, STRING, or CODEBLOCK.", 0x0002, tok));
					break;
			} // Choice Rule

			parent.Token.UpdateRange(node.Token);
		} // NonTerminalSymbol: NameValue

		public ParseTree ParseExtProduction(string input, ParseTree tree) // NonTerminalSymbol: ExtProduction
		{
			scanner.Init(input);
			this.tree = tree;
			ParseExtProduction(tree);
			tree.Skipped = scanner.Skipped;
			return tree;
		}

		private void ParseExtProduction(ParseNode parent) // NonTerminalSymbol: ExtProduction
		{
			Token tok;
			ParseNode n;
			ParseNode node = parent.CreateNode(scanner.GetToken(TokenType.ExtProduction), "ExtProduction");
			parent.Nodes.Add(node);


			 // Concat Rule
			tok = scanner.LookAhead(TokenType.IDENTIFIER, TokenType.SQUAREOPEN); // ZeroOrMore Rule
			while (tok.Type == TokenType.SQUAREOPEN)
			{
				ParseAttribute(node); // NonTerminal Rule: Attribute
				tok = scanner.LookAhead(TokenType.IDENTIFIER, TokenType.SQUAREOPEN); // ZeroOrMore Rule
			}

			 // Concat Rule
			ParseProduction(node); // NonTerminal Rule: Production

			parent.Token.UpdateRange(node.Token);
		} // NonTerminalSymbol: ExtProduction

		public ParseTree ParseAttribute(string input, ParseTree tree) // NonTerminalSymbol: Attribute
		{
			scanner.Init(input);
			this.tree = tree;
			ParseAttribute(tree);
			tree.Skipped = scanner.Skipped;
			return tree;
		}

		private void ParseAttribute(ParseNode parent) // NonTerminalSymbol: Attribute
		{
			Token tok;
			ParseNode n;
			ParseNode node = parent.CreateNode(scanner.GetToken(TokenType.Attribute), "Attribute");
			parent.Nodes.Add(node);


			 // Concat Rule
			tok = scanner.Scan(TokenType.SQUAREOPEN); // Terminal Rule: SQUAREOPEN
			n = node.CreateNode(tok, tok.ToString() );
			node.Token.UpdateRange(tok);
			node.Nodes.Add(n);
			if (tok.Type != TokenType.SQUAREOPEN) {
				tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.SQUAREOPEN.ToString(), 0x1001, tok));
				return;
			}

			 // Concat Rule
			tok = scanner.Scan(TokenType.IDENTIFIER); // Terminal Rule: IDENTIFIER
			n = node.CreateNode(tok, tok.ToString() );
			node.Token.UpdateRange(tok);
			node.Nodes.Add(n);
			if (tok.Type != TokenType.IDENTIFIER) {
				tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.IDENTIFIER.ToString(), 0x1001, tok));
				return;
			}

			 // Concat Rule
			tok = scanner.LookAhead(TokenType.SQUARECLOSE, TokenType.BRACKETOPEN); // Option Rule
			if (tok.Type == TokenType.BRACKETOPEN)
			{

				 // Concat Rule
				tok = scanner.Scan(TokenType.BRACKETOPEN); // Terminal Rule: BRACKETOPEN
				n = node.CreateNode(tok, tok.ToString() );
				node.Token.UpdateRange(tok);
				node.Nodes.Add(n);
				if (tok.Type != TokenType.BRACKETOPEN) {
					tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.BRACKETOPEN.ToString(), 0x1001, tok));
					return;
				}

				 // Concat Rule
				tok = scanner.LookAhead(TokenType.BRACKETCLOSE, TokenType.INTEGER, TokenType.DOUBLE, TokenType.STRING, TokenType.HEX); // Option Rule
				if (tok.Type == TokenType.INTEGER
				    || tok.Type == TokenType.DOUBLE
				    || tok.Type == TokenType.STRING
				    || tok.Type == TokenType.HEX)
				{
					ParseParams(node); // NonTerminal Rule: Params
				}

				 // Concat Rule
				tok = scanner.Scan(TokenType.BRACKETCLOSE); // Terminal Rule: BRACKETCLOSE
				n = node.CreateNode(tok, tok.ToString() );
				node.Token.UpdateRange(tok);
				node.Nodes.Add(n);
				if (tok.Type != TokenType.BRACKETCLOSE) {
					tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.BRACKETCLOSE.ToString(), 0x1001, tok));
					return;
				}
			}

			 // Concat Rule
			tok = scanner.Scan(TokenType.SQUARECLOSE); // Terminal Rule: SQUARECLOSE
			n = node.CreateNode(tok, tok.ToString() );
			node.Token.UpdateRange(tok);
			node.Nodes.Add(n);
			if (tok.Type != TokenType.SQUARECLOSE) {
				tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.SQUARECLOSE.ToString(), 0x1001, tok));
				return;
			}

			parent.Token.UpdateRange(node.Token);
		} // NonTerminalSymbol: Attribute

		public ParseTree ParseParams(string input, ParseTree tree) // NonTerminalSymbol: Params
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
			parent.Nodes.Add(node);


			 // Concat Rule
			ParseParam(node); // NonTerminal Rule: Param

			 // Concat Rule
			tok = scanner.LookAhead(TokenType.COMMA); // ZeroOrMore Rule
			while (tok.Type == TokenType.COMMA)
			{

				 // Concat Rule
				tok = scanner.Scan(TokenType.COMMA); // Terminal Rule: COMMA
				n = node.CreateNode(tok, tok.ToString() );
				node.Token.UpdateRange(tok);
				node.Nodes.Add(n);
				if (tok.Type != TokenType.COMMA) {
					tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.COMMA.ToString(), 0x1001, tok));
					return;
				}

				 // Concat Rule
				ParseParam(node); // NonTerminal Rule: Param
				tok = scanner.LookAhead(TokenType.COMMA); // ZeroOrMore Rule
			}

			parent.Token.UpdateRange(node.Token);
		} // NonTerminalSymbol: Params

		public ParseTree ParseParam(string input, ParseTree tree) // NonTerminalSymbol: Param
		{
			scanner.Init(input);
			this.tree = tree;
			ParseParam(tree);
			tree.Skipped = scanner.Skipped;
			return tree;
		}

		private void ParseParam(ParseNode parent) // NonTerminalSymbol: Param
		{
			Token tok;
			ParseNode n;
			ParseNode node = parent.CreateNode(scanner.GetToken(TokenType.Param), "Param");
			parent.Nodes.Add(node);

			tok = scanner.LookAhead(TokenType.INTEGER, TokenType.DOUBLE, TokenType.STRING, TokenType.HEX);
			switch (tok.Type)
			{ // Choice Rule
				case TokenType.INTEGER:
					tok = scanner.Scan(TokenType.INTEGER); // Terminal Rule: INTEGER
					n = node.CreateNode(tok, tok.ToString() );
					node.Token.UpdateRange(tok);
					node.Nodes.Add(n);
					if (tok.Type != TokenType.INTEGER) {
						tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.INTEGER.ToString(), 0x1001, tok));
						return;
					}
					break;
				case TokenType.DOUBLE:
					tok = scanner.Scan(TokenType.DOUBLE); // Terminal Rule: DOUBLE
					n = node.CreateNode(tok, tok.ToString() );
					node.Token.UpdateRange(tok);
					node.Nodes.Add(n);
					if (tok.Type != TokenType.DOUBLE) {
						tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.DOUBLE.ToString(), 0x1001, tok));
						return;
					}
					break;
				case TokenType.STRING:
					tok = scanner.Scan(TokenType.STRING); // Terminal Rule: STRING
					n = node.CreateNode(tok, tok.ToString() );
					node.Token.UpdateRange(tok);
					node.Nodes.Add(n);
					if (tok.Type != TokenType.STRING) {
						tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.STRING.ToString(), 0x1001, tok));
						return;
					}
					break;
				case TokenType.HEX:
					tok = scanner.Scan(TokenType.HEX); // Terminal Rule: HEX
					n = node.CreateNode(tok, tok.ToString() );
					node.Token.UpdateRange(tok);
					node.Nodes.Add(n);
					if (tok.Type != TokenType.HEX) {
						tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.HEX.ToString(), 0x1001, tok));
						return;
					}
					break;
				default:
					tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected INTEGER, DOUBLE, STRING, or HEX.", 0x0002, tok));
					break;
			} // Choice Rule

			parent.Token.UpdateRange(node.Token);
		} // NonTerminalSymbol: Param

		public ParseTree ParseProduction(string input, ParseTree tree) // NonTerminalSymbol: Production
		{
			scanner.Init(input);
			this.tree = tree;
			ParseProduction(tree);
			tree.Skipped = scanner.Skipped;
			return tree;
		}

		private void ParseProduction(ParseNode parent) // NonTerminalSymbol: Production
		{
			Token tok;
			ParseNode n;
			ParseNode node = parent.CreateNode(scanner.GetToken(TokenType.Production), "Production");
			parent.Nodes.Add(node);


			 // Concat Rule
			tok = scanner.Scan(TokenType.IDENTIFIER); // Terminal Rule: IDENTIFIER
			n = node.CreateNode(tok, tok.ToString() );
			node.Token.UpdateRange(tok);
			node.Nodes.Add(n);
			if (tok.Type != TokenType.IDENTIFIER) {
				tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.IDENTIFIER.ToString(), 0x1001, tok));
				return;
			}

			 // Concat Rule
			tok = scanner.Scan(TokenType.ARROW); // Terminal Rule: ARROW
			n = node.CreateNode(tok, tok.ToString() );
			node.Token.UpdateRange(tok);
			node.Nodes.Add(n);
			if (tok.Type != TokenType.ARROW) {
				tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.ARROW.ToString(), 0x1001, tok));
				return;
			}

			 // Concat Rule
			ParseRule(node); // NonTerminal Rule: Rule

			 // Concat Rule
			tok = scanner.LookAhead(TokenType.CODEBLOCK, TokenType.SEMICOLON, TokenType.COLON); // Option Rule
			if (tok.Type == TokenType.COLON)
			{

				 // Concat Rule
				tok = scanner.Scan(TokenType.COLON); // Terminal Rule: COLON
				n = node.CreateNode(tok, tok.ToString() );
				node.Token.UpdateRange(tok);
				node.Nodes.Add(n);
				if (tok.Type != TokenType.COLON) {
					tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.COLON.ToString(), 0x1001, tok));
					return;
				}

				 // Concat Rule
				tok = scanner.Scan(TokenType.TYPE); // Terminal Rule: TYPE
				n = node.CreateNode(tok, tok.ToString() );
				node.Token.UpdateRange(tok);
				node.Nodes.Add(n);
				if (tok.Type != TokenType.TYPE) {
					tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.TYPE.ToString(), 0x1001, tok));
					return;
				}

				 // Concat Rule
				tok = scanner.LookAhead(TokenType.DEFAULT); // Option Rule
				if (tok.Type == TokenType.DEFAULT)
				{

					 // Concat Rule
					tok = scanner.Scan(TokenType.DEFAULT); // Terminal Rule: DEFAULT
					n = node.CreateNode(tok, tok.ToString() );
					node.Token.UpdateRange(tok);
					node.Nodes.Add(n);
					if (tok.Type != TokenType.DEFAULT) {
						tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.DEFAULT.ToString(), 0x1001, tok));
						return;
					}

					 // Concat Rule
					tok = scanner.Scan(TokenType.BRACKETOPEN); // Terminal Rule: BRACKETOPEN
					n = node.CreateNode(tok, tok.ToString() );
					node.Token.UpdateRange(tok);
					node.Nodes.Add(n);
					if (tok.Type != TokenType.BRACKETOPEN) {
						tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.BRACKETOPEN.ToString(), 0x1001, tok));
						return;
					}

					 // Concat Rule
					tok = scanner.Scan(TokenType.DEFAULT_VALUE); // Terminal Rule: DEFAULT_VALUE
					n = node.CreateNode(tok, tok.ToString() );
					node.Token.UpdateRange(tok);
					node.Nodes.Add(n);
					if (tok.Type != TokenType.DEFAULT_VALUE) {
						tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.DEFAULT_VALUE.ToString(), 0x1001, tok));
						return;
					}

					 // Concat Rule
					tok = scanner.Scan(TokenType.BRACKETCLOSE); // Terminal Rule: BRACKETCLOSE
					n = node.CreateNode(tok, tok.ToString() );
					node.Token.UpdateRange(tok);
					node.Nodes.Add(n);
					if (tok.Type != TokenType.BRACKETCLOSE) {
						tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.BRACKETCLOSE.ToString(), 0x1001, tok));
						return;
					}
				}
			}

			 // Concat Rule
			tok = scanner.LookAhead(TokenType.CODEBLOCK, TokenType.SEMICOLON);
			switch (tok.Type)
			{ // Choice Rule
				case TokenType.CODEBLOCK:
					tok = scanner.Scan(TokenType.CODEBLOCK); // Terminal Rule: CODEBLOCK
					n = node.CreateNode(tok, tok.ToString() );
					node.Token.UpdateRange(tok);
					node.Nodes.Add(n);
					if (tok.Type != TokenType.CODEBLOCK) {
						tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.CODEBLOCK.ToString(), 0x1001, tok));
						return;
					}
					break;
				case TokenType.SEMICOLON:
					tok = scanner.Scan(TokenType.SEMICOLON); // Terminal Rule: SEMICOLON
					n = node.CreateNode(tok, tok.ToString() );
					node.Token.UpdateRange(tok);
					node.Nodes.Add(n);
					if (tok.Type != TokenType.SEMICOLON) {
						tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.SEMICOLON.ToString(), 0x1001, tok));
						return;
					}
					break;
				default:
					tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected CODEBLOCK or SEMICOLON.", 0x0002, tok));
					break;
			} // Choice Rule

			parent.Token.UpdateRange(node.Token);
		} // NonTerminalSymbol: Production

		public ParseTree ParseRule(string input, ParseTree tree) // NonTerminalSymbol: Rule
		{
			scanner.Init(input);
			this.tree = tree;
			ParseRule(tree);
			tree.Skipped = scanner.Skipped;
			return tree;
		}

		private void ParseRule(ParseNode parent) // NonTerminalSymbol: Rule
		{
			Token tok;
			ParseNode n;
			ParseNode node = parent.CreateNode(scanner.GetToken(TokenType.Rule), "Rule");
			parent.Nodes.Add(node);

			tok = scanner.LookAhead(TokenType.VERBATIM_STRING, TokenType.STRING, TokenType.IDENTIFIER, TokenType.BRACKETOPEN);
			switch (tok.Type)
			{ // Choice Rule
				case TokenType.VERBATIM_STRING:
					tok = scanner.Scan(TokenType.VERBATIM_STRING); // Terminal Rule: VERBATIM_STRING
					n = node.CreateNode(tok, tok.ToString() );
					node.Token.UpdateRange(tok);
					node.Nodes.Add(n);
					if (tok.Type != TokenType.VERBATIM_STRING) {
						tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.VERBATIM_STRING.ToString(), 0x1001, tok));
						return;
					}
					break;
				case TokenType.STRING:
					tok = scanner.Scan(TokenType.STRING); // Terminal Rule: STRING
					n = node.CreateNode(tok, tok.ToString() );
					node.Token.UpdateRange(tok);
					node.Nodes.Add(n);
					if (tok.Type != TokenType.STRING) {
						tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.STRING.ToString(), 0x1001, tok));
						return;
					}
					break;
				case TokenType.IDENTIFIER:
				case TokenType.BRACKETOPEN:
					ParseSubrule(node); // NonTerminal Rule: Subrule
					break;
				default:
					tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected VERBATIM_STRING, STRING, IDENTIFIER, or BRACKETOPEN.", 0x0002, tok));
					break;
			} // Choice Rule

			parent.Token.UpdateRange(node.Token);
		} // NonTerminalSymbol: Rule

		public ParseTree ParseSubrule(string input, ParseTree tree) // NonTerminalSymbol: Subrule
		{
			scanner.Init(input);
			this.tree = tree;
			ParseSubrule(tree);
			tree.Skipped = scanner.Skipped;
			return tree;
		}

		private void ParseSubrule(ParseNode parent) // NonTerminalSymbol: Subrule
		{
			Token tok;
			ParseNode n;
			ParseNode node = parent.CreateNode(scanner.GetToken(TokenType.Subrule), "Subrule");
			parent.Nodes.Add(node);


			 // Concat Rule
			ParseConcatRule(node); // NonTerminal Rule: ConcatRule

			 // Concat Rule
			tok = scanner.LookAhead(TokenType.PIPE); // ZeroOrMore Rule
			while (tok.Type == TokenType.PIPE)
			{

				 // Concat Rule
				tok = scanner.Scan(TokenType.PIPE); // Terminal Rule: PIPE
				n = node.CreateNode(tok, tok.ToString() );
				node.Token.UpdateRange(tok);
				node.Nodes.Add(n);
				if (tok.Type != TokenType.PIPE) {
					tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.PIPE.ToString(), 0x1001, tok));
					return;
				}

				 // Concat Rule
				ParseConcatRule(node); // NonTerminal Rule: ConcatRule
				tok = scanner.LookAhead(TokenType.PIPE); // ZeroOrMore Rule
			}

			parent.Token.UpdateRange(node.Token);
		} // NonTerminalSymbol: Subrule

		public ParseTree ParseConcatRule(string input, ParseTree tree) // NonTerminalSymbol: ConcatRule
		{
			scanner.Init(input);
			this.tree = tree;
			ParseConcatRule(tree);
			tree.Skipped = scanner.Skipped;
			return tree;
		}

		private void ParseConcatRule(ParseNode parent) // NonTerminalSymbol: ConcatRule
		{
			Token tok;
			ParseNode n;
			ParseNode node = parent.CreateNode(scanner.GetToken(TokenType.ConcatRule), "ConcatRule");
			parent.Nodes.Add(node);

			do { // OneOrMore Rule
				ParseSymbol(node); // NonTerminal Rule: Symbol
				tok = scanner.LookAhead(TokenType.IDENTIFIER, TokenType.BRACKETOPEN);
			} while (tok.Type == TokenType.IDENTIFIER
			    || tok.Type == TokenType.BRACKETOPEN); // OneOrMore Rule

			parent.Token.UpdateRange(node.Token);
		} // NonTerminalSymbol: ConcatRule

		public ParseTree ParseSymbol(string input, ParseTree tree) // NonTerminalSymbol: Symbol
		{
			scanner.Init(input);
			this.tree = tree;
			ParseSymbol(tree);
			tree.Skipped = scanner.Skipped;
			return tree;
		}

		private void ParseSymbol(ParseNode parent) // NonTerminalSymbol: Symbol
		{
			Token tok;
			ParseNode n;
			ParseNode node = parent.CreateNode(scanner.GetToken(TokenType.Symbol), "Symbol");
			parent.Nodes.Add(node);


			 // Concat Rule
			tok = scanner.LookAhead(TokenType.IDENTIFIER, TokenType.BRACKETOPEN);
			switch (tok.Type)
			{ // Choice Rule
				case TokenType.IDENTIFIER:
					tok = scanner.Scan(TokenType.IDENTIFIER); // Terminal Rule: IDENTIFIER
					n = node.CreateNode(tok, tok.ToString() );
					node.Token.UpdateRange(tok);
					node.Nodes.Add(n);
					if (tok.Type != TokenType.IDENTIFIER) {
						tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.IDENTIFIER.ToString(), 0x1001, tok));
						return;
					}
					break;
				case TokenType.BRACKETOPEN:

					 // Concat Rule
					tok = scanner.Scan(TokenType.BRACKETOPEN); // Terminal Rule: BRACKETOPEN
					n = node.CreateNode(tok, tok.ToString() );
					node.Token.UpdateRange(tok);
					node.Nodes.Add(n);
					if (tok.Type != TokenType.BRACKETOPEN) {
						tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.BRACKETOPEN.ToString(), 0x1001, tok));
						return;
					}

					 // Concat Rule
					ParseSubrule(node); // NonTerminal Rule: Subrule

					 // Concat Rule
					tok = scanner.Scan(TokenType.BRACKETCLOSE); // Terminal Rule: BRACKETCLOSE
					n = node.CreateNode(tok, tok.ToString() );
					node.Token.UpdateRange(tok);
					node.Nodes.Add(n);
					if (tok.Type != TokenType.BRACKETCLOSE) {
						tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.BRACKETCLOSE.ToString(), 0x1001, tok));
						return;
					}
					break;
				default:
					tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected IDENTIFIER or BRACKETOPEN.", 0x0002, tok));
					break;
			} // Choice Rule

			 // Concat Rule
			tok = scanner.LookAhead(TokenType.UNARYOPER); // Option Rule
			if (tok.Type == TokenType.UNARYOPER)
			{
				tok = scanner.Scan(TokenType.UNARYOPER); // Terminal Rule: UNARYOPER
				n = node.CreateNode(tok, tok.ToString() );
				node.Token.UpdateRange(tok);
				node.Nodes.Add(n);
				if (tok.Type != TokenType.UNARYOPER) {
					tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.UNARYOPER.ToString(), 0x1001, tok));
					return;
				}
			}

			parent.Token.UpdateRange(node.Token);
		} // NonTerminalSymbol: Symbol



	}

	#endregion Parser
}
