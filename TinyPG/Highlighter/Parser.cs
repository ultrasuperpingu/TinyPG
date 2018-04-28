// Generated by TinyPG v1.3 available at www.codeproject.com

using System;
using System.Collections.Generic;

namespace TinyPG.Highlighter
{
	#region Parser

	public partial class Parser
	{
		private Scanner scanner;
		private ParseTree tree;

		public Parser(Scanner scanner)
		{
			this.scanner = scanner;
		}

		public ParseTree Parse(string input, string fileName)
		{
			tree = new ParseTree();
			return Parse(input, fileName, tree);
		}

		public ParseTree Parse(string input, string fileName, ParseTree tree)
		{
			scanner.Init(input, fileName);

			this.tree = tree;
			ParseStart(tree);
			tree.Skipped = scanner.Skipped;

			return tree;
		}

		private void ParseStart(ParseNode parent)
		{
			Token tok;
			ParseNode n;
			ParseNode node = parent.CreateNode(scanner.GetToken(TokenType.Start), "Start");
			parent.Nodes.Add(node);



			tok = scanner.LookAhead(TokenType.GRAMMARCOMMENTLINE, TokenType.GRAMMARCOMMENTBLOCK, TokenType.ATTRIBUTEOPEN, TokenType.GRAMMARSTRING, TokenType.GRAMMARARROW, TokenType.GRAMMARNONKEYWORD, TokenType.GRAMMARKEYWORD, TokenType.GRAMMARSYMBOL, TokenType.CODEBLOCKOPEN, TokenType.DIRECTIVEOPEN);
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
				{
					case TokenType.GRAMMARCOMMENTLINE:
					case TokenType.GRAMMARCOMMENTBLOCK:
						ParseCommentBlock(node);
						break;
					case TokenType.ATTRIBUTEOPEN:
						ParseAttributeBlock(node);
						break;
					case TokenType.GRAMMARSTRING:
					case TokenType.GRAMMARARROW:
					case TokenType.GRAMMARNONKEYWORD:
					case TokenType.GRAMMARKEYWORD:
					case TokenType.GRAMMARSYMBOL:
						ParseGrammarBlock(node);
						break;
					case TokenType.CODEBLOCKOPEN:
						ParseCodeBlock(node);
						break;
					case TokenType.DIRECTIVEOPEN:
						ParseDirectiveBlock(node);
						break;
					default:
						tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found.", 0x0002, tok));
						break;
				}
				tok = scanner.LookAhead(TokenType.GRAMMARCOMMENTLINE, TokenType.GRAMMARCOMMENTBLOCK, TokenType.ATTRIBUTEOPEN, TokenType.GRAMMARSTRING, TokenType.GRAMMARARROW, TokenType.GRAMMARNONKEYWORD, TokenType.GRAMMARKEYWORD, TokenType.GRAMMARSYMBOL, TokenType.CODEBLOCKOPEN, TokenType.DIRECTIVEOPEN);
			}


			tok = scanner.Scan(TokenType.EOF);
			n = node.CreateNode(tok, tok.ToString());
			node.Token.UpdateRange(tok);
			node.Nodes.Add(n);
			if (tok.Type != TokenType.EOF)
			{
				tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.EOF.ToString(), 0x1001, tok));
				return;
			}

			parent.Token.UpdateRange(node.Token);
		}

		private void ParseCommentBlock(ParseNode parent)
		{
			Token tok;
			ParseNode n;
			ParseNode node = parent.CreateNode(scanner.GetToken(TokenType.CommentBlock), "CommentBlock");
			parent.Nodes.Add(node);

			do
			{
				tok = scanner.LookAhead(TokenType.GRAMMARCOMMENTLINE, TokenType.GRAMMARCOMMENTBLOCK);
				switch (tok.Type)
				{
					case TokenType.GRAMMARCOMMENTLINE:
						tok = scanner.Scan(TokenType.GRAMMARCOMMENTLINE);
						n = node.CreateNode(tok, tok.ToString());
						node.Token.UpdateRange(tok);
						node.Nodes.Add(n);
						if (tok.Type != TokenType.GRAMMARCOMMENTLINE)
						{
							tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.GRAMMARCOMMENTLINE.ToString(), 0x1001, tok));
							return;
						}
						break;
					case TokenType.GRAMMARCOMMENTBLOCK:
						tok = scanner.Scan(TokenType.GRAMMARCOMMENTBLOCK);
						n = node.CreateNode(tok, tok.ToString());
						node.Token.UpdateRange(tok);
						node.Nodes.Add(n);
						if (tok.Type != TokenType.GRAMMARCOMMENTBLOCK)
						{
							tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.GRAMMARCOMMENTBLOCK.ToString(), 0x1001, tok));
							return;
						}
						break;
					default:
						tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found.", 0x0002, tok));
						break;
				}
				tok = scanner.LookAhead(TokenType.GRAMMARCOMMENTLINE, TokenType.GRAMMARCOMMENTBLOCK);
			} while (tok.Type == TokenType.GRAMMARCOMMENTLINE
				|| tok.Type == TokenType.GRAMMARCOMMENTBLOCK);

			parent.Token.UpdateRange(node.Token);
		}

		private void ParseDirectiveBlock(ParseNode parent)
		{
			Token tok;
			ParseNode n;
			ParseNode node = parent.CreateNode(scanner.GetToken(TokenType.DirectiveBlock), "DirectiveBlock");
			parent.Nodes.Add(node);



			tok = scanner.Scan(TokenType.DIRECTIVEOPEN);
			n = node.CreateNode(tok, tok.ToString());
			node.Token.UpdateRange(tok);
			node.Nodes.Add(n);
			if (tok.Type != TokenType.DIRECTIVEOPEN)
			{
				tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.DIRECTIVEOPEN.ToString(), 0x1001, tok));
				return;
			}


			tok = scanner.LookAhead(TokenType.WHITESPACE, TokenType.DIRECTIVEKEYWORD, TokenType.DIRECTIVESYMBOL, TokenType.DIRECTIVENONKEYWORD, TokenType.DIRECTIVESTRING);
			while (tok.Type == TokenType.WHITESPACE
				|| tok.Type == TokenType.DIRECTIVEKEYWORD
				|| tok.Type == TokenType.DIRECTIVESYMBOL
				|| tok.Type == TokenType.DIRECTIVENONKEYWORD
				|| tok.Type == TokenType.DIRECTIVESTRING)
			{
				tok = scanner.LookAhead(TokenType.WHITESPACE, TokenType.DIRECTIVEKEYWORD, TokenType.DIRECTIVESYMBOL, TokenType.DIRECTIVENONKEYWORD, TokenType.DIRECTIVESTRING);
				switch (tok.Type)
				{
					case TokenType.WHITESPACE:
						tok = scanner.Scan(TokenType.WHITESPACE);
						n = node.CreateNode(tok, tok.ToString());
						node.Token.UpdateRange(tok);
						node.Nodes.Add(n);
						if (tok.Type != TokenType.WHITESPACE)
						{
							tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.WHITESPACE.ToString(), 0x1001, tok));
							return;
						}
						break;
					case TokenType.DIRECTIVEKEYWORD:
						tok = scanner.Scan(TokenType.DIRECTIVEKEYWORD);
						n = node.CreateNode(tok, tok.ToString());
						node.Token.UpdateRange(tok);
						node.Nodes.Add(n);
						if (tok.Type != TokenType.DIRECTIVEKEYWORD)
						{
							tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.DIRECTIVEKEYWORD.ToString(), 0x1001, tok));
							return;
						}
						break;
					case TokenType.DIRECTIVESYMBOL:
						tok = scanner.Scan(TokenType.DIRECTIVESYMBOL);
						n = node.CreateNode(tok, tok.ToString());
						node.Token.UpdateRange(tok);
						node.Nodes.Add(n);
						if (tok.Type != TokenType.DIRECTIVESYMBOL)
						{
							tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.DIRECTIVESYMBOL.ToString(), 0x1001, tok));
							return;
						}
						break;
					case TokenType.DIRECTIVENONKEYWORD:
						tok = scanner.Scan(TokenType.DIRECTIVENONKEYWORD);
						n = node.CreateNode(tok, tok.ToString());
						node.Token.UpdateRange(tok);
						node.Nodes.Add(n);
						if (tok.Type != TokenType.DIRECTIVENONKEYWORD)
						{
							tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.DIRECTIVENONKEYWORD.ToString(), 0x1001, tok));
							return;
						}
						break;
					case TokenType.DIRECTIVESTRING:
						tok = scanner.Scan(TokenType.DIRECTIVESTRING);
						n = node.CreateNode(tok, tok.ToString());
						node.Token.UpdateRange(tok);
						node.Nodes.Add(n);
						if (tok.Type != TokenType.DIRECTIVESTRING)
						{
							tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.DIRECTIVESTRING.ToString(), 0x1001, tok));
							return;
						}
						break;
					default:
						tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found.", 0x0002, tok));
						break;
				}
				tok = scanner.LookAhead(TokenType.WHITESPACE, TokenType.DIRECTIVEKEYWORD, TokenType.DIRECTIVESYMBOL, TokenType.DIRECTIVENONKEYWORD, TokenType.DIRECTIVESTRING);
			}


			tok = scanner.LookAhead(TokenType.DIRECTIVECLOSE);
			if (tok.Type == TokenType.DIRECTIVECLOSE)
			{
				tok = scanner.Scan(TokenType.DIRECTIVECLOSE);
				n = node.CreateNode(tok, tok.ToString());
				node.Token.UpdateRange(tok);
				node.Nodes.Add(n);
				if (tok.Type != TokenType.DIRECTIVECLOSE)
				{
					tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.DIRECTIVECLOSE.ToString(), 0x1001, tok));
					return;
				}
			}

			parent.Token.UpdateRange(node.Token);
		}

		private void ParseGrammarBlock(ParseNode parent)
		{
			Token tok;
			ParseNode n;
			ParseNode node = parent.CreateNode(scanner.GetToken(TokenType.GrammarBlock), "GrammarBlock");
			parent.Nodes.Add(node);

			do
			{
				tok = scanner.LookAhead(TokenType.GRAMMARSTRING, TokenType.GRAMMARARROW, TokenType.GRAMMARNONKEYWORD, TokenType.GRAMMARKEYWORD, TokenType.GRAMMARSYMBOL);
				switch (tok.Type)
				{
					case TokenType.GRAMMARSTRING:
						tok = scanner.Scan(TokenType.GRAMMARSTRING);
						n = node.CreateNode(tok, tok.ToString());
						node.Token.UpdateRange(tok);
						node.Nodes.Add(n);
						if (tok.Type != TokenType.GRAMMARSTRING)
						{
							tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.GRAMMARSTRING.ToString(), 0x1001, tok));
							return;
						}
						break;
					case TokenType.GRAMMARARROW:
						tok = scanner.Scan(TokenType.GRAMMARARROW);
						n = node.CreateNode(tok, tok.ToString());
						node.Token.UpdateRange(tok);
						node.Nodes.Add(n);
						if (tok.Type != TokenType.GRAMMARARROW)
						{
							tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.GRAMMARARROW.ToString(), 0x1001, tok));
							return;
						}
						break;
					case TokenType.GRAMMARNONKEYWORD:
						tok = scanner.Scan(TokenType.GRAMMARNONKEYWORD);
						n = node.CreateNode(tok, tok.ToString());
						node.Token.UpdateRange(tok);
						node.Nodes.Add(n);
						if (tok.Type != TokenType.GRAMMARNONKEYWORD)
						{
							tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.GRAMMARNONKEYWORD.ToString(), 0x1001, tok));
							return;
						}
						break;
					case TokenType.GRAMMARKEYWORD:
						tok = scanner.Scan(TokenType.GRAMMARKEYWORD);
						n = node.CreateNode(tok, tok.ToString());
						node.Token.UpdateRange(tok);
						node.Nodes.Add(n);
						if (tok.Type != TokenType.GRAMMARKEYWORD)
						{
							tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.GRAMMARKEYWORD.ToString(), 0x1001, tok));
							return;
						}
						break;
					case TokenType.GRAMMARSYMBOL:
						tok = scanner.Scan(TokenType.GRAMMARSYMBOL);
						n = node.CreateNode(tok, tok.ToString());
						node.Token.UpdateRange(tok);
						node.Nodes.Add(n);
						if (tok.Type != TokenType.GRAMMARSYMBOL)
						{
							tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.GRAMMARSYMBOL.ToString(), 0x1001, tok));
							return;
						}
						break;
					default:
						tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found.", 0x0002, tok));
						break;
				}
				tok = scanner.LookAhead(TokenType.GRAMMARSTRING, TokenType.GRAMMARARROW, TokenType.GRAMMARNONKEYWORD, TokenType.GRAMMARKEYWORD, TokenType.GRAMMARSYMBOL);
			} while (tok.Type == TokenType.GRAMMARSTRING
				|| tok.Type == TokenType.GRAMMARARROW
				|| tok.Type == TokenType.GRAMMARNONKEYWORD
				|| tok.Type == TokenType.GRAMMARKEYWORD
				|| tok.Type == TokenType.GRAMMARSYMBOL);

			parent.Token.UpdateRange(node.Token);
		}

		private void ParseAttributeBlock(ParseNode parent)
		{
			Token tok;
			ParseNode n;
			ParseNode node = parent.CreateNode(scanner.GetToken(TokenType.AttributeBlock), "AttributeBlock");
			parent.Nodes.Add(node);



			tok = scanner.Scan(TokenType.ATTRIBUTEOPEN);
			n = node.CreateNode(tok, tok.ToString());
			node.Token.UpdateRange(tok);
			node.Nodes.Add(n);
			if (tok.Type != TokenType.ATTRIBUTEOPEN)
			{
				tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.ATTRIBUTEOPEN.ToString(), 0x1001, tok));
				return;
			}


			tok = scanner.LookAhead(TokenType.ATTRIBUTEKEYWORD, TokenType.ATTRIBUTENONKEYWORD, TokenType.ATTRIBUTESYMBOL);
			while (tok.Type == TokenType.ATTRIBUTEKEYWORD
				|| tok.Type == TokenType.ATTRIBUTENONKEYWORD
				|| tok.Type == TokenType.ATTRIBUTESYMBOL)
			{
				tok = scanner.LookAhead(TokenType.ATTRIBUTEKEYWORD, TokenType.ATTRIBUTENONKEYWORD, TokenType.ATTRIBUTESYMBOL);
				switch (tok.Type)
				{
					case TokenType.ATTRIBUTEKEYWORD:
						tok = scanner.Scan(TokenType.ATTRIBUTEKEYWORD);
						n = node.CreateNode(tok, tok.ToString());
						node.Token.UpdateRange(tok);
						node.Nodes.Add(n);
						if (tok.Type != TokenType.ATTRIBUTEKEYWORD)
						{
							tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.ATTRIBUTEKEYWORD.ToString(), 0x1001, tok));
							return;
						}
						break;
					case TokenType.ATTRIBUTENONKEYWORD:
						tok = scanner.Scan(TokenType.ATTRIBUTENONKEYWORD);
						n = node.CreateNode(tok, tok.ToString());
						node.Token.UpdateRange(tok);
						node.Nodes.Add(n);
						if (tok.Type != TokenType.ATTRIBUTENONKEYWORD)
						{
							tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.ATTRIBUTENONKEYWORD.ToString(), 0x1001, tok));
							return;
						}
						break;
					case TokenType.ATTRIBUTESYMBOL:
						tok = scanner.Scan(TokenType.ATTRIBUTESYMBOL);
						n = node.CreateNode(tok, tok.ToString());
						node.Token.UpdateRange(tok);
						node.Nodes.Add(n);
						if (tok.Type != TokenType.ATTRIBUTESYMBOL)
						{
							tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.ATTRIBUTESYMBOL.ToString(), 0x1001, tok));
							return;
						}
						break;
					default:
						tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found.", 0x0002, tok));
						break;
				}
				tok = scanner.LookAhead(TokenType.ATTRIBUTEKEYWORD, TokenType.ATTRIBUTENONKEYWORD, TokenType.ATTRIBUTESYMBOL);
			}


			tok = scanner.LookAhead(TokenType.ATTRIBUTECLOSE);
			if (tok.Type == TokenType.ATTRIBUTECLOSE)
			{
				tok = scanner.Scan(TokenType.ATTRIBUTECLOSE);
				n = node.CreateNode(tok, tok.ToString());
				node.Token.UpdateRange(tok);
				node.Nodes.Add(n);
				if (tok.Type != TokenType.ATTRIBUTECLOSE)
				{
					tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.ATTRIBUTECLOSE.ToString(), 0x1001, tok));
					return;
				}
			}

			parent.Token.UpdateRange(node.Token);
		}

		private void ParseCodeBlock(ParseNode parent)
		{
			Token tok;
			ParseNode n;
			ParseNode node = parent.CreateNode(scanner.GetToken(TokenType.CodeBlock), "CodeBlock");
			parent.Nodes.Add(node);



			tok = scanner.Scan(TokenType.CODEBLOCKOPEN);
			n = node.CreateNode(tok, tok.ToString());
			node.Token.UpdateRange(tok);
			node.Nodes.Add(n);
			if (tok.Type != TokenType.CODEBLOCKOPEN)
			{
				tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.CODEBLOCKOPEN.ToString(), 0x1001, tok));
				return;
			}


			tok = scanner.LookAhead(TokenType.DOTNET_COMMENTLINE, TokenType.DOTNET_COMMENTBLOCK, TokenType.DOTNET_TYPES, TokenType.DOTNET_KEYWORD, TokenType.DOTNET_SYMBOL, TokenType.DOTNET_STRING, TokenType.DOTNET_NONKEYWORD);
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
				{
					case TokenType.DOTNET_COMMENTLINE:
						tok = scanner.Scan(TokenType.DOTNET_COMMENTLINE);
						n = node.CreateNode(tok, tok.ToString());
						node.Token.UpdateRange(tok);
						node.Nodes.Add(n);
						if (tok.Type != TokenType.DOTNET_COMMENTLINE)
						{
							tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.DOTNET_COMMENTLINE.ToString(), 0x1001, tok));
							return;
						}
						break;
					case TokenType.DOTNET_COMMENTBLOCK:
						tok = scanner.Scan(TokenType.DOTNET_COMMENTBLOCK);
						n = node.CreateNode(tok, tok.ToString());
						node.Token.UpdateRange(tok);
						node.Nodes.Add(n);
						if (tok.Type != TokenType.DOTNET_COMMENTBLOCK)
						{
							tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.DOTNET_COMMENTBLOCK.ToString(), 0x1001, tok));
							return;
						}
						break;
					case TokenType.DOTNET_TYPES:
						tok = scanner.Scan(TokenType.DOTNET_TYPES);
						n = node.CreateNode(tok, tok.ToString());
						node.Token.UpdateRange(tok);
						node.Nodes.Add(n);
						if (tok.Type != TokenType.DOTNET_TYPES)
						{
							tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.DOTNET_TYPES.ToString(), 0x1001, tok));
							return;
						}
						break;
					case TokenType.DOTNET_KEYWORD:
						tok = scanner.Scan(TokenType.DOTNET_KEYWORD);
						n = node.CreateNode(tok, tok.ToString());
						node.Token.UpdateRange(tok);
						node.Nodes.Add(n);
						if (tok.Type != TokenType.DOTNET_KEYWORD)
						{
							tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.DOTNET_KEYWORD.ToString(), 0x1001, tok));
							return;
						}
						break;
					case TokenType.DOTNET_SYMBOL:
						tok = scanner.Scan(TokenType.DOTNET_SYMBOL);
						n = node.CreateNode(tok, tok.ToString());
						node.Token.UpdateRange(tok);
						node.Nodes.Add(n);
						if (tok.Type != TokenType.DOTNET_SYMBOL)
						{
							tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.DOTNET_SYMBOL.ToString(), 0x1001, tok));
							return;
						}
						break;
					case TokenType.DOTNET_STRING:
						tok = scanner.Scan(TokenType.DOTNET_STRING);
						n = node.CreateNode(tok, tok.ToString());
						node.Token.UpdateRange(tok);
						node.Nodes.Add(n);
						if (tok.Type != TokenType.DOTNET_STRING)
						{
							tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.DOTNET_STRING.ToString(), 0x1001, tok));
							return;
						}
						break;
					case TokenType.DOTNET_NONKEYWORD:
						tok = scanner.Scan(TokenType.DOTNET_NONKEYWORD);
						n = node.CreateNode(tok, tok.ToString());
						node.Token.UpdateRange(tok);
						node.Nodes.Add(n);
						if (tok.Type != TokenType.DOTNET_NONKEYWORD)
						{
							tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.DOTNET_NONKEYWORD.ToString(), 0x1001, tok));
							return;
						}
						break;
					default:
						tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found.", 0x0002, tok));
						break;
				}
				tok = scanner.LookAhead(TokenType.DOTNET_COMMENTLINE, TokenType.DOTNET_COMMENTBLOCK, TokenType.DOTNET_TYPES, TokenType.DOTNET_KEYWORD, TokenType.DOTNET_SYMBOL, TokenType.DOTNET_STRING, TokenType.DOTNET_NONKEYWORD);
			}


			tok = scanner.LookAhead(TokenType.CODEBLOCKCLOSE);
			if (tok.Type == TokenType.CODEBLOCKCLOSE)
			{
				tok = scanner.Scan(TokenType.CODEBLOCKCLOSE);
				n = node.CreateNode(tok, tok.ToString());
				node.Token.UpdateRange(tok);
				node.Nodes.Add(n);
				if (tok.Type != TokenType.CODEBLOCKCLOSE)
				{
					tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.CODEBLOCKCLOSE.ToString(), 0x1001, tok));
					return;
				}
			}

			parent.Token.UpdateRange(node.Token);
		}


	}

	#endregion Parser
}
