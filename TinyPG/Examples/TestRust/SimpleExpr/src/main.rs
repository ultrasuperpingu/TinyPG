#![forbid(unsafe_code)]
use std::collections::HashMap;
use parser::Parser;
use scanner::Scanner;
use parse_tree::IParserTree;
pub mod scanner;
pub mod parser;
pub mod parse_tree;

fn main() {
	let s = Scanner::new();
	let mut p = Parser::new(s);
	let input = "5*3+12/(1+2*test)";
	let tree = p.parse(input);
	println!("{}", tree.print_tree());
	let mut error=false;
	for e in &tree.errors {
		println!("{}", e.message);
		error = true;
	}
	if error {
		return;
	}
	let mut context: HashMap<String, i32> = HashMap::new();
	context.insert("test".to_string(), 2);
	println!("{} = {:?}", input, tree.eval(&mut vec![Box::new(context)]).unwrap().downcast::<i32>().unwrap());
}


