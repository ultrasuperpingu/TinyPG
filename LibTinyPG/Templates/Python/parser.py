# Automatically generated from source file: <%SourceFilename%>
# By TinyPG v<%GeneratorVersion%> available at https://github.com/ultrasuperpingu/TinyPG

from .parse_tree import ParseTree, ParseError
from .scanner import Token, TokenType

#namespace <%Namespace%>
class Parser:
	def __init__(self, scanner):
		self.scanner = scanner;

	def Parse(self, input, tree): # -> ParseTree
		if tree == None:
			tree = ParseTree();
		self.scanner.Init(input);

		self.tree = tree;
		self.ParseStart(tree);
		tree.Skipped = self.scanner.Skipped;

		return tree;

<%ParseNonTerminals%>

