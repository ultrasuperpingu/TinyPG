package tinyexe;

public class StaticFunction extends Function
{
    private FunctionDelegate functionDelegate;
    private FunctionContextDelegate functionContextDelegate;
    /// <summary>
    /// the actual function implementation
    /// </summary>
    public FunctionDelegate getFunctionDelegate() { return functionDelegate; }
    public void setFunctionDelegate(FunctionDelegate val) { functionDelegate = val; }

    public FunctionContextDelegate getFunctionContextDelegate() { return functionContextDelegate; }
    public void setFunctionContextDelegate(FunctionContextDelegate val) { functionContextDelegate = val; }

    @Override
    public Object Eval(Object[] parameters, ParseTree tree)
    {
        tree.Context.PushScope(null);

        Object result = null;
        if (functionDelegate != null)
            result = functionDelegate.invoke(parameters);
        else if (functionContextDelegate != null)
            result = functionContextDelegate.invoke(parameters, tree.Context);
        tree.Context.PopScope();
        return result;
    }

    public StaticFunction(String name, FunctionDelegate function, int minParameters, int maxParameters)
    {
        this.name=name;
        functionDelegate = function;
        this.minParameters = minParameters;
        this.maxParameters = maxParameters;
        this.arguments = new Variables();
    }

    public StaticFunction(String name, FunctionContextDelegate function, int minParameters, int maxParameters)
    {
        this.name = name;
        functionContextDelegate = function;
        this.minParameters = minParameters;
        this.maxParameters = maxParameters;
        this.arguments = new Variables();
    }
}