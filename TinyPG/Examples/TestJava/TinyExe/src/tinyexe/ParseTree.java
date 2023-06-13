// Automatically generated from source file: TinyExpEval_java.tpg
// By TinyPG v1.5 available at https://github.com/ultrasuperpingu/TinyPG

package tinyexe;
import java.util.ArrayList;


class ParseErrors extends ArrayList<ParseError>
{
	public boolean haveBlockingErrors()
	{
		return this.stream().filter(e -> e.isWarning() == false) != null;
	}
	public boolean haveWarnings()
	{
		return this.stream().filter(e -> e.isWarning() == true) != null;
	}
	public ArrayList<ParseError> getWarnings()
	{
		ArrayList<ParseError> warnings = new ArrayList<>();
		for(ParseError e : this)
		{
			if(e.isWarning())
				warnings.add(e);
		}
		return warnings;
	}
	public ArrayList<ParseError> getBlockingErrors()
	{
		ArrayList<ParseError> warnings = new ArrayList<>();
		for(ParseError e : this)
		{
			if(!e.isWarning())
				warnings.add(e);
		}
		return warnings;
	}
}

class ParseError
{
	private String message;
	private int code;
	private int line;
	private int col;
	private int pos;
	private int length;
	private boolean isWarning;
	

	public int getCode() { return code; }
	public int getLine() { return line; }
	public int getColumn() { return col; }
	public int getPosition() { return pos; }
	public int getLength() { return length; }
	public String getMessage() { return message; }
	public boolean isWarning() { return isWarning; }

	// just for the sake of serialization
	public ParseError()
	{
	}

	public ParseError(String message, int code, ParseNode node)
	{
		this(message, code, node.Token);
	}
	
	public ParseError(String message, int code, ParseNode node, boolean isWarning)
	{
		this(message, code, node.Token, isWarning);
	}

	public ParseError(String message, int code, Token token)
	{
		this(message, code, token.getLine(), token.getColumn(), token.getStartPos(), token.getLength(), false);
	}
	
	public ParseError(String message, int code, Token token, boolean isWarning)
	{
		this(message, code, token.getLine(), token.getColumn(), token.getStartPos(), token.getLength(), isWarning);
	}

	public ParseError(String message, int code)
	{
		this(message, code, 0, 0, 0, 0, false);
	}
	
	public ParseError(String message, int code, boolean isWarning)
	{
		this(message, code, 0, 0, 0, 0, isWarning);
	}
	public ParseError(String message, int code, int line, int col, int pos, int length, boolean isWarning)
	{
		this.message = message;
		this.code = code;
		this.line = line;
		this.col = col;
		this.pos = pos;
		this.length = length;
		this.isWarning = isWarning;
	}
}

// rootlevel of the node tree
public class ParseTree extends ParseNode
{
	public ParseErrors Errors;

	public ArrayList<Token> Skipped;

	public ParseTree()
	{
		super(new Token(), "ParseTree");
		Token.Type = TokenType.Start;
		Token.setText("Root");
		Errors = new ParseErrors();
	}

	public String PrintTree()
	{
		StringBuilder sb = new StringBuilder();
		int indent = 0;
		PrintNode(sb, this, indent);
		return sb.toString();
	}

	private void PrintNode(StringBuilder sb, ParseNode node, int indent)
	{
		
		for(int i=0;i<indent;i++) {
			sb.append(' ');
		}

		
		sb.append(node.getText() + "\n");

		for (ParseNode n : node.getNodes())
			PrintNode(sb, n, indent + 2);
	}
	
	/// <summary>
	/// this is the entry point for executing and evaluating the parse tree.
	/// </summary>
	/// <param name="paramlist">additional optional input parameters</param>
	/// <returns>the output of the evaluation function</returns>
	public Object Eval(Object... paramlist)
	{
		return getNodes().get(0).Eval(this, paramlist);
	}
}

class ParseNode
{
	protected String text;
	protected ArrayList<ParseNode> nodes;
	
	public ArrayList<ParseNode> getNodes() { return nodes;}
	
	public ParseNode Parent;
	public Token Token; // the token/rule

	public String getText() { // text to display in parse tree 
		return text;
	}
	
	public void setText(String value) { text = value; }

	public ParseNode CreateNode(Token token, String text)
	{
		ParseNode node = new ParseNode(token, text);
		node.Parent = this;
		return node;
	}

	protected ParseNode(Token token, String text)
	{
		this.Token = token;
		this.text = text;
		this.nodes = new ArrayList<ParseNode>();
	}
	protected ParseNode GetTokenNode(TokenType type, int index)
	{
		if (index < 0)
			return null;
		// left to right
		for (ParseNode node : nodes)
		{
			if (node.Token.Type == type)
			{
				index--;
				if (index < 0)
				{
					return node;
				}
			}
		}
		return null;
	}

	protected boolean IsTokenPresent(TokenType type, int index)
	{
		ParseNode node = GetTokenNode(type, index);
		return node != null;
	}
	protected String GetTerminalValue(TokenType type, int index)
	{
		ParseNode node = GetTokenNode(type, index);
		if (node != null)
			return node.Token.getText();
		return null;
	}

	protected Object GetValue(TokenType type, int index, Object... paramlist)
	{
		return GetValue(type, new int[]{ index }, paramlist);
	}

	protected Object GetValue(TokenType type, int[] index, Object... paramlist)
	{
		Object o = null;
		if (index[0] < 0) return o;

		// left to right
		for (ParseNode node : nodes)
		{
			if (node.Token.Type == type)
			{
				index[0]--;
				if (index[0] < 0)
				{
					o = node.Eval(paramlist);
					break;
				}
			}
		}
		return o;
	}

	/// <summary>
	/// this implements the evaluation functionality, cannot be used directly
	/// </summary>
	/// <param name="tree">the parsetree itself</param>
	/// <param name="paramlist">optional input parameters</param>
	/// <returns>a partial result of the evaluation</returns>
	public Object Eval(Object... paramlist)
	{
		Object Value = null;

		switch (Token.Type)
		{
			case Start:
				Value = EvalStart(paramlist);
				break;
			case Function:
				Value = EvalFunction(paramlist);
				break;
			case PrimaryExpression:
				Value = EvalPrimaryExpression(paramlist);
				break;
			case ParenthesizedExpression:
				Value = EvalParenthesizedExpression(paramlist);
				break;
			case UnaryExpression:
				Value = EvalUnaryExpression(paramlist);
				break;
			case PowerExpression:
				Value = EvalPowerExpression(paramlist);
				break;
			case MultiplicativeExpression:
				Value = EvalMultiplicativeExpression(paramlist);
				break;
			case AdditiveExpression:
				Value = EvalAdditiveExpression(paramlist);
				break;
			case ConcatEpression:
				Value = EvalConcatEpression(paramlist);
				break;
			case RelationalExpression:
				Value = EvalRelationalExpression(paramlist);
				break;
			case EqualityExpression:
				Value = EvalEqualityExpression(paramlist);
				break;
			case ConditionalAndExpression:
				Value = EvalConditionalAndExpression(paramlist);
				break;
			case ConditionalOrExpression:
				Value = EvalConditionalOrExpression(paramlist);
				break;
			case AssignmentExpression:
				Value = EvalAssignmentExpression(paramlist);
				break;
			case Expression:
				Value = EvalExpression(paramlist);
				break;
			case Params:
				Value = EvalParams(paramlist);
				break;
			case Literal:
				Value = EvalLiteral(paramlist);
				break;
			case IntegerLiteral:
				Value = EvalIntegerLiteral(paramlist);
				break;
			case RealLiteral:
				Value = EvalRealLiteral(paramlist);
				break;
			case StringLiteral:
				Value = EvalStringLiteral(paramlist);
				break;
			case Variable:
				Value = EvalVariable(paramlist);
				break;

			default:
				Value = Token.getText();
				break;
		}
		return Value;
	}

	protected Object EvalStart(Object... paramlist)
	{
			return this.GetExpressionValue(0, paramlist);
	}

	protected Object GetStartValue(int index, Object... paramlist)
	{
		Object o = null;
		ParseNode node = GetTokenNode(TokenType.Start, index);
		if (node != null)
			o = node.EvalStart(paramlist);
		return o;
	}

	protected Object EvalFunction(Object... paramlist)
	{
			ParseTree tree = (ParseTree)paramlist[0];
			ParseNode funcNode = this.nodes.get(0);
			ParseNode paramNode = this.nodes.get(2);
		
			if (tree.Context == null)
			{
				tree.Errors.add(new ParseError("No context defined", 1041, this));
				return null;
			}
			if (tree.Context.getCurrentStackSize() > 50)
			{
				tree.Errors.add(new ParseError("Stack overflow: " + funcNode.Token.getText() + "()", 1046, this));
				return null;
			}
			String key = funcNode.Token.getText().toLowerCase();
			if (!tree.Context.getFunctions().containsKey(key))
			{
				tree.Errors.add(new ParseError("Fuction not defined: " + funcNode.Token.getText() + "()", 1042, this));
				return null;
			}
		
			// retrieves the function from declared functions
			Function func = tree.Context.getFunctions().get(key);
		
			// evaluate the function parameters
			Object[] parameters = new Object[0];
			if (paramNode.Token.Type == TokenType.Params)
				parameters = ((ArrayList<Object>)paramNode.Eval(tree, paramlist)).toArray();
			if (parameters.length < func.getMinParameters()) 
			{
				tree.Errors.add(new ParseError("At least " + func.getMinParameters() + " parameter(s) expected", 1043, this));
				return null; // illegal number of parameters
			}
			else if (parameters.length > func.getMaxParameters())
			{
				tree.Errors.add(new ParseError("No more than " + func.getMaxParameters() + " parameter(s) expected", 1044, this));
				return null; // illegal number of parameters
			}
			
			var fres = func.Eval(parameters, tree);
			//String t_params = String.Join("; ", parameters);
			//Tracer.Trace("Func", "Eval "+func.Name+"("+ t_params + ") = "+fres);
			return fres;
	}

	protected Object GetFunctionValue(int index, Object... paramlist)
	{
		Object o = null;
		ParseNode node = GetTokenNode(TokenType.Function, index);
		if (node != null)
			o = node.EvalFunction(paramlist);
		return o;
	}

	protected Object EvalPrimaryExpression(Object... paramlist)
	{
			ParseTree tree = (ParseTree)paramlist[0];
			TokenType type = this.nodes.get(0).Token.Type;
			if (type == TokenType.Function)
				return this.GetFunctionValue(0, paramlist);
			else if (type == TokenType.Literal)
				return this.GetLiteralValue(0, paramlist);
			else if (type == TokenType.ParenthesizedExpression)
				return this.GetParenthesizedExpressionValue(0, paramlist);
			else if (type == TokenType.Variable)
				return this.GetVariableValue(0, paramlist);
		
			tree.Errors.add(new ParseError("Illegal EvalPrimaryExpression format", 1097, this));
			return null;
	}

	protected Object GetPrimaryExpressionValue(int index, Object... paramlist)
	{
		Object o = null;
		ParseNode node = GetTokenNode(TokenType.PrimaryExpression, index);
		if (node != null)
			o = node.EvalPrimaryExpression(paramlist);
		return o;
	}

	protected Object EvalParenthesizedExpression(Object... paramlist)
	{
			return this.GetExpressionValue(0, paramlist);
	}

	protected Object GetParenthesizedExpressionValue(int index, Object... paramlist)
	{
		Object o = null;
		ParseNode node = GetTokenNode(TokenType.ParenthesizedExpression, index);
		if (node != null)
			o = node.EvalParenthesizedExpression(paramlist);
		return o;
	}

	protected Object EvalUnaryExpression(Object... paramlist)
	{
			ParseTree tree = (ParseTree)paramlist[0];
			TokenType type = this.nodes.get(0).Token.Type;
			if (type == TokenType.PrimaryExpression)
				return this.GetPrimaryExpressionValue(0, paramlist);
		
			if (type == TokenType.MINUS)
			{
				Object val = this.GetUnaryExpressionValue(0, paramlist);
				if (val instanceof Double)
					return -((double)val);
		
				tree.Errors.add(new ParseError("Illegal UnaryExpression format, cannot interpret minus " + val.toString(), 1095, this));
				return null;
			}
			else if (type == TokenType.PLUS)
			{
				Object val = this.GetUnaryExpressionValue(0, paramlist);
				return val;
			}
			else if (type == TokenType.NOT)
			{
				Object val = this.GetUnaryExpressionValue(0, paramlist);
				if (val instanceof Boolean)
					return !((boolean)val);
		
				tree.Errors.add(new ParseError("Illegal UnaryExpression format, cannot interpret negate " + val.toString(), 1098, this));
				return null;
			}
		
			tree.Errors.add(new ParseError("Illegal UnaryExpression format", 1099, this));
			return null;
	}

	protected Object GetUnaryExpressionValue(int index, Object... paramlist)
	{
		Object o = null;
		ParseNode node = GetTokenNode(TokenType.UnaryExpression, index);
		if (node != null)
			o = node.EvalUnaryExpression(paramlist);
		return o;
	}

	protected Object EvalPowerExpression(Object... paramlist)
	{
			ParseTree tree = (ParseTree)paramlist[0];
			Object result = this.GetUnaryExpressionValue(0, paramlist);
		
			// IMPORTANT: scanning and calculating the power is done from Left to Right.
			// this is conform the Excel evaluation of power, but not conform strict mathematical guidelines
			// this means that a^b^c evaluates to (a^b)^c  (Excel uses the same kind of evaluation)
			// stricly mathematical speaking a^b^c should evaluate to a^(b^c) (therefore calculating the powers from Right to Left)
			for (int i = 1; i < nodes.size(); i += 2)
			{
				Token token = nodes.get(i).Token;
				Object val = nodes.get(i+1).Eval(tree, paramlist);
				if (token.Type == TokenType.POWER)
					result = result = Math.pow((double)(result), (double)(val));
			}
		
			return result;
	}

	protected Object GetPowerExpressionValue(int index, Object... paramlist)
	{
		Object o = null;
		ParseNode node = GetTokenNode(TokenType.PowerExpression, index);
		if (node != null)
			o = node.EvalPowerExpression(paramlist);
		return o;
	}

	protected Object EvalMultiplicativeExpression(Object... paramlist)
	{
			ParseTree tree = (ParseTree)paramlist[0];
			Object result = this.GetPowerExpressionValue(0, paramlist);
			for (int i = 1; i < nodes.size(); i+=2 )
			{
				Token token = nodes.get(i).Token;
				Object val = nodes.get(i+1).Eval(tree, paramlist);
				if (token.Type == TokenType.ASTERIKS)
					result = Util.ConvertToDouble(result) * Util.ConvertToDouble(val);
				else if (token.Type == TokenType.SLASH)
						result = Util.ConvertToDouble(result) / Util.ConvertToDouble(val);
				else if (token.Type == TokenType.PERCENT)
					result = Util.ConvertToDouble(result) % Util.ConvertToDouble(val);
			}
		
			return result;
	}

	protected Object GetMultiplicativeExpressionValue(int index, Object... paramlist)
	{
		Object o = null;
		ParseNode node = GetTokenNode(TokenType.MultiplicativeExpression, index);
		if (node != null)
			o = node.EvalMultiplicativeExpression(paramlist);
		return o;
	}

	protected Object EvalAdditiveExpression(Object... paramlist)
	{
			ParseTree tree = (ParseTree)paramlist[0];
			Object result = this.GetMultiplicativeExpressionValue(0, paramlist);
			for (int i = 1; i < nodes.size(); i += 2)
			{
				Token token = nodes.get(i).Token;
				Object val = nodes.get(i+1).Eval(tree, paramlist);
				if (token.Type == TokenType.PLUS)
					result = Util.ConvertToDouble(result) + Util.ConvertToDouble(val);
				else if (token.Type == TokenType.MINUS)
					result = Util.ConvertToDouble(result) - Util.ConvertToDouble(val);
			}
		
			return result;
	}

	protected Object GetAdditiveExpressionValue(int index, Object... paramlist)
	{
		Object o = null;
		ParseNode node = GetTokenNode(TokenType.AdditiveExpression, index);
		if (node != null)
			o = node.EvalAdditiveExpression(paramlist);
		return o;
	}

	protected Object EvalConcatEpression(Object... paramlist)
	{
			ParseTree tree = (ParseTree)paramlist[0];
			Object result = this.GetAdditiveExpressionValue(0, paramlist);
			for (int i = 1; i < nodes.size(); i += 2)
			{
				Token token = nodes.get(i).Token;
				Object val = nodes.get(i+1).Eval(tree, paramlist);
				if (token.Type == TokenType.AMP)
					result = Util.ConvertToString(result) + Util.ConvertToString(val);
			}
			return result;
	}

	protected Object GetConcatEpressionValue(int index, Object... paramlist)
	{
		Object o = null;
		ParseNode node = GetTokenNode(TokenType.ConcatEpression, index);
		if (node != null)
			o = node.EvalConcatEpression(paramlist);
		return o;
	}

	protected Object EvalRelationalExpression(Object... paramlist)
	{
			ParseTree tree = (ParseTree)paramlist[0];
			Object result = this.GetConcatEpressionValue(0, paramlist);
			for (int i = 1; i < nodes.size(); i += 2)
			{
				Token token = nodes.get(i).Token;
				Object val = nodes.get(i+1).Eval(tree, paramlist);
		
				// compare as numbers
				if (result instanceof Double && val instanceof Double)
				{
					if (token.Type == TokenType.LESSTHAN)
						result = Util.ConvertToDouble(result) < Util.ConvertToDouble(val);
					else if (token.Type == TokenType.LESSEQUAL)
						result = Util.ConvertToDouble(result) <= Util.ConvertToDouble(val);
					else if (token.Type == TokenType.GREATERTHAN)
						result = Util.ConvertToDouble(result) > Util.ConvertToDouble(val);
					else if (token.Type == TokenType.GREATEREQUAL)
						result = Util.ConvertToDouble(result) >= Util.ConvertToDouble(val);
				}
				else // compare as strings
				{
					int comp = Util.ConvertToString(result).compareTo(Util.ConvertToString(val));
					if (token.Type == TokenType.LESSTHAN)
						result = comp < 0;
					else if (token.Type == TokenType.LESSEQUAL)
						result = comp <= 0;
					else if (token.Type == TokenType.GREATERTHAN)
						result = comp > 0;
					else if (token.Type == TokenType.GREATEREQUAL)
						result = comp >= 0;
				}
				
			}
			return result;
	}

	protected Object GetRelationalExpressionValue(int index, Object... paramlist)
	{
		Object o = null;
		ParseNode node = GetTokenNode(TokenType.RelationalExpression, index);
		if (node != null)
			o = node.EvalRelationalExpression(paramlist);
		return o;
	}

	protected Object EvalEqualityExpression(Object... paramlist)
	{
			ParseTree tree = (ParseTree)paramlist[0];
			Object result = this.GetRelationalExpressionValue(0, paramlist);
			for (int i = 1; i < nodes.size(); i += 2)
			{
				Token token = nodes.get(i).Token;
				Object val = nodes.get(i+1).Eval(tree, paramlist);
				if (token.Type == TokenType.EQUAL)
					result = result.equals(val);
				else if (token.Type == TokenType.NOTEQUAL)
					result = !result.equals(val);
			}
			return result;
	}

	protected Object GetEqualityExpressionValue(int index, Object... paramlist)
	{
		Object o = null;
		ParseNode node = GetTokenNode(TokenType.EqualityExpression, index);
		if (node != null)
			o = node.EvalEqualityExpression(paramlist);
		return o;
	}

	protected Object EvalConditionalAndExpression(Object... paramlist)
	{
			ParseTree tree = (ParseTree)paramlist[0];
			Object result = this.GetEqualityExpressionValue(0, paramlist);
			for (int i = 1; i < nodes.size(); i += 2)
			{
				Token token = nodes.get(i).Token;
				Object val = nodes.get(i+1).Eval(tree, paramlist);
				if (token.Type == TokenType.AMPAMP)
					result = Util.ConvertToBoolean(result) && Util.ConvertToBoolean(val);
			}
			return result;
	}

	protected Object GetConditionalAndExpressionValue(int index, Object... paramlist)
	{
		Object o = null;
		ParseNode node = GetTokenNode(TokenType.ConditionalAndExpression, index);
		if (node != null)
			o = node.EvalConditionalAndExpression(paramlist);
		return o;
	}

	protected Object EvalConditionalOrExpression(Object... paramlist)
	{
			ParseTree tree = (ParseTree)paramlist[0];
			Object result = this.GetConditionalAndExpressionValue(0, paramlist);
			for (int i = 1; i < nodes.size(); i += 2)
			{
				Token token = nodes.get(i).Token;
				Object val = nodes.get(i+1).Eval(tree, paramlist);
				if (token.Type == TokenType.PIPEPIPE)
					result = Util.ConvertToBoolean(result) || Util.ConvertToBoolean(val);
			}
			return result;
	}

	protected Object GetConditionalOrExpressionValue(int index, Object... paramlist)
	{
		Object o = null;
		ParseNode node = GetTokenNode(TokenType.ConditionalOrExpression, index);
		if (node != null)
			o = node.EvalConditionalOrExpression(paramlist);
		return o;
	}

	protected Object EvalAssignmentExpression(Object... paramlist)
	{
			ParseTree tree = (ParseTree)paramlist[0];
			Object result = this.GetConditionalOrExpressionValue(0, paramlist);
			if (nodes.size() >= 5 && result instanceof Boolean
				&& nodes.get(1).Token.Type == TokenType.QUESTIONMARK
				&& nodes.get(3).Token.Type == TokenType.COLON)
			{
				if (Util.ConvertToBoolean(result))
					result = nodes.get(2).Eval(tree, paramlist); // return 1st argument
				else
					result = nodes.get(4).Eval(tree, paramlist); // return 2nd argument
			}
			return result;
	}

	protected Object GetAssignmentExpressionValue(int index, Object... paramlist)
	{
		Object o = null;
		ParseNode node = GetTokenNode(TokenType.AssignmentExpression, index);
		if (node != null)
			o = node.EvalAssignmentExpression(paramlist);
		return o;
	}

	protected Object EvalExpression(Object... paramlist)
	{
			ParseTree tree = (ParseTree)paramlist[0];
			// if only left hand side available, this is not an assignment, simple evaluate expression
			if (nodes.size() == 1)                 
				return this.GetAssignmentExpressionValue(0, paramlist); // return the result
		
			if (nodes.size() != 3)
			{
				tree.Errors.add(new ParseError("Illegal EvalExpression format", 1092, this));
				return null;
			}
		
			// ok, this is an assignment so declare the function or variable
			// assignment only allowed to function or to a variable
			ParseNode v = GetFunctionOrVariable(nodes.get(0));
			if (v == null)
			{
				tree.Errors.add(new ParseError("Can only assign to function or variable", 1020, this));
				return null;
			}
		
			if (tree.Context == null)
			{
				tree.Errors.add(new ParseError("No context defined", 1041, this));
				return null;
			}
		
			if (v.Token.Type == TokenType.VARIABLE)
			{
				// simply overwrite any previous defnition
				String key = v.Token.getText();
				tree.Context.getGlobals().put(key, this.GetAssignmentExpressionValue(1, paramlist));
				return tree.Context.getGlobals().get(key) ;
			}
			else if (v.Token.Type == TokenType.Function)
			{
		
				String key = v.getNodes().get(0).Token.getText();
		
				// function lookup is case insensitive
				if (tree.Context.getFunctions().containsKey(key.toLowerCase()))
					if (!(tree.Context.getFunctions().get(key.toLowerCase()) instanceof DynamicFunction))
					{
						tree.Errors.add(new ParseError("Built in functions cannot be overwritten", 1050, this));
						return null;
					}
		
				// lets determine the input variables. 
				// functions must be of te form f(x;y;z) = x+y*z;
				// check the function parameters to be of type Variable, error otherwise
				Variables vars = new Variables();
				ParseNode paramsNode = v.getNodes().get(2);
				if (paramsNode.Token.Type == TokenType.Params)
				{   // function has parameters, so check if they are all variable declarations
					for (int i = 0; i < paramsNode.getNodes().size(); i += 2)
					{
						ParseNode varNode = GetFunctionOrVariable(paramsNode.getNodes().get(i));
						if (varNode == null || varNode.Token.Type != TokenType.VARIABLE)
						{
							tree.Errors.add(new ParseError("Function declaration may only contain variables", 1051, this));
							return null;
						}
						// simply declare the variable, no need to evaluate the value of it at this point. 
						// evaluation will be done when the function is executed
						// note, variables are Case Sensitive (!)
						vars.put(varNode.Token.getText(), null);
					}
				}
				// we have all the info we need to know to declare the dynamicly defined function
				// pass on nodes[2] which is the Right Hand Side (RHS) of the assignment
				// nodes[2] will be evaluated at runtime when the function is executed.
				DynamicFunction dynf = new DynamicFunction(key, nodes.get(2), vars, vars.size(), vars.size());
				tree.Context.getFunctions().put(key.toLowerCase(), dynf);
				return dynf;
			}
		
			return null;
	}

	protected Object GetExpressionValue(int index, Object... paramlist)
	{
		Object o = null;
		ParseNode node = GetTokenNode(TokenType.Expression, index);
		if (node != null)
			o = node.EvalExpression(paramlist);
		return o;
	}

	protected Object EvalParams(Object... paramlist)
	{
			ParseTree tree = (ParseTree)paramlist[0];
			ArrayList<Object> parameters = new ArrayList<Object>();
			for (int i = 0; i < nodes.size(); i += 2)
			{
				if (nodes.get(i).Token.Type == TokenType.Expression)
				{
					Object val = nodes.get(i).Eval(tree, paramlist);
					parameters.add(val);
				}
			}
			return parameters;
	}

	protected Object GetParamsValue(int index, Object... paramlist)
	{
		Object o = null;
		ParseNode node = GetTokenNode(TokenType.Params, index);
		if (node != null)
			o = node.EvalParams(paramlist);
		return o;
	}

	protected Object EvalLiteral(Object... paramlist)
	{
			ParseTree tree = (ParseTree)paramlist[0];
			TokenType type = this.nodes.get(0).Token.Type;
			if (type == TokenType.StringLiteral)
				return this.GetStringLiteralValue(0, paramlist);
			else if (type == TokenType.IntegerLiteral)
				return this.GetIntegerLiteralValue(0, paramlist);
			else if (type == TokenType.RealLiteral)
				return this.GetRealLiteralValue(0, paramlist);
			else if (type == TokenType.BOOLEANLITERAL)
			{
				String val = this.GetTerminalValue(TokenType.BOOLEANLITERAL, 0).toString();
				return Util.ConvertToBoolean(val);
			}
		
			tree.Errors.add(new ParseError("illegal Literal format", 1003, this));
			return null;
	}

	protected Object GetLiteralValue(int index, Object... paramlist)
	{
		Object o = null;
		ParseNode node = GetTokenNode(TokenType.Literal, index);
		if (node != null)
			o = node.EvalLiteral(paramlist);
		return o;
	}

	protected Object EvalIntegerLiteral(Object... paramlist)
	{
			ParseTree tree = (ParseTree)paramlist[0];
			if (this.GetTerminalValue(TokenType.DECIMALINTEGERLITERAL, 0) != null)
				return Util.ConvertToDouble(this.GetTerminalValue(TokenType.DECIMALINTEGERLITERAL, 0));
			if (this.GetTerminalValue(TokenType.HEXINTEGERLITERAL, 0) != null)
			{
				String hex = this.GetTerminalValue(TokenType.HEXINTEGERLITERAL, 0).toString();
				return Util.ConvertToDouble(Integer.decode(hex));
			}
		
			tree.Errors.add(new ParseError("illegal IntegerLiteral format", 1002, this));
			return null;
	}

	protected Object GetIntegerLiteralValue(int index, Object... paramlist)
	{
		Object o = null;
		ParseNode node = GetTokenNode(TokenType.IntegerLiteral, index);
		if (node != null)
			o = node.EvalIntegerLiteral(paramlist);
		return o;
	}

	protected Double EvalRealLiteral(Object... paramlist)
	{
			ParseTree tree = (ParseTree)paramlist[0];
			if (this.GetTerminalValue(TokenType.REALLITERAL, 0) != null)
			{
				return Util.ConvertToDouble(this.GetTerminalValue(TokenType.REALLITERAL, 0));
			}
			tree.Errors.add(new ParseError("illegal RealLiteral format", 1001, this));
			return null;
	}

	protected Double GetRealLiteralValue(int index, Object... paramlist)
	{
		Double o = null;
		ParseNode node = GetTokenNode(TokenType.RealLiteral, index);
		if (node != null)
			o = node.EvalRealLiteral(paramlist);
		return o;
	}

	protected String EvalStringLiteral(Object... paramlist)
	{
			ParseTree tree = (ParseTree)paramlist[0];
			if (this.GetTerminalValue(TokenType.STRINGLITERAL, 0) != null)
			{
				String r = (String)this.GetTerminalValue(TokenType.STRINGLITERAL, 0);
				r = r.substring(1, r.length() - 3); // strip quotes
				return r;
			}
		
			tree.Errors.add(new ParseError("illegal StringLiteral format", 1000, this));
			return "";
	}

	protected String GetStringLiteralValue(int index, Object... paramlist)
	{
		String o = null;
		ParseNode node = GetTokenNode(TokenType.StringLiteral, index);
		if (node != null)
			o = node.EvalStringLiteral(paramlist);
		return o;
	}

	protected Object EvalVariable(Object... paramlist)
	{
			ParseTree tree = (ParseTree)paramlist[0];
			if (tree.Context == null)
			{
				tree.Errors.add(new ParseError("No context defined", 1041, this));
				return null;
			}
		
			String key = (String)this.GetTerminalValue(TokenType.VARIABLE, 0);
			// first check if the variable was declared in scope of a function
			var scope_var = tree.Context.GetScopeVariable(key);
			if(scope_var != null)
				return scope_var;
			
			// if not in scope of a function
			// next check if the variable was declared as a global variable
			if (tree.Context.getGlobals() != null && tree.Context.getGlobals().containsKey(key))
				return tree.Context.getGlobals().get(key);
		
			//variable not found
			tree.Errors.add(new ParseError("Variable not defined: " + key, 1039, this));
			return null;
	}

	protected Object GetVariableValue(int index, Object... paramlist)
	{
		Object o = null;
		ParseNode node = GetTokenNode(TokenType.Variable, index);
		if (node != null)
			o = node.EvalVariable(paramlist);
		return o;
	}





	// helper function to find access the function or variable
	private ParseNode GetFunctionOrVariable(ParseNode n)
	{
		// found the right node, exit
		if (n.Token.Type == TokenType.Function || n.Token.Type == TokenType.VARIABLE)
			return n;

		if (n.getNodes().size() == 1) // search lower branch (left side only, may not contain other node branches)
			return GetFunctionOrVariable(n.getNodes().get(0));

		// function or variable not found in branch
		return null;
	}

	public Context Context = new Context();

}

