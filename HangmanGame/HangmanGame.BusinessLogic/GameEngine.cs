using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HangmanGame.BusinessLogic
{
    public class GameEngine
    {
        private readonly List<string> _words = new List<string>
        { "apple", "banana", "orange", "grape", "kiwi", "strawberry", "pineapple", "blueberry", "peach", "watermelon" };
        private string _targetWord;
        private char[] _guessedWordDisplay;
        private readonly List<char> _incorrectGuesses = new List<char>();
        public int MaxGuesses { get; } = 6;
        public int GuessesLeft => MaxGuesses - _incorrectGuesses.Count;
        public bool IsGameOver { get; private set; }
        public bool DidWin { get; private set; }

        public void StartNewGame()
        {
            Random random = new Random();
            _targetWord = _words[random.Next(_words.Count)];

            _guessedWordDisplay = new char[_targetWord.Length];
            Array.Fill(_guessedWordDisplay, '_');

            _incorrectGuesses.Clear();
            IsGameOver = false;
            DidWin = false;
        }

        public bool GuessLetter(char letter)
        {
            if (IsGameOver || char.IsDigit(letter))
                return false;

            char lowerCaseLetter = char.ToLower(letter);
            bool found = false;

            for (int i = 0; i < _targetWord.Length; i++)
            {
                if (_targetWord[i] == lowerCaseLetter)
                {
                    _guessedWordDisplay[i] = lowerCaseLetter;
                    found = true;
                }
            }

            if (!found && !_incorrectGuesses.Contains(lowerCaseLetter))
            {
                _incorrectGuesses.Add(lowerCaseLetter);
            }

            CheckGameStatus();

            return found;
        }

        public bool GuessWord(string word)
        {
            if (IsGameOver) return false;

            bool win = string.Equals(_targetWord, word.ToLower(), StringComparison.Ordinal);

            if (win)
            {
                _guessedWordDisplay = _targetWord.ToCharArray();
                DidWin = true;
                IsGameOver = true;
            }
            else
            {
                _incorrectGuesses.Clear();
                _incorrectGuesses.Add('!');
                DidWin = false;
                IsGameOver = true;
            }

            return win;
        }

        private void CheckGameStatus()
        {
            if (GuessesLeft <= 0)
            {
                IsGameOver = true;
                DidWin = false;
                return;
            }

            if (!_guessedWordDisplay.Contains('_'))
            {
                IsGameOver = true;
                DidWin = true;
            }
        }

        public string GetDisplayWord()
        {
            return string.Join(" ", _guessedWordDisplay);
        }

        public string GetIncorrectGuesses()
        {
            return string.Join(", ", _incorrectGuesses);
        }

        public string GetTargetWord()
        {
            return _targetWord;
        }

        public int CalculateScore()
        {
            if (!DidWin) return 0;
            return Math.Max(0, 100 - _incorrectGuesses.Count * 10);
        }
    }
}