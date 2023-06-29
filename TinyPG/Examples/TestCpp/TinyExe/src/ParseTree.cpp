// Automatically generated from source file: TinyExpEval_cpp.tpg
// By TinyPG v1.5 available at https://github.com/ultrasuperpingu/TinyPG

#include "ParseTree.h"

namespace TinyExe
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
				case TokenType::Start:
					Value = EvalStart(paramlist);
					break;
				case TokenType::Function:
					Value = EvalFunction(paramlist);
					break;
				case TokenType::PrimaryExpression:
					Value = EvalPrimaryExpression(paramlist);
					break;
				case TokenType::ParenthesizedExpression:
					Value = EvalParenthesizedExpression(paramlist);
					break;
				case TokenType::UnaryExpression:
					Value = EvalUnaryExpression(paramlist);
					break;
				case TokenType::PowerExpression:
					Value = EvalPowerExpression(paramlist);
					break;
				case TokenType::MultiplicativeExpression:
					Value = EvalMultiplicativeExpression(paramlist);
					break;
				case TokenType::AdditiveExpression:
					Value = EvalAdditiveExpression(paramlist);
					break;
				case TokenType::ConcatEpression:
					Value = EvalConcatEpression(paramlist);
					break;
				case TokenType::RelationalExpression:
					Value = EvalRelationalExpression(paramlist);
					break;
				case TokenType::EqualityExpression:
					Value = EvalEqualityExpression(paramlist);
					break;
				case TokenType::ConditionalAndExpression:
					Value = EvalConditionalAndExpression(paramlist);
					break;
				case TokenType::ConditionalOrExpression:
					Value = EvalConditionalOrExpression(paramlist);
					break;
				case TokenType::AssignmentExpression:
					Value = EvalAssignmentExpression(paramlist);
					break;
				case TokenType::Expression:
					Value = EvalExpression(paramlist);
					break;
				case TokenType::Params:
					Value = EvalParams(paramlist);
					break;
				case TokenType::Literal:
					Value = EvalLiteral(paramlist);
					break;
				case TokenType::IntegerLiteral:
					Value = EvalIntegerLiteral(paramlist);
					break;
				case TokenType::RealLiteral:
					Value = EvalRealLiteral(paramlist);
					break;
				case TokenType::StringLiteral:
					Value = EvalStringLiteral(paramlist);
					break;
				case TokenType::Variable:
					Value = EvalVariable(paramlist);
					break;

		default:
			Value = &TokenVal.Text;
			break;
		}
		return Value;
	}

	std::any ParseNode::EvalStart(const std::vector<std::any>& paramlist)
	{
		return this->GetExpressionValue(0, paramlist);
	}

	std::any ParseNode::GetStartValue(int index, const std::vector<std::any>& paramlist)
	{
		ParseNode* node = GetTokenNode(TokenType::Start, index);
		if (node != NULL)
			return node->EvalStart(paramlist);
		throw std::runtime_error("No Start[index] found.");
	}

	std::any ParseNode::EvalFunction(const std::vector<std::any>& paramlist)
	{
		ParseTree* tree = std::any_cast<ParseTree*>(paramlist[0]);
		ParseNode* funcNode = this->Nodes[0];
		ParseNode* paramNode = this->Nodes[2];
	
		if (tree->context == NULL)
		{
			tree->Errors.push_back(ParseError("No context defined", 1041, this));
			return NULL;
		}
		if (tree->context->CurrentStackSize > 50)
		{
			tree->Errors.push_back(ParseError("Stack overflow: " + funcNode->TokenVal.Text + "()", 1046, this));
			return NULL;
		}
		std::string key = str_tolower(funcNode->TokenVal.Text);
		if (!containsKey(tree->context->functions, key))
		{
			tree->Errors.push_back(ParseError("Fuction not defined: " + funcNode->TokenVal.Text + "()", 1042, this));
			return NULL;
		}
	
		// retrieves the function from declared functions
		class Function* func = tree->context->functions[key];
	
		// evaluate the function parameters
		std::vector<std::any> parameters = std::vector<std::any>();
		if (paramNode->TokenVal.Type == TokenType::Params)
			parameters = std::any_cast<std::vector<std::any>>(paramNode->Eval(paramlist));
		if (parameters.size() < func->MinParameters) 
		{
			tree->Errors.push_back(ParseError("At least " + std::to_string(func->MinParameters) + " parameter(s) expected", 1043, this));
			return NULL; // illegal number of parameters
		}
		else if (parameters.size() > func->MaxParameters)
		{
			tree->Errors.push_back(ParseError("No more than " + std::to_string(func->MaxParameters) + " parameter(s) expected", 1044, this));
			return NULL; // illegal number of parameters
		}
		
		return func->Eval(parameters, tree);
	}

	std::any ParseNode::GetFunctionValue(int index, const std::vector<std::any>& paramlist)
	{
		ParseNode* node = GetTokenNode(TokenType::Function, index);
		if (node != NULL)
			return node->EvalFunction(paramlist);
		throw std::runtime_error("No Function[index] found.");
	}

	std::any ParseNode::EvalPrimaryExpression(const std::vector<std::any>& paramlist)
	{
		ParseTree* tree = std::any_cast<ParseTree*>(paramlist[0]);
		TokenType type = this->Nodes[0]->TokenVal.Type;
		if (type == TokenType::Function)
			return this->GetFunctionValue(0, paramlist);
		else if (type == TokenType::Literal)
			return this->GetLiteralValue(0, paramlist);
		else if (type == TokenType::ParenthesizedExpression)
			return this->GetParenthesizedExpressionValue(0, paramlist);
		else if (type == TokenType::Variable)
			return this->GetVariableValue(0, paramlist);
	
		tree->Errors.push_back(ParseError("Illegal EvalPrimaryExpression format", 1097, this));
		return NULL;
	}

	std::any ParseNode::GetPrimaryExpressionValue(int index, const std::vector<std::any>& paramlist)
	{
		ParseNode* node = GetTokenNode(TokenType::PrimaryExpression, index);
		if (node != NULL)
			return node->EvalPrimaryExpression(paramlist);
		throw std::runtime_error("No PrimaryExpression[index] found.");
	}

	std::any ParseNode::EvalParenthesizedExpression(const std::vector<std::any>& paramlist)
	{
		return this->GetExpressionValue(0, paramlist);
	}

	std::any ParseNode::GetParenthesizedExpressionValue(int index, const std::vector<std::any>& paramlist)
	{
		ParseNode* node = GetTokenNode(TokenType::ParenthesizedExpression, index);
		if (node != NULL)
			return node->EvalParenthesizedExpression(paramlist);
		throw std::runtime_error("No ParenthesizedExpression[index] found.");
	}

	std::any ParseNode::EvalUnaryExpression(const std::vector<std::any>& paramlist)
	{
		ParseTree* tree = std::any_cast<ParseTree*>(paramlist[0]);
		TokenType type = this->Nodes[0]->TokenVal.Type;
		if (type == TokenType::PrimaryExpression)
			return this->GetPrimaryExpressionValue(0, paramlist);
	
		if (type == TokenType::MINUS)
		{
			std::any val = this->GetUnaryExpressionValue(0, paramlist);
			if (auto x = std::any_cast<double>(&val))
				return -*x;
			if (auto x = std::any_cast<int>(&val))
				return -*x;
	
			tree->Errors.push_back(ParseError("Illegal UnaryExpression format, cannot interpret minus " + ConvertToString(val), 1095, this));
			return NULL;
		}
		else if (type == TokenType::PLUS)
		{
			std::any val = this->GetUnaryExpressionValue(0, paramlist);
			return val;
		}
		else if (type == TokenType::NOT)
		{
			std::any val = this->GetUnaryExpressionValue(0, paramlist);
			if (auto x = std::any_cast<bool>(&val))
				return !(*x);
	
			tree->Errors.push_back(ParseError("Illegal UnaryExpression format, cannot interpret negate " + ConvertToString(val), 1098, this));
			return NULL;
		}
	
		tree->Errors.push_back(ParseError("Illegal UnaryExpression format", 1099, this));
		return NULL;
	}

	std::any ParseNode::GetUnaryExpressionValue(int index, const std::vector<std::any>& paramlist)
	{
		ParseNode* node = GetTokenNode(TokenType::UnaryExpression, index);
		if (node != NULL)
			return node->EvalUnaryExpression(paramlist);
		throw std::runtime_error("No UnaryExpression[index] found.");
	}

	std::any ParseNode::EvalPowerExpression(const std::vector<std::any>& paramlist)
	{
		ParseTree* tree = std::any_cast<ParseTree*>(paramlist[0]);
		std::any result = this->GetUnaryExpressionValue(0, paramlist);
	
		// IMPORTANT: scanning and calculating the power is done from Left to Right.
		// this is conform the Excel evaluation of power, but not conform strict mathematical guidelines
		// this means that a^b^c evaluates to (a^b)^c  (Excel uses the same kind of evaluation)
		// stricly mathematical speaking a^b^c should evaluate to a^(b^c) (therefore calculating the powers from Right to Left)
		for (int i = 1; i < Nodes.size(); i += 2)
		{
			Token token = Nodes[i]->TokenVal;
			std::any val = Nodes[i + 1]->Eval(paramlist);
			if (token.Type == TokenType::POWER)
				result = std::pow(ConvertToDouble(result), ConvertToDouble(val));
		}
	
		return result;
	}

	std::any ParseNode::GetPowerExpressionValue(int index, const std::vector<std::any>& paramlist)
	{
		ParseNode* node = GetTokenNode(TokenType::PowerExpression, index);
		if (node != NULL)
			return node->EvalPowerExpression(paramlist);
		throw std::runtime_error("No PowerExpression[index] found.");
	}

	std::any ParseNode::EvalMultiplicativeExpression(const std::vector<std::any>& paramlist)
	{
		ParseTree* tree = std::any_cast<ParseTree*>(paramlist[0]);
		std::any result = this->GetPowerExpressionValue(0, paramlist);
		for (int i = 1; i < Nodes.size(); i+=2 )
		{
			Token token = Nodes[i]->TokenVal;
			std::any val = Nodes[i+1]->Eval(paramlist);
			if (token.Type == TokenType::ASTERIKS)
				result = ConvertToDouble(result) * ConvertToDouble(val);
			else if (token.Type == TokenType::SLASH)
				result = ConvertToDouble(result) / ConvertToDouble(val);
			else if (token.Type == TokenType::PERCENT)
				result = ConvertToInt32(result) % ConvertToInt32(val);
		}
	
		return result;
	}

	std::any ParseNode::GetMultiplicativeExpressionValue(int index, const std::vector<std::any>& paramlist)
	{
		ParseNode* node = GetTokenNode(TokenType::MultiplicativeExpression, index);
		if (node != NULL)
			return node->EvalMultiplicativeExpression(paramlist);
		throw std::runtime_error("No MultiplicativeExpression[index] found.");
	}

	std::any ParseNode::EvalAdditiveExpression(const std::vector<std::any>& paramlist)
	{
		ParseTree* tree = std::any_cast<ParseTree*>(paramlist[0]);
		std::any result = this->GetMultiplicativeExpressionValue(0, paramlist);
		for (int i = 1; i < Nodes.size(); i += 2)
		{
			Token token = Nodes[i]->TokenVal;
			std::any val = Nodes[i + 1]->Eval(paramlist);
			if (token.Type == TokenType::PLUS)
				result = ConvertToDouble(result) + ConvertToDouble(val);
			else if (token.Type == TokenType::MINUS)
				result = ConvertToDouble(result) - ConvertToDouble(val);
		}
	
		return result;
	}

	std::any ParseNode::GetAdditiveExpressionValue(int index, const std::vector<std::any>& paramlist)
	{
		ParseNode* node = GetTokenNode(TokenType::AdditiveExpression, index);
		if (node != NULL)
			return node->EvalAdditiveExpression(paramlist);
		throw std::runtime_error("No AdditiveExpression[index] found.");
	}

	std::any ParseNode::EvalConcatEpression(const std::vector<std::any>& paramlist)
	{
		ParseTree* tree = std::any_cast<ParseTree*>(paramlist[0]);
		std::any result = this->GetAdditiveExpressionValue(0, paramlist);
		for (int i = 1; i < Nodes.size(); i += 2)
		{
			Token token = Nodes[i]->TokenVal;
			std::any val = Nodes[i + 1]->Eval(paramlist);
			if (token.Type == TokenType::AMP)
				result = ConvertToString(result) + ConvertToString(val);
		}
		return result;
	}

	std::any ParseNode::GetConcatEpressionValue(int index, const std::vector<std::any>& paramlist)
	{
		ParseNode* node = GetTokenNode(TokenType::ConcatEpression, index);
		if (node != NULL)
			return node->EvalConcatEpression(paramlist);
		throw std::runtime_error("No ConcatEpression[index] found.");
	}

	std::any ParseNode::EvalRelationalExpression(const std::vector<std::any>& paramlist)
	{
		ParseTree* tree = std::any_cast<ParseTree*>(paramlist[0]);
		std::any result = this->GetConcatEpressionValue(0, paramlist);
		for (int i = 1; i < Nodes.size(); i += 2)
		{
			Token token = Nodes[i]->TokenVal;
			std::any val = Nodes[i + 1]->Eval(paramlist);
	
			// compare as numbers
			if (result.has_value() && result.type().name() == "double" && val.has_value() && val.type().name() == "double")
			{
				if (token.Type == TokenType::LESSTHAN)
					result = ConvertToDouble(result) < ConvertToDouble(val);
				else if (token.Type == TokenType::LESSEQUAL)
					result = ConvertToDouble(result) <= ConvertToDouble(val);
				else if (token.Type == TokenType::GREATERTHAN)
					result = ConvertToDouble(result) > ConvertToDouble(val);
				else if (token.Type == TokenType::GREATEREQUAL)
					result = ConvertToDouble(result) >= ConvertToDouble(val);
			}
			else // compare as strings
			{
				if (token.Type == TokenType::LESSTHAN)
					result = ConvertToString(result) < ConvertToString(val);
				else if (token.Type == TokenType::LESSEQUAL)
					result = ConvertToString(result) <= ConvertToString(val);
				else if (token.Type == TokenType::GREATERTHAN)
					result = ConvertToString(result) > ConvertToString(val);
				else if (token.Type == TokenType::GREATEREQUAL)
					result = ConvertToString(result) >= ConvertToString(val);
			}
			
		}
		return result;
	}

	std::any ParseNode::GetRelationalExpressionValue(int index, const std::vector<std::any>& paramlist)
	{
		ParseNode* node = GetTokenNode(TokenType::RelationalExpression, index);
		if (node != NULL)
			return node->EvalRelationalExpression(paramlist);
		throw std::runtime_error("No RelationalExpression[index] found.");
	}

	std::any ParseNode::EvalEqualityExpression(const std::vector<std::any>& paramlist)
	{
		ParseTree* tree = std::any_cast<ParseTree*>(paramlist[0]);
		std::any result = this->GetRelationalExpressionValue(0, paramlist);
		for (int i = 1; i < Nodes.size(); i += 2)
		{
			Token token = Nodes[i]->TokenVal;
			std::any val = Nodes[i + 1]->Eval(paramlist);
			if (result.has_value() && !val.has_value() || !result.has_value() && val.has_value())
			{
				if (token.Type == TokenType::EQUAL)
					result = false;
				else if (token.Type == TokenType::NOTEQUAL)
					result = true;
			}
			else if (!result.has_value() && !val.has_value())
			{
				if (token.Type == TokenType::EQUAL)
					result = true;
				else if (token.Type == TokenType::NOTEQUAL)
					result = false;
			}
			else // if (result.has_value() && val.has_value())
			{
				if (result.type().name() != val.type().name())
				{
					if (token.Type == TokenType::EQUAL)
						result = true;
					else if (token.Type == TokenType::NOTEQUAL)
						result = false;
				}
				else
				{
					if (auto x = std::any_cast<bool>(&val))
					{
						if (token.Type == TokenType::EQUAL)
							result = std::any_cast<bool>(result) == std::any_cast<bool>(val);
						else if (token.Type == TokenType::NOTEQUAL)
							result = std::any_cast<bool>(result) != std::any_cast<bool>(val);
					}
					else if (auto x = std::any_cast<double>(&val))
					{
						if (token.Type == TokenType::EQUAL)
							result = std::any_cast<double>(result) == std::any_cast<double>(val);
						else if (token.Type == TokenType::NOTEQUAL)
							result = std::any_cast<double>(result) != std::any_cast<double>(val);
					}
					else if (auto x = std::any_cast<int>(&val))
					{
						if (token.Type == TokenType::EQUAL)
							result = std::any_cast<int>(result) == std::any_cast<int>(val);
						else if (token.Type == TokenType::NOTEQUAL)
							result = std::any_cast<int>(result) != std::any_cast<int>(val);
					}
					else if (auto x = std::any_cast<std::string>(&val))
					{
						if (token.Type == TokenType::EQUAL)
							result = std::any_cast<std::string>(result) == std::any_cast<std::string>(val);
						else if (token.Type == TokenType::NOTEQUAL)
							result = std::any_cast<std::string>(result) != std::any_cast<std::string>(val);
					}
				}
			}
		}
		return result;
	}

	std::any ParseNode::GetEqualityExpressionValue(int index, const std::vector<std::any>& paramlist)
	{
		ParseNode* node = GetTokenNode(TokenType::EqualityExpression, index);
		if (node != NULL)
			return node->EvalEqualityExpression(paramlist);
		throw std::runtime_error("No EqualityExpression[index] found.");
	}

	std::any ParseNode::EvalConditionalAndExpression(const std::vector<std::any>& paramlist)
	{
		ParseTree* tree = std::any_cast<ParseTree*>(paramlist[0]);
		std::any result = this->GetEqualityExpressionValue(0, paramlist);
		for (int i = 1; i < Nodes.size(); i += 2)
		{
			Token token = Nodes[i]->TokenVal;
			std::any val = Nodes[i + 1]->Eval(paramlist);
			if (token.Type == TokenType::AMPAMP)
				result = ConvertToBoolean(result) && ConvertToBoolean(val);
		}
		return result;
	}

	std::any ParseNode::GetConditionalAndExpressionValue(int index, const std::vector<std::any>& paramlist)
	{
		ParseNode* node = GetTokenNode(TokenType::ConditionalAndExpression, index);
		if (node != NULL)
			return node->EvalConditionalAndExpression(paramlist);
		throw std::runtime_error("No ConditionalAndExpression[index] found.");
	}

	std::any ParseNode::EvalConditionalOrExpression(const std::vector<std::any>& paramlist)
	{
		ParseTree* tree = std::any_cast<ParseTree*>(paramlist[0]);
		std::any result = this->GetConditionalAndExpressionValue(0, paramlist);
		for (int i = 1; i < Nodes.size(); i += 2)
		{
			Token token = Nodes[i]->TokenVal;
			std::any val = Nodes[i + 1]->Eval(paramlist);
			if (token.Type == TokenType::PIPEPIPE)
				result = ConvertToBoolean(result) || ConvertToBoolean(val);
		}
		return result;
	}

	std::any ParseNode::GetConditionalOrExpressionValue(int index, const std::vector<std::any>& paramlist)
	{
		ParseNode* node = GetTokenNode(TokenType::ConditionalOrExpression, index);
		if (node != NULL)
			return node->EvalConditionalOrExpression(paramlist);
		throw std::runtime_error("No ConditionalOrExpression[index] found.");
	}

	std::any ParseNode::EvalAssignmentExpression(const std::vector<std::any>& paramlist)
	{
		ParseTree* tree = std::any_cast<ParseTree*>(paramlist[0]);
		std::any result = this->GetConditionalOrExpressionValue(0, paramlist);
		if (Nodes.size() >= 5 && result.has_value() && std::any_cast<int>(&result)
			&& Nodes[1]->TokenVal.Type == TokenType::QUESTIONMARK
			&& Nodes[3]->TokenVal.Type == TokenType::COLON)
		{
			if (ConvertToBoolean(result))
				result = Nodes[2]->Eval(paramlist); // return 1st argument
			else
				result = Nodes[4]->Eval(paramlist); // return 2nd argumen
		}
		return result;
	}

	std::any ParseNode::GetAssignmentExpressionValue(int index, const std::vector<std::any>& paramlist)
	{
		ParseNode* node = GetTokenNode(TokenType::AssignmentExpression, index);
		if (node != NULL)
			return node->EvalAssignmentExpression(paramlist);
		throw std::runtime_error("No AssignmentExpression[index] found.");
	}

	std::any ParseNode::EvalExpression(const std::vector<std::any>& paramlist)
	{
		ParseTree* tree = std::any_cast<ParseTree*>(paramlist[0]);
		// if only left hand side available, this is not an assignment, simple evaluate expression
		if (Nodes.size() == 1)
			return this->GetAssignmentExpressionValue(0, paramlist); // return the result
	
		if (Nodes.size() != 3)
		{
			tree->Errors.push_back(ParseError("Illegal EvalExpression format", 1092, this));
			return NULL;
		}
	
		// ok, this is an assignment so declare the function or variable
		// assignment only allowed to function or to a variable
		ParseNode* v = GetFunctionOrVariable(Nodes[0]);
		if (v == NULL)
		{
			tree->Errors.push_back(ParseError("Can only assign to function or variable", 1020, this));
			return NULL;
		}
	
		if (tree->context == NULL)
		{
			tree->Errors.push_back(ParseError("No context defined", 1041, this));
			return NULL;
		}
	
		if (v->TokenVal.Type == TokenType::VARIABLE)
		{
			// simply overwrite any previous defnition
			std::string key = v->TokenVal.Text;
			tree->context->Globals[key] = this->GetAssignmentExpressionValue(1, paramlist);
			return tree->context->Globals[key] ;
		}
		else if (v->TokenVal.Type == TokenType::Function)
		{
	
			std::string key = v->Nodes[0]->TokenVal.Text;
	
			// function lookup is case insensitive
			if (containsKey(tree->context->functions, str_tolower(key)))
			{
				auto func = tree->context->functions[str_tolower(key)];
				if (!func->IsDynamic())
				{
					tree->Errors.push_back(ParseError("Built in functions cannot be overwritten", 1050, this));
					return std::any();
				}
			}
			// lets determine the input variables. 
			// functions must be of te form f(x;y;z) = x+y*z;
			// check the function parameters to be of type Variable, error otherwise
			Variables* vars = new Variables();
			ParseNode* paramsNode = v->Nodes[2];
			if (paramsNode->TokenVal.Type == TokenType::Params)
			{   // function has parameters, so check if they are all variable declarations
				for (int i = 0; i < paramsNode->Nodes.size(); i += 2)
				{
					ParseNode* varNode = GetFunctionOrVariable(paramsNode->Nodes[i]);
					if (varNode == NULL || varNode->TokenVal.Type != TokenType::VARIABLE)
					{
						tree->Errors.push_back(ParseError("Function declaration may only contain variables", 1051, this));
						return NULL;
					}
					// simply declare the variable, no need to evaluate the value of it at this point. 
					// evaluation will be done when the function is executed
					// note, variables are Case Sensitive (!)
					vars->insert(std::make_pair(varNode->TokenVal.Text, std::any()));
				}
			}
			// we have all the info we need to know to declare the dynamicly defined function
			// pass on nodes[2] which is the Right Hand Side (RHS) of the assignment
			// nodes[2] will be evaluated at runtime when the function is executed.
			DynamicFunction* dynf = new DynamicFunction(key, Nodes[2], vars, (int)vars->size(), (int)vars->size());
			tree->context->functions[str_tolower(key)] = dynf;
			return dynf;
		}
	
		return std::any();
	}

	std::any ParseNode::GetExpressionValue(int index, const std::vector<std::any>& paramlist)
	{
		ParseNode* node = GetTokenNode(TokenType::Expression, index);
		if (node != NULL)
			return node->EvalExpression(paramlist);
		throw std::runtime_error("No Expression[index] found.");
	}

	std::any ParseNode::EvalParams(const std::vector<std::any>& paramlist)
	{
		ParseTree* tree = std::any_cast<ParseTree*>(paramlist[0]);
		std::vector<std::any> parameters = std::vector<std::any>();
		for (int i = 0; i < Nodes.size(); i += 2)
		{
			if (Nodes[i]->TokenVal.Type == TokenType::Expression)
			{
				std::any val = Nodes[i]->Eval(paramlist);
				parameters.push_back(val);
			}
		}
		return parameters;
	}

	std::any ParseNode::GetParamsValue(int index, const std::vector<std::any>& paramlist)
	{
		ParseNode* node = GetTokenNode(TokenType::Params, index);
		if (node != NULL)
			return node->EvalParams(paramlist);
		throw std::runtime_error("No Params[index] found.");
	}

	std::any ParseNode::EvalLiteral(const std::vector<std::any>& paramlist)
	{
		ParseTree* tree = std::any_cast<ParseTree*>(paramlist[0]);
		TokenType type = this->Nodes[0]->TokenVal.Type;
		if (type == TokenType::StringLiteral)
			return this->GetStringLiteralValue(0, paramlist);
		else if (type == TokenType::IntegerLiteral)
			return this->GetIntegerLiteralValue(0, paramlist);
		else if (type == TokenType::RealLiteral)
			return this->GetRealLiteralValue(0, paramlist);
		else if (type == TokenType::BOOLEANLITERAL)
		{
			return this->GetTerminalValue(TokenType::BOOLEANLITERAL, 0) == "true";
		}
	
		tree->Errors.push_back(ParseError("illegal Literal format", 1003, this));
		return NULL;
	}

	std::any ParseNode::GetLiteralValue(int index, const std::vector<std::any>& paramlist)
	{
		ParseNode* node = GetTokenNode(TokenType::Literal, index);
		if (node != NULL)
			return node->EvalLiteral(paramlist);
		throw std::runtime_error("No Literal[index] found.");
	}

	std::any ParseNode::EvalIntegerLiteral(const std::vector<std::any>& paramlist)
	{
		ParseTree* tree = std::any_cast<ParseTree*>(paramlist[0]);
		if (this->IsTokenPresent(TokenType::DECIMALINTEGERLITERAL, 0))
			return ConvertToDouble(this->GetTerminalValue(TokenType::DECIMALINTEGERLITERAL, 0));
		if (this->IsTokenPresent(TokenType::HEXINTEGERLITERAL, 0))
		{
			std::string hex = this->GetTerminalValue(TokenType::HEXINTEGERLITERAL, 0);
			int x;
			std::stringstream ss;
			ss << std::hex << hex.substr(2, hex.length() - 2);
			ss >> x;
			return ConvertToDouble(x);
		}
	
		tree->Errors.push_back(ParseError("illegal IntegerLiteral format", 1002, this));
		return NULL;
	}

	std::any ParseNode::GetIntegerLiteralValue(int index, const std::vector<std::any>& paramlist)
	{
		ParseNode* node = GetTokenNode(TokenType::IntegerLiteral, index);
		if (node != NULL)
			return node->EvalIntegerLiteral(paramlist);
		throw std::runtime_error("No IntegerLiteral[index] found.");
	}

	std::any ParseNode::EvalRealLiteral(const std::vector<std::any>& paramlist)
	{
		ParseTree* tree = std::any_cast<ParseTree*>(paramlist[0]);
		if (this->IsTokenPresent(TokenType::REALLITERAL, 0))
		{
			return ConvertToDouble(this->GetTerminalValue(TokenType::REALLITERAL, 0));
		}
		tree->Errors.push_back(ParseError("illegal RealLiteral format", 1001, this));
		return NULL;
	}

	std::any ParseNode::GetRealLiteralValue(int index, const std::vector<std::any>& paramlist)
	{
		ParseNode* node = GetTokenNode(TokenType::RealLiteral, index);
		if (node != NULL)
			return node->EvalRealLiteral(paramlist);
		throw std::runtime_error("No RealLiteral[index] found.");
	}

	std::any ParseNode::EvalStringLiteral(const std::vector<std::any>& paramlist)
	{
		ParseTree* tree = std::any_cast<ParseTree*>(paramlist[0]);
		if (this->IsTokenPresent(TokenType::STRINGLITERAL, 0))
		{
			std::string r = this->GetTerminalValue(TokenType::STRINGLITERAL, 0);
			r = r.substr(1, r.length() - 2); // strip quotes
			return r;
		}
	
		tree->Errors.push_back(ParseError("illegal StringLiteral format", 1000, this));
		return "";
	}

	std::any ParseNode::GetStringLiteralValue(int index, const std::vector<std::any>& paramlist)
	{
		ParseNode* node = GetTokenNode(TokenType::StringLiteral, index);
		if (node != NULL)
			return node->EvalStringLiteral(paramlist);
		throw std::runtime_error("No StringLiteral[index] found.");
	}

	std::any ParseNode::EvalVariable(const std::vector<std::any>& paramlist)
	{
		ParseTree* tree = std::any_cast<ParseTree*>(paramlist[0]);
		if (tree->context == NULL)
		{
			tree->Errors.push_back(ParseError("No context defined", 1041, this));
			return NULL;
		}
	
		std::string key = (std::string)this->GetTerminalValue(TokenType::VARIABLE, 0);
		// first check if the variable was declared in scope of a function
		std::any scope_var = tree->context->GetScopeVariable(key);
		if(scope_var.has_value())
			return scope_var;
		
		// if not in scope of a function
		// next check if the variable was declared as a global variable
		if (containsKey(tree->context->Globals,key))
			return tree->context->Globals[key];
	
		//variable not found
		tree->Errors.push_back(ParseError("Variable not defined: " + key, 1039, this));
		return std::any();
	}

	std::any ParseNode::GetVariableValue(int index, const std::vector<std::any>& paramlist)
	{
		ParseNode* node = GetTokenNode(TokenType::Variable, index);
		if (node != NULL)
			return node->EvalVariable(paramlist);
		throw std::runtime_error("No Variable[index] found.");
	}



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
