using System;
using System.Collections.Generic;

namespace Tund5;

class Walls
{
    // Seinte klass – tekitab ja haldab mänguala piire.
    private List<Figure> wallList;

    public Walls(int mapWidth, int mapHeight)
    {
        wallList = new List<Figure>();

        HorLine upLine = new HorLine(0, mapWidth - 2, 0, '+');
        HorLine downLine = new HorLine(0, mapWidth - 2, mapHeight - 1, '+');
        VerLine leftLine = new VerLine(0, mapHeight - 1, 0, '+');
        VerLine rightLine = new VerLine(0, mapHeight - 1, mapWidth - 2, '+');

        wallList.Add(upLine);
        wallList.Add(downLine);
        wallList.Add(leftLine);
        wallList.Add(rightLine);
    }

    internal bool IsHit(Figure figure)
    {
        foreach (var wall in wallList)
        {
            if (wall.IsHit(figure))
                return true;
        }
        return false;
    }

    public void Draw()
    {
        foreach (var wall in wallList)
            wall.Draw();
    }
}