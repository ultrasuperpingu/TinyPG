/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Classes/Main.java to edit this template
 */
package tinypg;
import java.util.HashMap;
/**
 *
 * @author ultrasuperpingu
 */
public class SimpleExpr {

    /**
     * @param args the command line arguments
     */
    public static void main(String[] args) {
        HashMap<String,Integer> context=new HashMap<>();
        context.put("_5",5);
        context.put("_15",15);
        context.put("test",2);
        Parser p=new Parser(new Scanner());
        ParseTree tree = p.Parse("_5*3+_15/(2*test)");
        tree.setContext(context);
        System.out.println(tree.Eval());
    }
    
}
