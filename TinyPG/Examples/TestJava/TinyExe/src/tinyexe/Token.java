/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Classes/Class.java to edit this template
 */
package TinyExe;

import java.util.ArrayList;

public class Token
{
	private String file;
	private int line;
	private int column;
	private int startpos;
	private int endpos;
	private String text;

	public String getFile() { 
		return file; 
	}
	public void setFile(String value) {
		file = value;
	}

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