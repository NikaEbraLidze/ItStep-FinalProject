using System;
using GuessingGame.DAL;

namespace GuessingGame.BLL
{
    public class GameService
    {
        private readonly ScoreRepository _repo = new ScoreRepository();
        public GameResult PlayGame(string playerName, GameDifficulty difficulty)
        {
            var game = new NumberGuessGame(difficulty);
            int attemptsUsed = 0;

            while (attemptsUsed < 10)
            {
                Console.Write("Enter number: ");
                string input = Console.ReadLine();
                if (!int.TryParse(input, out int guess))
                {
                    Console.WriteLine("Invalid input, please enter a number.");
                    continue;
                }

                attemptsUsed++;

                string feedback = game.Guess(guess);
                Console.WriteLine(feedback);

                if (feedback == "Correct")
                {
                    return new GameResult
                    {
                        PlayerName = playerName,
                        Score = game.AttemptsLeft,
                        AttemptsUsed = attemptsUsed,
                        Difficulty = difficulty
                    };
                }
            }

            return new GameResult
            {
                PlayerName = playerName,
                Score = 0,
                AttemptsUsed = attemptsUsed,
                Difficulty = difficulty
            };
        }

        // public GameResult PlayGame(string playerName, GameDifficulty difficulty)
        // {
        //     var game = new NumberGuessGame(difficulty);

        //     while (game.AttemptsLeft > 0)
        //     {
        //         Console.Write("Enter number: ");
        //         int guess = int.Parse(Console.ReadLine());

        //         string feedback = game.Guess(guess);
        //         Console.WriteLine(feedback);

        //         if (feedback == "Correct")
        //         {
        //             return new GameResult
        //             {
        //                 PlayerName = playerName,
        //                 Score = game.AttemptsLeft,
        //                 AttemptsUsed = 10 - game.AttemptsLeft,
        //                 Difficulty = difficulty
        //             };
        //         }
        //     }

        //     return new GameResult
        //     {
        //         PlayerName = playerName,
        //         Score = 0,
        //         AttemptsUsed = 10,
        //         Difficulty = difficulty
        //     };
        // }

        public void SaveScore(GameResult result)
        {
            _repo.SaveResult(result);
        }

        public void PrintTopScores()
        {
            var scores = _repo.LoadScores();

            foreach (var s in scores)
            {
                Console.WriteLine($"{s.PlayerName} — Score: {s.Score} — Difficulty: {s.Difficulty}");
            }
        }
    }
}