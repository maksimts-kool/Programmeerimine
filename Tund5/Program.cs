using System;
using System.Reflection.Metadata.Ecma335;
using System.Threading;

namespace Tund5;

public class Program
{
    // Mängu põhiprogramm – siin käivitatakse mäng.
    // Loob seina, mao, toidu ning haldab mängutsüklit.
    public static void Main(string[] args)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("--- Madu mäng ---");
            Console.WriteLine("1. Alusta mängu");
            Console.WriteLine("2. Vaata vastused");
            Console.WriteLine("3. Välju");
            Console.Write("Vali: ");

            ConsoleKey key = Console.ReadKey(true).Key;

            if (key == ConsoleKey.D1)
            {
                StartGame();
            }
            else if (key == ConsoleKey.D2)
            {
                VastuseTegija sm = new VastuseTegija();
                sm.Show();
            }
            else if (key == ConsoleKey.D3)
            {
                break;
            }
        }
    }

    private static void StartGame()
    {
        Console.Clear();
        Console.WriteLine("Vali raskusaste: (1) Lihtne, (2) Keskmine, (3) Raske");
        ConsoleKey k = Console.ReadKey(true).Key;
        int speed = 100;
        if (k == ConsoleKey.D1)
            speed = 150;
        else if (k == ConsoleKey.D2)
            speed = 100;
        else if (k == ConsoleKey.D3)
            speed = 50;

        try
        {
            Console.SetWindowSize(80, 25);
            Console.SetBufferSize(80, 25);
        }
        catch { }

        Walls walls = new Walls(80, 25);
        walls.Draw();

        List<Takistused> obst = new List<Takistused>();
        Takistused o1 = new Takistused(20, 8, 30, 9, '#');
        Takistused o2 = new Takistused(50, 15, 55, 16, '#');
        obst.Add(o1);
        obst.Add(o2);
        foreach (var o in obst)
            o.Draw();

        Point p = new Point(4, 5, '*');
        Snake s = new Snake(p, 4, Direction.RIGHT);
        s.Draw();

        FoodCreator fc = new FoodCreator(80, 25);
        List<Point> foods = new List<Point>();
        Dictionary<Point, int> foodValues = new Dictionary<Point, int>();
        for (int i = 0; i < 3; i++)
        {
            int fpoints;
            Point f = fc.CreateFood(out fpoints);
            foods.Add(f);
            foodValues[f] = fpoints;
            f.Draw();
        }

        int skoor = 0;
        HeliTegija sm = new HeliTegija();

        while (true)
        {
            if (walls.IsHit(s) || s.IsHitTail() || o1.IsHit(s) || o2.IsHit(s))
                break;

            bool ateSomething = false;
            Point eatenFood = null;

            foreach (Point f in foods)
            {
                if (s.Eat(f, foodValues[f]))
                {
                    skoor += foodValues[f];
                    eatenFood = f;
                    ateSomething = true;
                    break;
                }
            }

            if (ateSomething && eatenFood != null)
            {
                foods.Remove(eatenFood);
                foodValues.Remove(eatenFood);
                int fpoints;
                Point f = fc.CreateFood(out fpoints);
                foods.Add(f);
                foodValues[f] = fpoints;
                f.Draw();
            }
            else
            {
                s.Move();
            }

            Thread.Sleep(speed);

            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                s.HandleKey(key.Key);
            }
        }

        Console.SetCursorPosition(30, 12);
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("MÄNG LÄBI! Skoor: " + skoor);
        Console.ResetColor();

        string nimi = "";
        bool ok = false;
        while (ok == false)
        {
            try
            {
                Console.Write("\nSisesta oma nimi (vähemalt 3 tähte): ");
                nimi = Console.ReadLine();
                if (nimi.Length < 3)
                    throw new Exception("Liiga lühike nimi!");
                ok = true;
            }
            catch
            {
                Console.WriteLine("Viga: nimi peab sisaldama vähemalt 3 sümbolit!");
            }
        }

        VastuseTegija vt = new VastuseTegija();
        vt.Save(nimi, skoor);

        Console.WriteLine();
        Console.WriteLine("Tulemus on salvestatud!");
        Console.WriteLine("Vajuta klahvi, et lähema menüüsse...");
        Console.ReadKey();
    }
}