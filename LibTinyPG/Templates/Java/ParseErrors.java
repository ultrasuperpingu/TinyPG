// Automatically generated from source file: <%SourceFilename%>
// By TinyPG v1.5 available at https://github.com/ultrasuperpingu/TinyPG

package <%Namespace%>;
import java.util.ArrayList;


public class ParseErrors extends ArrayList<ParseError>
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
