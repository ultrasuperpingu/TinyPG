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
	public:
		std::vector<ParseNode*> Nodes;

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
		virtual int EvalStart(const std::vector<std::any>& paramlist);
		virtual int GetStartValue(int index, const std::vector<std::any>& paramlist);
		virtual int EvalAddExpr(const std::vector<std::any>& paramlist);
		virtual int GetAddExprValue(int index, const std::vector<std::any>& paramlist);
		virtual int EvalMultExpr(const std::vector<std::any>& paramlist);
		virtual int GetMultExprValue(int index, const std::vector<std::any>& paramlist);
		virtual int EvalAtom(const std::vector<std::any>& paramlist);
		virtual int GetAtomValue(int index, const std::vector<std::any>& paramlist);



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
		ParseError(std::string message, int code, int line, int col, int pos, int length, bool isWarning = false);

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

		ParseTree();
		
		virtual ~ParseTree();

		std::string PrintTree();

		void PrintNode(std::string& sb, ParseNode* node, int indent);

		/// <summary>
		/// this is the entry point for executing and evaluating the parse tree.
		/// </summary>
		/// <param name="paramlist">additional optional input parameters</param>
		/// <returns>the output of the evaluation function</returns>
		// TODO: template the class (not the method) and/or use std::any
		template<typename T>
		T Eval(const std::vector<std::any>& paramlist = {});
	};

	template<typename T>
	inline T ParseTree::Eval(const std::vector<std::any>& paramlist)
	{
		return Nodes[0]->EvalStart(paramlist);
	}

}
