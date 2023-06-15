// Automatically generated from source file: <%SourceFilename%>
// By TinyPG v1.5 available at https://github.com/ultrasuperpingu/TinyPG

package <%Namespace%>;
import java.util.ArrayList;

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
