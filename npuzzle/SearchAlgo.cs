using System;
using System.Collections.Generic;

namespace npuzzle
{
    class SearchAlgo
    {
        public List<ASNode> AStar(ASNode root)
        {
            List<ASNode> PathToSolution = new List<ASNode>();
            List<ASNode> openSet = new List<ASNode>();
            List<ASNode> closedSet = new List<ASNode>();
            ASNode current = null;
            ASNode tmp = null;
            ASNode neighbour = null;
            double tmpG = 0.0;

            openSet.Add(root);
            while (openSet.Count > 0)
            {
                int winner = 0;
                for (int x = 0; x < openSet.Count; x++)
                    if (openSet[x].f < openSet[winner].f)
                        winner = x;

                current = openSet[winner];
                if (current.GoalTest())
                {
                    tmp = current;
                    PathToSolution.Add(tmp);
                    while (tmp.previous != null)
                    {
                        PathToSolution.Add(tmp.previous);
                        tmp = tmp.previous;
                    }
                    PathToSolution.Reverse();
                    break;
                    //return (PathToSolution);
                }
                openSet.Remove(current);
                closedSet.Add(current);
                //Console.WriteLine("==========Current===============");
                //current.PrintPuzzle();
                current.ExpandNode();
                //Console.WriteLine("===========neighbous======================");
                //foreach (ASNode element in closedSet)
                //{
                //    element.PrintPuzzle();

                //}
                //Console.WriteLine("=================================");
                //Console.ReadLine();
                foreach (ASNode element in current.neighbours)
                {
                    // Console.WriteLine("{0}", Isolvable(element.puzzle));
                    if (element.previous.previous != null)
                    {
                        //Optimization Avoid repeating same moves
                        if (!Ft_same(element.puzzle, element.previous.previous.puzzle))
                        {
                            neighbour = element;
                            if (!closedSet.Contains(neighbour))
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
                                neighbour.h = ManhattonHeuristic(neighbour);
                                neighbour.f = neighbour.g + neighbour.h;
                                neighbour.previous = current;
                            }
                        }
                    }
                    else
                    {
                        neighbour = element;
                        if (!closedSet.Contains(neighbour))
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
                            // neighbour.h = Heuristic(neighbour);
                            neighbour.f = neighbour.g + neighbour.h;
                            neighbour.previous = current;
                        }
                    }
                }
            }
            return (PathToSolution);
        }

        public static Spot SearchPos(int toFind, int[,] toSearch)
        {
            Spot pos = new Spot(0, 0);
            int dimention = Convert.ToInt32(Math.Sqrt(toSearch.Length));
            for (int x = 0; x < dimention; x++)
                for (int y = 0; y < dimention; y++)
                {
                    if (toSearch[x, y] == toFind)
                    {
                        pos.x = x;
                        pos.y = y;
                        return (pos);
                    }
                }
            return (pos);
        }
        // Heuristic
        public static double ManhattonHeuristic(ASNode current)
        {
            double Manhatton = 0.0;
            Spot start;
            Spot end;
            for (int x = 0; x < current.dimention; x++)
            {
                for (int y = 0; y < current.dimention; y++)
                {
                    if (current.puzzle[x, y] != current.goalGrid[x, y])
                    {
                        start = SearchPos(current.puzzle[x, y], current.puzzle);
                        end = SearchPos(current.puzzle[x, y], current.goalGrid);
                        Manhatton += Ft_distance(start, end);
                    }
                }
            }
            return (Manhatton);
        }

        private static double Ft_distance(Spot a, Spot b)
        {
            return (Math.Abs((a.x - b.x) + (a.y - b.y)));
        }
        public static bool Ft_same(int[,] a, int[,] b)
        {
            for (int x = 0; x < Math.Sqrt(a.Length); x++)
                for (int y = 0; y < Math.Sqrt(a.Length); y++)
                    if (a[x, y] != b[x, y])
                        return (false);

            return (true);
        }

        public bool Isolvable(int[,] grid, int dimentions)
        {
            int invert = Invertions(grid);

            Console.WriteLine("{0}", invert);
            Console.ReadLine();
            if (dimentions % 2 != 0)
            {
                if (invert % 2 != 0)
                    return (false);
                else
                    return (true);
            }
            else if (dimentions % 2 == 0)
            {
                Spot find = new Spot(0,0);

                find = find.SearchPos(0, dimentions, grid);
                int blank = find.x;
                if ((invert + blank - find.y) % 2 == 0)
                    return (false);
                else
                    return (true);
            }
            return (false);
        }

        static int Invertions(int[,] grid)
        {
            int invert = 0;
            List<int> inve = new List<int>();
            //int x = 0;
            //int y = 0;
            //int zx = 0;
            //int zy = 0;
            //int index = 1;
            int rows = Convert.ToInt32(Math.Sqrt(grid.Length));
            int cols = rows;
            int max = (rows * cols);

            //while (index <= max)
            //{
            //    while (y < cols)
            //    {
            //        if (index == max + 1)
            //            break;
            //        inve.Add(grid[x, y]);

            //        index++;
            //        y++;
            //    }

            //    index--;
            //    y--;
            //    x++;
            //    while (x < rows)
            //    {
            //        if (index == max + 1)
            //            break;
            //        inve.Add(grid[x, y]);
            //        index++;
            //        x++;
            //    }
            //    index--;
            //    x--;
            //    y--;
            //    while (y >= zy)
            //    {
            //        if (index == max + 1)
            //            break;
            //        inve.Add(grid[x, y]);
            //        index++;
            //        y--;
            //    }
            //    index--;
            //    y++;
            //    x--;
            //    while (x > zx)
            //    {
            //        if (index == max + 1)
            //            break;
            //        inve.Add(grid[x, y]);
            //        index++;
            //        x--;
            //    }
            //    if (index == max + 1)
            //        break;
            //    index--;
            //    x++;
            //    y++;
            //    cols--;
            //    rows--;
            //    zy++;
            //    zx++;
            //}

            //inve.Remove(0);

            int x = 0;
            foreach (int element in grid)
            {     
                Console.WriteLine(element);

                for (int y = 0; y < Math.Sqrt(grid.Length); y++)
                {
                    Console.WriteLine("{0} and {1}", element, grid[x, y]);
                    if (element > grid[x, y] && element != 0 && grid[x, y] != 0)
                    {
                        Console.WriteLine("yey");
                        invert++;
                    }
                }
                Console.WriteLine("******{0}*********", x);
                x++;
                if (x == Math.Sqrt(grid.Length))
                    x = 0;
            }
            //for (int i = 0; i < inve.Count; i++)
            //    for (int j = i; j < inve.Count; j++)
            //        if (inve[i] > inve[j])
            //            invert++;
            Console.WriteLine("===={0}", invert);
            return (invert);
        }
    }
}
