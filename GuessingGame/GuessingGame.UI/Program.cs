using System;
using GuessingGame.BLL;

namespace GuessingGame.UI
{
    class Program
    {
        // დავალება # 2 - რიცხვის გამომცნობი თამაში

        // ამ თამაშში პროგრამამ უნდა შექმნას შემთხვევითი რიცხვი, მომხმარებლის მიზანია გამოიცნოს ეს რიცხვი. 
        // მომხმარებელს რიცხვის გამოსაცნობად უნდა ჰქონდეს 10 მცდელობა,ყოველი შეყვანის შემდეგ პროგრამამ უნდა 
        // მიანიშნოს შემოყვანილი რიცხვი არსებულ რიცხვთან შედარებით მაღალია თუ დაბალი. თუ მომხმარებელი 10 მცდელობის 
        // განმავლობაში გამოიცნობს რიცხვს ის მოიგებს, თუ არა მაშინ დამარცხდება.

        // თამაშის დაწყებისას მომხმარებელს უნდა შეეძლოს აირჩიოს თამაშის სირთულის მაჩვენებელი:
        // Easy(მარტივი) - რიცხვი   1 - 15 დიაპზონში.
        // Medium(საშუალო) - რიცხვი - 1 - 25 დიაპაზონში.
        // Hard(რთული) - რიცხვი - 1 - 50 დიაპაზონში.

        // მომხმარებლის თამაშების ისტორია უნდა ინახებოდეს CSV ფორმატში მის სახელთან და უმაღლეს ქულასთან ერთად, 
        // რათა მომხმარებელმა შეძლოს TOP 10 მოთამაშის და მისი შედეგების ნახვა და რეკორდების გაუმჯობესება.

        static void Main(string[] args)
        {
            var service = new GameService();

            Console.Write("Enter your name: ");
            string name = Console.ReadLine();

            while (true)
            {
                GameDifficulty difficulty;
                string input;

                while (true)
                {
                    Console.Clear();
                    Console.Write("Choose difficulty (1=Easy, 2=Medium, 3=Hard): ");
                    input = Console.ReadLine();

                    if (input == "1")
                    {
                        difficulty = GameDifficulty.Easy;
                        break;
                    }
                    else if (input == "2")
                    {
                        difficulty = GameDifficulty.Medium;
                        break;
                    }
                    else if (input == "3")
                    {
                        difficulty = GameDifficulty.Hard;
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid input! Please enter 1, 2, or 3.");
                    }
                }

                var result = service.PlayGame(name, difficulty);

                Console.Clear();
                Console.WriteLine(result.Score > 0
                    ? $"Congratulations! You won! Score: {result.Score}"
                    : "You lost! Better luck next time.");

                service.SaveScore(result);

                Console.WriteLine("\nTOP 10 PLAYERS:");
                service.PrintTopScores();

                Console.WriteLine("\nEnter ctrl + c to Exict or Enter to continue");
            }
        }
    }
}
