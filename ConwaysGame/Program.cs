using System;

namespace ConwaysGame
{
    public class ConwaysGameClass
    {
        public static void Main(string[] args)
        {
            // TEMP: Start with a known grid
            var grid = new List<List<int>>{
                new List<int> { 0, 0, 0},
                new List<int> { 1, 1, 1},
                new List<int> { 0, 0, 0},
            };

            PrintGrid(grid);

            foreach(var iteration in Enumerable.Range(1,2))
            {
                PrintGrid(Transition(grid));
            }
        }

        public static void PrintGrid(List<List<int>> grid)
        {
            Console.WriteLine("Grid state:");

            foreach(var row in grid)
            {
                var formattedDataRow = string.Empty;
                var tableRow = string.Empty;

                foreach(var col in row)
                {
                    formattedDataRow += String.Format($"{col, -1} | ");
                    tableRow += (String.Format($"--|-"));
                }

                // Remove extra row separator before printing
                formattedDataRow = formattedDataRow.Remove(formattedDataRow.Length - 3, 2);
                tableRow = tableRow.Remove(tableRow.Length - 3, 2);

                Console.WriteLine(formattedDataRow);
                Console.WriteLine(tableRow);
            }
        }

        public static List<List<int>> Transition(List<List<int>> grid)
        {
            var finalGrid = new List<List<int>>();

            // ASSUME: width is same for all rows
            var row = grid.Count();
            var col = grid[0].Count();

            for (int rr = 0; rr < row; rr++)
            {
                // Initialize new row in final grid
                finalGrid.Add(new List<int>(col));

                for (int cc = 0; cc < col; cc++)
                {
                    var peers = new List<int>(8);

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

                    var peerCount = peers.Count(x => x==1);

                    if (grid[rr][cc] == 1 && peerCount < 2)
                    {
                        finalGrid[rr].Add(0);
                    }
                    else if (grid[rr][cc] == 1 && (peerCount == 2 || peerCount == 3))
                    {
                        finalGrid[rr].Add(1);
                    }
                    else if (grid[rr][cc] == 1 && (peerCount >= 3))
                    {
                        finalGrid[rr].Add(0);
                    }
                    else if (grid[rr][cc] == 0 && (peerCount == 3))
                    {
                        finalGrid[rr].Add(1);
                    }
                    else {
                        finalGrid[rr].Add(0);
                    }
                }
            }

            return finalGrid;
        }

    }
}
