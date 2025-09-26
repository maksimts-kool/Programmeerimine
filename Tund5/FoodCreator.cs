using System;
using System.Collections.Generic;

namespace Tund5
{
    class FoodCreator
    {
        private int mapWidth;
        private int mapHeight;
        private Random random = new Random();

        public FoodCreator(int mapWidth, int mapHeight)
        {
            this.mapWidth = mapWidth;
            this.mapHeight = mapHeight;
        }

        public FoodItem CreateFood(
            List<Takistused> obstacles = null,
            List<FoodItem> existingFoods = null,
            Snake snake = null,
            int maxAttempts = 1000)
        {
            int attempts = 0;

            while (attempts < maxAttempts)
            {
                attempts++;

                int x = random.Next(2, mapWidth - 2);
                int y = random.Next(2, mapHeight - 2);

                int type = random.Next(0, 3);
                char sym = '$';
                int value = 1;
                if (type == 1)
                {
                    sym = '%';
                    value = 3;
                }
                else if (type == 2)
                {
                    sym = '!';
                    value = -2;
                }

                var candidatePoint = new Point(x, y, sym);

                bool bad = false;

                if (existingFoods != null)
                {
                    foreach (var ef in existingFoods)
                    {
                        if (ef.P.IsHit(candidatePoint))
                        {
                            bad = true;
                            break;
                        }
                    }
                }
                if (bad) continue;

                if (obstacles != null)
                {
                    foreach (var o in obstacles)
                    {
                        if (o.IsHit(candidatePoint))
                        {
                            bad = true;
                            break;
                        }
                    }
                }
                if (bad) continue;

                if (snake != null)
                {
                    if (snake.IsHit(candidatePoint))
                        bad = true;
                }
                if (bad) continue;

                int ttl = random.Next(50, 150);

                var fi = new FoodItem(candidatePoint, value, ttl);
                return fi;
            }

            for (int yy = 2; yy < mapHeight - 2; yy++)
            {
                for (int xx = 2; xx < mapWidth - 2; xx++)
                {
                    var p = new Point(xx, yy, '$');
                    bool bad = false;
                    if (existingFoods != null)
                    {
                        foreach (var ef in existingFoods)
                        {
                            if (ef.P.IsHit(p)) { bad = true; break; }
                        }
                    }
                    if (bad) continue;
                    if (obstacles != null)
                    {
                        foreach (var o in obstacles)
                        {
                            if (o.IsHit(p)) { bad = true; break; }
                        }
                    }
                    if (bad) continue;
                    if (snake != null)
                    {
                        if (snake.IsHit(p)) { bad = true; }
                    }
                    if (bad) continue;

                    return new FoodItem(p, 1, 200);
                }
            }

            return new FoodItem(new Point(2, 2, '$'), 1, 200);
        }
    }
}