using System;

namespace Tund5
{
    class FoodItem
    {
        public Point P { get; }
        public int Value { get; set; }
        public int TTL { get; set; }
        public FoodItem(Point p, int value, int ttl)
        {
            P = p;
            Value = value;
            TTL = ttl;
        }

        public void Draw() => P.Draw();
        public bool Tick()
        {
            TTL--;
            return TTL <= 0;
        }
    }
}