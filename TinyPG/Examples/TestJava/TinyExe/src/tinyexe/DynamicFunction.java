package tinyexe;
import java.lang.*;

public class DynamicFunction extends Function
{
    /// <summary>
    /// points to the RHS of the assignment of this function
    /// this branch will be evaluated each time this function is executed
    /// </summary>
    private ParseNode Node;

    /// <summary>
    /// the list of parameters must correspond the the required set of Arguments
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    @Override
    public Object Eval(Object[] parameters, ParseTree tree)
    {
        // create a new scope for the arguments
        Variables pars = arguments.Clone();
        // now push a copy of the function arguments on the stack
        tree.Context.PushScope(pars);

        // assign the parameters to the current function scope variables            
        int i = 0;
        String[] keys = pars.keySet().toArray(new String[pars.size()]);

        for (String key : keys)
            pars.put(key, parameters[i++]);

        // execute the function here

        Object result = Node.Eval(tree, null);

        // clean up the stack
        tree.Context.PopScope();

        return result;
    }

    public DynamicFunction(String name, ParseNode node, Variables args, int minParameters, int maxParameters)
    {
        Node = node;
        arguments = args;
        this.minParameters = minParameters;
        this.maxParameters = maxParameters;
    }
}
