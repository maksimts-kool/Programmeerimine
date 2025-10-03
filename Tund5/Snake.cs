using System;
using System.Collections.Generic;
using System.Linq;

namespace Tund5
{
    class Snake : Figure
    {
        private Direction direction;
        private int grow = 0;

        public Snake(Point tail, int length, Direction dir)
        {
            direction = dir;
            points = new List<Point>();
            for (int i = 0; i < length; i++)
            {
                Point p = new Point(tail);
                p.Move(i, direction);
                points.Add(p);
            }
        }
        internal void Move()
        {
            Point head = GetNextPoint();
            points.Add(head);
            head.Draw();

            if (grow > 0) grow--;
            else
            {
                Point tail = points.First();
                points.Remove(tail);
                tail.Clear();
            }
        }

        public Point GetNextPoint()
        {
            Point head = points.Last();
            Point next = new Point(head);
            next.Move(1, direction);
            return next;
        }

        internal bool IsHitTail()
        {
            var head = points.Last();
            foreach (var p in points.Take(points.Count - 1))
                if (head.IsHit(p)) return true;
            return false;
        }

        public void HandleKey(ConsoleKey key)
        {
            if (key == ConsoleKey.LeftArrow && direction != Direction.RIGHT)
                direction = Direction.LEFT;
            else if (key == ConsoleKey.RightArrow && direction != Direction.LEFT)
                direction = Direction.RIGHT;
            else if (key == ConsoleKey.UpArrow && direction != Direction.DOWN)
                direction = Direction.UP;
            else if (key == ConsoleKey.DownArrow && direction != Direction.UP)
                direction = Direction.DOWN;
        }

        internal bool Eat(Point food, int value)
        {
            Point head = GetNextPoint();
            if (head.IsHit(food))
            {
                if (value > 0) grow += value;
                else if (value < 0)
                {
                    int removeCount = Math.Min(points.Count - 1, Math.Abs(value));
                    for (int i = 0; i < removeCount; i++)
                    {
                        var tail = points.First();
                        points.Remove(tail);
                        tail.Clear();
                    }
                }
                return true;
            }
            return false;
        }
    }
}