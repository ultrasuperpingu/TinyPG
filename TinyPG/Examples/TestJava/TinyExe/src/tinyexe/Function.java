package tinyexe;

	//public delegate object FunctionDelegate(object[] parameters);

	//public delegate object FunctionContextDelegate(object[] parameters, Context context);

public abstract class Function
{
    protected Variables arguments;
    /// <summary>
    /// define the arguments of the dynamic function
    /// </summary>
    public Variables getArguments() {
        return arguments;
    }

    protected String name;
    /// <summary>
    /// name of the function
    /// </summary>
    public String getName() {
        return name;
    }
    
    protected int maxParameters;
    /// <summary>
    /// minimum number of allowed parameters (default = 0)
    /// </summary>
    public int getMaxParameters() {
        return maxParameters;
    }
    
    protected int minParameters;
    /// <summary>
    /// maximum number of allowed parameters (default = 0)
    /// </summary>
    public int getMinParameters() {
        return minParameters;
    }

    public abstract Object Eval(Object[] parameters, ParseTree tree);

}
