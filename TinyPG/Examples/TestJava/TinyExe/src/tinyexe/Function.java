package tinyexe;

public abstract class Function
{
    protected Variables arguments;
    
    /**
     * Define the arguments of the dynamic function
     * @return arguments
     */
    public Variables getArguments() {
        return arguments;
    }

    protected String name;
    
    /**
     * Name of the function
     * @return name of the function
     */
    public String getName() {
        return name;
    }
    
    protected int maxParameters;
    
    /**
     * Maximum number of allowed parameters (default = 0)
     * @return maxParameters
     */
    public int getMaxParameters() {
        return maxParameters;
    }
    
    protected int minParameters;
    
    /**
     * Minimum number of allowed parameters (default = 0)
     * @return minParameters
     */
    public int getMinParameters() {
        return minParameters;
    }

    public abstract Object Eval(Object[] parameters, ParseTree tree);

}
