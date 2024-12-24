// Automatically generated from source file: <%SourceFilename%>
// By TinyPG v<%GeneratorVersion%> available at https://github.com/ultrasuperpingu/TinyPG

use fancy_regex::Regex;
use std::collections::HashMap;

#[derive(Debug)]
pub struct Scanner
{
	pub input: String,
	pub startpos : i32,
	pub endpos : i32,
	pub currentline : i32,
	pub currentcolumn : i32,
	pub currentposition : i32,
	pub skipped : Vec<Token>, // tokens that were skipped
	pub patterns : HashMap<TokenType, Regex>,

	look_ahead_token : Option<Token>,
	tokens : Vec<TokenType>,
	skip_list : Vec<TokenType> // tokens to be skipped
}
impl Default for Scanner {
	fn default() -> Self {
		Self::new()
	}
}
impl Scanner {
	pub fn new() -> Self
	{
		let mut regex : Regex;
		let mut ret=Self {
			input: String::from(""),
			startpos : 0,
			endpos : 0,
			currentline : 0,
			currentcolumn : 0,
			currentposition : 0,
			skipped : Vec::new(), // tokens that were skipped
			patterns : HashMap::new(),
			look_ahead_token : None,
			tokens : Vec::new(),
			skip_list : Vec::new() // tokens to be skipped
		};
<%SkipList%>

<%RegExps%>

		ret
	}


	pub fn init(&mut self, input : &str)
	{
		self.input = input.to_owned();
		self.startpos = 0;
		self.endpos = 0;
		self.currentline = 1;
		self.currentcolumn = 1;
		self.currentposition = 0;
		self.look_ahead_token = None;
	}

	pub fn get_token(&self, _type : TokenType) -> Token
	{
		let mut t = Token::new_with_start_end(self.startpos, self.endpos);
		t._type = _type;
		t
	}

	/// <summary>
	/// executes a lookahead of the next token
	/// and will advance the scan on the input string
	/// </summary>
	/// <returns></returns>
	pub fn scan(&mut self, expectedtokens: Vec<TokenType>) -> Token
	{
		let tok = self.look_ahead(expectedtokens); // temporarely retrieve the lookahead
		self.startpos = tok.endpos;
		self.endpos = tok.endpos; // set the tokenizer to the new scan position
		self.currentline = tok.line + (tok.text.len() - tok.text.replace("\n", "").len()) as i32;
		self.look_ahead_token.take().unwrap() // reset lookahead token, so scanning will continue
	}

	/// <summary>
	/// returns token with longest best match
	/// </summary>
	/// <returns></returns>
	pub fn look_ahead(&mut self,expectedtokens : Vec<TokenType>) -> Token
	{
		let mut startpos = self.startpos;
		let mut endpos = self.endpos;
		let mut currentline = self.currentline;
		let mut scantokens : Vec<TokenType>;
		let mut tok : Token;

		// this prevents double scanning and matching
		// increased performance
		// TODO: check this, what if the expected token are different since last call?
		// Check at least that LookAheadToken is part of the expected tokens
		if self.look_ahead_token.is_some()
		{
			let token = self.look_ahead_token.clone().unwrap();
			if token._type != TokenType::_UNDETERMINED_
				&& token._type != TokenType::_NONE_ {
				return token;
			}
		}

		// if no scantokens specified, then scan for all of them (= backward compatible)
		if expectedtokens.is_empty()
		{
			scantokens = self.tokens.clone();
		}
		else
		{
			scantokens = expectedtokens.clone();
			scantokens.append(&mut self.skip_list.clone());
		}

		loop
		{
			let mut len = -1;
			let mut index : TokenType = TokenType::_END_;
			//string input = Input.Substring(startpos);

			tok = Token::new_with_start_end(startpos, endpos);

			for scantoken in &scantokens
			{
				let r = &self.patterns[scantoken];
				//let m = r.match(Input, startpos);
				//if (m.Success && m.Index == startpos && ((m.Length > len) || (scantokens[i] < index && m.Length == len)))
				if let Ok(Some(caps)) = r.captures(&self.input[startpos as usize..]) 
				{
					//if (m.Index == startpos && ((m.Length > len) || (scantokens[i] < index && m.Length == len)))
					{
						len = caps[0].len() as i32;
						index = *scantoken;
					}
				}
			}

			if index >= TokenType::_NONE_ && len >= 0
			{
				tok.endpos = startpos + len;
				tok.text = self.input[tok.startpos as usize..(tok.startpos+len) as usize].to_string();
				tok._type = index;
			}
			else if tok.startpos == tok.endpos
			{
				if tok.startpos < self.input.len() as i32 {
					tok.text = self.input[tok.startpos as usize..tok.startpos as usize+1].to_string();
				} else {
					tok.text = "EOF".to_string();
				}
			}

			// Update the line and column count for error reporting.
			tok.line = currentline;
			if tok.startpos < self.input.len() as i32 {
				tok.column = tok.startpos - self.input[tok.startpos as usize..].to_string().rfind('\n').unwrap_or_default() as i32;
			}
			if self.skip_list.contains(&tok._type)
			{
				startpos = tok.endpos;
				endpos = tok.endpos;
				currentline = tok.line + (tok.text.len() - tok.text.replace("\n", "").len()) as i32;
				self.skipped.push(tok.clone());
			}
			else
			{
				// only assign to non-skipped tokens
				tok.skipped = self.skipped.clone(); // assign prior skips to this token
				self.skipped = Vec::new (); //reset skips
			}
			if !self.skip_list.contains(&tok._type) {
				break;
			}
		}
		

		self.look_ahead_token = Some(tok);
		self.look_ahead_token.clone().unwrap()
	}
}



#[derive(PartialEq, Eq, Hash, Clone, Debug, Copy, PartialOrd)]
#[allow(clippy::upper_case_acronyms)]
pub enum TokenType
{
<%TokenType%>

	//End
	_END_ = 10000000
}

#[derive(Debug, Clone)]
pub struct Token
{
	pub line : i32,
	pub column : i32,
	pub startpos : i32,
	pub endpos : i32,
	pub text : String,

	// contains all prior skipped symbols
	pub skipped : Vec<Token>,
	pub _type : TokenType
}
impl Default for Token {
	fn default() -> Self {
		Self::new()
	}
}

impl Token {
	pub fn new() -> Self
	{
		Self::new_with_start_end(0,0)
	}

	pub fn new_with_start_end(start: i32, end : i32) -> Self
	{
		Self {
			_type : TokenType::_UNDETERMINED_,
			startpos : start,
			endpos : end,
			text : String::from(""),
			line: 1,
			column: 1,
			skipped: vec![],
		}
	}

	pub fn update_range(&mut self, token:&Token)
	{
		if token.startpos < self.startpos {
			self.startpos = token.startpos;
		}
		if token.endpos > self.endpos {
			self.endpos = token.endpos;
		}
	}

}
impl std::fmt::Display for Token {
	fn fmt(&self, f: &mut std::fmt::Formatter) -> std::fmt::Result {
//		if (Text != null)
//			return Type.ToString() + " '" + Text + "'";
//		else
//			return Type.ToString();
		write!(f, "{:?} '{}'", self._type, self.text)
	}
}


