// Automatically generated from source file: <%SourceFilename%>
// By TinyPG v<%GeneratorVersion%> available at https://github.com/ultrasuperpingu/TinyPG
<%HeaderCode%>

use crate::{parse_tree::{IParseNode, IParserTree, ParseError, ParseTree}, scanner::{Scanner, Token, TokenType}};
pub struct Parser
{
	scanner : Scanner
}
impl Parser {
	pub fn new(scanner:Scanner) -> Self
	{
		Self {scanner}
	}

	pub fn parse(&mut self, input: &str) -> ParseTree
	{
		self.parse_with_tree(input, ParseTree::new())
	}

	pub fn parse_with_tree(&mut self, input: &str, mut tree: ParseTree) -> ParseTree
	{
		self.scanner.init(input);

		self.parse_node_start(&mut tree, None);
		tree.skipped = self.scanner.skipped.clone();
		tree
	}

<%ParseNonTerminals%>

}
