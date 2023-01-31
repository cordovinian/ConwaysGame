using System;
using System.CommandLine.DragonFruit;
using System.IO;
using System.Text;

namespace ConwaysGame
{
    /// <summary>The main class for the program.</summary>
    public class ConwaysGameClass
    {
        private static List<List<int>> oscillatingBlinkerGrid = new List<List<int>>{
            new List<int> { 0, 1, 0},
            new List<int> { 0, 1, 0},
            new List<int> { 0, 1, 0},
        };

        /// <summary>
        /// Plays Conway's game of life for a number of iterations.
        /// </summary>
        /// <param name="input">The name of the input file to set as the starting grid state.</param>
        /// <param name="width">The width of the grid.</param>
        /// <param name="length">The length (height) of the grid.</param>
        /// <param name="alive">The expected number of cell that start alive.</param>
        /// <param name="iterations">The number of game iterations to go through.</param>
        /// <param name="refreshRate">How fast to recalculate next grid.</param>
        /// <param name="eachStep">Enable to display each iteration step.</param>
        /// <param name="gridLines">Enable showing grid lines.</param>
        /// <param name="boxLines">Enable showing bounding box lines.</param>
        public static void Main(
                                FileInfo input,
                                uint? width = 10,
                                uint? length = 10,
                                uint? alive = 10,
                                int iterations = 20,
                                int refreshRate = 100,
                                bool eachStep = false,
                                bool gridLines = false,
                                bool boxLines = false)
        {
            Grid inputGrid;

            Console.CursorVisible = false;

            if (input == null && width != null && length != null && alive != null)
            {
                inputGrid = new Grid((uint)width, (uint)length, (uint)alive);

                Console.WriteLine($"Seeded Grid - W:{width} L:{length} A:{alive}");
                WriteGrid(inputGrid, "Default State:", eachStep: true, gridLines, boxLines);
            }
            else if (input != null)
            {
                if (!input.Exists)
                {
                    Console.Error.WriteLine($"Input file '{input.FullName}' does not exist!");
                    return;
                }

                var gridParseResult = Grid.TryParseGrid(out inputGrid, input.FullName, null);
                if (!gridParseResult || inputGrid == null)
                {
                    Console.Error.WriteLine($"Failed to parse the file '{input.Name}' as a valid grid.");
                    return;
                }

                Console.WriteLine($"From file - '{input.Name}'");
                WriteGrid(inputGrid, $"Initial State:", eachStep: true, gridLines, boxLines);
            }
            else
            {
                inputGrid = new Grid(oscillatingBlinkerGrid);

                Console.WriteLine("Default Grid - 'Blinker'");
                WriteGrid(inputGrid, "Default State:", eachStep: true, gridLines, boxLines);
            }

            for (var ii = 1; ii <= iterations; ii++)
            {
                Thread.Sleep(refreshRate);
                inputGrid.Transition();
                WriteGrid(inputGrid, $"Step {ii}:", eachStep, gridLines, boxLines);
            }
        }

        private static void WriteGrid(
                                        Grid grid,
                                        string header = "Grid State:",
                                        bool eachStep = false,
                                        bool gridLines = false,
                                        bool boxLines = false)
        {
            var gridOutput = grid.ToString(gridLines, boxLines).Split('\n');

            var lineLength = gridOutput[0].Length;

            if (!eachStep)
            {
                // NOTE: Shift console up rows (value + separator) plus header plus space
                int newCursorRow = (Console.CursorTop - gridOutput.Length - 1) < 0 ? 0 : Console.CursorTop - gridOutput.Length - 1;
                Console.SetCursorPosition(0, newCursorRow);

                // Clear grid space
                for (int i = 0; i < gridOutput.Length + 1; i++)
                {
                    var clearLineLength = i == 0 ? Console.WindowWidth : lineLength;
                    Console.WriteLine(new string(' ', clearLineLength));
                }

                Console.SetCursorPosition(0, newCursorRow);
            }

            if (!string.IsNullOrEmpty(header))
            {
                Console.WriteLine(header);
            }

            // Grid
            foreach (var line in gridOutput)
            {
                Console.WriteLine(line);
            }
        }
    }
}
