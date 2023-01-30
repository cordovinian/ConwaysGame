using System;
using System.CommandLine.DragonFruit;
using System.IO;
using System.Text;

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
        /// <param name="inPlace">Enable to refresh grid in-place on console.</param>
        /// <param name="gridLines">Enable showing grid lines.</param>
        /// <param name="refreshRate">How fast to recalculate next grid.</param>
        public static void Main(FileInfo input, int iterations = 20, bool inPlace = true, bool gridLines = false, int refreshRate = 100)
        {
            var inputGrid = new List<List<int>>();

            Console.CursorVisible = false;

            if (input == null)
            {
                inputGrid = oscillatingBlinkerGrid;

                Console.WriteLine("Default Grid - 'Blinker'");
                WriteGrid(inputGrid, inPlace: false, gridLines, "Default State:");
            }
            else if (input != null)
            {
                if (!input.Exists)
                {
                    Console.Error.WriteLine($"Input file '{input.FullName}' does not exist!");
                    return;
                }

                inputGrid = ParseGrid(input.FullName, null);

                Console.WriteLine($"From file - '{input.Name}'");
                WriteGrid(inputGrid, inPlace: false, gridLines, $"Initial State:");
            }

            for (var ii = 1; ii <= iterations; ii++)
            {
                Thread.Sleep(refreshRate);
                inputGrid = Transition(inputGrid);
                WriteGrid(inputGrid, inPlace: inPlace, gridLines, $"Step {ii}:");
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
            delimeters = delimeters ?? new char[] { ',', '\t', ';' };

            var grid = new List<List<int>>();

            int columnCount = 0;

            foreach (var line in System.IO.File.ReadLines(inputFileInfo))
            {
                var splitLine = line.Split(delimeters, StringSplitOptions.TrimEntries);

                if (splitLine.Length <= 0)
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

        // Format the Grid type into a console output grid
        private static string FormatGrid(List<List<int>> grid, bool gridLines = false)
        {
            string output = string.Empty;

            foreach (var row in grid)
            {
                var formattedDataRow = string.Empty;
                var tableRow = string.Empty;

                foreach (var col in row)
                {
                    var cell = col == 1 ? '■' : ' ';
                    var separator = gridLines ? " | " : " ";

                    formattedDataRow += String.Format($"{cell,-1}{separator}");
                }


                if (gridLines)
                {
                    // NOTE: Remove extra row separator before printing
                    formattedDataRow = formattedDataRow.Remove(formattedDataRow.Length - 3, 2);
                    output += string.Concat(formattedDataRow, '\n');

                    tableRow = new StringBuilder().Insert(0, "--|-", row.Count).ToString();
                    tableRow = tableRow.Remove(tableRow.Length - 3, 2);
                    output += string.Concat(tableRow, '\n');
                }
                else
                {
                    output += string.Concat(formattedDataRow, '\n');
                }
            }

            return output;
        }

        private static void WriteGrid(List<List<int>> grid, bool inPlace = true, bool gridLines = false, string header = "Grid State:")
        {
            var output = FormatGrid(grid, gridLines).Split('\n');//, StringSplitOptions.RemoveEmptyEntries);

            if (inPlace)
            {
                // NOTE: Shift console up rows (value + separator) plus header plus space
                int newCursorRow = (Console.CursorTop - output.Length - 1) < 0 ? 0 : Console.CursorTop - output.Length - 1;
                Console.SetCursorPosition(0, newCursorRow);

                // Clear grid space
                for (int i = 0; i < output.Length + 1; i++)
                {
                    var clearLineLength = i == 0 ? Console.WindowWidth : output[0].Length;
                    Console.WriteLine(new string(' ', clearLineLength));
                }

                Console.SetCursorPosition(0, newCursorRow);
            }

            if (!string.IsNullOrEmpty(header))
            {
                Console.WriteLine(header);
            }

            foreach (var line in output)
            {
                Console.WriteLine(line);
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
                        if (cc - 1 >= 0)
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

                    var peerCount = peers.Count(x => x == 1);

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
                    else
                    {
                        finalGrid[rr].Add(0);
                    }
                }
            }

            return finalGrid;
        }

    }
}
