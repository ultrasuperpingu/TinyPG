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
<%VirtualEvalMethods%>

<%CustomCode%>
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
	public:
		bool containsErrors();
		bool containsWarnings();
		std::vector<ParseError> getErrors();
		std::vector<ParseError> getWarnings();
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

	inline bool ParseErrors::containsErrors()
	{
		return std::find_if(begin(), end(), [](ParseError i) {return !i.IsWarning; }) != end();
	}
	inline bool ParseErrors::containsWarnings()
	{
		return std::find_if(begin(), end(), [](ParseError i) {return i.IsWarning; }) != end();
	}
	inline std::vector<ParseError> ParseErrors::getErrors()
	{
		std::vector<ParseError> errors;
		std::copy_if(begin(), end(), std::back_inserter(errors), [](ParseError i) {return !i.IsWarning; });
		return errors;
	}
	inline std::vector<ParseError> ParseErrors::getWarnings()
	{
		std::vector<ParseError> warnings;
		std::copy_if(begin(), end(), std::back_inserter(warnings), [](ParseError i) {return i.IsWarning; });
		return warnings;
	}
}
