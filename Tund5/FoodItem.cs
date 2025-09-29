namespace Tund5
{
    class FoodItem
    {
        public Point P { get; }
        public int Value { get; set; }

        public FoodItem(Point p, int value)
        {
            P = p;
            Value = value;
        }

        public void Draw() => P.Draw();
    }
}