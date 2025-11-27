using HangmanGame.DataAccess;
using System.Collections.Generic;
using System.Linq;

namespace HangmanGame.BusinessLogic
{
    public class ScoreManager
    {
        private readonly XmlDataService _dataService;

        public ScoreManager(XmlDataService dataService)
        {
            _dataService = dataService;
        }

        public async void SaveScore(string playerName, int score)
        {
            if (string.IsNullOrWhiteSpace(playerName) || score <= 0)
                return;

            var scores = await _dataService.LoadScoresAsync();
            PlayerScore existingPlayer = scores
                .FirstOrDefault(p => p.Name.Equals(playerName, StringComparison.OrdinalIgnoreCase));

            if (existingPlayer != null)
            {
                if (score > existingPlayer.HighScore)
                    existingPlayer.HighScore = score;
            }
            else
            {
                scores.Add(new PlayerScore { Name = playerName, HighScore = score });
            }

            _dataService.SaveScoresAsync(scores);
        }

        public async Task<List<PlayerScore>> GetTopScores(int count)
        {
            var scores = await _dataService.LoadScoresAsync();

            return scores
                .OrderByDescending(s => s.HighScore)
                .Take(count)
                .ToList();
        }
    }
}