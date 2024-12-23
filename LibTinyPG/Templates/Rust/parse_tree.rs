// Automatically generated from source file: <%SourceFilename%>
// By TinyPG v<%GeneratorVersion%> available at https://github.com/ultrasuperpingu/TinyPG


<%HeaderCode%>

use crate::scanner::{Token, TokenType};

pub struct ParseError
{
	pub message : String,
	pub code : i32,
	pub line : i32,
	pub col : i32,
	pub pos : i32,
	pub length : i32,
	pub is_warning : bool
}
impl Default for ParseError {
	fn default() -> Self {
		Self::new()
	}
}
impl ParseError {
	pub fn new() -> Self
	{
		Self { message: "".to_string(), code: -1, line: 1, col: 1, pos: 0, length: 0, is_warning: false }
	}

	pub fn new2(message: String, code: i32, node: ParseNode, is_warning : bool) -> Self
	{
		Self::new3(message, code, node.token, is_warning)
	}

	pub fn new3(message:String, code:i32, token:Token, is_warning:bool) -> Self
	{
		Self::new5( message, code, token.line, token.column, token.startpos, token.text.len() as i32, is_warning)
	}

	pub fn new4(message:String, code:i32, is_warning:bool) -> Self
	{
		Self::new5( message, code, 0, 0, 0, 0, is_warning)
	}

	pub fn new5(message:String, code:i32, line:i32, col:i32, pos:i32, length:i32, is_warning:bool) -> Self
	{
		Self { message, code, line, col, pos, length, is_warning }
	}
}

// rootlevel of the node tree
pub struct ParseTree //: ParseNode
{
	pub node:Option<Box<dyn IParseNode>>,
	pub errors:Vec<ParseError>,
	pub skipped:Vec<Token>,
}
impl Default for ParseTree {
	fn default() -> Self {
		Self::new()
	}
}
impl ParseTree {

	pub fn new() -> Self
	{
		Self {
			node:Some(Box::new(ParseNode { text: "Root".to_string(), nodes: vec![], token: Token::new() })),
			errors : vec![],
			skipped:vec![]
		}
	}

	pub fn print_tree(&self) -> String
	{
		Self::print_node(self.node.as_ref().unwrap(), 0)
	}

	fn print_node(node:&Box<dyn IParseNode>, indent:usize) -> String
	{
		let mut content = "".to_string();
		for _i in 0..indent {
			content+=" ";
		}
		content += node.get_text().as_str();
		for n in node.get_nodes() {
			content+="\r\n";
			content+=Self::print_node(n, indent + 2).as_str();
		}
		content
	}
	/*
	/// <summary>
	/// this is the entry point for executing and evaluating the parse tree.
	/// </summary>
	/// <param name="paramlist">additional optional input parameters</param>
	/// <returns>the output of the evaluation function</returns>
	pub fn Eval(&self, paramlist:Vec<object>) -> object
	{
		self.node.nodes[0].EvalNode(self.node, paramlist)
	}*/
}
pub trait IParserTree : IParseNode {
}
pub trait IParseNode {
	fn create_node(&self, token:Token, text:String) -> Box<dyn IParseNode>;
	fn get_token_node(&self, _type:TokenType, index:i32) -> Option<&dyn IParseNode>;
	fn is_token_present(&self, _type:TokenType, index: i32) -> bool;
	fn get_terminal_value(&self, _type:TokenType, index: i32) -> String;
	fn get_token(&self) -> &Token;
	fn get_token_mut(&mut self) -> &mut Token;
	fn get_text(&self) -> &String;
	fn add_node(&mut self, node:Box<dyn IParseNode>);
	fn get_nodes(&self) -> &Vec<Box<dyn IParseNode>>;

<%VirtualEvalMethodsDecl%>
}
pub struct ParseNode
{
	pub text:String,
	pub nodes:Vec<Box<dyn IParseNode>>,
	pub token:Token, // the token/rule
}

impl IParseNode for ParseNode {

	fn create_node(&self, token:Token, text:String) -> Box<dyn IParseNode>
	{
		let node = ParseNode::new(token, text);
		Box::new(node)
	}

	fn get_token_node(&self, _type:TokenType, mut index:i32) -> Option<&dyn IParseNode>
	{
		if index < 0 {
			return None;
		}
		// left to right
		for node in &self.nodes
		{
			if node.get_token()._type == _type
			{
				index-=1;
				if index < 0
				{
					return Some(node.as_ref());
				}
			}
		}
		None
	}

	fn is_token_present(&self, _type:TokenType, index: i32) -> bool
	{
		let node = self.get_token_node(_type, index);
		node.is_some()
	}

	fn get_terminal_value(&self, _type:TokenType, index: i32) -> String
	{
		let node = self.get_token_node(_type, index);
		if let Some(n) = &node {
			return n.get_token().text.clone();
		}
		"".to_string()
	}
	fn get_token(&self) -> &Token
	{
		&self.token
	}
	fn get_token_mut(&mut self) -> &mut Token
	{
		&mut self.token
	}
	fn add_node(&mut self, node:Box<dyn IParseNode>)
	{
		self.nodes.push(node);
	}
	fn get_text(&self) -> &String
	{
		&self.text
	}
	fn get_nodes(&self) -> &Vec<Box<dyn IParseNode>>
	{
		&self.nodes
	}
	/*fn GetValue(&self, _type:TokenType, index:i32/*, paramlist: Vec<object>*/) -> object
	{
		let mut index2 = index;
		return self.GetValue2(_type, &mut index2/*, paramlist*/);
	}

	fn GetValue2(&self, _type:TokenType, index:&mut i32/*, paramlist: Vec<object>*/) -> object
	{
		object o = null;
		if *index < 0 {
			return o;
		}

		// left to right
		for node in self.nodes
		{
			if (node.Token.Type == _type)
			{
				index-=1;
				if (*index < 0)
				{
					o = node.EvalNode(paramlist);
					break;
				}
			}
		}
		return o;
	}*/


	/*
	/// <summary>
	/// this implements the evaluation functionality, cannot be used directly
	/// </summary>
	/// <param name="tree">the parsetree itself</param>
	/// <param name="paramlist">optional input parameters</param>
	/// <returns>a partial result of the evaluation</returns>
	fn EvalNode(&self, paramlist: Vec<object>) -> object
	{
		let Value:object = None;

		match self.Token._type
		{
				TokenType::Start=> {
					Value = self.EvalStart(/*paramlist*/);
				},
				TokenType::AddExpr=> {
					Value = self.EvalAddExpr(/*paramlist*/);
				},
				TokenType::MultExpr=> {
					Value = self.EvalMultExpr(/*paramlist*/);
				},
				TokenType::Atom=> {
					Value = self.EvalAtom(/*paramlist*/);
				},

			_ =>{
				Value = self.Token.text;
			}
		}
		Value
	}*/


<%VirtualEvalMethods%>

<%CustomCode%>
}

impl ParseNode {
	pub fn new(token:Token, text:String) -> Self
	{
		Self {
			token,
			text,
			nodes : vec![],
		}
	}
}