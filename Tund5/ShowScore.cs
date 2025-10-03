using System;

namespace Tund5
{
    class ScoreBoard
    {
        private int x;
        private int y;

        public ScoreBoard(int x = 2, int y = 0)
        {
            this.x = x;
            this.y = y;
        }

        // выводит счет на экран
        public void ShowScore(int score, string difficulty)
        {
            Console.SetCursorPosition(x, y);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write($"   Skoor: {score} | Raskus: {difficulty}   ");
            Console.ResetColor();
        }
    }
}