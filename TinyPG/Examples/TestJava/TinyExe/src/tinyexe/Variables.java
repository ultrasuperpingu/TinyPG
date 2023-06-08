package TinyExe;
import java.util.HashMap;

public class Variables extends HashMap<String, Object>
{
    /// <summary>
    /// clones this set of variables
    /// this is required in order to support local scope and recursion
    /// a copy of the set of variables (arguments in a function) will be pushed on the scope stack
    /// </summary>
    /// <returns></returns>
    public Variables Clone()
    {
        Variables vars = new Variables();
        for (String key : this.keySet())
            vars.put(key, this.get(key));

        return vars;
    }
}