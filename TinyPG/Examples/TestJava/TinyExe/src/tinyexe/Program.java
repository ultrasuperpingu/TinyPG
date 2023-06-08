package TinyExe;

public class Program
{
    public static void main(String[] args)
    {
        Context context = new Context();
        java.util.Scanner s = new java.util.Scanner(System.in);
        context.getGlobals().put("testDouble", 0.1);
        context.getGlobals().put("testInt", 5);
        System.out.println("Enter an expression (empty to exit):");
        String expression = "5*3+(testInt * testDouble)/2";
        System.out.println("> " + expression);
        while (expression != null && expression.length()>0)
        {
            Parser p = new Parser(new Scanner());
            ParseTree tree = p.Parse(expression);
            if (!tree.Errors.isEmpty())
            {
                for(ParseError e : tree.Errors)
                {
                    System.out.println("Col " + e.getColumn() + ": " + e.getMessage());
                }
            }
            else
            {
                tree.Context=context;
                var res = tree.Eval();
                System.out.println("< "+res);
            }
            System.out.println("> ");
            expression = s.nextLine();
        }
    }
}
