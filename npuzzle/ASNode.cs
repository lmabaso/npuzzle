using System;
using System.Collections.Generic;

namespace npuzzle
{
    class ASNode
    {
        public double f;
        public double g;
        public double h;

        public List<ASNode> neighbours = new List<ASNode>();
        public ASNode previous;
        public int[,] puzzle;
        public int[,] goalGrid;
        public int dimention;
        public int x;
        public int y;

        public ASNode(int[,] initPuzzle, int[,] iniEnd)
        {
            puzzle = initPuzzle;
            dimention = Convert.ToInt32(Math.Sqrt(initPuzzle.Length));
            goalGrid = iniEnd;/*Ft_expectedOutcome(dimention)*/
        }

        public void ExpandNode()
        {
            for (int i = 0; i < dimention; i++)
            {
                for (int j = 0; j < dimention; j++)
                {
                    if (puzzle[i, j] == 0)
                    {
                        x = i;
                        y = j;
                    }
                }
            }

            MoveUp(puzzle, x, y);
            MoveRight(puzzle, x, y);
            MoveDown(puzzle, x, y);
            MoveLeft(puzzle, x, y);
        }

        public void MoveRight(int[,] initPuzzle, int i, int j)
        {
            if (j < dimention - 1)
            {
                int[,] pc = new int[dimention, dimention];
                CopyPuzzle(pc, initPuzzle);

                int tmp = pc[i, j + 1];
                pc[i, j + 1] = pc[i, j];
                pc[i, j] = tmp;

                ASNode child = new ASNode(pc,goalGrid);
                //child.h = Heuristic(child);
                neighbours.Add(child);
                child.previous = this;
            }
        }

        public void MoveLeft(int[,] initPuzzle, int i, int j)
        {
            if (j > 0)
            {
                int[,] pc = new int[dimention, dimention];
                CopyPuzzle(pc, initPuzzle);

                int tmp = pc[i, j - 1];
                pc[i, j - 1] = pc[i, j];
                pc[i, j] = tmp;

                ASNode child = new ASNode(pc, goalGrid);
                neighbours.Add(child);
                //child.h = Heuristic(child);
                child.previous = this;
            }
        }

        public void MoveUp(int[,] initPuzzle, int i, int j)
        {
            if (i > 0)
            {
                int[,] pc = new int[dimention, dimention];
                CopyPuzzle(pc, initPuzzle);

                int tmp = pc[i - 1, j];
                pc[i - 1, j] = pc[i, j];
                pc[i, j] = tmp;

                ASNode child = new ASNode(pc,goalGrid);
                //child.h = Heuristic(child); ;
                neighbours.Add(child);
                child.previous = this;
            }
        }

        public void MoveDown(int[,] initPuzzle, int i, int j)
        {
            if (i < dimention - 1)
            {
                int[,] pc = new int[dimention, dimention];
                CopyPuzzle(pc, initPuzzle);

                int tmp = pc[i + 1, j];
                pc[i + 1, j] = pc[i, j];
                pc[i, j] = tmp;

                ASNode child = new ASNode(pc, goalGrid);
                //child.h = Heuristic(child);
                neighbours.Add(child);
                child.previous = this;
            }
        }

        public void PrintPuzzle()
        {
            Console.WriteLine();
            for (int x = 0; x < dimention; x++)
            {
                for (int y = 0; y < dimention; y++)
                    Console.Write("{0,2} ", puzzle[x, y]);
                Console.WriteLine();
            }
        }

        public void CopyPuzzle(int[,] a, int[,] b)
        {
            for (int x = 0; x < dimention; x++)
            {
                for (int y = 0; y < dimention; y++)
                {
                    a[x, y] = b[x, y];
                }
            }
        }

        public bool IsSamePuzzle(int[,] initPuzzle)
        {
            bool samePuzzle = true;

            for (int x = 0; x < dimention; x++)
            {
                for (int y = 0; y < dimention; y++)
                {
                    if (initPuzzle[x, y] != puzzle[x, y])
                    {
                        return (false);
                    }
                }
            }
            return (samePuzzle);
        }

        public bool GoalTest()
        {
            bool isGoal = true;

            for (int x = 0; x < dimention; x++)
            {
                for (int y = 0; y < dimention; y++)
                {
                    if (puzzle[x, y] != goalGrid[x, y])
                    {
                        return (false);
                    }
                }
            }
            return (isGoal);
        }
    }
}
