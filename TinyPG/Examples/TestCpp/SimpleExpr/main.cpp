#include <iostream>
#include "Parser.h"

void printNode(TinyPG::ParseNode* n, int indent=1)
{
	for (int i = 0; i < indent; i++)
		std::cerr << "\t";
	std::cerr << "node: " << n->Text <<" --- " << n->TokenVal.Text << std::endl;
	for (auto node : n->Nodes)
	{
		printNode(node, indent+1);
	}

}
int main()
{
	TinyPG::Scanner s = TinyPG::Scanner();
	TinyPG::Parser p = TinyPG::Parser(s);
	auto tree = p.Parse("(_5 + 3) + _15 / (4 - 2)");
	std::map<std::string, int> context;
	context.insert(std::pair<std::string, int>("_5", 5));
	context.insert(std::pair<std::string, int>("_15", 15));
	tree.setContext(&context);
	//auto tree = p.Parse("1 == 5 / $i_toto$");
	std::cerr << "errors: " << tree.Errors.size() << std::endl;
	for (int i = 0; i < tree.Errors.size(); i++)
	{
		std::cerr << "\terror: " << tree.Errors[i].Line << ","<< tree.Errors[i].Column<< " : " << tree.Errors[i].Message << std::endl;
	}
	std::cerr << "nodes: " << tree.Nodes.size() << std::endl;
	for (auto n : tree.Nodes)
	{
		printNode(n);
	}
	std::string sb;
	tree.PrintNode(sb, &tree, 0);
	std::cerr << sb << std::endl;
	std::cerr << tree.Nodes[0]->EvalStart(tree, {}) << std::endl;
	return 0;
}