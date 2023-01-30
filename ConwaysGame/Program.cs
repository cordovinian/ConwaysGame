using System;
using System.CommandLine.DragonFruit;
using System.IO;

namespace ConwaysGame
{
    /// <summary>The main class for the program.</summary>
    public class ConwaysGameClass
    {
        /// <summary>
        /// Plays Conway's game of life for a number of iterations.
        /// </summary>
        /// <param name="iterations">The number of game iterations to go through.</param>
        /// <param name="input">The name of the input file to set as the starting grid state.</param>
        public static void Main(FileInfo input, int iterations=3)
        {
            var inputGrid = new List<List<int>>();

            if (input == null)
            {
                inputGrid = oscillatingBlinkerGrid;

                PrintGrid(inputGrid, "Default grid state:");
            }
            else if (input != null)
            {
                if (!input.Exists)
                {
                    Console.Error.WriteLine($"Input file '{input.FullName}' does not exist!");
                    return;
                }

                inputGrid = ParseGrid(input.FullName, null);

                PrintGrid(inputGrid, $"Initial State from file '{input.Name}':");
            }

            for (var ii = 1; ii <= iterations; ii++)
            {
                inputGrid = Transition(inputGrid);
                PrintGrid(inputGrid, $"Step {ii}:");
            }
        }

        private static List<List<int>> oscillatingBlinkerGrid = new List<List<int>>{
                new List<int> { 0, 1, 0},
                new List<int> { 0, 1, 0},
                new List<int> { 0, 1, 0},
            };

        // Parses a text file as a grid.
        private static List<List<int>> ParseGrid(string inputFileInfo, char[]? delimeters)
        {
            delimeters = delimeters ?? new char[] {',', '\t', ';'};

            var grid = new List<List<int>>();

            int columnCount = 0;

            foreach (var line in System.IO.File.ReadLines(inputFileInfo))
            {
                var splitLine = line.Split(delimeters, StringSplitOptions.TrimEntries);

                if (splitLine.Length <=0)
                {
                    throw new Exception("Parsed values don't match the expected number of cells in the row");
                }

                columnCount = columnCount == 0 ? splitLine.Length : columnCount;

                var row = new List<int>();

                foreach (var cell in splitLine)
                {
                    int cellValue = 0;
                    int.TryParse(cell, out cellValue);
                    row.Add(cellValue);
                }

                if (row.Count != columnCount)
                {
                    throw new Exception("Parsed values don't match the expected number of cells in the row");
                }

                grid.Add(row);
            }

            return grid;
        }

        private static void PrintGrid(List<List<int>> grid, string header = "Grid State:")
        {
            if (!string.IsNullOrEmpty(header))
            {
                Console.WriteLine(header);
            }

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

        /// Method to analyze the current state of the game grid and transition to the next state.
        protected static List<List<int>> Transition(List<List<int>> grid)
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
