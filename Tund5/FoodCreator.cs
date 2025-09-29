using System;
using System.Collections.Generic;

namespace Tund5
{
    class FoodCreator
    {
        private int mapWidth;
        private int mapHeight;
        private Random random = new Random();

        private int minPoints;
        private int maxPoints;
        private double poisonChance;

        public FoodCreator(int mapWidth, int mapHeight, int minPoints, int maxPoints, double poisonChance)
        {
            this.mapWidth = mapWidth;
            this.mapHeight = mapHeight;
            this.minPoints = minPoints;
            this.maxPoints = maxPoints;
            this.poisonChance = poisonChance;
        }

        public FoodItem CreateFood(Snake snake, List<FoodItem> existingFoods, List<Takistused> obstacles)
        {
            while (true)
            {
                int x = random.Next(2, mapWidth - 2);
                int y = random.Next(2, mapHeight - 2);

                if (random.NextDouble() < poisonChance)
                {
                    Point p = new Point(x, y, '!');
                    return new FoodItem(p, -2);
                }

                int points = random.Next(minPoints, maxPoints + 1);
                char sym = (points > minPoints) ? '%' : '$';

                Point good = new Point(x, y, sym);

                bool bad = false;
                if (snake.IsHit(good)) bad = true;
                foreach (var f in existingFoods)
                    if (f.P.IsHit(good)) bad = true;
                foreach (var o in obstacles)
                    if (o.IsHit(good)) bad = true;

                if (!bad)
                    return new FoodItem(good, points);
            }
        }
    }
}