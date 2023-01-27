using System;

namespace ConwaysGame
{
    public class ConwaysGameClass
    {
        public static void Main(string[] args)
        {
            // See https://aka.ms/new-console-template for more information
            Console.WriteLine("Hello, World!");

            var startGrid = new List<List<bool>>{
                new List<bool> { false, false, false},
                new List<bool> { true, true, true},
                new List<bool> { false, false, false},
            };
            PrintGrid(startGrid);
            var finalGrid = Transition(startGrid);
            PrintGrid(finalGrid);

        }

        public static bool ReturnsTrueAlways() => true;

        public static void PrintGrid(List<List<bool>> grid)
        {
            Console.WriteLine("Grid state:");
            foreach(var row in grid)
            {
                Console.WriteLine(row);
            }
        }
        public static List<List<bool>> Transition(List<List<bool>> grid)
        {
            var finalGrid = new List<List<bool>>();

            var row = grid.Count();
            // Assume width is same for all rows
            var col = grid[0].Count();

            for (int rr = 0; rr < row; rr++)
            {
                for (int cc = 0; cc < col; cc++)
                {
                    var peers = new List<bool>(8);

                    if (rr - 1 >= 0)
                    {
                        if (cc-1 >= 0)
                        {
                            peers.Add(grid[rr - 1][cc - 1]);
                        }
                        peers.Add(grid[rr - 1][cc]);
                        if (cc + 1 < col)
                            peers.Add(grid[rr - 1][cc + 1]);
                    }

                    if (cc - 1 >= 0)
                    {
                        peers.Add(grid[rr][cc - 1]);
                    }
                    if (cc + 1 < col)
                    {
                        peers.Add(grid[rr][cc + 1]);
                    }

                    if (rr + 1 < row)
                    {
                        if (cc - 1 >= 0)
                        {
                            peers.Add(grid[rr + 1][cc - 1]);
                        }
                        peers.Add(grid[rr + 1][cc]);
                        if (cc + 1 < col)
                        {
                            peers.Add(grid[rr + 1][cc + 1]);
                        }
                    }

                    var peerCount = peers.Count(x => x==true);

                    if (grid[rr][cc] == true && peerCount < 2)
                    {
                        finalGrid[rr][cc] = false;
                    }
                    else if (grid[rr][cc] == true && (peerCount == 2 || peerCount == 3))
                    {
                        finalGrid[rr][cc] = true;
                    }
                    else if (grid[rr][cc] == true && (peerCount >= 3))
                    {
                        finalGrid[rr][cc] = false;
                    }
                    else if (grid[rr][cc] == false && (peerCount == 3))
                    {
                        finalGrid[rr][cc] = true;
                    }
                    else {
                        finalGrid[rr][cc] = false;
                    }
                }
            }

            return finalGrid;
        }

    }
}
