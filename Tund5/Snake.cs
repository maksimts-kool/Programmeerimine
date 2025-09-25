using System;
using System.Collections.Generic;
using System.Linq;

namespace Tund5;

class Snake : Figure
{
    // Madu – koosneb punktidest ning liigub kindlas suunas.
    // Haldab liikumist, söömist ja kokkupõrkeid.
    private Direction direction;
    private int grow = 0; // сколько сегментов нужно ещё вырастить
    public Snake(Point tail, int length, Direction _direction)
    {
        direction = _direction;
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

        if (grow > 0)
        {
            // если нужно расти — не удаляем хвост
            grow--;
        }
        else
        {
            // обычный ход: удаляем хвост
            Point tail = points.First();
            points.Remove(tail);
            tail.Clear();
        }
    }

    public Point GetNextPoint()
    {
        Point head = points.Last();
        Point nextPoint = new Point(head);
        nextPoint.Move(1, direction);
        return nextPoint;
    }

    internal bool IsHitTail()
    {
        var head = points.Last();
        for (int i = 0; i < points.Count - 1; i++)
        {
            if (head.IsHit(points[i]))
                return true;
        }
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

    internal bool Eat(Point food, int foodPoints)
    {
        Point head = GetNextPoint();
        if (head.IsHit(food))
        {
            // подсчёт роста: например, $=1, %=3, != -2
            if (foodPoints > 0)
            {
                grow += foodPoints; // змейка вырастет "сама" на следующих шагах
            }
            else if (foodPoints < 0)
            {
                // уменьшаем длину
                int removeCount = Math.Min(points.Count - 1, Math.Abs(foodPoints));
                for (int i = 0; i < removeCount; i++)
                {
                    Point tail = points.First();
                    points.Remove(tail);
                    tail.Clear();
                }
            }
            return true;
        }
        return false;
    }
}