// Automatically generated from source file: TinyExpEval_java.tpg
// By TinyPG v1.6 available at https://github.com/ultrasuperpingu/TinyPG

package tinyexe;

public class ParseError
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