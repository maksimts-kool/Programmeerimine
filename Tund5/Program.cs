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
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        while (true) //tsükkel
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

        Difficult diff = Difficult.Easy;
        if (k == ConsoleKey.D2)
            diff = Difficult.Medium;
        else if (k == ConsoleKey.D3)
            diff = Difficult.Hard;

        try
        {
            Console.SetWindowSize(80, 25);
            Console.SetBufferSize(80, 25);
        }
        catch { }

        Walls walls = new Walls(80, 25);
        walls.Draw();

        List<Takistused> obst = Takistused.GenerateObstacles(80, 25, diff.Obstacles);
        foreach (var o in obst)
            o.Draw();

        Point start = new Point(4, 5, '*');
        Snake snake = new Snake(start, 4, Direction.RIGHT);
        snake.Draw();

        FoodCreator fc = new FoodCreator(80, 25, diff.MinPoints, diff.MaxPoints, diff.PoisonChance);
        List<FoodItem> foods = new List<FoodItem>();
        for (int i = 0; i < diff.FoodCount; i++)
        {
            var f = fc.CreateFood(snake, foods, obst);
            foods.Add(f);
            f.Draw();
        }

        int skoor = 0;
        HeliTegija sm = new HeliTegija();
        sm.PlayBackground();

        ScoreBoard board = new ScoreBoard();
        board.ShowScore(0, diff.Name);

        Console.CursorVisible = false;
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
                    board.ShowScore(skoor, diff.Name);
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
            board.ShowScore(skoor, diff.Name);



            Thread.Sleep(diff.Speed);

            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                snake.HandleKey(key.Key);
            }
        }

        sm.StopBackground();
        sm.PlayGameOver();
        string playerName = GameOver.Show(skoor);

        VastuseTegija vt = new VastuseTegija();
        vt.Save(playerName, skoor, diff.Name);

        Console.WriteLine();
        Console.WriteLine("Tulemus on salvestatud!");
        Console.WriteLine("Vajuta mingit klahvi...");
        Console.ReadKey();
    }
}