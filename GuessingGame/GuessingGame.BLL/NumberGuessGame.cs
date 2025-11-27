namespace GuessingGame.BLL
{
    public class NumberGuessGame
    {
        private readonly int _target;
        private int _attemptsLeft = 10;
        public int AttemptsLeft => _attemptsLeft;
        public NumberGuessGame(GameDifficulty difficulty)
        {
            Random randomNum = new Random();
            _target = randomNum.Next(1, (int)difficulty + 1);
        }
        public string Guess(int number)
        {
            if (_attemptsLeft <= 0)
                return "No attempts left!";

            _attemptsLeft--;

            if (number == _target)
                return "Correct";

            return number > _target ? "Lower" : "Higher";
        }
        public bool IsWin(int number) => number == _target;
    }
}