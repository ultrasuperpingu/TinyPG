package tinyexe;

/**
 *
 * @author ultrasuperpingu
 */
public interface FunctionContextDelegate {   
    Object invoke(Object[] parameters, Context context);
}
