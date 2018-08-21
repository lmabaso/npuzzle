
namespace npuzzle
{
    class Spot
    {
        public int x;
        public int y;
        public Spot(int initialX, int initialY)
        {
            x = initialX;
            y = initialY;
        }

        public Spot SearchPos(int toFind, int dimentions, int[,] toSearch)
        {
            Spot pos = new Spot(0, 0);

            for (int x = 0; x < dimentions; x++)
                for (int y = 0; y < dimentions; y++)
                    if (toSearch[x, y] == toFind)
                    {
                        pos.x = x;
                        pos.y = y;
                        return (pos);
                    }
            return (pos);
        }
    }
}
