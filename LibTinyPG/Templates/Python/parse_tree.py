# Automatically generated from source file: <%SourceFilename%>
# By TinyPG v<%GeneratorVersion%> available at https://github.com/ultrasuperpingu/TinyPG

from .scanner import Token, TokenType

<%HeaderCode%>

#namespace <%Namespace%>


class ParseError:

	def FromToken(message, code, token, isWarning = False):
		return ParseError(message, code, token.Line, token.Column, token.StartPos, token.Length(), isWarning);

	def __init__(self, message, code, line, col, pos, length, isWarning = False):
		self.Message = message;
		self.Code = code;
		self.Line = line;
		self.Column = col;
		self.Position = pos;
		self.Length = length;
		self.IsWarning = isWarning;


class ParseNode:

	def CreateNode(self, token, text):
		node = ParseNode(token, text);
		node.Parent = self;
		return node;

	def __init__(self, token, text):
		self.Token = token;
		self.Text = text;
		self.Nodes = [];
		self.Parent = None;

	def GetTokenNode(self, type, index):
		if index < 0:
			return None;
		# left to right
		for node in self.Nodes:
			if node.Token.Type == type:
				index-=1;
				if index < 0:
					return node;
		return None;

	def IsTokenPresent(self, type, index): # -> bool
		node = self.GetTokenNode(type, index);
		return node != None;

	def GetTerminalValue(self, type, index): # -> string 
		node = self.GetTokenNode(type, index);
		if (node != None):
			return node.Token.Text;
		return None;

	#def GetValue(self, type, index, paramlist):
	#	return GetValue(type, ref index, paramlist);
	#
	#def GetValue(self, type, ref index, paramlist)
	#	object o = null;
	#	if (index < 0):
	#		return o;
	#
	#	# left to right
	#	for node in nodes:
	#		if (node.Token.Type == type):
	#			index--;
	#			if (index < 0):
	#				o = node.EvalNode(paramlist);
	#				break;
	#	return o;

	"""This implements the evaluation functionality, cannot be used directly
	<param name="tree">the parsetree itself</param>
	<param name="paramlist">optional input parameters</param>
	<returns>a partial result of the evaluation</returns>"""
	def EvalNode(self, paramlist):
		Value = None;

<%EvalSymbols%>
		else:
			Value = self.Token.Text;
		return Value;

<%VirtualEvalMethods%>

<%CustomCode%>


# rootlevel of the node tree
class ParseTree(ParseNode):

	def __init__(self):
		ParseNode.__init__(self, Token(0,0), "ParseTree");
		self.Token.Type = TokenType.Start;
		self.Token.Text = "Root";
		self.Errors = [];
		self.Skipped = [];

	def __repr__(self):
		res="";
		indent = 0;
		res=self.PrintNode(res, self, indent);
		return res;

	def PrintNode(self, sb, node, indent):
		space = " " * indent;

		sb+=space;
		sb+=node.Text;
		sb+="\n";

		for n in node.Nodes:
			sb=self.PrintNode(sb, n, indent + 2);
		return sb;

	"""This is the entry point for executing and evaluating the parse tree."""
	""" <param name="paramlist">additional optional input parameters</param>
		<returns>the output of the evaluation function</returns>"""
	def Eval(self, paramlist):
		params=[self]+paramlist;
		return self.Nodes[0].EvalNode(params);

