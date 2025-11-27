using System;

namespace CalculatorApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // დავალება # 1 - კონსოლური კალკულატორი 

            // კალკულატორის ფუნქცია მომხმარებლებს საშუალებას აძლევს შეასრულონ ძირითადი არითმეტიკული მოქმედებები (+, -, *, /).
            // მომხმარებლებს შეუძლიათ შეიყვანონ ორი ნომერი და აირჩიონ ოპერაცია შედეგის მისაღებად.
            // კალკულატორი ასევე შეიცავს შეყვანის ვალიდაციას არასწორი შეყვანების დასამუშავებლად,
            // იგი არ უნდა წყვეტდეს მუშაობას ერთი კონკრეტული ოპერაციის შესრულების შემდეგ.
            Console.WriteLine("Calculator strated!");

            while (true)
            {
                try
                {
                    double num1 = InputValidator.GetValidNumber("Enter Number 1: ");
                    string operation = InputValidator.GetValidOperation("Enter Operation (+, -, *, /): ");
                    double num2 = InputValidator.GetValidNumber("Enter Number 2: ");

                    double result = 0;
                    switch (operation)
                    {
                        case "+":
                            result = Calculator.Add(num1, num2);
                            break;
                        case "-":
                            result = Calculator.Subtract(num1, num2);
                            break;
                        case "*":
                            result = Calculator.Multiply(num1, num2);
                            break;
                        case "/":
                            result = Calculator.Divide(num1, num2);
                            break;
                        default:
                            Console.WriteLine(" Invalid Operation.");
                            continue;
                    }

                    Console.WriteLine($"\n Result: {num1} {operation} {num2} = {result}");

                    Console.WriteLine("\n Press Enter to continue, or Ctrl+C to stop.");
                    Console.ReadLine();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\n Error: {ex.Message}");
                }
            }
        }
    }
}