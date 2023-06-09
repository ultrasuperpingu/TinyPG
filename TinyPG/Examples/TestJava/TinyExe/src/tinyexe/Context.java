package tinyexe;

import java.util.ArrayList;
import java.util.Collections;

public class Context
{
    private static Context defaultContext;
    public static Context getDefault()
    {
        if (defaultContext == null)
            defaultContext = new Context();
        return defaultContext;
    }

    // list of functions currently in scope during an evaluation
    // note that this typically is NOT thread safe.

    // contains a list of variables that is in scope. Scope is used only for DynamicFunctions (for now)
    private ArrayList<Variables> inScope;
    private Functions functions; 
    public Functions getFunctions() {
        return functions;
    }
    private Variables globals; 
    public Variables getGlobals() {
        return globals;
    }

    /// <summary>
    /// check current stacksize
    /// is used for debugging purposes and error handling
    /// to prevent stackoverflows
    /// </summary>
    public int getCurrentStackSize()
    {
        return inScope.size();
    }

    public Variables getCurrentScope()
    {
        if (inScope.size() <= 0)
            return null;

        return inScope.get(inScope.size()-1);
    }

    /// <summary>
    /// Traverse all the local scopes and searches for the specified variable
    /// </summary>
    /// <returns>The scoped variable.</returns>
    /// <param name="key">Key.</param>
    public Object GetScopeVariable(String key)
    {
        ArrayList<Variables> reversed = new ArrayList<>(inScope);
        Collections.reverse(reversed);
        for (var scope : reversed)
        {
            if (scope != null && scope.containsKey(key))
                return scope.get(key);
        }
        return null;
    }

    public void PushScope(Variables vars)
    {
        inScope.add(vars);
    }

    public Variables PopScope()
    {
        if (inScope.size() <= 0)
            return null;

        Variables vars = inScope.get(inScope.size()-1);
        inScope.remove(inScope.size() - 1);
        return vars;
    }

    public Context()
    {
        Reset();
    }

    /// <summary>
    /// resets the context to its defaults
    /// </summary> 
    public void Reset()
    {
        inScope = new ArrayList<Variables>();
        functions = new Functions();
        globals = new Variables();
        functions.InitDefaults();
        globals.put("Pi", 3.1415926535897932384626433832795); // Math.Pi is not very precise
        globals.put("E", 2.7182818284590452353602874713527);  // Math.E is not very precise either
    }

    /// <summary>
    /// Thats not a Deep clone!
    /// Functions and Globals are shared!
    /// </summary>
    public Context Clone()
    {
        var c = new Context();
        c.globals = this.getGlobals();
        c.functions = this.getFunctions();
        return c;
    }
}
