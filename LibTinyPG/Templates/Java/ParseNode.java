// Automatically generated from source file: <%SourceFilename%>
// By TinyPG v1.5 available at https://github.com/ultrasuperpingu/TinyPG

package <%Namespace%>;
import java.util.ArrayList;
<%HeaderCode%>

public class ParseNode
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
<%EvalSymbols%>
			default:
				Value = Token.getText();
				break;
		}
		return Value;
	}

<%VirtualEvalMethods%>


<%CustomCode%>
}

