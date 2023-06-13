// Automatically generated from source file: <%SourceFilename%>
// By TinyPG v1.5 available at https://github.com/ultrasuperpingu/TinyPG

package <%Namespace%>;
import java.util.ArrayList;


class ParseErrors extends <%ParseErrors%>
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

class ParseError<%ParseError%>
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
public class ParseTree extends ParseNode<%IParseTree%>
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

class ParseNode<%IParseNode%>
{
	protected String text;
	protected ArrayList<ParseNode> nodes;
	<%ITokenGet%>
	public ArrayList<ParseNode> getNodes() { return nodes;}
	<%INodesGet%>
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
<%EvalSymbols%>
			default:
				Value = Token.getText();
				break;
		}
		return Value;
	}

<%VirtualEvalMethods%>


<%ParseTreeCustomCode%>
}

