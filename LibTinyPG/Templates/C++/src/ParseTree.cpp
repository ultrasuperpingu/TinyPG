// Automatically generated from source file: <%SourceFilename%>
// By TinyPG v<%GeneratorVersion%> available at https://github.com/ultrasuperpingu/TinyPG

#include "ParseTree.h"

namespace <%Namespace%>
{
	// ParseNode implementations
	ParseNode* ParseNode::CreateNode(Token token, std::string text)
	{
		ParseNode* node = new ParseNode(token, text);
		node->Parent = this;
		return node;
	}

	ParseNode::ParseNode(Token token, const std::string& text)
	{
		this->TokenVal = token;
		this->Text = text;
	}

	ParseNode::~ParseNode()
	{
		for (ParseNode* node : Nodes)
		{
			delete node;
		}
	}

	ParseNode* ParseNode::GetTokenNode(TokenType type, int index)
	{
		if (index < 0)
			return NULL;
		// left to right
		for (ParseNode* node : Nodes)
		{
			if (node->TokenVal.Type == type)
			{
				index--;
				if (index < 0)
				{
					return node;
				}
			}
		}
		return NULL;
	}

	bool ParseNode::IsTokenPresent(TokenType type, int index)
	{
		ParseNode* node = GetTokenNode(type, index);
		return node != NULL;
	}

	std::string ParseNode::GetTerminalValue(TokenType type, int index)
	{
		ParseNode* node = GetTokenNode(type, index);
		if (node != NULL)
			return node->TokenVal.Text;
		return "";
	}

	std::any ParseNode::GetValue(TokenType type, int index, std::vector<std::any> paramlist)
	{
		return GetValueRefIndex(type, index, paramlist);
	}

	std::any ParseNode::GetValueRefIndex(TokenType type, int& index, std::vector<std::any> paramlist)
	{
		std::any o;
		if (index < 0) return o;

		// left to right
		for (ParseNode* node : Nodes)
		{
			if (node->TokenVal.Type == type)
			{
				index--;
				if (index < 0)
				{
					o = node->Eval(paramlist);
					break;
				}
			}
		}
		return o;
	}

	std::any ParseNode::Eval(std::vector<std::any> paramlist)
	{
		std::any Value;

		switch (TokenVal.Type)
		{
<%EvalSymbols%>
		default:
			Value = &TokenVal.Text;
			break;
		}
		return Value;
	}

<%VirtualEvalMethodsImpl%>

	ParseTree::ParseTree() : ParseNode(Token(), "ParseTree")
	{
		TokenVal.Type = TokenType::Start;
		TokenVal.Text = "Root";
	}

	ParseTree::~ParseTree()
	{
		// Cleanup Children done in parent class
	}

	std::string ParseTree::PrintTree()
	{
		std::string sb;
		int indent = 0;
		PrintNode(sb, this, indent);
		return sb;
	}

	void ParseTree::PrintNode(std::string& sb, ParseNode* node, int indent)
	{

		std::string space = "";
		space.insert(0, indent, ' ');

		sb += space;
		sb += node->Text + "\n";

		for (ParseNode* n : node->Nodes)
			PrintNode(sb, n, indent + 2);
	}

	ParseError::ParseError() : ParseError("", 0, 0, 0, 0, 0, false)
	{
	}

	ParseError::ParseError(std::string message, int code, ParseNode& node, bool isWarning) : ParseError(message, code, node.TokenVal, isWarning)
	{
	}

	ParseError::ParseError(std::string message, int code, Token token, bool isWarning) : ParseError(message, code, token.Line, token.Column, token.StartPos, token.Length, isWarning)
	{
	}

	ParseError::ParseError(std::string message, int code, bool isWarning) : ParseError(message, code, 0, 0, 0, 0, isWarning)
	{
	}

	ParseError::ParseError(std::string message, int code, int line, int col, int pos, int length, bool isWarning)
	{
		this->Message = message;
		this->Code = code;
		this->Line = line;
		this->Column = col;
		this->Position = pos;
		this->Length = length;
		this->IsWarning = isWarning;
	}
}
