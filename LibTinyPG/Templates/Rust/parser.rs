// Automatically generated from source file: <%SourceFilename%>
// By TinyPG v<%GeneratorVersion%> available at https://github.com/ultrasuperpingu/TinyPG

use crate::{parse_tree::{IParseNode, ParseError, ParseTree}, scanner::{Scanner, Token, TokenType}};
pub struct Parser
{
	scanner : Scanner
}
impl Parser {
	pub fn new(scanner:Scanner) -> Self
	{
		Self {scanner : scanner}
	}

	pub fn Parse(&mut self, input : String) -> ParseTree
	{
		self.ParseWithTree(input, ParseTree::new())
	}

	pub fn ParseWithTree(&mut self, input:String, mut tree:ParseTree) -> ParseTree
	{
		self.scanner.Init(input);

		let mut node = tree.node.take().unwrap();
		self.ParseNodeStart(&mut tree, &mut node);
		tree.Skipped = self.scanner.skipped.clone();
		tree.node = Some(node);
		tree
	}

<%ParseNonTerminals%>

}
