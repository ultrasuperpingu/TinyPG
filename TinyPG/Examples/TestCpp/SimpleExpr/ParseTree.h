// Automatically generated from source file: simple expression2_cpp.tpg
// By TinyPG v1.5 available at https://github.com/ultrasuperpingu/TinyPG

#pragma once
#include <string>
#include <vector>
#include <any>
#include "Scanner.h"


namespace TinyPG
{
	class ParseTree;
	class ParseNode
	{
		friend class ParseTree;
		protected:
		
		public:
		std::vector<ParseNode*> Nodes;
		
		//[XmlIgnore] // avoid circular references when serializing
		ParseNode* Parent;
		Token TokenVal; // the token/rule

		std::string Text;

		inline virtual ParseNode* CreateNode(Token token, std::string text)
		{
			ParseNode* node = new ParseNode(token, text);
			node->Parent = this;
			return node;
		}

		protected:
		inline ParseNode(Token token, const std::string& text)
		{
			this->TokenVal = token;
			this->Text = text;
			//this->nodes = new List<ParseNode>();
		}

		inline virtual ~ParseNode()
		{
			for (ParseNode* node : Nodes)
			{
				delete node;
			}
		}

		inline virtual ParseNode* GetTokenNode(TokenType type, int index)
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
		inline virtual bool IsTokenPresent(TokenType type, int index)
		{
			ParseNode* node = GetTokenNode(type, index);
			return node != NULL;
		}
		
		inline virtual std::string GetTerminalValue(TokenType type, int index)
		{
			ParseNode* node = GetTokenNode(type, index);
			if (node != NULL)
				return node->TokenVal.Text;
			return "";
		}

		inline std::any GetValue(TokenType type, int index, std::vector<std::any> paramlist)
		{
			GetValueRefIndex(type, index, paramlist);
		}

		inline std::any GetValueRefIndex(TokenType type, int& index, std::vector<std::any> paramlist)
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

		public:
		/// <summary>
		/// this implements the evaluation functionality, cannot be used directly
		/// </summary>
		/// <param name="tree">the parsetree itself</param>
		/// <param name="paramlist">optional input parameters</param>
		/// <returns>a partial result of the evaluation</returns>
		inline std::any Eval(std::vector<std::any> paramlist)
		{
			std::any Value;

			switch (TokenVal.Type)
			{
				case TokenType::Start:
					Value = EvalStart(paramlist);
					break;
				case TokenType::AddExpr:
					Value = EvalAddExpr(paramlist);
					break;
				case TokenType::MultExpr:
					Value = EvalMultExpr(paramlist);
					break;
				case TokenType::Atom:
					Value = EvalAtom(paramlist);
					break;

				default:
					Value = &TokenVal.Text;
					break;
			}
			return Value;
		}
		protected:
		inline virtual int EvalStart(const std::vector<std::any>& paramlist)
		{
			return this->GetAddExprValue(0, paramlist);
		}

		inline virtual int GetStartValue(int index, const std::vector<std::any>& paramlist)
		{
			ParseNode* node = GetTokenNode(TokenType::Start, index);
			if (node != NULL)
				return node->EvalStart(paramlist);
			throw std::exception("No Start[index] found.");
		}

		inline virtual int EvalAddExpr(const std::vector<std::any>& paramlist)
		{
			int Value = this->GetMultExprValue(0, paramlist);
			int i = 1;
			while (this->IsTokenPresent(TokenType::MultExpr, i))
			{
				std::string sign = this->GetTerminalValue(TokenType::PLUSMINUS, i-1);
				if (sign == "+")
					Value += this->GetMultExprValue(i++, paramlist);
				else 
					Value -= this->GetMultExprValue(i++, paramlist);
			}
		
			return Value;
		}

		inline virtual int GetAddExprValue(int index, const std::vector<std::any>& paramlist)
		{
			ParseNode* node = GetTokenNode(TokenType::AddExpr, index);
			if (node != NULL)
				return node->EvalAddExpr(paramlist);
			throw std::exception("No AddExpr[index] found.");
		}

		inline virtual int EvalMultExpr(const std::vector<std::any>& paramlist)
		{
			int Value = this->GetAtomValue(0, paramlist);
			int i = 1;
			while (this->IsTokenPresent(TokenType::Atom, i))
			{
				std::string sign = this->GetTerminalValue(TokenType::MULTDIV, i-1);
				if (sign == "*")
					Value *= this->GetAtomValue(i++, paramlist);
				else 
					Value /= this->GetAtomValue(i++, paramlist);
			}
			return Value;
		}

		inline virtual int GetMultExprValue(int index, const std::vector<std::any>& paramlist)
		{
			ParseNode* node = GetTokenNode(TokenType::MultExpr, index);
			if (node != NULL)
				return node->EvalMultExpr(paramlist);
			throw std::exception("No MultExpr[index] found.");
		}

		inline virtual int EvalAtom(const std::vector<std::any>& paramlist)
		{
			if (this->IsTokenPresent(TokenType::NUMBER, 0))
				return std::stoi(this->GetTerminalValue(TokenType::NUMBER, 0));
			if (this->IsTokenPresent(TokenType::ID, 0))
				return getVarValue(this->GetTerminalValue(TokenType::ID, 0));
			return this->GetAddExprValue(0, paramlist);
		}

		inline virtual int GetAtomValue(int index, const std::vector<std::any>& paramlist)
		{
			ParseNode* node = GetTokenNode(TokenType::Atom, index);
			if (node != NULL)
				return node->EvalAtom(paramlist);
			throw std::exception("No Atom[index] found.");
		}




	protected:
	std::map<std::string, int>* context;
	public:
	std::map<std::string, int>* getContext() {
		if(context == NULL && Parent != NULL)
		{
			return Parent->getContext();
		}
		return context;
	}

	void setContext(std::map<std::string, int>* value) {
		context = value;
	}

	int getVarValue(std::string id) {
		//TODO: check variable exists
		return getContext() == NULL?0:(*getContext())[id];
	}

	};
	
	class ParseError
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
		inline ParseError() : ParseError("", 0, "", 0, 0, 0, 0, false)
		{
		}

		inline ParseError(std::string message, int code, ParseNode& node, bool isWarning=false) : ParseError(message, code, node.TokenVal, isWarning)
		{
		}

		inline ParseError(std::string message, int code, Token token, bool isWarning=false) : ParseError(message, code, token.File, token.Line, token.Column, token.StartPos, token.Length, isWarning)
		{
		}

		inline ParseError(std::string message, int code, bool isWarning = false) : ParseError(message, code, "", 0, 0, 0, 0, isWarning)
		{
		}

		inline ParseError(std::string message, int code, std::string file, int line, int col, int pos, int length, bool isWarning = false)
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
	};
	
	class ParseErrors : public std::vector<ParseError>
	{
	};

	
	// rootlevel of the node tree
	class ParseTree : public ParseNode
	{
	public:
		ParseErrors Errors;

		std::vector<Token> Skipped;

		inline ParseTree() : ParseNode(Token(), "ParseTree")
		{
			TokenVal.Type = TokenType::Start;
			TokenVal.Text = "Root";
			//Errors = new ParseErrors();
		}
		
		virtual inline ~ParseTree()
		{
			// Cleanup Children done in parent class
		}

		inline std::string PrintTree()
		{
			std::string sb;
			int indent = 0;
			PrintNode(sb, this, indent);
			return sb;
		}

		inline void PrintNode(std::string& sb, ParseNode* node, int indent)
		{

			std::string space = "";
			space.insert(0, indent, ' ');

			sb += space;
			sb += node->Text + "\n";

			for (ParseNode* n : node->Nodes)
				PrintNode(sb, n, indent + 2);
		}

		/// <summary>
		/// this is the entry point for executing and evaluating the parse tree.
		/// </summary>
		/// <param name="paramlist">additional optional input parameters</param>
		/// <returns>the output of the evaluation function</returns>
		// TODO: template the class (not the method) and/or use std::any
		template<typename T>
		inline T Eval(const std::vector<std::any>& paramlist = {})
		{
			return Nodes[0]->EvalStart(paramlist);
		}
	};
 
}
