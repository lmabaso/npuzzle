using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Npuzzle
{
    class Program
    {
        class Spot
        {
            public double f;
            public double g;
            public double h;
            public string value;
            public List<Spot> neighbors;
            public int x;
            public int y;
            public Spot previous;
            public bool wall = false;

            public Spot(bool initialWall)
            {
                wall = initialWall;
            }
            public Spot(Spot prev)
            {
                previous = prev;
            }
            public Spot(List<Spot> initialNeighbors)
            {
                neighbors = initialNeighbors;
            }

            public Spot(int initialX, int initialY)
            {
                x = initialX;
                y = initialY;
            }

            public Spot(int initialF, int initialG, int initialH, string initialValue, int initialX, int initialY)
            {
                f = initialF;
                g = initialG;
                h = initialH;
                value = initialValue;
                x = initialX;
                y = initialY;
            }
        }
        class SpotPosition
        {
            public int x;
            public int y;
            public SpotPosition(int initialX, int initialY)
            {
                x = initialX;
                y = initialY;
            }
        }

        static Spot[,] inputGrid;
        static Spot[,] endGrid;
        //static Spot start = null;
        static Spot end = null;
        static List<string> OverrollPath = new List<string>();
        static int dimentions;


        static double Heuristic(Spot a, Spot b)
        {
            //return (Math.Sqrt((a.x - b.x) + (a.y - b.y)));
            return (Math.Abs((a.x - b.x) + (a.y - b.y)));
        }

        static void Main(string[] args)
        {
            StreamReader sr = new StreamReader(@"text.txt");
            Reader(sr);

            endGrid = Ft_expectedOutcome(dimentions);
            Draw(inputGrid);
            Solve();
            //OverrollPath.RemoveAll(p => string.IsNumber(0));

            Console.WriteLine("Moves made : {0}", OverrollPath.Count);
            Console.ReadLine();
        }

        static void Reader(StreamReader sr)
        {
            List<string> newlines = new List<string>();
            string toAdd = null;
            string[] tmp = null;
            List<string> ntmp;
            int y = 2;

            do
            {
                toAdd = sr.ReadLine();
                if (toAdd != null && toAdd != "")
                {
                    newlines.Add(toAdd);
                }
            } while (toAdd != null);

            dimentions = Convert.ToInt32(newlines[1]);
            inputGrid = InitializeGrid(dimentions);
            for (int i = 0; i < dimentions; i++)
            {
                tmp = newlines[y].Split(' ');
                ntmp = tmp.ToList<string>();
                ntmp.RemoveAll(p => string.IsNullOrEmpty(p));
                tmp = ntmp.ToArray();
                for (int j = 0; j < dimentions; j++)
                {
                    inputGrid[i, j].value = tmp[j];
                }
                y++;
            }
        }

        static void Ft_swap(ref string a, ref string b)
        {
            string tmp;

            tmp = a;
            a = b;
            b = tmp;
        }
        static void Ft_clear()
        {
            foreach (Spot element in inputGrid)
            {
                element.f = 0;
                element.g = 0;
                element.h = 0;
                element.previous = null;
            }
        }

        static string Ft_moveStuff(List<Spot> path)
        {
            string record = null;

            Ft_clear();

            foreach (Spot element in path)
            {
                Console.Write("->{0} ", element.value);
                record += element.value;
                if (element.value != "0")
                    OverrollPath.Add(element.value);
            }

            Console.WriteLine();
            for (int x = 0; x < path.Count - 1; x++)
            {
                if (inputGrid[path[x].x, path[x].y].value == "0" || inputGrid[path[x + 1].x, path[x + 1].y].value == "0")
                    Ft_swap(ref inputGrid[path[x].x, path[x].y].value, ref inputGrid[path[x + 1].x, path[x + 1].y].value);
                Draw(inputGrid);
            }
            return (record);
        }

        static Spot[] Ft_findCornaers(ref Spot[,] tmpGrid)
        {
            Spot[] corners = new Spot[4];

            corners[0] = endGrid[0, 0];
            corners[1] = endGrid[0, dimentions - 1];
            corners[2] = endGrid[dimentions - 1, dimentions - 1];
            corners[3] = endGrid[dimentions - 1, 0];

            Ft_swap(ref tmpGrid[corners[1].x + 1, corners[1].y].value, ref tmpGrid[corners[1].x, corners[1].y].value);
            Ft_swap(ref tmpGrid[corners[1].x, corners[1].y - 1].value, ref tmpGrid[corners[1].x, corners[1].y].value);

            Ft_swap(ref tmpGrid[corners[2].x, corners[2].y - 1].value, ref tmpGrid[corners[2].x, corners[2].y].value);
            Ft_swap(ref tmpGrid[corners[2].x - 1, corners[2].y].value, ref tmpGrid[corners[2].x, corners[2].y].value);

            Ft_swap(ref tmpGrid[corners[3].x - 1, corners[3].y].value, ref tmpGrid[corners[3].x, corners[3].y].value);
            Ft_swap(ref tmpGrid[corners[3].x, corners[3].y + 1].value, ref tmpGrid[corners[3].x, corners[3].y].value);

            return (corners);
        }

        static bool Ft_isRepeat(string[] repeatation)
        {
            return ((repeatation[0] == repeatation[2] && repeatation[1] == repeatation[3]) ? true : false);
        }

        static bool Ft_isInbounds(int x, int y)
        {
            if (x >= 0 && y >= 0 && x <= dimentions - 1 && y <= dimentions - 1)
                return (true);
            return (false);
        }

        static List<Spot> Ft_toFreeze(Spot prev, string a, string b)
        {
            List<Spot> tofreeze = new List<Spot>();

            foreach (Spot element in endGrid[prev.x, prev.y].neighbors)
            {
                if (element.value != a && element.value != b)
                    tofreeze.Add(element);
            }
            return (tofreeze);
        }

        static void Solve2(string value)
        {
            Spot start;

            Spot prev;
            Spot nextPrev;
            Spot anotherPrev;

            Spot current;
            string[] triangulate = new string[3];
            string toFind;

            int num = Convert.ToInt32(value);
            int previous = num - 1;
            int nextPrevious = previous - 1;

            List<Spot> isNeighbour;


            triangulate[0] = nextPrevious.ToString();
            triangulate[1] = previous.ToString();
            triangulate[2] = num.ToString();

            Spot[,] tmpGrid = Ft_expectedOutcome(dimentions);

            current = SearchPos(value, endGrid);
            prev = SearchPos(previous.ToString(), inputGrid);
            nextPrev = SearchPos(nextPrevious.ToString(), inputGrid);

            tmpGrid = Ft_expectedOutcome(dimentions);

            anotherPrev = null;
            isNeighbour = Ft_toFreeze(nextPrev, (nextPrevious - 1).ToString(), prev.ToString());

            // Choose the correct neighbour
            foreach (Spot element in isNeighbour)
            {
                if (element.value != triangulate[1] && element.value != triangulate[0])
                {
                    anotherPrev = SearchPos(inputGrid[element.x, element.y].value, inputGrid);
                    break;
                }
            }

            inputGrid[prev.x, prev.y].wall = false;
            inputGrid[nextPrev.x, nextPrev.y].wall = false;
            inputGrid[anotherPrev.x, anotherPrev.y].wall = false;

            Ft_swap(ref tmpGrid[anotherPrev.x, anotherPrev.y].value, ref tmpGrid[nextPrev.x, nextPrev.y].value);
            Ft_swap(ref tmpGrid[nextPrev.x, nextPrev.y].value, ref tmpGrid[prev.x, prev.y].value);
            //Ft_swap(ref tmpGrid[current.x, current.y].value, ref tmpGrid[prev.x, prev.y].value);

            // Freeze the spot adjacent to neighbour
            foreach (Spot element in Ft_toFreeze(prev, value, nextPrev.ToString()))
            {
                inputGrid[element.x, element.y].wall = true;
            }

            foreach (string element in triangulate)
            {
                do
                {
                    // Console.WriteLine("Phase 1");
                    // Move to start target position Phase 1
                    toFind = element;
                    start = SearchPos("0", inputGrid);
                    end = SearchPos(toFind, inputGrid);
                    if (tmpGrid[end.x, end.y].value == inputGrid[end.x, end.y].value)
                        break;
                    Ft_moveStuff(Astar(start));
                    Ft_clear();
                    end = SearchPos(toFind, inputGrid);
                    inputGrid[end.x, end.y].wall = true;

                    // Console.WriteLine("Phase 2");
                    // Move to target position Phase 2
                    toFind = element;
                    start = SearchPos("0", inputGrid);
                    end = SearchPos(toFind, tmpGrid);
                    if (tmpGrid[end.x, end.y].value == inputGrid[end.x, end.y].value)
                        break;
                    Ft_moveStuff(Astar(start));
                    Ft_clear();
                    end = SearchPos(toFind, inputGrid);
                    inputGrid[end.x, end.y].wall = false;
                } while (tmpGrid[end.x, end.y].value != inputGrid[end.x, end.y].value);
                if (tmpGrid[end.x, end.y].value == inputGrid[end.x, end.y].value)
                    inputGrid[end.x, end.y].wall = true;
            }

            // Unfreeze the spot adjacent to neighbour
            foreach (Spot element in Ft_toFreeze(prev, value, nextPrev.ToString()))
            {
                inputGrid[element.x, element.y].wall = false;
            }

            //Reverse the array
            List<string> tmpList = triangulate.ToList<string>();
            tmpList.Reverse();
            triangulate = tmpList.ToArray();
            Console.WriteLine(" wall == {0}", inputGrid[3, 3].wall);

            //Move everything into place
            foreach (string element in triangulate)
            {
                //Unfreeze the needed spot
                end = SearchPos(element, inputGrid);
                inputGrid[end.x, end.y].wall = false;
                do
                {
                    // Console.WriteLine("<-->Phase 1");
                    // Move to start target position Phase 1
                    toFind = element;
                    start = SearchPos("0", inputGrid);
                    end = SearchPos(toFind, inputGrid);
                    if (endGrid[end.x, end.y].value == inputGrid[end.x, end.y].value)
                        break;
                    Ft_moveStuff(Astar(start));
                    Ft_clear();
                    end = SearchPos(toFind, inputGrid);
                    inputGrid[end.x, end.y].wall = true;

                    // Console.WriteLine("Phase 2");
                    // Move to target position Phase 2
                    toFind = element;
                    start = SearchPos("0", inputGrid);
                    end = SearchPos(toFind, tmpGrid);
                    if (endGrid[end.x, end.y].value == inputGrid[end.x, end.y].value)
                        break;
                    Ft_moveStuff(Astar(start));
                    Ft_clear();
                    end = SearchPos(toFind, inputGrid);
                    inputGrid[end.x, end.y].wall = false;
                } while (endGrid[end.x, end.y].value != inputGrid[end.x, end.y].value);
                if (endGrid[end.x, end.y].value == inputGrid[end.x, end.y].value)
                    inputGrid[end.x, end.y].wall = true;
            }
        }

        static void Solve()
        {
            Spot start;
            string toFind;
            int p = 1;
            int repeatationCount = 0;
            string[] repeatation = new string[4];


            Spot[,] tmpGrid = Ft_expectedOutcome(dimentions);
            Spot[] corners = Ft_findCornaers(ref tmpGrid);


            while (p <= 15)
            {
                toFind = p.ToString();
                end = SearchPos(toFind, inputGrid);
                if (endGrid[end.x, end.y].value == inputGrid[end.x, end.y].value)
                {
                    inputGrid[end.x, end.y].wall = true;
                    p++;
                }
                else
                {
                    do
                    {
                        // Console.WriteLine("Phase 1");
                        // Move to start target position Phase 1
                        toFind = p.ToString();
                        start = SearchPos("0", inputGrid);
                        end = SearchPos(toFind, inputGrid);
                        if (endGrid[end.x, end.y].value == inputGrid[end.x, end.y].value)
                            break;
                        // Save records for repeatation and move tiles
                        repeatation[repeatationCount] = Ft_moveStuff(Astar(start));
                        // Check for repeatation
                        if (Ft_isRepeat(repeatation))
                        {
                            Solve2(toFind);
                        }
                        repeatationCount++;
                        if (repeatationCount == repeatation.Length)
                            repeatationCount = 0;

                        // Restart f,g and h
                        Ft_clear();
                        end = SearchPos(toFind, inputGrid);
                        inputGrid[end.x, end.y].wall = true;

                        // Console.WriteLine("Phase 2");
                        //Move to target position Phase 2
                        toFind = p.ToString();
                        start = SearchPos("0", inputGrid);
                        end = SearchPos(toFind, endGrid);
                        if (endGrid[end.x, end.y].value == inputGrid[end.x, end.y].value)
                            break;
                        // Move tiles
                        Ft_moveStuff(Astar(start));
                        Ft_clear();
                        end = SearchPos(toFind, inputGrid);
                        inputGrid[end.x, end.y].wall = false;
                    } while (endGrid[end.x, end.y].value != inputGrid[end.x, end.y].value);
                    if (endGrid[end.x, end.y].value == inputGrid[end.x, end.y].value)
                        inputGrid[end.x, end.y].wall = true;

                    do
                    {
                        // Console.WriteLine("Phase 1");
                        //Move to start target position Phase 1
                        toFind = endGrid[0, 0].neighbors[0].value;
                        start = SearchPos("0", inputGrid);
                        end = SearchPos(toFind, inputGrid);
                        if (endGrid[end.x, end.y].value == inputGrid[end.x, end.y].value)
                            break;
                        repeatation[repeatationCount] = Ft_moveStuff(Astar(start));
                        if (Ft_isRepeat(repeatation))
                        {
                            Console.WriteLine("True");
                            Solve2(toFind);
                        }
                        repeatationCount++;
                        if (repeatationCount == repeatation.Length)
                            repeatationCount = 0;

                        Ft_clear();
                        end = SearchPos(toFind, inputGrid);
                        inputGrid[end.x, end.y].wall = true;

                        // Console.WriteLine("Phase 2");
                        //Move to target position Phase 2
                        toFind = endGrid[0, 0].neighbors[0].value;
                        start = SearchPos("0", inputGrid);
                        end = SearchPos(toFind, endGrid);
                        if (endGrid[end.x, end.y].value == inputGrid[end.x, end.y].value)
                            break;
                        Ft_moveStuff(Astar(start));
                        Ft_clear();
                        end = SearchPos(toFind, inputGrid);
                        inputGrid[end.x, end.y].wall = false;
                    } while (endGrid[end.x, end.y].value != inputGrid[end.x, end.y].value);
                    if (endGrid[end.x, end.y].value == inputGrid[end.x, end.y].value)
                        inputGrid[end.x, end.y].wall = false;
                    p = 1;
                }
            }
            Console.WriteLine("Solved!!!");
        }

        static List<Spot> Astar(Spot start)
        {
            List<Spot> openSet = new List<Spot>();
            List<Spot> closedset = new List<Spot>();
            List<Spot> path = new List<Spot>();
            Spot current = null;
            List<Spot> neighbours = null;
            Spot neighbour = null;
            Spot tmp = null;
            double tmpG = 0.0;


            openSet.Add(inputGrid[start.x, start.y]);

            while (openSet.Count > 0)
            {
                int winner = 0;
                for (int x = 0; x < openSet.Count; x++)
                    if (openSet[x].f < openSet[winner].f)
                        winner = x;

                current = openSet[winner];
                if (current.x == end.x && current.y == end.y)
                {
                    tmp = current;
                    path.Add(tmp);
                    while (tmp.previous != null)
                    {
                        path.Add(tmp.previous);
                        tmp = tmp.previous;
                    }
                }
                openSet.Remove(current);
                closedset.Add(current);
                neighbours = current.neighbors;
                for (int i = 0; i < neighbours.Count; i++)
                {
                    neighbour = neighbours[i];
                    if (!closedset.Contains(neighbour) && !neighbour.wall)
                    {
                        tmpG = current.g + 1;
                        if (openSet.Contains(neighbour))
                        {
                            if (tmpG < neighbour.g)
                                neighbour.g = tmpG;
                        }
                        else
                        {
                            neighbour.g = tmpG;
                            openSet.Add(neighbour);
                        }
                        neighbour.h = Heuristic(neighbour, end);
                        neighbour.f = neighbour.g + neighbour.h;
                        neighbour.previous = current;
                    }
                }
            }
            path.Reverse();
            return (path);
        }
        static Spot SearchPos(string toFind, Spot[,] toSearch)
        {
            Spot pos = new Spot(0, 0);

            int dimensions = Ft_getDimesions(toSearch);
            for (int x = 0; x < dimensions; x++)
                for (int y = 0; y < dimensions; y++)
                {
                    if (toSearch[x, y].value == toFind)
                    {
                        pos.x = x;
                        pos.y = y;
                        return (pos);
                    }
                }
            return (pos);
        }
        static Spot[,] InitializeGrid(int dimensions)
        {
            Spot[,] grid = new Spot[dimensions, dimensions];

            for (int x = 0; x < dimensions; x++)
                for (int y = 0; y < dimensions; y++)
                    grid[x, y] = new Spot(0, 0, 0, "0", x, y);
            foreach (Spot element in grid)
                grid[element.x, element.y].neighbors = Ft_getneighbours(grid, element.x, element.y);
            return (grid);
        }
        static List<Spot> Ft_getneighbours(Spot[,] grid, int x, int y)
        {
            List<Spot> neighbors = new List<Spot>();

            if (x < Math.Sqrt(grid.Length) - 1)
                neighbors.Add(grid[x + 1, y]);
            if (x > 0)
                neighbors.Add(grid[x - 1, y]);
            if (y < Math.Sqrt(grid.Length) - 1)
                neighbors.Add(grid[x, y + 1]);
            if (y > 0)
                neighbors.Add(grid[x, y - 1]);
            return (neighbors);
        }
        static int Ft_getDimesions(Spot[,] grid)
        {
            int glength = grid.Length;
            int dimensions = Convert.ToInt32(Math.Sqrt(glength));

            return (dimensions);
        }
        static void Draw(Spot[,] grid)
        {
            int dimensions = Ft_getDimesions(grid);

            //Console.Clear();
            for (int row = 0; row < dimensions; row++)
            {
                for (int col = 0; col < dimensions; col++)
                    Console.Write("{0,2} ", grid[row, col].value);
                Console.Write("\n");
            }
            Console.WriteLine("");
        }
        static Spot[,] Ft_expectedOutcome(int dimesions)
        {
            int x = 0;
            int y = 0;
            int zx = 0;
            int zy = 0;
            int index = 1;
            int rows = dimesions;
            int cols = rows;
            int max = rows * cols;
            Spot[,] grid = InitializeGrid(dimesions);
            while (index < max)
            {
                while (y < cols)
                {
                    if (index == max)
                        break;
                    grid[x, y].value = Convert.ToString(index++);
                    y++;
                }
                index--;
                y--;
                while (x < rows)
                {
                    if (index == max)
                        break;
                    grid[x, y].value = Convert.ToString(index++);
                    x++;
                }
                index--;
                x--;
                while (y >= zy)
                {
                    if (index == max)
                        break;
                    grid[x, y].value = Convert.ToString(index++);
                    y--;
                }
                index--;
                y++;
                while (x > zx)
                {
                    if (index == max)
                        break;
                    grid[x, y].value = Convert.ToString(index++);
                    x--;
                }
                if (index == max)
                    break;
                index--;
                x++;
                cols--;
                rows--;
                zy++;
                zx++;
            }
            return (grid);
        }
    }
}
