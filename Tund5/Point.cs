using System;

namespace Tund5
{
    // Punkt, mis määrab objekti asukoha ja sümboli konsoolis.
    // Kasutatakse mao osade, seinte ja toidu kirjeldamiseks.
    class Point
    {
        public int X;
        public int Y;
        public char Sym;

        public Point() { }

        public Point(int x, int y, char sym)
        {
            X = x;
            Y = y;
            Sym = sym;
        }

        public Point(Point p)
        {
            X = p.X;
            Y = p.Y;
            Sym = p.Sym;
        }

        public void Move(int offset, Direction direction)
        {
            if (direction == Direction.RIGHT) X += offset;
            else if (direction == Direction.LEFT) X -= offset;
            else if (direction == Direction.UP) Y -= offset;
            else if (direction == Direction.DOWN) Y += offset;
        }

        public bool IsHit(Point p) => p.X == X && p.Y == Y;

        public void Draw()
        {
            Console.SetCursorPosition(X, Y);
            DrawSymbol(Sym);
        }

        public void Clear()
        {
            Console.SetCursorPosition(X, Y);
            Console.Write(" ");
        }
        private void DrawSymbol(char sym)
        {
            switch (sym)
            {
                case '*':
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("■");
                    break;

                case '$':
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.Write("O");
                    break;

                case '%':
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write("@");
                    break;

                case '!':
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("X");
                    break;

                case '#':
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.Write("█");
                    break;

                case '+':
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("█");
                    break;

                default:
                    Console.ResetColor();
                    Console.Write(" ");
                    break;
            }
            Console.ResetColor();
        }

        public override string ToString() => $"{X},{Y},{Sym}";
    }
}