using System;
using System.Collections.Generic;

namespace Tund5;

class VerLine : Figure
{
    // Vertikaalne joon – kujutab seina külgmist osa.
    public VerLine(int yUp, int yDown, int x, char sym)
    {
        points = new List<Point>();
        for (int y = yUp; y <= yDown; y++)
        {
            points.Add(new Point(x, y, sym));
        }
    }
}