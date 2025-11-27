using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GuessingGame.BLL;

namespace GuessingGame.DAL
{
    public class ScoreRepository
    {
        private const string FilePath = @"../../../scores.csv";

        public void SaveResult(GameResult result)
        {
            string csv = $"{result.PlayerName},{result.Score},{result.AttemptsUsed},{result.Difficulty},{result.Date}";
            File.AppendAllLines(FilePath, new[] { csv });
        }

        public List<GameResult> LoadScores()
        {
            if (!File.Exists(FilePath))
                return new List<GameResult>();

            return File.ReadAllLines(FilePath)
                .Select(line =>
                {
                    var parts = line.Split(',');

                    return new GameResult
                    {
                        PlayerName = parts[0],
                        Score = int.Parse(parts[1]),
                        AttemptsUsed = int.Parse(parts[2]),
                        Difficulty = Enum.Parse<GameDifficulty>(parts[3]),
                        Date = DateTime.Parse(parts[4])
                    };
                })
                .OrderByDescending(s => s.Score)
                .Take(10)
                .ToList();
        }
    }
}