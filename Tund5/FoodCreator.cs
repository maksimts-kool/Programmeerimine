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
                    if (IsValid(p, snake, existingFoods, obstacles))
                        return new FoodItem(p, -2);
                }
                else
                {
                    int value = random.Next(minPoints, maxPoints + 1);
                    char sym = (value > minPoints) ? '%' : '$';
                    Point p = new Point(x, y, sym);

                    if (IsValid(p, snake, existingFoods, obstacles))
                        return new FoodItem(p, value);
                }
            }
        }
        private bool IsValid(Point p, Snake snake, List<FoodItem> foods, List<Takistused> obstacles)
        {
            if (snake.IsHit(p)) return false;

            if (foods != null)
                foreach (var f in foods)
                    if (f.P.IsHit(p)) return false;

            if (obstacles != null)
                foreach (var o in obstacles)
                    if (o.IsHit(p)) return false;

            return true;
        }
    }
}