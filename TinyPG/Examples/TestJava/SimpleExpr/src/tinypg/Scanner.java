// Automatically generated from source file: simple expression2_java.tpg
// By TinyPG v1.5 available at https://github.com/ultrasuperpingu/TinyPG

package tinypg;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.regex.Pattern;
import java.util.regex.Matcher;
import java.util.Arrays;
import java.util.List;

public class Scanner
{
	public String Input;
	public int StartPos = 0;
	public int EndPos = 0;
	public String CurrentFile;
	public int CurrentLine;
	public int CurrentColumn;
	public int CurrentPosition;
	public ArrayList<Token> Skipped; // tokens that were skipped
	public HashMap<TokenType, Pattern> Patterns;

	private Token LookAheadToken;
	private ArrayList<TokenType> Tokens;
	private ArrayList<TokenType> SkipList; // tokens to be skipped
	private static final List<TokenType> tokenTypeList = Arrays.asList(TokenType.values());
	
	public Scanner()
	{
		Pattern  regex;
		Patterns = new HashMap<TokenType, Pattern>();
		Tokens = new ArrayList<TokenType>();
		LookAheadToken = null;
		Skipped = new ArrayList<Token>();

		SkipList = new ArrayList<TokenType>();
		SkipList.add(TokenType.WHITESPACE);

		regex = Pattern.compile("^\\s*$");
		Patterns.put(TokenType.EOF, regex);
		Tokens.add(TokenType.EOF);

		regex = Pattern.compile("[0-9]+");
		Patterns.put(TokenType.NUMBER, regex);
		Tokens.add(TokenType.NUMBER);

		regex = Pattern.compile("[a-zA-Z_][a-zA-Z0-9_]*");
		Patterns.put(TokenType.ID, regex);
		Tokens.add(TokenType.ID);

		regex = Pattern.compile("(\\+|-)");
		Patterns.put(TokenType.PLUSMINUS, regex);
		Tokens.add(TokenType.PLUSMINUS);

		regex = Pattern.compile("\\*|/");
		Patterns.put(TokenType.MULTDIV, regex);
		Tokens.add(TokenType.MULTDIV);

		regex = Pattern.compile("\\(");
		Patterns.put(TokenType.BROPEN, regex);
		Tokens.add(TokenType.BROPEN);

		regex = Pattern.compile("\\)");
		Patterns.put(TokenType.BRCLOSE, regex);
		Tokens.add(TokenType.BRCLOSE);

		regex = Pattern.compile("\\s+");
		Patterns.put(TokenType.WHITESPACE, regex);
		Tokens.add(TokenType.WHITESPACE);


	}

	public void Init(String input)
	{
		Init(input, "");	
	}
	
	public void Init(String input, String fileName)
	{
		this.Input = input;
		StartPos = 0;
		EndPos = 0;
		CurrentFile = fileName;
		CurrentLine = 1;
		CurrentColumn = 1;
		CurrentPosition = 0;
		LookAheadToken = null;
	}

	Token GetToken(TokenType type)
	{
		Token t = new Token(this.StartPos, this.EndPos);
		t.Type = type;
		return t;
	}

	/// <summary>
	/// executes a lookahead of the next token
	/// and will advance the scan on the input string
	/// </summary>
	/// <returns></returns>
	Token Scan(TokenType... expectedtokens)
	{
		Token tok = LookAhead(expectedtokens); // temporarely retrieve the lookahead
		LookAheadToken = null; // reset lookahead token, so scanning will continue
		StartPos=tok.getEndPos();
		EndPos=tok.getEndPos(); // set the tokenizer to the new scan position
		CurrentLine = tok.getLine() + (tok.getText().length() - tok.getText().replace("\n", "").length());
		CurrentFile = tok.getFile();
		return tok;
	}

	/// <summary>
	/// returns token with longest best match
	/// </summary>
	/// <returns></returns>
	Token LookAhead(TokenType... expectedtokens)
	{
		int i;
		int startpos = StartPos;
		int endpos = EndPos;
		int currentline = CurrentLine;
		String currentFile = CurrentFile;
		Token tok = null;
		ArrayList<TokenType> scantokens;


		// this prevents double scanning and matching
		// increased performance
		if (LookAheadToken != null 
			&& LookAheadToken.Type != TokenType._UNDETERMINED_ 
			&& LookAheadToken.Type != TokenType._NONE_) return LookAheadToken;

		// if no scantokens specified, then scan for all of them (= backward compatible)
		if (expectedtokens.length == 0)
			scantokens = Tokens;
		else
		{
			scantokens = new ArrayList<TokenType>(Arrays.asList(expectedtokens));
			scantokens.addAll(SkipList);
		}

		do
		{

			int len = -1;
			int index = Integer.MAX_VALUE;
			String input = Input.substring(startpos);

			tok = new Token(startpos, endpos);

			for (i = 0; i < scantokens.size(); i++)
			{
				Pattern  r = Patterns.get(scantokens.get(i));
				Matcher m = r.matcher(input);
				//c# code for reference
				//if (m.Success && m.Index == 0 && ((m.Length > len) || (scantokens[i] < index && m.Length == len )))
				//changed "m.find() && m.start() == 0" to "m.lookingAt()" for optimization
				//if (m.find() && m.start() == 0 && ((m.end() - m.start() > len) || (tokenTypeList.indexOf(scantokens.get(i)) < index && m.end() - m.start() == len )))
				if (m.lookingAt() && ((m.end() - m.start() > len) || (tokenTypeList.indexOf(scantokens.get(i)) < index && m.end() - m.start() == len )))
				{
					len = m.end() - m.start();
					index = tokenTypeList.indexOf(scantokens.get(i));
				}
			}

			if (index >= 0 && len >= 0)
			{
				tok.setEndPos(startpos + len);
				tok.setText(Input.substring(tok.getStartPos(), tok.getStartPos() + len));
				tok.Type = TokenType.values()[index];
			}
			else if (tok.getStartPos() == tok.getEndPos())
			{
				if (tok.getStartPos() < Input.length())
					tok.setText(Input.substring(tok.getStartPos(), tok.getStartPos() + 1));
				else
					 tok.setText("EOF");
			}
			// Update the line and column count for error reporting.
			tok.setFile(currentFile);
			tok.setLine(currentline);
			if (tok.getStartPos() < Input.length())
				tok.setColumn(tok.getStartPos() - Input.lastIndexOf('\n', tok.getStartPos()));

			if (SkipList.contains(tok.Type))
			{
				startpos = tok.getEndPos();
				endpos = tok.getEndPos();
				currentline = tok.getLine() + (tok.getText().length() - tok.getText().replace("\n", "").length());
				currentFile = tok.getFile();
				Skipped.add(tok);
			}
			else
			{
				// only assign to non-skipped tokens
				tok.setSkipped (Skipped); // assign prior skips to this token
				Skipped=new ArrayList<Token>(); //reset skips
			}
		}
		while (SkipList.contains(tok.Type));

		LookAheadToken = tok;
		return tok;
	}
}


enum TokenType
{

	//Non terminal tokens:
	_NONE_         ,
	_UNDETERMINED_ ,

	//Non terminal tokens:
	Start          ,
	AddExpr        ,
	MultExpr       ,
	Atom           ,

	//Terminal tokens:
	EOF            ,
	NUMBER         ,
	ID             ,
	PLUSMINUS      ,
	MULTDIV        ,
	BROPEN         ,
	BRCLOSE        ,
	WHITESPACE     
}

class Token
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