using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace npuzzle
{
    class Program
    {
        static int dimentions;
        static int Main(string[] args)
        {
            StreamReader sr = new StreamReader(@"text.txt");
            int[,] newGrid = Reader(sr);
            int[,] endGrid = Ft_expectedOutcome();
            SearchAlgo ui = new SearchAlgo();

            if (!ui.Isolvable(newGrid, dimentions))
            {
                Console.WriteLine("Puzzle is Unsolvable");
                Console.ReadLine();
                return (0);
            }
            else
            {
                Console.WriteLine("Puzzle is Solvable");

                ASNode puzzle = new ASNode(newGrid, endGrid);
                List<ASNode> solution = ui.AStar(puzzle);
                DisplayResults(solution);

            }
            Console.ReadLine();
            return (1);
        }

        static void DisplayResults(List<ASNode> solution)
        {
            if (solution.Count > 0)
            {
                foreach (ASNode element in solution)
                    element.PrintPuzzle();
                Console.WriteLine("Moves taken: {0}", solution.Count);
            }
            else
                Console.WriteLine("No Path to Solution");
        }

        static int[,] Reader(StreamReader sr)
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
                    newlines.Add(toAdd);
            } while (toAdd != null);
            dimentions = Convert.ToInt32(newlines[1]);
            int[,] newGrid = new int[dimentions, dimentions];
            for (int i = 0; i < dimentions; i++)
            {
                tmp = newlines[y].Split(' ');
                ntmp = tmp.ToList();
                ntmp.RemoveAll(p => string.IsNullOrEmpty(p));
                tmp = ntmp.ToArray();
                for (int j = 0; j < dimentions; j++)
                {
                    newGrid[i, j] = Convert.ToInt32(tmp[j]);
                }
                y++;
            }
            return (newGrid);
        }

        static int[,] Ft_expectedOutcome()
        {
            int x = 0;
            int y = 0;
            int zx = 0;
            int zy = 0;
            int index = 1;
            int rows = dimentions;
            int cols = rows;
            int max = rows * cols;
            //Spot[,] grid = InitializeGrid(dimesions);
            int[,] grid = new int[dimentions, dimentions];
            while (index < max)
            {
                while (y < cols)
                {
                    if (index == max)
                        break;
                    grid[x, y] = index++;
                    y++;
                }
                index--;
                y--;
                while (x < rows)
                {
                    if (index == max)
                        break;
                    grid[x, y] = index++;
                    x++;
                }
                index--;
                x--;
                while (y >= zy)
                {
                    if (index == max)
                        break;
                    grid[x, y] = index++;
                    y--;
                }
                index--;
                y++;
                while (x > zx)
                {
                    if (index == max)
                        break;
                    grid[x, y] = index++;
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
