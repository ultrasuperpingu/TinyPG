# Automatically generated from source file: <%SourceFilename%>
# By TinyPG v<%GeneratorVersion%> available at https://github.com/ultrasuperpingu/TinyPG

from .scanner import Token, TokenType

<%HeaderCode%>

#namespace <%Namespace%>


class ParseError:
	#private string message;
	#private int code;
	#private int line;
	#private int col;
	#private int pos;
	#private int length;
	#private bool isWarning;
	#
	#public int Code { get { return code; } }
	#public int Line { get { return line; } }
	#public int Column { get { return col; } }
	#public int Position { get { return pos; } }
	#public int Length { get { return length; } }
	#public string Message { get { return message; } }
	#public bool IsWarning { get { return isWarning; } }
	#
	## just for the sake of serialization
	#def ParseError()
	#{
	#}
	#
	#public ParseError(string message, int code, ParseNode node, bool isWarning = false) : this(message, code, node.Token)
	#{
	#}
	#
	def FromToken(message, code, token, isWarning = False):
		return ParseError(message, code, token.Line, token.Column, token.StartPos, token.Length(), isWarning);

	#public ParseError(string message, int code, bool isWarning = false) : this(message, code, 0, 0, 0, 0, isWarning)
	#{
	#}

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
	
		for n in node.Nodes:
			self.PrintNode(sb, n, indent + 2);
		return sb;

	"""This is the entry point for executing and evaluating the parse tree."""
	""" <param name="paramlist">additional optional input parameters</param>
		<returns>the output of the evaluation function</returns>"""
	def Eval(self, paramlist):
		return self.Nodes[0].EvalNode(self, paramlist);

