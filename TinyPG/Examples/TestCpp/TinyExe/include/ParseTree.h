// Automatically generated from source file: TinyExpEval_cpp.tpg
// By TinyPG v1.5 available at https://github.com/ultrasuperpingu/TinyPG

#pragma once
#include <string>
#include <vector>
#include <any>
#include "Scanner.h"
#include <any>
#include <sstream>
#include <cmath>
#include "Function.h"
#include "Context.h"
#include "DynamicFunction.h"
#include "Util.h"

namespace TinyExe
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
		virtual std::any EvalStart(const std::vector<std::any>& paramlist);
		virtual std::any GetStartValue(int index, const std::vector<std::any>& paramlist);
		virtual std::any EvalFunction(const std::vector<std::any>& paramlist);
		virtual std::any GetFunctionValue(int index, const std::vector<std::any>& paramlist);
		virtual std::any EvalPrimaryExpression(const std::vector<std::any>& paramlist);
		virtual std::any GetPrimaryExpressionValue(int index, const std::vector<std::any>& paramlist);
		virtual std::any EvalParenthesizedExpression(const std::vector<std::any>& paramlist);
		virtual std::any GetParenthesizedExpressionValue(int index, const std::vector<std::any>& paramlist);
		virtual std::any EvalUnaryExpression(const std::vector<std::any>& paramlist);
		virtual std::any GetUnaryExpressionValue(int index, const std::vector<std::any>& paramlist);
		virtual std::any EvalPowerExpression(const std::vector<std::any>& paramlist);
		virtual std::any GetPowerExpressionValue(int index, const std::vector<std::any>& paramlist);
		virtual std::any EvalMultiplicativeExpression(const std::vector<std::any>& paramlist);
		virtual std::any GetMultiplicativeExpressionValue(int index, const std::vector<std::any>& paramlist);
		virtual std::any EvalAdditiveExpression(const std::vector<std::any>& paramlist);
		virtual std::any GetAdditiveExpressionValue(int index, const std::vector<std::any>& paramlist);
		virtual std::any EvalConcatEpression(const std::vector<std::any>& paramlist);
		virtual std::any GetConcatEpressionValue(int index, const std::vector<std::any>& paramlist);
		virtual std::any EvalRelationalExpression(const std::vector<std::any>& paramlist);
		virtual std::any GetRelationalExpressionValue(int index, const std::vector<std::any>& paramlist);
		virtual std::any EvalEqualityExpression(const std::vector<std::any>& paramlist);
		virtual std::any GetEqualityExpressionValue(int index, const std::vector<std::any>& paramlist);
		virtual std::any EvalConditionalAndExpression(const std::vector<std::any>& paramlist);
		virtual std::any GetConditionalAndExpressionValue(int index, const std::vector<std::any>& paramlist);
		virtual std::any EvalConditionalOrExpression(const std::vector<std::any>& paramlist);
		virtual std::any GetConditionalOrExpressionValue(int index, const std::vector<std::any>& paramlist);
		virtual std::any EvalAssignmentExpression(const std::vector<std::any>& paramlist);
		virtual std::any GetAssignmentExpressionValue(int index, const std::vector<std::any>& paramlist);
		virtual std::any EvalExpression(const std::vector<std::any>& paramlist);
		virtual std::any GetExpressionValue(int index, const std::vector<std::any>& paramlist);
		virtual std::any EvalParams(const std::vector<std::any>& paramlist);
		virtual std::any GetParamsValue(int index, const std::vector<std::any>& paramlist);
		virtual std::any EvalLiteral(const std::vector<std::any>& paramlist);
		virtual std::any GetLiteralValue(int index, const std::vector<std::any>& paramlist);
		virtual std::any EvalIntegerLiteral(const std::vector<std::any>& paramlist);
		virtual std::any GetIntegerLiteralValue(int index, const std::vector<std::any>& paramlist);
		virtual std::any EvalRealLiteral(const std::vector<std::any>& paramlist);
		virtual std::any GetRealLiteralValue(int index, const std::vector<std::any>& paramlist);
		virtual std::any EvalStringLiteral(const std::vector<std::any>& paramlist);
		virtual std::any GetStringLiteralValue(int index, const std::vector<std::any>& paramlist);
		virtual std::any EvalVariable(const std::vector<std::any>& paramlist);
		virtual std::any GetVariableValue(int index, const std::vector<std::any>& paramlist);



	// helper function to find access the function or variable
	protected:
	ParseNode* GetFunctionOrVariable(ParseNode* n)
	{
		// found the right node, exit
		if (n->TokenVal.Type == TokenType::Function || n->TokenVal.Type == TokenType::VARIABLE)
			return n;

		if (n->Nodes.size() == 1) // search lower branch (left side only, may not contain other node branches)
			return GetFunctionOrVariable(n->Nodes[0]);

		// function or variable not found in branch
		return NULL;
	}
	public:
		Context* Context = NULL;

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
