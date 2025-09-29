using System;
using System.Collections.Generic;
using System.IO;

namespace Tund5
{
    class VastuseTegija
    {
        string filePath = "vastused.txt";

        public void Save(string playerName, int score, string difficulty)
        {
            using (var w = new StreamWriter(filePath, true))
            {
                w.WriteLine(playerName + ";" + score + ";" + difficulty + ";" + DateTime.Now);
            }
        }

        public List<string[]> Load()
        {
            var scores = new List<string[]>();
            if (File.Exists(filePath))
            {
                foreach (var line in File.ReadAllLines(filePath))
                {
                    var parts = line.Split(';');
                    if (parts.Length >= 3) scores.Add(parts);
                }
            }
            return scores;
        }

        public void Show()
        {
            Console.Clear();
            Console.WriteLine("--- Tulemustabel ---\n");

            var scores = Load();
            scores.Sort((a, b) => int.Parse(b[1]).CompareTo(int.Parse(a[1])));

            if (scores.Count == 0)
            {
                Console.WriteLine("Veel pole tulemusi.");
            }
            else
            {
                int place = 1;
                foreach (var s in scores)
                {
                    Console.WriteLine($"{place}. {s[0]} - {s[1]} punkti ({s[2]}, {s[3]})");
                    place++;
                }
            }

            Console.WriteLine("\nVajuta klahvi...");
            Console.ReadKey();
        }
    }
}