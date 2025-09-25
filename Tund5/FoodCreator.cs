using System;

namespace Tund5;

class FoodCreator
{
    // Klass, mis tekitab uusi toidupunkte (mida madu sööb).
    private int mapWidth;
    private int mapHeight;
    private Random random = new Random();

    public FoodCreator(int mapWidth, int mapHeight)
    {
        this.mapWidth = mapWidth;
        this.mapHeight = mapHeight;
    }

    public Point CreateFood(out int points)
    {
        int x = random.Next(2, mapWidth - 2);
        int y = random.Next(2, mapHeight - 2);

        int type = random.Next(0, 3);
        char sym = '$';
        points = 1; // normaalne
        if (type == 1)
        {
            sym = '%';
            points = 3;
        } // +normaalne
        else if (type == 2)
        {
            sym = '!';
            points = -2;
        } // paha

        return new Point(x, y, sym);
    }
}