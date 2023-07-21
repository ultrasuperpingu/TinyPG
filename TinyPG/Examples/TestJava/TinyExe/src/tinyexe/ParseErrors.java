// Automatically generated from source file: TinyExpEval_java.tpg
// By TinyPG v1.6 available at https://github.com/ultrasuperpingu/TinyPG

package tinyexe;
import java.util.ArrayList;


public class ParseErrors extends ArrayList<ParseError>
{
	public boolean containsErrors()
	{
		return this.stream().filter(e -> e.isWarning() == false) != null;
	}
	public boolean containsWarnings()
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
	public ArrayList<ParseError> getErrors()
	{
		ArrayList<ParseError> errors = new ArrayList<>();
		for(ParseError e : this)
		{
			if(!e.isWarning())
				errors.add(e);
		}
		return errors;
	}
}
