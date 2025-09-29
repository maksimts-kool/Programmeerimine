using System;
using System.Collections.Generic;

namespace Tund5
{
    class Takistused : Figure
    {
        public Takistused(int x1, int y1, int x2, int y2, char sym = '#')
        {
            points = new List<Point>();
            for (int x = x1; x <= x2; x++)
            {
                for (int y = y1; y <= y2; y++)
                {
                    points.Add(new Point(x, y, sym));
                }
            }
        }
        public static List<Takistused> GenerateObstacles(int mapWidth, int mapHeight, int count)
        {
            var rand = new Random();
            var result = new List<Takistused>();

            int attempts = 0;

            while (result.Count < count && attempts < 1000)
            {
                attempts++;

                int w = rand.Next(3, 6);
                int h = rand.Next(2, 4);

                int x1 = rand.Next(2, mapWidth - w - 2);
                int y1 = rand.Next(2, mapHeight - h - 2);
                int x2 = x1 + w;
                int y2 = y1 + h;

                var newRect = (x1, y1, x2, y2);

                bool tooClose = false;
                foreach (var o in result)
                {
                    var rect = GetBounds(o);

                    if (RectsOverlap(newRect, (rect.x1 - 1, rect.y1 - 1, rect.x2 + 1, rect.y2 + 1)))
                    {
                        tooClose = true;
                        break;
                    }
                }

                if (!tooClose)
                {
                    var obstacle = new Takistused(x1, y1, x2, y2);
                    result.Add(obstacle);
                }
            }

            return result;
        }

        private static (int x1, int y1, int x2, int y2) GetBounds(Takistused t)
        {
            int minX = int.MaxValue, minY = int.MaxValue;
            int maxX = int.MinValue, maxY = int.MinValue;
            foreach (var p in t.points)
            {
                if (p.X < minX) minX = p.X;
                if (p.Y < minY) minY = p.Y;
                if (p.X > maxX) maxX = p.X;
                if (p.Y > maxY) maxY = p.Y;
            }
            return (minX, minY, maxX, maxY);
        }

        private static bool RectsOverlap((int x1, int y1, int x2, int y2) a, (int x1, int y1, int x2, int y2) b)
        {
            if (a.x2 < b.x1 || a.x1 > b.x2 || a.y2 < b.y1 || a.y1 > b.y2)
                return false;
            return true;
        }
    }
}