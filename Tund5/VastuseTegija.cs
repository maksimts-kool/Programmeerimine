using System;

namespace Tund5;

class VastuseTegija
{
    string filePath = "vastused.txt";

    public void Save(string playerName, int score)
    {
        StreamWriter writer = new StreamWriter(filePath, true);
        writer.WriteLine(playerName + ";" + score + ";" + DateTime.Now);
        writer.Close();
    }

    public List<string> Load()
    {
        List<string> scores = new List<string>();
        if (File.Exists(filePath))
        {
            StreamReader reader = new StreamReader(filePath);
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                scores.Add(line);
            }
            reader.Close();
        }
        return scores;
    }

    public void Show()
    {
        Console.Clear();
        Console.WriteLine("--- Vastused ---");
        Console.WriteLine();

        List<string> scores = Load();
        List<string[]> parts = new List<string[]>();
        foreach (string s in scores)
        {
            string[] arr = s.Split(';');
            if (arr.Length >= 2) parts.Add(arr);
        }

        for (int i = 0; i < parts.Count; i++)
        {
            for (int j = i + 1; j < parts.Count; j++)
            {
                int score1 = int.Parse(parts[i][1]);
                int score2 = int.Parse(parts[j][1]);
                if (score2 > score1)
                {
                    string[] temp = parts[i];
                    parts[i] = parts[j];
                    parts[j] = temp;
                }
            }
        }

        int place = 1;
        foreach (var p in parts)
        {
            Console.WriteLine(place + ". " + p[0] + " - " + p[1] + " punkti (" + p[2] + ")");
            place++;
        }

        if (parts.Count == 0)
        {
            Console.WriteLine("Veel pole tulemusi.");
        }

        Console.WriteLine();
        Console.WriteLine("Vajuta klahvi...");
        Console.ReadKey();
    }
}
