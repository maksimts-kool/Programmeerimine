namespace Tund5
{
    class Difficult
    {
        public int Speed { get; }
        public int FoodCount { get; }
        public int MinPoints { get; }
        public int MaxPoints { get; }
        public double PoisonChance { get; }
        public int Obstacles { get; }
        public string Name { get; }

        private Difficult(string name, int speed, int food, int minPts, int maxPts, double poison, int obstacles)
        {
            Name = name;
            Speed = speed;
            FoodCount = food;
            MinPoints = minPts;
            MaxPoints = maxPts;
            PoisonChance = poison;
            Obstacles = obstacles;
        }

        public static Difficult Easy =>
            new Difficult("Lihtne", 150, 3, 1, 2, 0.1, 2);

        public static Difficult Medium =>
            new Difficult("Keskmine", 100, 4, 1, 3, 0.25, 4);

        public static Difficult Hard =>
            new Difficult("Raske", 60, 6, 2, 5, 0.4, 6);
    }
}