// Automatically generated from source file: GrammarHighlighter.tpg
// By TinyPG v1.5 available at https://github.com/ultrasuperpingu/TinyPG

using System;
using System.Collections.Generic;

// Disable unused variable warnings which
// can happen during the parser generation.
//#pragma warning disable 168

namespace TinyPG.Highlighter
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
			tok = scanner.LookAhead(TokenType.GRAMMARCOMMENTLINE, TokenType.GRAMMARCOMMENTBLOCK, TokenType.ATTRIBUTEOPEN, TokenType.GRAMMARSTRING, TokenType.GRAMMARARROW, TokenType.GRAMMARNONKEYWORD, TokenType.GRAMMARKEYWORD, TokenType.GRAMMARSYMBOL, TokenType.CODEBLOCKOPEN, TokenType.DIRECTIVEOPEN, TokenType.EOF); // ZeroOrMore Rule
			while (tok.Type == TokenType.GRAMMARCOMMENTLINE
			    || tok.Type == TokenType.GRAMMARCOMMENTBLOCK
			    || tok.Type == TokenType.ATTRIBUTEOPEN
			    || tok.Type == TokenType.GRAMMARSTRING
			    || tok.Type == TokenType.GRAMMARARROW
			    || tok.Type == TokenType.GRAMMARNONKEYWORD
			    || tok.Type == TokenType.GRAMMARKEYWORD
			    || tok.Type == TokenType.GRAMMARSYMBOL
			    || tok.Type == TokenType.CODEBLOCKOPEN
			    || tok.Type == TokenType.DIRECTIVEOPEN)
			{
				tok = scanner.LookAhead(TokenType.GRAMMARCOMMENTLINE, TokenType.GRAMMARCOMMENTBLOCK, TokenType.ATTRIBUTEOPEN, TokenType.GRAMMARSTRING, TokenType.GRAMMARARROW, TokenType.GRAMMARNONKEYWORD, TokenType.GRAMMARKEYWORD, TokenType.GRAMMARSYMBOL, TokenType.CODEBLOCKOPEN, TokenType.DIRECTIVEOPEN);
				switch (tok.Type)
				{ // Choice Rule
					case TokenType.GRAMMARCOMMENTLINE:
					case TokenType.GRAMMARCOMMENTBLOCK:
						ParseCommentBlock(node); // NonTerminal Rule: CommentBlock
						break;
					case TokenType.ATTRIBUTEOPEN:
						ParseAttributeBlock(node); // NonTerminal Rule: AttributeBlock
						break;
					case TokenType.GRAMMARSTRING:
					case TokenType.GRAMMARARROW:
					case TokenType.GRAMMARNONKEYWORD:
					case TokenType.GRAMMARKEYWORD:
					case TokenType.GRAMMARSYMBOL:
						ParseGrammarBlock(node); // NonTerminal Rule: GrammarBlock
						break;
					case TokenType.CODEBLOCKOPEN:
						ParseCodeBlock(node); // NonTerminal Rule: CodeBlock
						break;
					case TokenType.DIRECTIVEOPEN:
						ParseDirectiveBlock(node); // NonTerminal Rule: DirectiveBlock
						break;
					default:
						tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected GRAMMARCOMMENTLINE, GRAMMARCOMMENTBLOCK, ATTRIBUTEOPEN, GRAMMARSTRING, GRAMMARARROW, GRAMMARNONKEYWORD, GRAMMARKEYWORD, GRAMMARSYMBOL, CODEBLOCKOPEN, or DIRECTIVEOPEN.", 0x0002, tok));
						break;
				} // Choice Rule
				tok = scanner.LookAhead(TokenType.GRAMMARCOMMENTLINE, TokenType.GRAMMARCOMMENTBLOCK, TokenType.ATTRIBUTEOPEN, TokenType.GRAMMARSTRING, TokenType.GRAMMARARROW, TokenType.GRAMMARNONKEYWORD, TokenType.GRAMMARKEYWORD, TokenType.GRAMMARSYMBOL, TokenType.CODEBLOCKOPEN, TokenType.DIRECTIVEOPEN, TokenType.EOF); // ZeroOrMore Rule
			}

			// Concat Rule
			tok = scanner.Scan(TokenType.EOF); // Terminal Rule: EOF
			n = node.CreateNode(tok, tok.ToString() );
			node.Token.UpdateRange(tok);
			node.Nodes.Add(n);
			if (tok.Type != TokenType.EOF) {
				tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected 'EOF'.", 0x1001, tok));
				return;
			}

			parent.Token.UpdateRange(node.Token);
		} // NonTerminalSymbol: Start

		public ParseTree ParseCommentBlock(string input, ParseTree tree) // NonTerminalSymbol: CommentBlock
		{
			scanner.Init(input);
			this.tree = tree;
			ParseCommentBlock(tree);
			tree.Skipped = scanner.Skipped;
			return tree;
		}

		private void ParseCommentBlock(ParseNode parent) // NonTerminalSymbol: CommentBlock
		{
			Token tok;
			ParseNode n;
			ParseNode node = parent.CreateNode(scanner.GetToken(TokenType.CommentBlock), "CommentBlock");
			parent.Nodes.Add(node);

			do { // OneOrMore Rule
				tok = scanner.LookAhead(TokenType.GRAMMARCOMMENTLINE, TokenType.GRAMMARCOMMENTBLOCK);
				switch (tok.Type)
				{ // Choice Rule
					case TokenType.GRAMMARCOMMENTLINE:
						tok = scanner.Scan(TokenType.GRAMMARCOMMENTLINE); // Terminal Rule: GRAMMARCOMMENTLINE
						n = node.CreateNode(tok, tok.ToString() );
						node.Token.UpdateRange(tok);
						node.Nodes.Add(n);
						if (tok.Type != TokenType.GRAMMARCOMMENTLINE) {
							tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected 'GRAMMARCOMMENTLINE'.", 0x1001, tok));
							return;
						}
						break;
					case TokenType.GRAMMARCOMMENTBLOCK:
						tok = scanner.Scan(TokenType.GRAMMARCOMMENTBLOCK); // Terminal Rule: GRAMMARCOMMENTBLOCK
						n = node.CreateNode(tok, tok.ToString() );
						node.Token.UpdateRange(tok);
						node.Nodes.Add(n);
						if (tok.Type != TokenType.GRAMMARCOMMENTBLOCK) {
							tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected 'GRAMMARCOMMENTBLOCK'.", 0x1001, tok));
							return;
						}
						break;
					default:
						tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected GRAMMARCOMMENTLINE or GRAMMARCOMMENTBLOCK.", 0x0002, tok));
						break;
				} // Choice Rule
				tok = scanner.LookAhead(TokenType.GRAMMARCOMMENTLINE, TokenType.GRAMMARCOMMENTBLOCK);
			} while (tok.Type == TokenType.GRAMMARCOMMENTLINE
			    || tok.Type == TokenType.GRAMMARCOMMENTBLOCK); // OneOrMore Rule

			parent.Token.UpdateRange(node.Token);
		} // NonTerminalSymbol: CommentBlock

		public ParseTree ParseDirectiveBlock(string input, ParseTree tree) // NonTerminalSymbol: DirectiveBlock
		{
			scanner.Init(input);
			this.tree = tree;
			ParseDirectiveBlock(tree);
			tree.Skipped = scanner.Skipped;
			return tree;
		}

		private void ParseDirectiveBlock(ParseNode parent) // NonTerminalSymbol: DirectiveBlock
		{
			Token tok;
			ParseNode n;
			ParseNode node = parent.CreateNode(scanner.GetToken(TokenType.DirectiveBlock), "DirectiveBlock");
			parent.Nodes.Add(node);


			// Concat Rule
			tok = scanner.Scan(TokenType.DIRECTIVEOPEN); // Terminal Rule: DIRECTIVEOPEN
			n = node.CreateNode(tok, tok.ToString() );
			node.Token.UpdateRange(tok);
			node.Nodes.Add(n);
			if (tok.Type != TokenType.DIRECTIVEOPEN) {
				tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected 'DIRECTIVEOPEN'.", 0x1001, tok));
				return;
			}

			// Concat Rule
			tok = scanner.LookAhead(TokenType.WHITESPACE, TokenType.DIRECTIVEKEYWORD, TokenType.DIRECTIVESYMBOL, TokenType.CODEBLOCKOPEN, TokenType.DIRECTIVENONKEYWORD, TokenType.DIRECTIVESTRING, TokenType.DIRECTIVECLOSE); // ZeroOrMore Rule
			while (tok.Type == TokenType.WHITESPACE
			    || tok.Type == TokenType.DIRECTIVEKEYWORD
			    || tok.Type == TokenType.DIRECTIVESYMBOL
			    || tok.Type == TokenType.CODEBLOCKOPEN
			    || tok.Type == TokenType.DIRECTIVENONKEYWORD
			    || tok.Type == TokenType.DIRECTIVESTRING)
			{
				tok = scanner.LookAhead(TokenType.WHITESPACE, TokenType.DIRECTIVEKEYWORD, TokenType.DIRECTIVESYMBOL, TokenType.CODEBLOCKOPEN, TokenType.DIRECTIVENONKEYWORD, TokenType.DIRECTIVESTRING);
				switch (tok.Type)
				{ // Choice Rule
					case TokenType.WHITESPACE:
						tok = scanner.Scan(TokenType.WHITESPACE); // Terminal Rule: WHITESPACE
						n = node.CreateNode(tok, tok.ToString() );
						node.Token.UpdateRange(tok);
						node.Nodes.Add(n);
						if (tok.Type != TokenType.WHITESPACE) {
							tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected 'WHITESPACE'.", 0x1001, tok));
							return;
						}
						break;
					case TokenType.DIRECTIVEKEYWORD:
						tok = scanner.Scan(TokenType.DIRECTIVEKEYWORD); // Terminal Rule: DIRECTIVEKEYWORD
						n = node.CreateNode(tok, tok.ToString() );
						node.Token.UpdateRange(tok);
						node.Nodes.Add(n);
						if (tok.Type != TokenType.DIRECTIVEKEYWORD) {
							tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected 'DIRECTIVEKEYWORD'.", 0x1001, tok));
							return;
						}
						break;
					case TokenType.DIRECTIVESYMBOL:
						tok = scanner.Scan(TokenType.DIRECTIVESYMBOL); // Terminal Rule: DIRECTIVESYMBOL
						n = node.CreateNode(tok, tok.ToString() );
						node.Token.UpdateRange(tok);
						node.Nodes.Add(n);
						if (tok.Type != TokenType.DIRECTIVESYMBOL) {
							tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected 'DIRECTIVESYMBOL'.", 0x1001, tok));
							return;
						}
						break;
					case TokenType.CODEBLOCKOPEN:
						ParseCodeBlock(node); // NonTerminal Rule: CodeBlock
						break;
					case TokenType.DIRECTIVENONKEYWORD:
						tok = scanner.Scan(TokenType.DIRECTIVENONKEYWORD); // Terminal Rule: DIRECTIVENONKEYWORD
						n = node.CreateNode(tok, tok.ToString() );
						node.Token.UpdateRange(tok);
						node.Nodes.Add(n);
						if (tok.Type != TokenType.DIRECTIVENONKEYWORD) {
							tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected 'DIRECTIVENONKEYWORD'.", 0x1001, tok));
							return;
						}
						break;
					case TokenType.DIRECTIVESTRING:
						tok = scanner.Scan(TokenType.DIRECTIVESTRING); // Terminal Rule: DIRECTIVESTRING
						n = node.CreateNode(tok, tok.ToString() );
						node.Token.UpdateRange(tok);
						node.Nodes.Add(n);
						if (tok.Type != TokenType.DIRECTIVESTRING) {
							tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected 'DIRECTIVESTRING'.", 0x1001, tok));
							return;
						}
						break;
					default:
						tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected WHITESPACE, DIRECTIVEKEYWORD, DIRECTIVESYMBOL, CODEBLOCKOPEN, DIRECTIVENONKEYWORD, or DIRECTIVESTRING.", 0x0002, tok));
						break;
				} // Choice Rule
				tok = scanner.LookAhead(TokenType.WHITESPACE, TokenType.DIRECTIVEKEYWORD, TokenType.DIRECTIVESYMBOL, TokenType.CODEBLOCKOPEN, TokenType.DIRECTIVENONKEYWORD, TokenType.DIRECTIVESTRING, TokenType.DIRECTIVECLOSE); // ZeroOrMore Rule
			}

			// Concat Rule
			tok = scanner.LookAhead(TokenType.DIRECTIVECLOSE); // Option Rule
			if (tok.Type == TokenType.DIRECTIVECLOSE)
			{
				tok = scanner.Scan(TokenType.DIRECTIVECLOSE); // Terminal Rule: DIRECTIVECLOSE
				n = node.CreateNode(tok, tok.ToString() );
				node.Token.UpdateRange(tok);
				node.Nodes.Add(n);
				if (tok.Type != TokenType.DIRECTIVECLOSE) {
					tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected 'DIRECTIVECLOSE'.", 0x1001, tok));
					return;
				}
			}

			parent.Token.UpdateRange(node.Token);
		} // NonTerminalSymbol: DirectiveBlock

		public ParseTree ParseGrammarBlock(string input, ParseTree tree) // NonTerminalSymbol: GrammarBlock
		{
			scanner.Init(input);
			this.tree = tree;
			ParseGrammarBlock(tree);
			tree.Skipped = scanner.Skipped;
			return tree;
		}

		private void ParseGrammarBlock(ParseNode parent) // NonTerminalSymbol: GrammarBlock
		{
			Token tok;
			ParseNode n;
			ParseNode node = parent.CreateNode(scanner.GetToken(TokenType.GrammarBlock), "GrammarBlock");
			parent.Nodes.Add(node);

			do { // OneOrMore Rule
				tok = scanner.LookAhead(TokenType.GRAMMARSTRING, TokenType.GRAMMARARROW, TokenType.GRAMMARNONKEYWORD, TokenType.GRAMMARKEYWORD, TokenType.GRAMMARSYMBOL);
				switch (tok.Type)
				{ // Choice Rule
					case TokenType.GRAMMARSTRING:
						tok = scanner.Scan(TokenType.GRAMMARSTRING); // Terminal Rule: GRAMMARSTRING
						n = node.CreateNode(tok, tok.ToString() );
						node.Token.UpdateRange(tok);
						node.Nodes.Add(n);
						if (tok.Type != TokenType.GRAMMARSTRING) {
							tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected 'GRAMMARSTRING'.", 0x1001, tok));
							return;
						}
						break;
					case TokenType.GRAMMARARROW:
						tok = scanner.Scan(TokenType.GRAMMARARROW); // Terminal Rule: GRAMMARARROW
						n = node.CreateNode(tok, tok.ToString() );
						node.Token.UpdateRange(tok);
						node.Nodes.Add(n);
						if (tok.Type != TokenType.GRAMMARARROW) {
							tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected 'GRAMMARARROW'.", 0x1001, tok));
							return;
						}
						break;
					case TokenType.GRAMMARNONKEYWORD:
						tok = scanner.Scan(TokenType.GRAMMARNONKEYWORD); // Terminal Rule: GRAMMARNONKEYWORD
						n = node.CreateNode(tok, tok.ToString() );
						node.Token.UpdateRange(tok);
						node.Nodes.Add(n);
						if (tok.Type != TokenType.GRAMMARNONKEYWORD) {
							tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected 'GRAMMARNONKEYWORD'.", 0x1001, tok));
							return;
						}
						break;
					case TokenType.GRAMMARKEYWORD:
						tok = scanner.Scan(TokenType.GRAMMARKEYWORD); // Terminal Rule: GRAMMARKEYWORD
						n = node.CreateNode(tok, tok.ToString() );
						node.Token.UpdateRange(tok);
						node.Nodes.Add(n);
						if (tok.Type != TokenType.GRAMMARKEYWORD) {
							tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected 'GRAMMARKEYWORD'.", 0x1001, tok));
							return;
						}
						break;
					case TokenType.GRAMMARSYMBOL:
						tok = scanner.Scan(TokenType.GRAMMARSYMBOL); // Terminal Rule: GRAMMARSYMBOL
						n = node.CreateNode(tok, tok.ToString() );
						node.Token.UpdateRange(tok);
						node.Nodes.Add(n);
						if (tok.Type != TokenType.GRAMMARSYMBOL) {
							tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected 'GRAMMARSYMBOL'.", 0x1001, tok));
							return;
						}
						break;
					default:
						tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected GRAMMARSTRING, GRAMMARARROW, GRAMMARNONKEYWORD, GRAMMARKEYWORD, or GRAMMARSYMBOL.", 0x0002, tok));
						break;
				} // Choice Rule
				tok = scanner.LookAhead(TokenType.GRAMMARSTRING, TokenType.GRAMMARARROW, TokenType.GRAMMARNONKEYWORD, TokenType.GRAMMARKEYWORD, TokenType.GRAMMARSYMBOL);
			} while (tok.Type == TokenType.GRAMMARSTRING
			    || tok.Type == TokenType.GRAMMARARROW
			    || tok.Type == TokenType.GRAMMARNONKEYWORD
			    || tok.Type == TokenType.GRAMMARKEYWORD
			    || tok.Type == TokenType.GRAMMARSYMBOL); // OneOrMore Rule

			parent.Token.UpdateRange(node.Token);
		} // NonTerminalSymbol: GrammarBlock

		public ParseTree ParseAttributeBlock(string input, ParseTree tree) // NonTerminalSymbol: AttributeBlock
		{
			scanner.Init(input);
			this.tree = tree;
			ParseAttributeBlock(tree);
			tree.Skipped = scanner.Skipped;
			return tree;
		}

		private void ParseAttributeBlock(ParseNode parent) // NonTerminalSymbol: AttributeBlock
		{
			Token tok;
			ParseNode n;
			ParseNode node = parent.CreateNode(scanner.GetToken(TokenType.AttributeBlock), "AttributeBlock");
			parent.Nodes.Add(node);


			// Concat Rule
			tok = scanner.Scan(TokenType.ATTRIBUTEOPEN); // Terminal Rule: ATTRIBUTEOPEN
			n = node.CreateNode(tok, tok.ToString() );
			node.Token.UpdateRange(tok);
			node.Nodes.Add(n);
			if (tok.Type != TokenType.ATTRIBUTEOPEN) {
				tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected 'ATTRIBUTEOPEN'.", 0x1001, tok));
				return;
			}

			// Concat Rule
			tok = scanner.LookAhead(TokenType.ATTRIBUTEKEYWORD, TokenType.ATTRIBUTENONKEYWORD, TokenType.ATTRIBUTESYMBOL, TokenType.ATTRIBUTECLOSE); // ZeroOrMore Rule
			while (tok.Type == TokenType.ATTRIBUTEKEYWORD
			    || tok.Type == TokenType.ATTRIBUTENONKEYWORD
			    || tok.Type == TokenType.ATTRIBUTESYMBOL)
			{
				tok = scanner.LookAhead(TokenType.ATTRIBUTEKEYWORD, TokenType.ATTRIBUTENONKEYWORD, TokenType.ATTRIBUTESYMBOL);
				switch (tok.Type)
				{ // Choice Rule
					case TokenType.ATTRIBUTEKEYWORD:
						tok = scanner.Scan(TokenType.ATTRIBUTEKEYWORD); // Terminal Rule: ATTRIBUTEKEYWORD
						n = node.CreateNode(tok, tok.ToString() );
						node.Token.UpdateRange(tok);
						node.Nodes.Add(n);
						if (tok.Type != TokenType.ATTRIBUTEKEYWORD) {
							tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected 'ATTRIBUTEKEYWORD'.", 0x1001, tok));
							return;
						}
						break;
					case TokenType.ATTRIBUTENONKEYWORD:
						tok = scanner.Scan(TokenType.ATTRIBUTENONKEYWORD); // Terminal Rule: ATTRIBUTENONKEYWORD
						n = node.CreateNode(tok, tok.ToString() );
						node.Token.UpdateRange(tok);
						node.Nodes.Add(n);
						if (tok.Type != TokenType.ATTRIBUTENONKEYWORD) {
							tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected 'ATTRIBUTENONKEYWORD'.", 0x1001, tok));
							return;
						}
						break;
					case TokenType.ATTRIBUTESYMBOL:
						tok = scanner.Scan(TokenType.ATTRIBUTESYMBOL); // Terminal Rule: ATTRIBUTESYMBOL
						n = node.CreateNode(tok, tok.ToString() );
						node.Token.UpdateRange(tok);
						node.Nodes.Add(n);
						if (tok.Type != TokenType.ATTRIBUTESYMBOL) {
							tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected 'ATTRIBUTESYMBOL'.", 0x1001, tok));
							return;
						}
						break;
					default:
						tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected ATTRIBUTEKEYWORD, ATTRIBUTENONKEYWORD, or ATTRIBUTESYMBOL.", 0x0002, tok));
						break;
				} // Choice Rule
				tok = scanner.LookAhead(TokenType.ATTRIBUTEKEYWORD, TokenType.ATTRIBUTENONKEYWORD, TokenType.ATTRIBUTESYMBOL, TokenType.ATTRIBUTECLOSE); // ZeroOrMore Rule
			}

			// Concat Rule
			tok = scanner.LookAhead(TokenType.ATTRIBUTECLOSE); // Option Rule
			if (tok.Type == TokenType.ATTRIBUTECLOSE)
			{
				tok = scanner.Scan(TokenType.ATTRIBUTECLOSE); // Terminal Rule: ATTRIBUTECLOSE
				n = node.CreateNode(tok, tok.ToString() );
				node.Token.UpdateRange(tok);
				node.Nodes.Add(n);
				if (tok.Type != TokenType.ATTRIBUTECLOSE) {
					tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected 'ATTRIBUTECLOSE'.", 0x1001, tok));
					return;
				}
			}

			parent.Token.UpdateRange(node.Token);
		} // NonTerminalSymbol: AttributeBlock

		public ParseTree ParseCodeBlock(string input, ParseTree tree) // NonTerminalSymbol: CodeBlock
		{
			scanner.Init(input);
			this.tree = tree;
			ParseCodeBlock(tree);
			tree.Skipped = scanner.Skipped;
			return tree;
		}

		private void ParseCodeBlock(ParseNode parent) // NonTerminalSymbol: CodeBlock
		{
			Token tok;
			ParseNode n;
			ParseNode node = parent.CreateNode(scanner.GetToken(TokenType.CodeBlock), "CodeBlock");
			parent.Nodes.Add(node);


			// Concat Rule
			tok = scanner.Scan(TokenType.CODEBLOCKOPEN); // Terminal Rule: CODEBLOCKOPEN
			n = node.CreateNode(tok, tok.ToString() );
			node.Token.UpdateRange(tok);
			node.Nodes.Add(n);
			if (tok.Type != TokenType.CODEBLOCKOPEN) {
				tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected 'CODEBLOCKOPEN'.", 0x1001, tok));
				return;
			}

			// Concat Rule
			tok = scanner.LookAhead(TokenType.DOTNET_COMMENTLINE, TokenType.DOTNET_COMMENTBLOCK, TokenType.DOTNET_TYPES, TokenType.DOTNET_KEYWORD, TokenType.DOTNET_SYMBOL, TokenType.DOTNET_STRING, TokenType.DOTNET_NONKEYWORD, TokenType.CODEBLOCKCLOSE); // ZeroOrMore Rule
			while (tok.Type == TokenType.DOTNET_COMMENTLINE
			    || tok.Type == TokenType.DOTNET_COMMENTBLOCK
			    || tok.Type == TokenType.DOTNET_TYPES
			    || tok.Type == TokenType.DOTNET_KEYWORD
			    || tok.Type == TokenType.DOTNET_SYMBOL
			    || tok.Type == TokenType.DOTNET_STRING
			    || tok.Type == TokenType.DOTNET_NONKEYWORD)
			{
				tok = scanner.LookAhead(TokenType.DOTNET_COMMENTLINE, TokenType.DOTNET_COMMENTBLOCK, TokenType.DOTNET_TYPES, TokenType.DOTNET_KEYWORD, TokenType.DOTNET_SYMBOL, TokenType.DOTNET_STRING, TokenType.DOTNET_NONKEYWORD);
				switch (tok.Type)
				{ // Choice Rule
					case TokenType.DOTNET_COMMENTLINE:
						tok = scanner.Scan(TokenType.DOTNET_COMMENTLINE); // Terminal Rule: DOTNET_COMMENTLINE
						n = node.CreateNode(tok, tok.ToString() );
						node.Token.UpdateRange(tok);
						node.Nodes.Add(n);
						if (tok.Type != TokenType.DOTNET_COMMENTLINE) {
							tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected 'DOTNET_COMMENTLINE'.", 0x1001, tok));
							return;
						}
						break;
					case TokenType.DOTNET_COMMENTBLOCK:
						tok = scanner.Scan(TokenType.DOTNET_COMMENTBLOCK); // Terminal Rule: DOTNET_COMMENTBLOCK
						n = node.CreateNode(tok, tok.ToString() );
						node.Token.UpdateRange(tok);
						node.Nodes.Add(n);
						if (tok.Type != TokenType.DOTNET_COMMENTBLOCK) {
							tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected 'DOTNET_COMMENTBLOCK'.", 0x1001, tok));
							return;
						}
						break;
					case TokenType.DOTNET_TYPES:
						tok = scanner.Scan(TokenType.DOTNET_TYPES); // Terminal Rule: DOTNET_TYPES
						n = node.CreateNode(tok, tok.ToString() );
						node.Token.UpdateRange(tok);
						node.Nodes.Add(n);
						if (tok.Type != TokenType.DOTNET_TYPES) {
							tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected 'DOTNET_TYPES'.", 0x1001, tok));
							return;
						}
						break;
					case TokenType.DOTNET_KEYWORD:
						tok = scanner.Scan(TokenType.DOTNET_KEYWORD); // Terminal Rule: DOTNET_KEYWORD
						n = node.CreateNode(tok, tok.ToString() );
						node.Token.UpdateRange(tok);
						node.Nodes.Add(n);
						if (tok.Type != TokenType.DOTNET_KEYWORD) {
							tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected 'DOTNET_KEYWORD'.", 0x1001, tok));
							return;
						}
						break;
					case TokenType.DOTNET_SYMBOL:
						tok = scanner.Scan(TokenType.DOTNET_SYMBOL); // Terminal Rule: DOTNET_SYMBOL
						n = node.CreateNode(tok, tok.ToString() );
						node.Token.UpdateRange(tok);
						node.Nodes.Add(n);
						if (tok.Type != TokenType.DOTNET_SYMBOL) {
							tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected 'DOTNET_SYMBOL'.", 0x1001, tok));
							return;
						}
						break;
					case TokenType.DOTNET_STRING:
						tok = scanner.Scan(TokenType.DOTNET_STRING); // Terminal Rule: DOTNET_STRING
						n = node.CreateNode(tok, tok.ToString() );
						node.Token.UpdateRange(tok);
						node.Nodes.Add(n);
						if (tok.Type != TokenType.DOTNET_STRING) {
							tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected 'DOTNET_STRING'.", 0x1001, tok));
							return;
						}
						break;
					case TokenType.DOTNET_NONKEYWORD:
						tok = scanner.Scan(TokenType.DOTNET_NONKEYWORD); // Terminal Rule: DOTNET_NONKEYWORD
						n = node.CreateNode(tok, tok.ToString() );
						node.Token.UpdateRange(tok);
						node.Nodes.Add(n);
						if (tok.Type != TokenType.DOTNET_NONKEYWORD) {
							tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected 'DOTNET_NONKEYWORD'.", 0x1001, tok));
							return;
						}
						break;
					default:
						tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected DOTNET_COMMENTLINE, DOTNET_COMMENTBLOCK, DOTNET_TYPES, DOTNET_KEYWORD, DOTNET_SYMBOL, DOTNET_STRING, or DOTNET_NONKEYWORD.", 0x0002, tok));
						break;
				} // Choice Rule
				tok = scanner.LookAhead(TokenType.DOTNET_COMMENTLINE, TokenType.DOTNET_COMMENTBLOCK, TokenType.DOTNET_TYPES, TokenType.DOTNET_KEYWORD, TokenType.DOTNET_SYMBOL, TokenType.DOTNET_STRING, TokenType.DOTNET_NONKEYWORD, TokenType.CODEBLOCKCLOSE); // ZeroOrMore Rule
			}

			// Concat Rule
			tok = scanner.LookAhead(TokenType.CODEBLOCKCLOSE); // Option Rule
			if (tok.Type == TokenType.CODEBLOCKCLOSE)
			{
				tok = scanner.Scan(TokenType.CODEBLOCKCLOSE); // Terminal Rule: CODEBLOCKCLOSE
				n = node.CreateNode(tok, tok.ToString() );
				node.Token.UpdateRange(tok);
				node.Nodes.Add(n);
				if (tok.Type != TokenType.CODEBLOCKCLOSE) {
					tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected 'CODEBLOCKCLOSE'.", 0x1001, tok));
					return;
				}
			}

			parent.Token.UpdateRange(node.Token);
		} // NonTerminalSymbol: CodeBlock



	}

	#endregion Parser
}
