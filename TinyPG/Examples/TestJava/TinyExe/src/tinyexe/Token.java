// Automatically generated from source file: TinyExpEval_java.tpg
// By TinyPG v1.5 available at https://github.com/ultrasuperpingu/TinyPG

package tinyexe;
import java.util.ArrayList;


public class Token
{
	private int line;
	private int column;
	private int startpos;
	private int endpos;
	private String text;

	public int getLine() { 
		return line;
	}
	public void setLine(int value) {
		line = value;
	}

	public int getColumn() {
		return column;
	}
	public void setColumn(int value) {
		column = value;
	}
	
	// contains all prior skipped symbols
	private ArrayList<Token> skipped;

	public int getStartPos() { 
		return startpos;
	}
	public void setStartPos(int value)
	{
		startpos = value;
	}

	public int getLength() { 
		return endpos - startpos;
	}

	public int getEndPos() { 
		return endpos; 
		
	}
	public void setEndPos(int value)
	{
		endpos = value;
	}

	public String getText() { 
		return text;
	}
	public void setText(String value)
	{
		text = value;
	}

	public ArrayList<Token> getSkipped() { 
		return skipped;
	}
	public void setSkipped(ArrayList<Token> value) {
		skipped = value;
	}

	public TokenType Type;

	public Token()
	{
		this(0, 0);
	}

	public Token(int start, int end)
	{
		Type = TokenType._UNDETERMINED_;
		startpos = start;
		endpos = end;
		text = "";
	}

	public void UpdateRange(Token token)
	{
		if (token.getStartPos() < startpos) startpos = token.getStartPos();
		if (token.getEndPos() > endpos) endpos = token.getEndPos();
	}

	@Override
	public String toString()
	{
		if (text != null)
			return Type.toString() + " '" + getText() + "'";
		else
			return Type.toString();
	}

}
