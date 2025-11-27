namespace GuessingGame.BLL
{
    public class GameResult
    {
        public required string PlayerName { get; set; }
        public int Score { get; set; }
        public int AttemptsUsed { get; set; }
        public GameDifficulty Difficulty { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
    }
}