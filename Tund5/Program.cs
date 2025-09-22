using System;
using System.Threading;

namespace Tund5;

public class Program
{
    // Mängu põhiprogramm – siin käivitatakse mäng.
    // Loob seina, mao, toidu ning haldab mängutsüklit.
    public static void Main(string[] args)
    {
        try
        {
            Console.SetWindowSize(80, 25);
            Console.SetBufferSize(80, 25);
        }
        catch { }

        Walls walls = new Walls(80, 25);
        walls.Draw();

        Point p = new Point(4, 5, '*');
        Snake s = new Snake(p, 4, Direction.RIGHT);
        s.Draw();

        FoodCreator fc = new FoodCreator(80, 25, '$');
        Point food = fc.CreateFood();
        food.Draw();

        while (true)
        {
            if (walls.IsHit(s) || s.IsHitTail())
                break;

            if (s.Eat(food))
            {
                food = fc.CreateFood();
                food.Draw();
            }
            else
            {
                s.Move();
            }

            Thread.Sleep(100);

            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                s.HandleKey(key.Key);
            }
        }

        Console.SetCursorPosition(30, 12);
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("GAME OVER");
        Console.ResetColor();
        Console.ReadKey();
    }
}