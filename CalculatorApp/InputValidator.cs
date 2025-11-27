namespace CalculatorApp
{
    // მონაცემთა ვალიდაცია
    public static class InputValidator
    {
        public static double GetValidNumber(string propt)
        {
            double number;
            bool isValid = false;

            do
            {
                Console.Write(propt);

                string input = Console.ReadLine()?.Trim();

                if (double.TryParse(input, out number))
                    isValid = true;
                else
                    Console.WriteLine("Error: please enter correct number.");

            } while (!isValid);

            return number;
        }

        public static string GetValidOperation(string propt)
        {
            string operation;
            bool isValid = false;

            do
            {
                Console.Write(propt);
                operation = Console.ReadLine()?.Trim();

                if (operation == "+" || operation == "-" || operation == "*" || operation == "/")
                {
                    isValid = true;
                }
                else
                    Console.WriteLine("Error: please enter correct operation.");

            } while (!isValid);

            return operation;
        }
    }
}