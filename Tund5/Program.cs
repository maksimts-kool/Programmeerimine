using System;
using System.Reflection.Metadata.Ecma335;
using System.Threading;
using System.Linq;

namespace Tund5;

public class Program
{
    // Mängu põhiprogramm – siin käivitatakse mäng.
    // Loob seina, mao, toidu ning haldab mängutsüklit.
    // Uued võimalused: takistused, raskusaste, helid, vastuste salvestus, erinevad toidupunktid.
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

        int snakeStartX = 4;
        int snakeStartY = 5;
        int snakeStartLen = 4;
        int snakeStartX2 = snakeStartX + snakeStartLen - 1;
        var forbidden = new List<(int x1, int y1, int x2, int y2)>
        { (snakeStartX, snakeStartY, snakeStartX2, snakeStartY) };


        List<Takistused> obst = Takistused.GenerateObstacles(
            80, 25,
            3,
            margin: 2,
            minW: 3, maxW: 10,
            minH: 2, maxH: 6,
            forbidden: forbidden
        );

        foreach (var o in obst)
            o.Draw();

        Point p = new Point(4, 5, '*');
        Snake s = new Snake(p, 4, Direction.RIGHT);
        s.Draw();

        FoodCreator fc = new FoodCreator(80, 25);
        List<FoodItem> foods = new List<FoodItem>();
        for (int i = 0; i < 3; i++)
        {
            var fi = fc.CreateFood(obst, foods, s);
            foods.Add(fi);
            fi.Draw();
        }

        int skoor = 0;
        HeliTegija sm = new HeliTegija();
        sm.PlayBackground();

        while (true)
        {
            bool hitObstacle = false;
            foreach (var o in obst)
            {
                if (o.IsHit(s))
                {
                    hitObstacle = true;
                    break;
                }
            }
            if (walls.IsHit(s) || s.IsHitTail() || hitObstacle)
                break;

            List<FoodItem> expired = new List<FoodItem>();
            foreach (var fi in foods)
            {
                bool nowExpired = fi.Tick();
                if (nowExpired)
                    expired.Add(fi);
            }

            foreach (var ex in expired)
            {

                ex.P.Clear();
                foods.Remove(ex);

                var newFi = fc.CreateFood(obst, foods, s);
                foods.Add(newFi);
                newFi.Draw();
            }

            bool ateSomething = false;
            FoodItem eatenFood = null;

            foreach (var fi in foods)
            {
                if (s.Eat(fi.P, fi.Value))
                {
                    skoor += fi.Value;
                    eatenFood = fi;
                    ateSomething = true;
                    sm.PlayEat();
                    break;
                }
            }

            if (ateSomething && eatenFood != null)
            {
                eatenFood.P.Clear();
                foods.Remove(eatenFood);

                s.Move();

                var newFi = fc.CreateFood(obst, foods, s);
                foods.Add(newFi);
                newFi.Draw();
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

        sm.StopBackground();
        sm.PlayGameOver();

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