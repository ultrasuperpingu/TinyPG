from simple_expr import Scanner, Parser;

scanner = Scanner();
parser = Parser(scanner);
input = "5*3+12/(1+2*test)";
tree=parser.Parse(input, None);
print(tree)

error=False;
for e in tree.Errors:
	print(e.Message);
	error = True;
if error:
	exit();
context = {};
context["test"]=2;
print("{} = {}".format(input, tree.Eval([context])));
