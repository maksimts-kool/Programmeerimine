using System;
using System.Collections.Generic;

namespace Tund5
{
    class Takistused : Figure
    {
        public Takistused(int x1, int y1, int x2, int y2, char sym)
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
        public static List<Takistused> GenerateObstacles(
            int mapWidth,
            int mapHeight,
            int count,
            int margin = 2,
            int minW = 3,
            int maxW = 9,
            int minH = 2,
            int maxH = 5,
            List<(int x1, int y1, int x2, int y2)> forbidden = null,
            int maxAttempts = 1000,
            char sym = '#')
        {
            var result = new List<Takistused>();
            var placed = new List<(int x1, int y1, int x2, int y2)>();
            var rand = new Random();

            if (forbidden != null)
            {
                placed.AddRange(forbidden);
            }

            int attempts = 0;
            while (result.Count < count && attempts < maxAttempts)
            {
                attempts++;

                int w = rand.Next(minW, maxW + 1);
                int h = rand.Next(minH, maxH + 1);

                int x1 = rand.Next(margin, Math.Max(margin + 1, mapWidth - margin - w));
                int y1 = rand.Next(margin, Math.Max(margin + 1, mapHeight - margin - h));
                int x2 = x1 + w;
                int y2 = y1 + h;

                var rect = (x1, y1, x2, y2);

                bool overlaps = false;
                foreach (var r in placed)
                {
                    if (RectsOverlap(rect, r))
                    {
                        overlaps = true;
                        break;
                    }
                }
                if (overlaps) continue;

                var o = new Takistused(x1, y1, x2, y2, sym);
                result.Add(o);
                placed.Add(rect);
            }

            return result;
        }

        private static bool RectsOverlap(
            (int x1, int y1, int x2, int y2) a,
            (int x1, int y1, int x2, int y2) b)
        {
            if (a.x2 < b.x1 || a.x1 > b.x2 || a.y2 < b.y1 || a.y1 > b.y2)
                return false;
            return true;
        }
    }
}