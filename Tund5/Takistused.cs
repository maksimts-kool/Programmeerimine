using System;

namespace Tund5;

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
}
