package tinyexe;
import java.util.Arrays;
import java.util.Comparator;
import java.util.HashMap;
import java.util.Random;

//public delegate object FunctionDelegate(object[] parameters);
//public delegate object FunctionContextDelegate(object[] parameters, Context context);


public class Functions extends HashMap<String, Function>
{
    private static Functions defaultFunctions;
    private final Random crand = new Random();

    public static Functions getDefaults()
    {
        if (defaultFunctions == null)
        {
            defaultFunctions = new Functions();
            defaultFunctions.InitDefaults();
        }
        return defaultFunctions;
    }

    public void InitDefaults()
    {
        this.put("help", new StaticFunction("Help", (Object[] ps) -> Help(), 0, 0));
        this.put("about",
            new StaticFunction("About", (Object[] ps) -> "@TinyExe - a Tiny Expression Evaluator v1.0\r\nby Herre Kuijpers - Copyright © 2011 under the CPOL license", 0, 0));

        // high precision functions
        this.put("abs", new StaticFunction("Abs", (Object[] ps) -> Math.abs(Util.ConvertToDouble(ps[0])), 1, 1));
        this.put("acos", new StaticFunction("Acos", (Object[] ps) -> Math.acos(Util.ConvertToDouble(ps[0])), 1, 1));
        this.put("asin", new StaticFunction("Abs", (Object[] ps) -> Math.asin(Util.ConvertToDouble(ps[0])), 1, 1));
        this.put("atan", new StaticFunction("Atan", (Object[] ps) -> Math.atan(Util.ConvertToDouble(ps[0])), 1, 1));
        this.put("atan2", new StaticFunction("Abs", (Object[] ps) -> Math.atan2(Util.ConvertToDouble(ps[0]), Util.ConvertToDouble(ps[1])), 2, 2));
        this.put("ceiling", new StaticFunction("Ceiling", (Object[] ps) -> Math.ceil(Util.ConvertToDouble(ps[0])), 1, 1));
        this.put("cos", new StaticFunction("Cos", (Object[] ps) -> Math.cos(Util.ConvertToDouble(ps[0])), 1, 1));
        this.put("cosh", new StaticFunction("Cosh", (Object[] ps) -> Math.cosh(Util.ConvertToDouble(ps[0])), 1, 1));
        this.put("exp", new StaticFunction("Exp", (Object[] ps) -> Math.exp(Util.ConvertToDouble(ps[0])), 1, 1));
        this.put("int", new StaticFunction("int", (Object[] ps) -> (int)Math.floor(Util.ConvertToDouble(ps[0])), 1, 1));
        this.put("fact", new StaticFunction("Fact", (Object[] ps) -> Fact(ps), 1, 1));
        this.put("floor", new StaticFunction("Floor", (Object[] ps) -> Math.floor(Util.ConvertToDouble(ps[0])), 1, 1));
        this.put("log", new StaticFunction("Log", (Object[] ps) -> Log(ps), 1, 2));
        this.put("ln", new StaticFunction("Ln", (Object[] ps) -> Math.log(Util.ConvertToDouble(ps[0])), 1, 1));
        this.put("pow", new StaticFunction("Pow", (Object[] ps) -> Math.log(Util.ConvertToDouble(ps[0])), 2, 2));
        this.put("round", new StaticFunction("Ln", (Object[] ps) -> Math.round(Util.ConvertToDouble(ps[0])), 1, 1));
        this.put("rand", new StaticFunction("Rand", (Object[] ps) -> crand.nextInt((int)(double)Util.ConvertToDouble(ps[0])), 1, 1));
        this.put("sign", new StaticFunction("Sign", (Object[] ps) -> Math.signum(Util.ConvertToDouble(ps[0])), 1, 1));
        this.put("sin", new StaticFunction("Sin", (Object[] ps) -> Math.sin(Util.ConvertToDouble(ps[0])), 1, 1));
        this.put("sinh", new StaticFunction("Sinh", (Object[] ps) -> Math.sinh(Util.ConvertToDouble(ps[0])), 1, 1));
        this.put("sqr", new StaticFunction("Sqr", (Object[] ps) -> Util.ConvertToDouble(ps[0])*Util.ConvertToDouble(ps[0]), 1, 1));
        this.put("sqrt", new StaticFunction("Sqrt", (Object[] ps) -> Math.sqrt(Util.ConvertToDouble(ps[0])), 1, 1));
        
        //this.put("trunc", new StaticFunction("Trunc", delegate (object[] ps) { return Math.Truncate(Convert.ToDouble(ps[0])); }, 1, 1));
        
        // array functions
        this.put("avg", new StaticFunction("Avg", (Object[] ps) -> Avg(ps), 1, Integer.MAX_VALUE ));
        this.put("stdev", new StaticFunction("StDev", (Object[] ps) -> StDev(ps), 1, Integer.MAX_VALUE ));
        this.put("var", new StaticFunction("Var", (Object[] ps) -> Var(ps), 1, Integer.MAX_VALUE ));
        this.put("max", new StaticFunction("Max", (Object[] ps) -> Max(ps), 1, Integer.MAX_VALUE ));
        this.put("median", new StaticFunction("Median", (Object[] ps) -> Median(ps), 1, Integer.MAX_VALUE ));
        this.put("min", new StaticFunction("Min", (Object[] ps) -> Min(ps), 1, Integer.MAX_VALUE ));

        //boolean functions
        this.put("not", new StaticFunction("Not", (Object[] ps) -> !Util.ConvertToBoolean(ps[0]), 1, 1));
        this.put("if", new StaticFunction("If", (Object[] ps) -> Util.ConvertToBoolean(ps[0]) ? ps[1] : ps[2], 3, 3));
        this.put("and", new StaticFunction("And", (Object[] ps) -> Util.ConvertToBoolean(ps[0]) && Util.ConvertToBoolean(ps[1]), 2, 2));
        this.put("or", new StaticFunction("Or", (Object[] ps) -> Util.ConvertToBoolean(ps[0]) || Util.ConvertToBoolean(ps[1]), 2, 2));
        

        // string functions
        this.put("left", new StaticFunction("Left", (Object[] ps) ->
        {
            int len = Util.ConvertToInt32(ps[1]) < ps[0].toString().length() ? Util.ConvertToInt32(ps[1]) : ps[0].toString().length();
            return ps[0].toString().substring(0, len);
        }, 2, 2));

        this.put("right", new StaticFunction("Right", (Object[] ps) ->
        {
            int len = Util.ConvertToInt32(ps[1]) < ps[0].toString().length() ? Util.ConvertToInt32(ps[1]) : ps[0].toString().length();
            return ps[0].toString().substring(ps[0].toString().length() - len, ps[0].toString().length());
        }, 2, 2));

        this.put("mid", new StaticFunction("Mid", (Object[] ps) ->
        {
            int idx = Util.ConvertToInt32(ps[1]) < ps[0].toString().length() ? Util.ConvertToInt32(ps[1]) : ps[0].toString().length();
            int len = Util.ConvertToInt32(ps[2]) < ps[0].toString().length() - idx ? Util.ConvertToInt32(ps[2]) : ps[0].toString().length() - idx;
            return ps[0].toString().substring(idx, idx + len);
        }, 3, 3));

        this.put("hex", new StaticFunction("Hex", (Object[] ps) -> String.format("%x", Util.ConvertToInt32(ps[0].toString())), 1, 1));
        this.put("format", new StaticFunction("Format", (Object[] ps) -> String.format(ps[0].toString(), ps[1]), 2, 2));
        this.put("len", new StaticFunction("Len", (Object[] ps) -> Util.ConvertToDouble(ps[0].toString().length()), 1, 1));
        this.put("lower", new StaticFunction("Lower", (Object[] ps) -> ps[0].toString().toLowerCase(), 1, 1));
        this.put("upper", new StaticFunction("Upper", (Object[] ps) -> ps[0].toString().toUpperCase(), 1, 1));
        this.put("val", new StaticFunction("Val", (Object[] ps) -> Util.ConvertToDouble(ps[0]), 1, 1));

        this.put("rshift", new StaticFunction("rshift", (Object[] ps) -> {
            return Util.ConvertToInt32(ps[0]) >> Util.ConvertToInt32(ps[1]);
        }, 2, 2));
        this.put("lshift", new StaticFunction("lshift", (Object[] ps) -> {
            return Util.ConvertToInt32(ps[0]) << Util.ConvertToInt32(ps[1]);
        }, 2, 2));
        this.put("bitand", new StaticFunction("bitand", (Object[] ps) -> {
            return Util.ConvertToInt32(ps[0]) & Util.ConvertToInt32(ps[1]);
        }, 2, 2));
        this.put("bitor", new StaticFunction("bitor", (Object[] ps) -> {
            return Util.ConvertToInt32(ps[0]) | Util.ConvertToInt32(ps[1]);
        }, 2, 2));
        this.put("bitxor", new StaticFunction("bitxor", (Object[] ps) -> {
            return Util.ConvertToInt32(ps[0]) ^ Util.ConvertToInt32(ps[1]);
        }, 2, 2));
    }

    /// <summary>
    /// calculates the average over a list of numeric values
    /// </summary>
    /// <param name="ps">list of numeric values</param>
    /// <returns>the average value</returns>
    private static Object Avg(Object[] ps)
    {
        double total = 0;
        for (Object o : ps)
            total += Util.ConvertToDouble(o);

        return total / ps.length;
    }

    /// <summary>
    /// calculates the median over a list of numeric values
    /// </summary>
    /// <param name="ps">list of numeric values</param>
    /// <returns>the median value</returns>
    private static Object Median(Object[] ps)
    {
        Object[] ordered=ps.clone();
        Arrays.sort(ordered, Comparator.comparingDouble( o -> Util.ConvertToDouble(o)));
        
        if (ordered.length % 2 == 1)
            return ordered[ordered.length / 2];
        else
            return (Util.ConvertToDouble(ordered[ordered.length / 2]) + Util.ConvertToDouble(ordered[ordered.length / 2-1]))/2;
    }

    /// <summary>
    /// calculates the statistical variance over a list of numeric values
    /// </summary>
    /// <param name="ps">list of numeric values</param>
    /// <returns>the variance</returns>
    private static Object Var(Object[] ps)
    {
        double avg = Util.ConvertToDouble(Avg(ps));
        double total = 0;
        for (Object o : ps)
            total += (Util.ConvertToDouble(o) - avg) * (Util.ConvertToDouble(o) - avg);

        return total / (ps.length-1);
    }

    /// <summary>
    /// calculates the statistical standard deviation over a list of numeric values
    /// </summary>
    /// <param name="ps">list of numeric values</param>
    /// <returns>the standard deviation</returns>
    private static Object StDev(Object[] ps)
    {
        double var = Util.ConvertToDouble(Var(ps));
        return Math.sqrt(var);
    }

    /// <summary>
    /// generic Log implementation, allows 1 or 2 parameters
    /// </summary>
    /// <param name="ps">numeric values</param>
    /// <returns>Log of the value</returns>
    private static Object Log(Object[] ps)
    {
        if (ps.length == 1)
            return Math.log10(Util.ConvertToDouble(ps[0]));

        if (ps.length == 2)
            return Math.log(Util.ConvertToDouble(ps[0])) / Math.log(Util.ConvertToDouble(ps[1]));

        return null;
    }

    private static Object Fact(Object[] ps)
    {
        double total = 1;

        for (int i = Util.ConvertToInt32(ps[0]); i > 1; i--)
            total *= i;

        return total;
    }

    private static Object Max(Object[] ps)
    {
        double max = Double.MIN_VALUE;

        for (Object o : ps)
        {
            double val = Util.ConvertToDouble(o);
            if (val > max)
                max = val;
        }
        return max;
    }

    private static Object Min(Object[] ps)
    {
        double min = Double.MAX_VALUE;

        for (Object o : ps)
        {
            double val = Util.ConvertToDouble(o);
            if (val < min)
                min = val;
        }
        return min;
    }

    private static Object Help()
    {
        StringBuilder help = new StringBuilder();
        help.append("Tiny Expression Evalutator can evaluate expression containing the following functions:\n");
        String[] keys = Functions.getDefaults().keySet().toArray(String[]::new);
        for (String key : keys)
        {
            Function func = Functions.getDefaults().get(key);
            help.append(func.getName()).append(" ");
        }
        return help.toString();
    }

}