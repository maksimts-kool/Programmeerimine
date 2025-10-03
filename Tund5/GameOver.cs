using System;

namespace Tund5
{
    class GameOver
    {
        public static string Show(int score)
        {
            Console.Clear();
            int width = 40;
            int height = 5;
            int startX = (80 - width) / 2;
            int startY = (25 - height) / 2;

            Console.ForegroundColor = ConsoleColor.Yellow;

            for (int x = startX; x < startX + width; x++)
            {
                Console.SetCursorPosition(x, startY);
                Console.Write("═");
                Console.SetCursorPosition(x, startY + height - 1);
                Console.Write("═");
            }

            for (int y = startY; y < startY + height; y++)
            {
                Console.SetCursorPosition(startX, y);
                Console.Write("║");
                Console.SetCursorPosition(startX + width - 1, y);
                Console.Write("║");
            }

            Console.SetCursorPosition(startX, startY);
            Console.Write("╔");
            Console.SetCursorPosition(startX + width - 1, startY);
            Console.Write("╗");
            Console.SetCursorPosition(startX, startY + height - 1);
            Console.Write("╚");
            Console.SetCursorPosition(startX + width - 1, startY + height - 1);
            Console.Write("╝");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(startX + 8, startY + 2);
            Console.Write($"MÄNG LÄBI!  Skoor: {score}");
            Console.ResetColor();

            Console.SetCursorPosition(startX, startY + height + 1);
            Console.Write("Sisesta oma nimi (vähemalt 3 tähte): ");

            string nimi = "";
            while (true)
            {
                nimi = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(nimi) && nimi.Length >= 3) break;
                Console.WriteLine("Viga: nimi peab sisaldama vähemalt 3 sümbolit!");
            }

            return nimi;
        }
    }
}