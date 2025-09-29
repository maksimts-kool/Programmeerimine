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
            Console.WriteLine("Madu mäng");
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
        int foodCount = 3;
        int minPoints = 1;
        int maxPoints = 2;
        double poisonChance = 0.1;
        int obstaclesCount = 2;
        string difficulty = "Lihtne";

        if (k == ConsoleKey.D1) // lihtne
        {
            speed = 150;
            foodCount = 3;
            minPoints = 1;
            maxPoints = 2;
            poisonChance = 0.1;
            obstaclesCount = 2;
            difficulty = "Lihtne";
        }
        else if (k == ConsoleKey.D2) // keskmine
        {
            speed = 100;
            foodCount = 4;
            minPoints = 1;
            maxPoints = 3;
            poisonChance = 0.25;
            obstaclesCount = 4;
            difficulty = "Keskmine";
        }
        else if (k == ConsoleKey.D3) // raske
        {
            speed = 60;
            foodCount = 6;
            minPoints = 2;
            maxPoints = 5;
            poisonChance = 0.4;
            obstaclesCount = 6;
            difficulty = "Raske";
        }

        try
        {
            Console.SetWindowSize(80, 25);
            Console.SetBufferSize(80, 25);
        }
        catch { }

        Walls walls = new Walls(80, 25);
        walls.Draw();

        List<Takistused> obst = Takistused.GenerateObstacles(80, 25, obstaclesCount);
        foreach (var o in obst)
            o.Draw();

        Point start = new Point(4, 5, '*');
        Snake snake = new Snake(start, 4, Direction.RIGHT);
        snake.Draw();

        FoodCreator fc = new FoodCreator(80, 25, minPoints, maxPoints, poisonChance);
        List<FoodItem> foods = new List<FoodItem>();
        for (int i = 0; i < foodCount; i++)
        {
            var f = fc.CreateFood(snake, foods, obst);
            foods.Add(f);
            f.Draw();
        }

        int skoor = 0;
        HeliTegija sm = new HeliTegija();
        sm.PlayBackground();

        while (true)
        {
            // Kontroll kas madu sõi seina, sabaga või takistust
            bool hitObstacle = false;
            foreach (var o in obst)
            {
                if (o.IsHit(snake))
                {
                    hitObstacle = true;
                    break;
                }
            }
            if (walls.IsHit(snake) || snake.IsHitTail() || hitObstacle)
                break;

            FoodItem eaten = null;
            foreach (var f in foods)
            {
                if (snake.Eat(f.P, f.Value))
                {
                    skoor += f.Value;
                    eaten = f;
                    break;
                }
            }

            if (eaten != null)
            {
                sm.PlayEat();
                eaten.P.Clear();
                foods.Remove(eaten);

                // Uus toit
                var nf = fc.CreateFood(snake, foods, obst);
                foods.Add(nf);
                nf.Draw();
            }

            snake.Move();

            Thread.Sleep(speed);

            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                snake.HandleKey(key.Key);
            }
        }

        sm.StopBackground();
        sm.PlayGameOver();

        Console.SetCursorPosition(30, 12);
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("MÄNG LÄBI! Skoor: " + skoor);
        Console.ResetColor();

        string nimi = "";
        while (true)
        {
            Console.Write("\nSisesta oma nimi: ");
            nimi = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(nimi) && nimi.Length >= 3)
                break;
            Console.WriteLine("Viga: nimi peab olema 3 sümbolist!");
        }

        VastuseTegija vt = new VastuseTegija();
        vt.Save(nimi, skoor, difficulty);

        Console.WriteLine();
        Console.WriteLine("Tulemus on salvestatud!");
        Console.WriteLine("Vajuta mingit klahvi...");
        Console.ReadKey();
    }
}