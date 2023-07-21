// Automatically generated from source file: <%SourceFilename%>
// By TinyPG v<%GeneratorVersion%> available at https://github.com/ultrasuperpingu/TinyPG

package <%Namespace%>;
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
	public int CurrentLine;
	public int CurrentColumn;
	public int CurrentPosition;
	public ArrayList<Token> Skipped; // tokens that were skipped
	public HashMap<TokenType, Pattern> Patterns;

	private Token LookAheadToken;
	private final ArrayList<TokenType> Tokens;
	private final ArrayList<TokenType> SkipList; // tokens to be skipped
	private static final List<TokenType> tokenTypeList = Arrays.asList(TokenType.values());
	
	public Scanner()
	{
		Pattern  regex;
		Patterns = new HashMap<TokenType, Pattern>();
		Tokens = new ArrayList<TokenType>();
		LookAheadToken = null;
		Skipped = new ArrayList<Token>();

		SkipList = new ArrayList<TokenType>();
<%SkipList%>
<%RegExps%>
	}

	public void Init(String input)
	{
		this.Input = input;
		StartPos = 0;
		EndPos = 0;
		CurrentLine = 1;
		CurrentColumn = 1;
		CurrentPosition = 0;
		LookAheadToken = null;
	}

	public Token GetToken(TokenType type)
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
	public Token Scan(TokenType... expectedtokens)
	{
		Token tok = LookAhead(expectedtokens); // temporarely retrieve the lookahead
		LookAheadToken = null; // reset lookahead token, so scanning will continue
		StartPos=tok.getEndPos();
		EndPos=tok.getEndPos(); // set the tokenizer to the new scan position
		CurrentLine = tok.getLine() + (tok.getText().length() - tok.getText().replace("\n", "").length());
		return tok;
	}

	/// <summary>
	/// returns token with longest best match
	/// </summary>
	/// <returns></returns>
	public Token LookAhead(TokenType... expectedtokens)
	{
		int i;
		int startpos = StartPos;
		int endpos = EndPos;
		int currentline = CurrentLine;
		Token tok = null;
		ArrayList<TokenType> scantokens;


		// this prevents double scanning and matching
		// increased performance
		// TODO: check this, what if the expected token are different since last call?
		// Check at least that LookAheadToken is part of the expected tokens
		if (LookAheadToken != null 
			&& LookAheadToken.Type != TokenType._UNDETERMINED_ 
			&& LookAheadToken.Type != TokenType._NONE_)
		{
			return LookAheadToken;
		}

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
			tok.setLine(currentline);
			if (tok.getStartPos() < Input.length())
				tok.setColumn(tok.getStartPos() - Input.lastIndexOf('\n', tok.getStartPos()));

			if (SkipList.contains(tok.Type))
			{
				startpos = tok.getEndPos();
				endpos = tok.getEndPos();
				currentline = tok.getLine() + (tok.getText().length() - tok.getText().replace("\n", "").length());
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
