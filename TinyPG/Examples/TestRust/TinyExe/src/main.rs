use std::any::Any;

use parser::Parser;
use scanner::Scanner;
use utils::ConvertToString;

mod parse_tree;
mod parser;
mod scanner;
mod utils;

fn main()
{
	let mut p=Parser::new(Scanner::new());
	//let mut tree= p.parse("f(x) = 12*x*x+5*x-15");
	let mut tree= p.parse("12*x*x+5*x-15");
	println!("{}", tree.print_tree());
	let mut paramlist: Vec<Box<dyn Any>> = vec![];
	//paramlist.push(Box::new(&mut tree));
	println!("{:?}", ConvertToString(tree.root.as_ref().unwrap().get_nodes()[0].eval_start(&mut paramlist)));
}