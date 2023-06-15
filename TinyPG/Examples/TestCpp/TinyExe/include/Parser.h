// Automatically generated from source file: TinyExpEval_cpp.tpg
// By TinyPG v1.5 available at https://github.com/ultrasuperpingu/TinyPG

#pragma once
#include <string>
#include "Scanner.h"
#include "ParseTree.h"

namespace TinyExe
{
	class Parser
	{
	private:
		Scanner& scanner;
		ParseTree* tree;
		ParseTree* instanciatedTree;
		void DeleteTree();
	public:
		Parser(Scanner& scanner);
		virtual ~Parser();

		ParseTree* Parse(const std::string& input);
		ParseTree* Parse(const std::string& input, ParseTree* tree);

	public:
		ParseTree* ParseStart(const std::string& input, ParseTree* tree);
	protected:
		void ParseStart(ParseNode* parent);
	public:
		ParseTree* ParseFunction(const std::string& input, ParseTree* tree);
	protected:
		void ParseFunction(ParseNode* parent);
	public:
		ParseTree* ParsePrimaryExpression(const std::string& input, ParseTree* tree);
	protected:
		void ParsePrimaryExpression(ParseNode* parent);
	public:
		ParseTree* ParseParenthesizedExpression(const std::string& input, ParseTree* tree);
	protected:
		void ParseParenthesizedExpression(ParseNode* parent);
	public:
		ParseTree* ParseUnaryExpression(const std::string& input, ParseTree* tree);
	protected:
		void ParseUnaryExpression(ParseNode* parent);
	public:
		ParseTree* ParsePowerExpression(const std::string& input, ParseTree* tree);
	protected:
		void ParsePowerExpression(ParseNode* parent);
	public:
		ParseTree* ParseMultiplicativeExpression(const std::string& input, ParseTree* tree);
	protected:
		void ParseMultiplicativeExpression(ParseNode* parent);
	public:
		ParseTree* ParseAdditiveExpression(const std::string& input, ParseTree* tree);
	protected:
		void ParseAdditiveExpression(ParseNode* parent);
	public:
		ParseTree* ParseConcatEpression(const std::string& input, ParseTree* tree);
	protected:
		void ParseConcatEpression(ParseNode* parent);
	public:
		ParseTree* ParseRelationalExpression(const std::string& input, ParseTree* tree);
	protected:
		void ParseRelationalExpression(ParseNode* parent);
	public:
		ParseTree* ParseEqualityExpression(const std::string& input, ParseTree* tree);
	protected:
		void ParseEqualityExpression(ParseNode* parent);
	public:
		ParseTree* ParseConditionalAndExpression(const std::string& input, ParseTree* tree);
	protected:
		void ParseConditionalAndExpression(ParseNode* parent);
	public:
		ParseTree* ParseConditionalOrExpression(const std::string& input, ParseTree* tree);
	protected:
		void ParseConditionalOrExpression(ParseNode* parent);
	public:
		ParseTree* ParseAssignmentExpression(const std::string& input, ParseTree* tree);
	protected:
		void ParseAssignmentExpression(ParseNode* parent);
	public:
		ParseTree* ParseExpression(const std::string& input, ParseTree* tree);
	protected:
		void ParseExpression(ParseNode* parent);
	public:
		ParseTree* ParseParams(const std::string& input, ParseTree* tree);
	protected:
		void ParseParams(ParseNode* parent);
	public:
		ParseTree* ParseLiteral(const std::string& input, ParseTree* tree);
	protected:
		void ParseLiteral(ParseNode* parent);
	public:
		ParseTree* ParseIntegerLiteral(const std::string& input, ParseTree* tree);
	protected:
		void ParseIntegerLiteral(ParseNode* parent);
	public:
		ParseTree* ParseRealLiteral(const std::string& input, ParseTree* tree);
	protected:
		void ParseRealLiteral(ParseNode* parent);
	public:
		ParseTree* ParseStringLiteral(const std::string& input, ParseTree* tree);
	protected:
		void ParseStringLiteral(ParseNode* parent);
	public:
		ParseTree* ParseVariable(const std::string& input, ParseTree* tree);
	protected:
		void ParseVariable(ParseNode* parent);



	};

}
