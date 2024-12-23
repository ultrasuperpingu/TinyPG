#![forbid(unsafe_code)]
use std::collections::HashMap;
use parser::Parser;
use scanner::Scanner;
pub mod scanner;
pub mod parser;
pub mod parse_tree;
//mod Scanner;
//mod Parser;
fn main() {
	let s = Scanner::new();
	let mut p = Parser::new(s);
	let tree = p.Parse("(_5 + 3) / _15 * (3 - 2)".to_string());
	println!("{}", tree.PrintTree());
	for e in &tree.Errors {
		println!("{}", e.message);
	}
	//println!("{:?}", tree.node.unwrap().get_nodes()[0].get_nodes().len());
	let mut context: HashMap<String, i32> = HashMap::new();
	context.insert("_5".to_string(), 5);
	context.insert("_15".to_string(), 15);
	//tree.setContext(&context);
	println!("{}", tree.node.unwrap().get_nodes()[0].EvalStart());
}

