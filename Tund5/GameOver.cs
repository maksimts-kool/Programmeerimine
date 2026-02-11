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

            int bottomY = Console.WindowHeight - 3;

            string nimi = "";
            while (true)
            {
                Console.SetCursorPosition(0, bottomY);
                Console.Write(new string(' ', Console.WindowWidth));
                Console.SetCursorPosition(0, bottomY);
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("Sisesta oma nimi (vähemalt 3 tähte): ");
                Console.ResetColor();

                nimi = Console.ReadLine() ?? "";

                if (!string.IsNullOrWhiteSpace(nimi) && nimi.Length >= 3)
                    break;

                Console.SetCursorPosition(0, bottomY + 1);
                Console.Write(new string(' ', Console.WindowWidth));
                Console.SetCursorPosition(0, bottomY + 1);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Viga: nimi peab sisaldama vähemalt 3 sümbolit!");
                Console.ResetColor();
            }

            Console.SetCursorPosition(0, bottomY + 1);
            Console.Write(new string(' ', Console.WindowWidth));

            return nimi;
        }
    }
}