// Automatically generated from source file: TinyExpEval.tpg
// By TinyPG v1.5 available at https://github.com/ultrasuperpingu/TinyPG


using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Globalization;
using System.Linq;

namespace TinyExe
{
	#region ParseTree
	[Serializable]
	public class ParseErrors : List<ParseError>
	{
		public bool ContainsErrors
		{
			get { return Find(e => e.IsWarning == false) != null; }
		}
		public bool ContainsWarnings
	{
			get { return Find(e => e.IsWarning == true) != null; }
		}
		public List<ParseError> Warnings
		{
			get { return FindAll(e => e.IsWarning == true); }
		}
		public List<ParseError> BlockingErrors
		{
			get { return FindAll(e => e.IsWarning == false); }
		}
	}

	[Serializable]
	public class ParseError
	{
		private string message;
		private int code;
		private int line;
		private int col;
		private int pos;
		private int length;
		private bool isWarning;

		public int Code { get { return code; } }
		public int Line { get { return line; } }
		public int Column { get { return col; } }
		public int Position { get { return pos; } }
		public int Length { get { return length; } }
		public string Message { get { return message; } }
		public bool IsWarning { get { return isWarning; } }

		// just for the sake of serialization
		public ParseError()
		{
		}

		public ParseError(string message, int code, ParseNode node, bool isWarning = false) : this(message, code, node.Token)
		{
		}

		public ParseError(string message, int code, Token token, bool isWarning = false) : this(message, code, token.Line, token.Column, token.StartPos, token.Length, isWarning)
		{
		}

		public ParseError(string message, int code, bool isWarning = false) : this(message, code, 0, 0, 0, 0, isWarning)
		{
		}

		public ParseError(string message, int code, int line, int col, int pos, int length, bool isWarning = false)
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
	[Serializable]
	public partial class ParseTree : ParseNode
	{
		public ParseErrors Errors;

		public List<Token> Skipped;

		public ParseTree() : base(new Token(), "ParseTree")
		{
			Token.Type = TokenType.Start;
			Token.Text = "Root";
			Errors = new ParseErrors();
		}

		public string PrintTree()
		{
			StringBuilder sb = new StringBuilder();
			int indent = 0;
			PrintNode(sb, this, indent);
			return sb.ToString();
		}

		private void PrintNode(StringBuilder sb, ParseNode node, int indent)
		{
			string space = "".PadLeft(indent, ' ');

			sb.Append(space);
			sb.AppendLine(node.Text);

			foreach (ParseNode n in node.Nodes)
				PrintNode(sb, n, indent + 2);
		}

		/// <summary>
		/// this is the entry point for executing and evaluating the parse tree.
		/// </summary>
		/// <param name="paramlist">additional optional input parameters</param>
		/// <returns>the output of the evaluation function</returns>
		public object Eval(params object[] paramlist)
		{
			return Nodes[0].EvalNode(this, paramlist);
		}
	}

	[Serializable]
	[XmlInclude(typeof(ParseTree))]
	public partial class ParseNode
	{
		protected string text;
		protected List<ParseNode> nodes;
		
		public List<ParseNode> Nodes { get {return nodes;} }
		
		[XmlIgnore] // avoid circular references when serializing
		public ParseNode Parent;
		public Token Token; // the token/rule

		[XmlIgnore] // skip redundant text (is part of Token)
		public string Text { // text to display in parse tree 
			get { return text;} 
			set { text = value; }
		} 

		public virtual ParseNode CreateNode(Token token, string text)
		{
			ParseNode node = new ParseNode(token, text);
			node.Parent = this;
			return node;
		}

		protected ParseNode(Token token, string text)
		{
			this.Token = token;
			this.text = text;
			this.nodes = new List<ParseNode>();
		}

		protected virtual ParseNode GetTokenNode(TokenType type, int index)
		{
			if (index < 0)
				return null;
			// left to right
			foreach (ParseNode node in nodes)
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

		protected virtual bool IsTokenPresent(TokenType type, int index)
		{
			ParseNode node = GetTokenNode(type, index);
			return node != null;
		}

		protected virtual string GetTerminalValue(TokenType type, int index)
		{
			ParseNode node = GetTokenNode(type, index);
			if (node != null)
				return node.Token.Text;
			return null;
		}

		protected object GetValue(TokenType type, int index, params object[] paramlist)
		{
			return GetValue(type, ref index, paramlist);
		}

		protected object GetValue(TokenType type, ref int index, params object[] paramlist)
		{
			object o = null;
			if (index < 0) return o;

			// left to right
			foreach (ParseNode node in nodes)
			{
				if (node.Token.Type == type)
				{
					index--;
					if (index < 0)
					{
						o = node.EvalNode(paramlist);
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
		internal object EvalNode(params object[] paramlist)
		{
			object Value = null;

			switch (Token.Type)
			{
				case TokenType.Start:
					Value = EvalStart(paramlist);
				break;
				case TokenType.Function:
					Value = EvalFunction(paramlist);
				break;
				case TokenType.PrimaryExpression:
					Value = EvalPrimaryExpression(paramlist);
				break;
				case TokenType.ParenthesizedExpression:
					Value = EvalParenthesizedExpression(paramlist);
				break;
				case TokenType.UnaryExpression:
					Value = EvalUnaryExpression(paramlist);
				break;
				case TokenType.PowerExpression:
					Value = EvalPowerExpression(paramlist);
				break;
				case TokenType.MultiplicativeExpression:
					Value = EvalMultiplicativeExpression(paramlist);
				break;
				case TokenType.AdditiveExpression:
					Value = EvalAdditiveExpression(paramlist);
				break;
				case TokenType.ConcatEpression:
					Value = EvalConcatEpression(paramlist);
				break;
				case TokenType.RelationalExpression:
					Value = EvalRelationalExpression(paramlist);
				break;
				case TokenType.EqualityExpression:
					Value = EvalEqualityExpression(paramlist);
				break;
				case TokenType.ConditionalAndExpression:
					Value = EvalConditionalAndExpression(paramlist);
				break;
				case TokenType.ConditionalOrExpression:
					Value = EvalConditionalOrExpression(paramlist);
				break;
				case TokenType.AssignmentExpression:
					Value = EvalAssignmentExpression(paramlist);
				break;
				case TokenType.Expression:
					Value = EvalExpression(paramlist);
				break;
				case TokenType.Params:
					Value = EvalParams(paramlist);
				break;
				case TokenType.Literal:
					Value = EvalLiteral(paramlist);
				break;
				case TokenType.IntegerLiteral:
					Value = EvalIntegerLiteral(paramlist);
				break;
				case TokenType.RealLiteral:
					Value = EvalRealLiteral(paramlist);
				break;
				case TokenType.StringLiteral:
					Value = EvalStringLiteral(paramlist);
				break;
				case TokenType.Variable:
					Value = EvalVariable(paramlist);
				break;

				default:
					Value = Token.Text;
					break;
			}
			return Value;
		}

		protected virtual object EvalStart(params object[] paramlist)
		{
			return this.GetExpressionValue(0, paramlist);
		}

		protected virtual object GetStartValue(int index, params object[] paramlist )
		{
			object o = default(object);
			ParseNode node = GetTokenNode(TokenType.Start, index);
			if (node != null)
				o = node.EvalStart(paramlist);
			return o;
		}

		protected virtual object EvalFunction(params object[] paramlist)
		{
			ParseTree tree = paramlist[0] as ParseTree;
			ParseNode funcNode = this.nodes[0];
			ParseNode paramNode = this.nodes[2];
		
			if (tree.Context == null)
			{
				tree.Errors.Add(new ParseError("No context defined", 1041, this));
				return null;
			}
			if (tree.Context.CurrentStackSize > 50)
			{
				tree.Errors.Add(new ParseError("Stack overflow: " + funcNode.Token.Text + "()", 1046, this));
				return null;
			}
			string key = funcNode.Token.Text.ToLowerInvariant();
			if (!tree.Context.Functions.ContainsKey(key))
			{
				tree.Errors.Add(new ParseError("Fuction not defined: " + funcNode.Token.Text + "()", 1042, this));
				return null;
			}
		
			// retrieves the function from declared functions
			Function func = tree.Context.Functions[key];
		
			// evaluate the function parameters
			object[] parameters = new object[0];
			if (paramNode.Token.Type == TokenType.Params)
				parameters = (paramNode.EvalNode(tree, paramlist) as List<object>).ToArray();
			if (parameters.Length < func.MinParameters) 
			{
				tree.Errors.Add(new ParseError("At least " + func.MinParameters.ToString() + " parameter(s) expected", 1043, this));
				return null; // illegal number of parameters
			}
			else if (parameters.Length > func.MaxParameters)
			{
				tree.Errors.Add(new ParseError("No more than " + func.MaxParameters.ToString() + " parameter(s) expected", 1044, this));
				return null; // illegal number of parameters
			}
			
			var fres = func.Eval(parameters, tree);
			//string t_params = String.Join("; ", parameters);
			//Tracer.Trace("Func", "Eval "+func.Name+"("+ t_params + ") = "+fres);
			return fres;
		}

		protected virtual object GetFunctionValue(int index, params object[] paramlist )
		{
			object o = default(object);
			ParseNode node = GetTokenNode(TokenType.Function, index);
			if (node != null)
				o = node.EvalFunction(paramlist);
			return o;
		}

		protected virtual object EvalPrimaryExpression(params object[] paramlist)
		{
			ParseTree tree = paramlist[0] as ParseTree;
			TokenType type = this.nodes[0].Token.Type;
			if (type == TokenType.Function)
				return this.GetFunctionValue(0, paramlist);
			else if (type == TokenType.Literal)
				return this.GetLiteralValue(0, paramlist);
			else if (type == TokenType.ParenthesizedExpression)
				return this.GetParenthesizedExpressionValue(0, paramlist);
			else if (type == TokenType.Variable)
				return this.GetVariableValue(0, paramlist);
		
			tree.Errors.Add(new ParseError("Illegal EvalPrimaryExpression format", 1097, this));
			return null;
		}

		protected virtual object GetPrimaryExpressionValue(int index, params object[] paramlist )
		{
			object o = default(object);
			ParseNode node = GetTokenNode(TokenType.PrimaryExpression, index);
			if (node != null)
				o = node.EvalPrimaryExpression(paramlist);
			return o;
		}

		protected virtual object EvalParenthesizedExpression(params object[] paramlist)
		{
			return this.GetExpressionValue(0, paramlist);
		}

		protected virtual object GetParenthesizedExpressionValue(int index, params object[] paramlist )
		{
			object o = default(object);
			ParseNode node = GetTokenNode(TokenType.ParenthesizedExpression, index);
			if (node != null)
				o = node.EvalParenthesizedExpression(paramlist);
			return o;
		}

		protected virtual object EvalUnaryExpression(params object[] paramlist)
		{
			ParseTree tree = paramlist[0] as ParseTree;
			TokenType type = this.nodes[0].Token.Type;
			if (type == TokenType.PrimaryExpression)
				return this.GetPrimaryExpressionValue(0, paramlist);
		
			if (type == TokenType.MINUS)
			{
				object val = this.GetUnaryExpressionValue(0, paramlist);
				if (val is double)
					return -((double)val);
		
				tree.Errors.Add(new ParseError("Illegal UnaryExpression format, cannot interpret minus " + val.ToString(), 1095, this));
				return null;
			}
			else if (type == TokenType.PLUS)
			{
				object val = this.GetUnaryExpressionValue(0, paramlist);
				return val;
			}
			else if (type == TokenType.NOT)
			{
				object val = this.GetUnaryExpressionValue(0, paramlist);
				if (val is bool)
					return !((bool)val);
		
				tree.Errors.Add(new ParseError("Illegal UnaryExpression format, cannot interpret negate " + val.ToString(), 1098, this));
				return null;
			}
		
			tree.Errors.Add(new ParseError("Illegal UnaryExpression format", 1099, this));
			return null;
		}

		protected virtual object GetUnaryExpressionValue(int index, params object[] paramlist )
		{
			object o = default(object);
			ParseNode node = GetTokenNode(TokenType.UnaryExpression, index);
			if (node != null)
				o = node.EvalUnaryExpression(paramlist);
			return o;
		}

		protected virtual object EvalPowerExpression(params object[] paramlist)
		{
			ParseTree tree = paramlist[0] as ParseTree;
			object result = this.GetUnaryExpressionValue(0, paramlist);
		
			// IMPORTANT: scanning and calculating the power is done from Left to Right.
			// this is conform the Excel evaluation of power, but not conform strict mathematical guidelines
			// this means that a^b^c evaluates to (a^b)^c  (Excel uses the same kind of evaluation)
			// stricly mathematical speaking a^b^c should evaluate to a^(b^c) (therefore calculating the powers from Right to Left)
			for (int i = 1; i < nodes.Count; i += 2)
			{
				Token token = nodes[i].Token;
				object val = nodes[i + 1].EvalNode(tree, paramlist);
				if (token.Type == TokenType.POWER)
					result = Math.Pow(Convert.ToDouble(result), Convert.ToDouble(val));
			}
		
			return result;
		}

		protected virtual object GetPowerExpressionValue(int index, params object[] paramlist )
		{
			object o = default(object);
			ParseNode node = GetTokenNode(TokenType.PowerExpression, index);
			if (node != null)
				o = node.EvalPowerExpression(paramlist);
			return o;
		}

		protected virtual object EvalMultiplicativeExpression(params object[] paramlist)
		{
			ParseTree tree = paramlist[0] as ParseTree;
			object result = this.GetPowerExpressionValue(0, paramlist);
			for (int i = 1; i < nodes.Count; i+=2 )
			{
				Token token = nodes[i].Token;
				object val = nodes[i+1].EvalNode(tree, paramlist);
				if (token.Type == TokenType.ASTERIKS)
					result = Convert.ToDouble(result) * Convert.ToDouble(val);
				else if (token.Type == TokenType.SLASH)
						result = Convert.ToDouble(result) / Convert.ToDouble(val);
				else if (token.Type == TokenType.PERCENT)
					result = Convert.ToDouble(result) % Convert.ToDouble(val);
			}
		
			return result;
		}

		protected virtual object GetMultiplicativeExpressionValue(int index, params object[] paramlist )
		{
			object o = default(object);
			ParseNode node = GetTokenNode(TokenType.MultiplicativeExpression, index);
			if (node != null)
				o = node.EvalMultiplicativeExpression(paramlist);
			return o;
		}

		protected virtual object EvalAdditiveExpression(params object[] paramlist)
		{
			ParseTree tree = paramlist[0] as ParseTree;
			object result = this.GetMultiplicativeExpressionValue(0, paramlist);
			for (int i = 1; i < nodes.Count; i += 2)
			{
				Token token = nodes[i].Token;
				object val = nodes[i + 1].EvalNode(tree, paramlist);
				if (token.Type == TokenType.PLUS)
					result = Convert.ToDouble(result) + Convert.ToDouble(val);
				else if (token.Type == TokenType.MINUS)
					result = Convert.ToDouble(result) - Convert.ToDouble(val);
			}
		
			return result;
		}

		protected virtual object GetAdditiveExpressionValue(int index, params object[] paramlist )
		{
			object o = default(object);
			ParseNode node = GetTokenNode(TokenType.AdditiveExpression, index);
			if (node != null)
				o = node.EvalAdditiveExpression(paramlist);
			return o;
		}

		protected virtual object EvalConcatEpression(params object[] paramlist)
		{
			ParseTree tree = paramlist[0] as ParseTree;
			object result = this.GetAdditiveExpressionValue(0, paramlist);
			for (int i = 1; i < nodes.Count; i += 2)
			{
				Token token = nodes[i].Token;
				object val = nodes[i + 1].EvalNode(tree, paramlist);
				if (token.Type == TokenType.AMP)
					result = Convert.ToString(result) + Convert.ToString(val);
			}
			return result;
		}

		protected virtual object GetConcatEpressionValue(int index, params object[] paramlist )
		{
			object o = default(object);
			ParseNode node = GetTokenNode(TokenType.ConcatEpression, index);
			if (node != null)
				o = node.EvalConcatEpression(paramlist);
			return o;
		}

		protected virtual object EvalRelationalExpression(params object[] paramlist)
		{
			ParseTree tree = paramlist[0] as ParseTree;
			object result = this.GetConcatEpressionValue(0, paramlist);
			for (int i = 1; i < nodes.Count; i += 2)
			{
				Token token = nodes[i].Token;
				object val = nodes[i + 1].EvalNode(tree, paramlist);
		
				// compare as numbers
				if (result is double && val is double)
				{
					if (token.Type == TokenType.LESSTHAN)
						result = Convert.ToDouble(result) < Convert.ToDouble(val);
					else if (token.Type == TokenType.LESSEQUAL)
						result = Convert.ToDouble(result) <= Convert.ToDouble(val);
					else if (token.Type == TokenType.GREATERTHAN)
						result = Convert.ToDouble(result) > Convert.ToDouble(val);
					else if (token.Type == TokenType.GREATEREQUAL)
						result = Convert.ToDouble(result) >= Convert.ToDouble(val);
				}
				else // compare as strings
				{
					int comp = string.Compare(Convert.ToString(result), Convert.ToString(val));
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

		protected virtual object GetRelationalExpressionValue(int index, params object[] paramlist )
		{
			object o = default(object);
			ParseNode node = GetTokenNode(TokenType.RelationalExpression, index);
			if (node != null)
				o = node.EvalRelationalExpression(paramlist);
			return o;
		}

		protected virtual object EvalEqualityExpression(params object[] paramlist)
		{
			ParseTree tree = paramlist[0] as ParseTree;
			object result = this.GetRelationalExpressionValue(0, paramlist);
			for (int i = 1; i < nodes.Count; i += 2)
			{
				Token token = nodes[i].Token;
				object val = nodes[i + 1].EvalNode(tree, paramlist);
				if (token.Type == TokenType.EQUAL)
					result = object.Equals(result, val);
				else if (token.Type == TokenType.NOTEQUAL)
					result = !object.Equals(result, val);
			}
			return result;
		}

		protected virtual object GetEqualityExpressionValue(int index, params object[] paramlist )
		{
			object o = default(object);
			ParseNode node = GetTokenNode(TokenType.EqualityExpression, index);
			if (node != null)
				o = node.EvalEqualityExpression(paramlist);
			return o;
		}

		protected virtual object EvalConditionalAndExpression(params object[] paramlist)
		{
			ParseTree tree = paramlist[0] as ParseTree;
			object result = this.GetEqualityExpressionValue(0, paramlist);
			for (int i = 1; i < nodes.Count; i += 2)
			{
				Token token = nodes[i].Token;
				object val = nodes[i + 1].EvalNode(tree, paramlist);
				if (token.Type == TokenType.AMPAMP)
					result = Convert.ToBoolean(result) && Convert.ToBoolean(val);
			}
			return result;
		}

		protected virtual object GetConditionalAndExpressionValue(int index, params object[] paramlist )
		{
			object o = default(object);
			ParseNode node = GetTokenNode(TokenType.ConditionalAndExpression, index);
			if (node != null)
				o = node.EvalConditionalAndExpression(paramlist);
			return o;
		}

		protected virtual object EvalConditionalOrExpression(params object[] paramlist)
		{
			ParseTree tree = paramlist[0] as ParseTree;
			object result = this.GetConditionalAndExpressionValue(0, paramlist);
			for (int i = 1; i < nodes.Count; i += 2)
			{
				Token token = nodes[i].Token;
				object val = nodes[i + 1].EvalNode(tree, paramlist);
				if (token.Type == TokenType.PIPEPIPE)
					result = Convert.ToBoolean(result) || Convert.ToBoolean(val);
			}
			return result;
		}

		protected virtual object GetConditionalOrExpressionValue(int index, params object[] paramlist )
		{
			object o = default(object);
			ParseNode node = GetTokenNode(TokenType.ConditionalOrExpression, index);
			if (node != null)
				o = node.EvalConditionalOrExpression(paramlist);
			return o;
		}

		protected virtual object EvalAssignmentExpression(params object[] paramlist)
		{
			ParseTree tree = paramlist[0] as ParseTree;
			object result = this.GetConditionalOrExpressionValue(0, paramlist);
			if (nodes.Count >= 5 && result is bool 
				&& nodes[1].Token.Type == TokenType.QUESTIONMARK
				&& nodes[3].Token.Type == TokenType.COLON)
			{
				if (Convert.ToBoolean(result))
					result = nodes[2].EvalNode(tree, paramlist); // return 1st argument
				else
					result = nodes[4].EvalNode(tree, paramlist); // return 2nd argumen
			}
			return result;
		}

		protected virtual object GetAssignmentExpressionValue(int index, params object[] paramlist )
		{
			object o = default(object);
			ParseNode node = GetTokenNode(TokenType.AssignmentExpression, index);
			if (node != null)
				o = node.EvalAssignmentExpression(paramlist);
			return o;
		}

		protected virtual object EvalExpression(params object[] paramlist)
		{
			ParseTree tree = paramlist[0] as ParseTree;
			// if only left hand side available, this is not an assignment, simple evaluate expression
			if (nodes.Count == 1)                 
				return this.GetAssignmentExpressionValue(0, paramlist); // return the result
		
			if (nodes.Count != 3)
			{
				tree.Errors.Add(new ParseError("Illegal EvalExpression format", 1092, this));
				return null;
			}
		
			// ok, this is an assignment so declare the function or variable
			// assignment only allowed to function or to a variable
			ParseNode v = GetFunctionOrVariable(nodes[0]);
			if (v == null)
			{
				tree.Errors.Add(new ParseError("Can only assign to function or variable", 1020, this));
				return null;
			}
		
			if (tree.Context == null)
			{
				tree.Errors.Add(new ParseError("No context defined", 1041, this));
				return null;
			}
		
			if (v.Token.Type == TokenType.VARIABLE)
			{
				// simply overwrite any previous defnition
				string key = v.Token.Text;
				tree.Context.Globals[key] = this.GetAssignmentExpressionValue(1, paramlist);
				return tree.Context.Globals[key] ;
			}
			else if (v.Token.Type == TokenType.Function)
			{
		
				string key = v.Nodes[0].Token.Text;
		
				// function lookup is case insensitive
				if (tree.Context.Functions.ContainsKey(key.ToLower()))
					if (!(tree.Context.Functions[key.ToLower()] is DynamicFunction))
					{
						tree.Errors.Add(new ParseError("Built in functions cannot be overwritten", 1050, this));
						return null;
					}
		
				// lets determine the input variables. 
				// functions must be of te form f(x;y;z) = x+y*z;
				// check the function parameters to be of type Variable, error otherwise
				Variables vars = new Variables();
				ParseNode paramsNode = v.Nodes[2];
				if (paramsNode.Token.Type == TokenType.Params)
				{   // function has parameters, so check if they are all variable declarations
					for (int i = 0; i < paramsNode.Nodes.Count; i += 2)
					{
						ParseNode varNode = GetFunctionOrVariable(paramsNode.Nodes[i]);
						if (varNode == null || varNode.Token.Type != TokenType.VARIABLE)
						{
							tree.Errors.Add(new ParseError("Function declaration may only contain variables", 1051, this));
							return null;
						}
						// simply declare the variable, no need to evaluate the value of it at this point. 
						// evaluation will be done when the function is executed
						// note, variables are Case Sensitive (!)
						vars.Add(varNode.Token.Text, null);
					}
				}
				// we have all the info we need to know to declare the dynamicly defined function
				// pass on nodes[2] which is the Right Hand Side (RHS) of the assignment
				// nodes[2] will be evaluated at runtime when the function is executed.
				DynamicFunction dynf = new DynamicFunction(key, nodes[2], vars, vars.Count, vars.Count);
				tree.Context.Functions[key.ToLower()] = dynf;
				return dynf;
			}
		
			return null;
		}

		protected virtual object GetExpressionValue(int index, params object[] paramlist )
		{
			object o = default(object);
			ParseNode node = GetTokenNode(TokenType.Expression, index);
			if (node != null)
				o = node.EvalExpression(paramlist);
			return o;
		}

		protected virtual object EvalParams(params object[] paramlist)
		{
			ParseTree tree = paramlist[0] as ParseTree;
			List<object> parameters = new List<object>();
			for (int i = 0; i < nodes.Count; i += 2)
			{
				if (nodes[i].Token.Type == TokenType.Expression)
				{
					object val = nodes[i].EvalNode(tree, paramlist);
					parameters.Add(val);
				}
			}
			return parameters;
		}

		protected virtual object GetParamsValue(int index, params object[] paramlist )
		{
			object o = default(object);
			ParseNode node = GetTokenNode(TokenType.Params, index);
			if (node != null)
				o = node.EvalParams(paramlist);
			return o;
		}

		protected virtual object EvalLiteral(params object[] paramlist)
		{
			ParseTree tree = paramlist[0] as ParseTree;
			TokenType type = this.nodes[0].Token.Type;
			if (type == TokenType.StringLiteral)
				return this.GetStringLiteralValue(0, paramlist);
			else if (type == TokenType.IntegerLiteral)
				return this.GetIntegerLiteralValue(0, paramlist);
			else if (type == TokenType.RealLiteral)
				return this.GetRealLiteralValue(0, paramlist);
			else if (type == TokenType.BOOLEANLITERAL)
			{
				string val = this.GetTerminalValue(TokenType.BOOLEANLITERAL, 0).ToString();
				return Convert.ToBoolean(val);
			}
		
			tree.Errors.Add(new ParseError("illegal Literal format", 1003, this));
			return null;
		}

		protected virtual object GetLiteralValue(int index, params object[] paramlist )
		{
			object o = default(object);
			ParseNode node = GetTokenNode(TokenType.Literal, index);
			if (node != null)
				o = node.EvalLiteral(paramlist);
			return o;
		}

		protected virtual object EvalIntegerLiteral(params object[] paramlist)
		{
			ParseTree tree = paramlist[0] as ParseTree;
			if (this.GetTerminalValue(TokenType.DECIMALINTEGERLITERAL, 0) != null)
				return Convert.ToDouble(this.GetTerminalValue(TokenType.DECIMALINTEGERLITERAL, 0));
			if (this.GetTerminalValue(TokenType.HEXINTEGERLITERAL, 0) != null)
			{
				string hex = this.GetTerminalValue(TokenType.HEXINTEGERLITERAL, 0).ToString();
				return Convert.ToDouble(Convert.ToInt64(hex.Substring(2, hex.Length - 2), 16));
			}
		
			tree.Errors.Add(new ParseError("illegal IntegerLiteral format", 1002, this));
			return null;
		}

		protected virtual object GetIntegerLiteralValue(int index, params object[] paramlist )
		{
			object o = default(object);
			ParseNode node = GetTokenNode(TokenType.IntegerLiteral, index);
			if (node != null)
				o = node.EvalIntegerLiteral(paramlist);
			return o;
		}

		protected virtual object EvalRealLiteral(params object[] paramlist)
		{
			ParseTree tree = paramlist[0] as ParseTree;
			if (this.GetTerminalValue(TokenType.REALLITERAL, 0) != null)
			{
				string val = string.Format(CultureInfo.InvariantCulture, "{0}", this.GetTerminalValue(TokenType.REALLITERAL, 0));
				return double.Parse(val, CultureInfo.InvariantCulture);
			}
			tree.Errors.Add(new ParseError("illegal RealLiteral format", 1001, this));
			return null;
		}

		protected virtual object GetRealLiteralValue(int index, params object[] paramlist )
		{
			object o = default(object);
			ParseNode node = GetTokenNode(TokenType.RealLiteral, index);
			if (node != null)
				o = node.EvalRealLiteral(paramlist);
			return o;
		}

		protected virtual object EvalStringLiteral(params object[] paramlist)
		{
			ParseTree tree = paramlist[0] as ParseTree;
			if (this.GetTerminalValue(TokenType.STRINGLITERAL, 0) != null)
			{
				string r = (string)this.GetTerminalValue(TokenType.STRINGLITERAL, 0);
				r = r.Substring(1, r.Length - 2); // strip quotes
				return r;
			}
		
			tree.Errors.Add(new ParseError("illegal StringLiteral format", 1000, this));
			return string.Empty;
		}

		protected virtual object GetStringLiteralValue(int index, params object[] paramlist )
		{
			object o = default(object);
			ParseNode node = GetTokenNode(TokenType.StringLiteral, index);
			if (node != null)
				o = node.EvalStringLiteral(paramlist);
			return o;
		}

		protected virtual object EvalVariable(params object[] paramlist)
		{
			ParseTree tree = paramlist[0] as ParseTree;
			if (tree.Context == null)
			{
				tree.Errors.Add(new ParseError("No context defined", 1041, this));
				return null;
			}
		
			string key = (string)this.GetTerminalValue(TokenType.VARIABLE, 0);
			// first check if the variable was declared in scope of a function
			var scope_var = tree.Context.GetScopeVariable(key);
			if(scope_var != null)
				return scope_var;
			
			// if not in scope of a function
			// next check if the variable was declared as a global variable
			if (tree.Context.Globals != null && tree.Context.Globals.ContainsKey(key))
				return tree.Context.Globals[key];
		
			//variable not found
			tree.Errors.Add(new ParseError("Variable not defined: " + key, 1039, this));
			return null;
		}

		protected virtual object GetVariableValue(int index, params object[] paramlist )
		{
			object o = default(object);
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

		if (n.Nodes.Count == 1) // search lower branch (left side only, may not contain other node branches)
			return GetFunctionOrVariable(n.Nodes[0]);

		// function or variable not found in branch
		return null;
	}
	public Context Context = new Context();

	}
	#endregion ParseTree
}
