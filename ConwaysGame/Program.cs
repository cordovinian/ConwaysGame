using System;

namespace ConwaysGame
{
    public class ConwaysGameClass
    {
        public static void Main(string[] args)
        {
            // TEMP: Start with a known grid
            var grid = new List<List<bool>>{
                new List<bool> { false, false, false},
                new List<bool> { true, true, true},
                new List<bool> { false, false, false},
            };

            PrintGrid(grid);

            foreach(var iteration in Enumerable.Range(1,2))
            {
                PrintGrid(Transition(grid));
            }
        }

        public static bool ReturnsTrueAlways() => true;

        public static void PrintGrid(List<List<bool>> grid)
        {
            Console.WriteLine("Grid state:");

            foreach(var row in grid)
            {
                var formattedDataRow = string.Empty;
                var tableRow = string.Empty;

                foreach(var col in row)
                {
                    formattedDataRow += String.Format($"{col, -5} | ");
                    tableRow += (String.Format($"----- | "));
                }

                // Remove extra row separator before printing
                formattedDataRow = formattedDataRow.Remove(formattedDataRow.Length - 3, 2);
                tableRow = tableRow.Remove(tableRow.Length - 3, 2);

                Console.WriteLine(formattedDataRow);
                Console.WriteLine(tableRow);
            }
        }

        public static List<List<bool>> Transition(List<List<bool>> grid)
        {
            var finalGrid = new List<List<bool>>();

            // Assume width is same for all rows
            var row = grid.Count();
            var col = grid[0].Count();

            for (int rr = 0; rr < row; rr++)
            {
                // Initialize new row in final grid
                finalGrid.Add(new List<bool>(col));

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
                        finalGrid[rr].Add(false);
                    }
                    else if (grid[rr][cc] == true && (peerCount == 2 || peerCount == 3))
                    {
                        finalGrid[rr].Add(true);
                    }
                    else if (grid[rr][cc] == true && (peerCount >= 3))
                    {
                        finalGrid[rr].Add(false);
                    }
                    else if (grid[rr][cc] == false && (peerCount == 3))
                    {
                        finalGrid[rr].Add(true);
                    }
                    else {
                        finalGrid[rr].Add(false);
                    }
                }
            }

            return finalGrid;
        }

    }
}
