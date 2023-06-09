// Automatically generated from source file: <%SourceFilename%>
// By TinyPG v1.5 available at https://github.com/ultrasuperpingu/TinyPG

#pragma once
#include <string>
#include <vector>
#include <any>
#include "Scanner.h"
<%HeaderCode%>

namespace <%Namespace%>
{
	class ParseTree;
	class ParseNode<%IParseNode%>
	{
		friend class ParseTree;
	protected:
		<%ITokenGet%>
	public:
		std::vector<ParseNode*> Nodes;
		<%INodesGet%>
		//[XmlIgnore] // avoid circular references when serializing
		ParseNode* Parent;
		Token TokenVal; // the token/rule

		std::string Text;

		inline virtual ParseNode* CreateNode(Token token, std::string text);

	protected:
		ParseNode(Token token, const std::string& text);
		inline virtual ~ParseNode();

		virtual ParseNode* GetTokenNode(TokenType type, int index);
		virtual bool IsTokenPresent(TokenType type, int index);

		virtual std::string GetTerminalValue(TokenType type, int index);
		std::any GetValue(TokenType type, int index, std::vector<std::any> paramlist);
		std::any GetValueRefIndex(TokenType type, int& index, std::vector<std::any> paramlist);

	public:
		/// <summary>
		/// this implements the evaluation functionality, cannot be used directly
		/// </summary>
		/// <param name="tree">the parsetree itself</param>
		/// <param name="paramlist">optional input parameters</param>
		/// <returns>a partial result of the evaluation</returns>
		std::any Eval(std::vector<std::any> paramlist);

		protected:
<%VirtualEvalMethods%>

<%ParseTreeCustomCode%>
	};
	
	class ParseError<%ParseError%>
	{
		public:
		std::string File;
		std::string Message;
		int Code;
		int Line;
		int Column;
		int Position;
		int Length;
		bool IsWarning;

		// just for the sake of serialization
		ParseError();
		ParseError(std::string message, int code, ParseNode& node, bool isWarning = false);
		ParseError(std::string message, int code, Token token, bool isWarning = false);
		ParseError(std::string message, int code, bool isWarning = false);
		ParseError(std::string message, int code, std::string file, int line, int col, int pos, int length, bool isWarning = false);

	};
	
	class ParseErrors : public <%ParseErrors%>
	{
	};

	
	// rootlevel of the node tree
	class ParseTree : public ParseNode<%IParseTree%>
	{
	public:
		ParseErrors Errors;

		std::vector<Token> Skipped;

		inline ParseTree();
		
		virtual inline ~ParseTree();

		inline std::string PrintTree();

		inline void PrintNode(std::string& sb, ParseNode* node, int indent);

		/// <summary>
		/// this is the entry point for executing and evaluating the parse tree.
		/// </summary>
		/// <param name="paramlist">additional optional input parameters</param>
		/// <returns>the output of the evaluation function</returns>
		// TODO: template the class (not the method) and/or use std::any
		template<typename T>
		inline T Eval(const std::vector<std::any>& paramlist = {});
	};

	// ParseNode implementations
	inline ParseNode* ParseNode::CreateNode(Token token, std::string text)
	{
		ParseNode* node = new ParseNode(token, text);
		node->Parent = this;
		return node;
	}

	inline ParseNode::ParseNode(Token token, const std::string& text)
	{
		this->TokenVal = token;
		this->Text = text;
		//this->nodes = new List<ParseNode>();
	}

	inline ParseNode::~ParseNode()
	{
		for (ParseNode* node : Nodes)
		{
			delete node;
		}
	}

	inline ParseNode* ParseNode::GetTokenNode(TokenType type, int index)
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
	inline bool ParseNode::IsTokenPresent(TokenType type, int index)
	{
		ParseNode* node = GetTokenNode(type, index);
		return node != NULL;
	}

	inline std::string ParseNode::GetTerminalValue(TokenType type, int index)
	{
		ParseNode* node = GetTokenNode(type, index);
		if (node != NULL)
			return node->TokenVal.Text;
		return "";
	}

	inline std::any ParseNode::GetValue(TokenType type, int index, std::vector<std::any> paramlist)
	{
		GetValueRefIndex(type, index, paramlist);
	}

	inline std::any ParseNode::GetValueRefIndex(TokenType type, int& index, std::vector<std::any> paramlist)
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

	inline std::any ParseNode::Eval(std::vector<std::any> paramlist)
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

	inline ParseTree::ParseTree() : ParseNode<%IParseTree%>(Token(), "ParseTree")
	{
		TokenVal.Type = TokenType::Start;
		TokenVal.Text = "Root";
		//Errors = new ParseErrors();
	}

	inline ParseTree::~ParseTree()
	{
		// Cleanup Children done in parent class
	}

	inline std::string ParseTree::PrintTree()
	{
		std::string sb;
		int indent = 0;
		PrintNode(sb, this, indent);
		return sb;
	}

	inline void ParseTree::PrintNode(std::string& sb, ParseNode* node, int indent)
	{

		std::string space = "";
		space.insert(0, indent, ' ');

		sb += space;
		sb += node->Text + "\n";

		for (ParseNode* n : node->Nodes)
			PrintNode(sb, n, indent + 2);
	}

	template<typename T>
	inline T ParseTree::Eval(const std::vector<std::any>& paramlist)
	{
		return Nodes[0]->EvalStart(paramlist);
	}

	inline ParseError::ParseError() : ParseError("", 0, "", 0, 0, 0, 0, false)
	{
	}

	inline ParseError::ParseError(std::string message, int code, ParseNode& node, bool isWarning) : ParseError(message, code, node.TokenVal, isWarning)
	{
	}

	inline ParseError::ParseError(std::string message, int code, Token token, bool isWarning) : ParseError(message, code, token.File, token.Line, token.Column, token.StartPos, token.Length, isWarning)
	{
	}

	inline ParseError::ParseError(std::string message, int code, bool isWarning) : ParseError(message, code, "", 0, 0, 0, 0, isWarning)
	{
	}

	inline ParseError::ParseError(std::string message, int code, std::string file, int line, int col, int pos, int length, bool isWarning)
	{
		this->File = file;
		this->Message = message;
		this->Code = code;
		this->Line = line;
		this->Column = col;
		this->Position = pos;
		this->Length = length;
		this->IsWarning = isWarning;
	}
}
