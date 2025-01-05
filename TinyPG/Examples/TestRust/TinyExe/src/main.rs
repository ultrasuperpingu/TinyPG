use std::any::Any;

use context::Context;
use parse_tree::IParserTree;
use parser::Parser;
use scanner::Scanner;
use utils::convert_to_string;

mod parse_tree;
mod parser;
mod scanner;
mod utils;
mod context;
mod functions;

fn main()
{
	let mut p=Parser::new(Scanner::new());
	let tree= p.parse("12*x*x+5*x-15");
	println!("{}", tree.print_tree());
	let context = Box::new(Context::new());
	println!("{:?}", convert_to_string(tree.eval(&mut vec![context])));
}