// Automatically generated from source file: simple expression2_java.tpg
// By TinyPG v1.5 available at https://github.com/ultrasuperpingu/TinyPG

package tinypg;
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
			case AddExpr:
				Value = EvalAddExpr(paramlist);
				break;
			case MultExpr:
				Value = EvalMultExpr(paramlist);
				break;
			case Atom:
				Value = EvalAtom(paramlist);
				break;

			default:
				Value = Token.getText();
				break;
		}
		return Value;
	}

	protected int EvalStart(Object... paramlist)
	{
			return this.GetAddExprValue(0, paramlist);
	}

	protected int GetStartValue(int index, Object... paramlist)
	{
		int o = 0;
		ParseNode node = GetTokenNode(TokenType.Start, index);
		if (node != null)
			o = node.EvalStart(paramlist);
		return o;
	}

	protected int EvalAddExpr(Object... paramlist)
	{
			int Value = this.GetMultExprValue(0, paramlist);
			int i = 1;
			while (this.IsTokenPresent(TokenType.MultExpr, i))
			{
				String sign = this.GetTerminalValue(TokenType.PLUSMINUS, i-1);
				if (sign.equals("+"))
					Value += this.GetMultExprValue(i++, paramlist);
				else 
					Value -= this.GetMultExprValue(i++, paramlist);
			}
		
			return Value;
	}

	protected int GetAddExprValue(int index, Object... paramlist)
	{
		int o = 0;
		ParseNode node = GetTokenNode(TokenType.AddExpr, index);
		if (node != null)
			o = node.EvalAddExpr(paramlist);
		return o;
	}

	protected int EvalMultExpr(Object... paramlist)
	{
			int Value = this.GetAtomValue(0, paramlist);
			int i = 1;
			while (this.IsTokenPresent(TokenType.Atom, i))
			{
				String sign = this.GetTerminalValue(TokenType.MULTDIV, i-1).toString();
				if (sign.equals("*"))
					Value *= this.GetAtomValue(i++, paramlist);
				else 
					Value /= this.GetAtomValue(i++, paramlist);
			}
			return Value;
	}

	protected int GetMultExprValue(int index, Object... paramlist)
	{
		int o = 0;
		ParseNode node = GetTokenNode(TokenType.MultExpr, index);
		if (node != null)
			o = node.EvalMultExpr(paramlist);
		return o;
	}

	protected int EvalAtom(Object... paramlist)
	{
			if (this.IsTokenPresent(TokenType.NUMBER, 0))
				return Integer.parseInt(this.GetTerminalValue(TokenType.NUMBER, 0));
			if (this.IsTokenPresent(TokenType.ID, 0))
				return getVarValue(this.GetTerminalValue(TokenType.ID, 0));
			else
				return this.GetAddExprValue(0, paramlist);
	}

	protected int GetAtomValue(int index, Object... paramlist)
	{
		int o = 0;
		ParseNode node = GetTokenNode(TokenType.Atom, index);
		if (node != null)
			o = node.EvalAtom(paramlist);
		return o;
	}





	protected java.util.HashMap<String,Integer> context;
	public java.util.HashMap<String,Integer> getContext() {
		if(context == null && Parent != null)
		{
			return Parent.getContext();
		}
		return context;
	}

	public void setContext(java.util.HashMap<String,Integer> value) {
		context = value;
	}

	public int getVarValue(String id) {
		return getContext() == null?0:getContext().get(id);
	}

}

