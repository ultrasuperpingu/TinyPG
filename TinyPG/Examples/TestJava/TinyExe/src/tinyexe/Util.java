
package TinyExe;

/**
 *
 * @author ultrasuperpingu
 */
public class Util {
    public static Double ConvertToDouble(Object o)
	{
        if(o instanceof Double double1)
            return double1;
        if(o instanceof Integer integer)
            return (double)integer;
		if(o instanceof Boolean boolean1)
            return boolean1?1.0:0.0;
		if(o instanceof String)
            return Double.valueOf(o.toString().replace(',', '.'));
		return 0.0;
	}
	public static String ConvertToString(Object o)
	{
        if(o instanceof String string)
            return string;
        if(o == null)
            return "";
		return o.toString().replace(',', '.');
	}
	public static Boolean ConvertToBoolean(Object o)
	{
		return false;
	}
    public static Integer ConvertToInt32(Object o)
    {
        if(o instanceof Boolean boolean1)
            return boolean1?1:0;
		if(o instanceof Double double1)
            return (int)(double)double1;
        if(o instanceof Integer integer)
            return integer;
		if(o instanceof String)
            return Integer.valueOf(o.toString());
		return 0;
    }
}
