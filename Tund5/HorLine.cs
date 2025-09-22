using System;
using System.Collections.Generic;

namespace Tund5;

class HorLine : Figure
{
    public HorLine(int xLeft, int xRight, int y, char sym)
    {
        points = new List<Point>();
        for (int x = xLeft; x <= xRight; x++)
        {
            points.Add(new Point(x, y, sym));
        }
    }

    public override void Draw()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        base.Draw();
        Console.ResetColor();
    }
}