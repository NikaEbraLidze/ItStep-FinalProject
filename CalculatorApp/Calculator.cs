namespace CalculatorApp
{
    // ბიზნეს ლოგიკა / ოპერაციები 
    public class Calculator
    {
        public static double Add(double n1, double n2)
        {
            return n1 + n2;
        }

        public static double Subtract(double n1, double n2)
        {
            return n1 - n2;
        }

        public static double Multiply(double n1, double n2)
        {
            return n1 * n2;
        }

        public static double Divide(double n1, double n2)
        {
            if (n2 == 0)
            {
                throw new DivideByZeroException("Division by zero is not allowed.");
            }
            return n1 / n2;
        }
    }
}