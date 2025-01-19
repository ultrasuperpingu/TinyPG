# Automatically generated from source file: <%SourceFilename%>
# By TinyPG v<%GeneratorVersion%> available at https://github.com/ultrasuperpingu/TinyPG


#namespace <%Namespace%>
from enum import IntEnum
import re

class Scanner:

	def __init__(self):
		self.Input = None;
		self.StartPos = 0;
		self.EndPos = 0;
		self.CurrentLine = 1;
		self.CurrentColumn = 1;
		self.CurrentPosition = 0;
		self.Skipped = [];
		self.Patterns = {};
		self.LookAheadToken = None;
		self.Tokens = [];
		self.Skipped = {};
		self.SkipList = [];

<%SkipList%>
<%RegExps%>

	def Init(self, input):
		self.Input = input;
		self.StartPos = 0;
		self.EndPos = 0;
		self.CurrentLine = 1;
		self.CurrentColumn = 1;
		self.CurrentPosition = 0;
		self.Skipped = [];
		self.LookAheadToken = None;

	def GetToken(self, type): # -> Token
		t = Token(self.StartPos, self.EndPos);
		t.Type = type;
		return t;

	"""Executes a lookahead of the next token
	and will advance the scan on the input string"""
	def Scan(self, expectedtokens): # -> Token
		tok = self.LookAhead(expectedtokens); # temporarely retrieve the lookahead
		self.LookAheadToken = None; # reset lookahead token, so scanning will continue
		self.StartPos = tok.EndPos;
		self.EndPos = tok.EndPos; # set the tokenizer to the new scan position
		self.CurrentLine = tok.Line + (len(tok.Text) - len(tok.Text.replace("\n", "")));
		return tok;

	"""Returns token with longest best match"""
	def LookAhead(self, expectedtokens): #-> Token
		startpos = self.StartPos;
		endpos = self.EndPos;
		currentline = self.CurrentLine;
		tok = None;
		scantokens = [];


		# this prevents double scanning and matching
		# increased performance
		# TODO: check this, what if the expected token are different since last call?
		# Check at least that LookAheadToken is part of the expected tokens
		if self.LookAheadToken != None \
			and self.LookAheadToken.Type != TokenType._UNDETERMINED_ \
			and self.LookAheadToken.Type != TokenType._NONE_:
			return self.LookAheadToken;

		# if no scantokens specified, then scan for all of them (= backward compatible)
		if len(expectedtokens) == 0:
			scantokens = self.Tokens;
		else:
			scantokens = [];
			scantokens += self.SkipList;

		while True:
			length = -1;
			index = TokenType.UNDETERMINED_;
			#string input = Input.Substring(startpos);

			tok = Token(startpos, endpos);

			for i in range(len(scantokens)):
				r = self.Patterns[scantokens[i]];
				m = r.match(self.Input[startpos:]);
				#if (m != None and m.Index == 0 and ((m.Length > length) or (scantokens[i] < index and m.Length == length))):
				if m != None:
					mlength=len(m[0]);
					if mlength > length or (scantokens[i] < index and mlength == length):
						length = mlength;
						index = scantokens[i];

			if index >= 0 and length >= 0:
				tok.EndPos = startpos + length;
				tok.Text = self.Input[tok.StartPos:tok.StartPos + length];
				tok.Type = index;
			elif tok.StartPos == tok.EndPos:
				if (tok.StartPos < len(self.Input)):
					tok.Text = self.Input[tok.StartPos:tok.StartPos+1];
				else:
					tok.Text = "EOF";


			# Update the line and column count for error reporting.
			tok.Line = currentline;
			if tok.StartPos < len(self.Input):
				tok.Column = tok.StartPos - self.Input.rfind('\n', tok.StartPos);

			if tok.Type in self.SkipList:
				startpos = tok.EndPos;
				endpos = tok.EndPos;
				currentline = tok.Line + (len(tok.Text) - len(tok.Text.replace("\n", "")));
				self.Skipped = [tok];
			else:
				# only assign to non-skipped tokens
				tok.Skipped = self.Skipped; # assign prior skips to this token
				self.Skipped = []; #reset skips
			if not tok.Type in self.SkipList:
				break

		LookAheadToken = tok;
		return tok;

class TokenType(IntEnum):
<%TokenType%>


class Token:

	def __init__(self, start, end):
		self.Type = TokenType.UNDETERMINED_;
		self.Line = 1;
		self.Column = 1;
		if start != None:
			self.StartPos = start;
		else:
			self.StartPos = 0;
		if end != None:
			self.EndPos = end;
		else:
			self.EndPos = 0;
		self.Text = "";
		self.Skipped = [];

	def UpdateRange(self, token):
		if token.StartPos < self.StartPos:
			self.StartPos = token.StartPos;
		if token.EndPos > self.EndPos:
			self.EndPos = token.EndPos;

	def Length(self): 
		return self.EndPos - self.StartPos;

	def __repr__(self):
		if self.Text != None:
			return str(self.Type) + " '" + self.Text + "'";
		else:
			return str(self.Type);

