// Automatically generated from source file: simple expression2.tpg
// By TinyPG v1.5 available at https://github.com/ultrasuperpingu/TinyPG

using System;
using System.Collections.Generic;

// Disable unused variable warnings which
// can happen during the parser generation.
//#pragma warning disable 168

namespace SimpleExpr
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
			ParseAddExpr(node); // NonTerminal Rule: AddExpr

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

		public ParseTree ParseAddExpr(string input, ParseTree tree) // NonTerminalSymbol: AddExpr
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
			parent.Nodes.Add(node);


			 // Concat Rule
			ParseMultExpr(node); // NonTerminal Rule: MultExpr

			 // Concat Rule
			tok = scanner.LookAhead(TokenType.PLUSMINUS); // ZeroOrMore Rule
			while (tok.Type == TokenType.PLUSMINUS)
			{

				 // Concat Rule
				tok = scanner.Scan(TokenType.PLUSMINUS); // Terminal Rule: PLUSMINUS
				n = node.CreateNode(tok, tok.ToString() );
				node.Token.UpdateRange(tok);
				node.Nodes.Add(n);
				if (tok.Type != TokenType.PLUSMINUS) {
					tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.PLUSMINUS.ToString(), 0x1001, tok));
					return;
				}

				 // Concat Rule
				ParseMultExpr(node); // NonTerminal Rule: MultExpr
			tok = scanner.LookAhead(TokenType.PLUSMINUS); // ZeroOrMore Rule
			}

			parent.Token.UpdateRange(node.Token);
		} // NonTerminalSymbol: AddExpr

		public ParseTree ParseMultExpr(string input, ParseTree tree) // NonTerminalSymbol: MultExpr
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
			parent.Nodes.Add(node);


			 // Concat Rule
			ParseAtom(node); // NonTerminal Rule: Atom

			 // Concat Rule
			tok = scanner.LookAhead(TokenType.MULTDIV); // ZeroOrMore Rule
			while (tok.Type == TokenType.MULTDIV)
			{

				 // Concat Rule
				tok = scanner.Scan(TokenType.MULTDIV); // Terminal Rule: MULTDIV
				n = node.CreateNode(tok, tok.ToString() );
				node.Token.UpdateRange(tok);
				node.Nodes.Add(n);
				if (tok.Type != TokenType.MULTDIV) {
					tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.MULTDIV.ToString(), 0x1001, tok));
					return;
				}

				 // Concat Rule
				ParseAtom(node); // NonTerminal Rule: Atom
			tok = scanner.LookAhead(TokenType.MULTDIV); // ZeroOrMore Rule
			}

			parent.Token.UpdateRange(node.Token);
		} // NonTerminalSymbol: MultExpr

		public ParseTree ParseAtom(string input, ParseTree tree) // NonTerminalSymbol: Atom
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
			parent.Nodes.Add(node);

			tok = scanner.LookAhead(TokenType.NUMBER, TokenType.BROPEN, TokenType.ID); // Choice Rule
			switch (tok.Type)
			{ // Choice Rule
				case TokenType.NUMBER:
					tok = scanner.Scan(TokenType.NUMBER); // Terminal Rule: NUMBER
					n = node.CreateNode(tok, tok.ToString() );
					node.Token.UpdateRange(tok);
					node.Nodes.Add(n);
					if (tok.Type != TokenType.NUMBER) {
						tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.NUMBER.ToString(), 0x1001, tok));
						return;
					}
					break;
				case TokenType.BROPEN:

					 // Concat Rule
					tok = scanner.Scan(TokenType.BROPEN); // Terminal Rule: BROPEN
					n = node.CreateNode(tok, tok.ToString() );
					node.Token.UpdateRange(tok);
					node.Nodes.Add(n);
					if (tok.Type != TokenType.BROPEN) {
						tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.BROPEN.ToString(), 0x1001, tok));
						return;
					}

					 // Concat Rule
					ParseAddExpr(node); // NonTerminal Rule: AddExpr

					 // Concat Rule
					tok = scanner.Scan(TokenType.BRCLOSE); // Terminal Rule: BRCLOSE
					n = node.CreateNode(tok, tok.ToString() );
					node.Token.UpdateRange(tok);
					node.Nodes.Add(n);
					if (tok.Type != TokenType.BRCLOSE) {
						tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.BRCLOSE.ToString(), 0x1001, tok));
						return;
					}
					break;
				case TokenType.ID:
					tok = scanner.Scan(TokenType.ID); // Terminal Rule: ID
					n = node.CreateNode(tok, tok.ToString() );
					node.Token.UpdateRange(tok);
					node.Nodes.Add(n);
					if (tok.Type != TokenType.ID) {
						tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.ID.ToString(), 0x1001, tok));
						return;
					}
					break;
				default:
					tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected NUMBER, BROPEN, or ID.", 0x0002, tok));
					break;
			} // Choice Rule

			parent.Token.UpdateRange(node.Token);
		} // NonTerminalSymbol: Atom



	}

	#endregion Parser
}
