using System.Text;

namespace ConwaysGame
{
    /// Grid object
    public class Grid
    {
        private List<List<int>> _grid { get; set; }

        /// Length of the grid.
        public int Length { get { return _grid.Count; } }

        /// Width of the grid.
        public int Width { get { return _grid.Count == 0 ? 0 : _grid[0].Count; } }

        /// Grid as a list of lists.
        public List<List<int>> As2DList() { return _grid; }

        /// <summary>Default constructor for Grid.</summary>
        public Grid()
        {
            _grid = new List<List<int>>();
        }

        /// <summary>Grid constructor with custom width, length, and seeded cells.</summary>
        public Grid(uint width, uint length, uint aliveCells)
        {
            _grid = RandomValuesGrid(width, length, aliveCells);
        }

        /// <summary>Generate a grid of randomly seeded cells.</summary>
        private static List<List<int>> RandomValuesGrid(uint width, uint length, uint alive)
        {
            var randomCellsGrid = new List<List<int>>((int)length);

            // Calculate the seconds between now and an arbitrary date to use as the seed value.
            var timeSeed = (int)Math.Round((DateTime.Now - DateTime.Today).TotalSeconds);

            var rando = new Random(timeSeed);

            var gridCellCount = width * length;
            var randoUpperBound = (int)(gridCellCount / alive);

            for (int rr = 0; rr < length; rr++)
            {
                randomCellsGrid.Add(new List<int>((int)width));

                for (int cc = 0; cc < width; cc++)
                {
                    var randomNumber = rando.Next(0, randoUpperBound);
                    var cellValue = (int)Math.Round((double)((randomNumber + 1) / randoUpperBound));
                    randomCellsGrid[rr].Add(cellValue);
                }
            }

            return randomCellsGrid;
        }

        /// <summary>Grid constructor using two dimentional list of int`.</summary>
        /// <param name="grid">Two dimentional list of int".</param>
        public Grid(List<List<int>> grid)
        {
            this._grid = grid;
        }

        /// <summary>Try to parse a text file as a grid</summary>
        /// <param name="grid">Two dimentional list of int.</param>
        /// <param name="inputFileInfo">Input file.</param>
        /// <param name="delimeters">Char array of possible cell delimiters.</param>
        public static bool TryParseGrid(out Grid? grid, string inputFileInfo, char[]? delimeters)
        {
            delimeters = delimeters ?? new char[] { ',', '\t', ';' };

            try
            {
                grid = parseGrid(inputFileInfo, delimeters);
            }
            catch (System.Exception)
            {
                grid = null;
                return false;
            }

            return true;
        }

        private static Grid parseGrid(string inputFileInfo, char[] delimeters)
        {
            var tryGrid = new List<List<int>>();

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
                    if (!int.TryParse(cell, out cellValue))
                    {
                        throw new Exception($"Failed to parse value '{cell}'.");
                    }
                    row.Add(cellValue);
                }

                if (row.Count != columnCount)
                {
                    throw new Exception("Parsed values don't match the expected number of cells in the row");
                }

                tryGrid.Add(row);
            }

            return new Grid(tryGrid);
        }

        /// <summary>Method to analyze the current state of the game grid and transition to the next state.</summary>
        public void Transition()
        {
            var finalGrid = new List<List<int>>();

            // ASSUME: width is same for all rows
            for (int rr = 0; rr < this.Length; rr++)
            {
                // Initialize new row in final grid
                finalGrid.Add(new List<int>(this.Width));

                for (int cc = 0; cc < this.Width; cc++)
                {
                    var peers = new List<int>(8);

                    if (rr - 1 >= 0)
                    {
                        if (cc - 1 >= 0)
                        {
                            peers.Add(_grid[rr - 1][cc - 1]);
                        }
                        peers.Add(_grid[rr - 1][cc]);
                        if (cc + 1 < this.Width)
                            peers.Add(_grid[rr - 1][cc + 1]);
                    }

                    if (cc - 1 >= 0)
                    {
                        peers.Add(_grid[rr][cc - 1]);
                    }
                    if (cc + 1 < this.Width)
                    {
                        peers.Add(_grid[rr][cc + 1]);
                    }

                    if (rr + 1 < this.Length)
                    {
                        if (cc - 1 >= 0)
                        {
                            peers.Add(_grid[rr + 1][cc - 1]);
                        }
                        peers.Add(_grid[rr + 1][cc]);
                        if (cc + 1 < this.Width)
                        {
                            peers.Add(_grid[rr + 1][cc + 1]);
                        }
                    }

                    var peerCount = peers.Count(x => x == 1);

                    if (_grid[rr][cc] == 1 && peerCount < 2)
                    {
                        finalGrid[rr].Add(0);
                    }
                    else if (_grid[rr][cc] == 1 && (peerCount == 2 || peerCount == 3))
                    {
                        finalGrid[rr].Add(1);
                    }
                    else if (_grid[rr][cc] == 1 && (peerCount >= 3))
                    {
                        finalGrid[rr].Add(0);
                    }
                    else if (_grid[rr][cc] == 0 && (peerCount == 3))
                    {
                        finalGrid[rr].Add(1);
                    }
                    else
                    {
                        finalGrid[rr].Add(0);
                    }
                }
            }

            _grid = finalGrid;
        }

        /// <summary>Returns a string representation of the grid.</summary>
        public string ToString(bool gridLines = false, bool boxLines = false)
        {
            string output = string.Empty;

            var lineLength = gridLines ? _grid.Count * 4 : _grid.Count * 2;

            var columnSeparator = gridLines ? " | " : " ";
            var boxVerticalLine = boxLines ? "|" : string.Empty;
            var boxHorizontalLine = '-';

            for (var rr = 0; rr < _grid.Count; rr++)
            {
                var joinedDataRow = String.Join(columnSeparator, _grid[rr]);
                var formattedDataRow = String.Concat(boxVerticalLine, ' ', joinedDataRow, ' ', boxVerticalLine, '\n');
                // TODO: string replace not optimal for large grids
                formattedDataRow = formattedDataRow.Replace('0', ' ');
                formattedDataRow = formattedDataRow.Replace('1', '■');

                output += string.Concat(formattedDataRow);

                if (gridLines && rr < (_grid.Count - 1))
                {
                    var tableRow = formattedDataRow;
                    // TODO: string replace not optimal for large grids
                    tableRow = tableRow.Replace("   ", "---");
                    tableRow = tableRow.Replace(" ■ ", "---");
                    output += tableRow;
                }
            }

            // Bounding box top and bottom
            if (boxLines)
            {
                var boxHorizontal = string.Concat(new string(boxHorizontalLine, output.IndexOf('\n')), '\n');
                output = string.Concat(boxHorizontal, output, boxHorizontal);
            }

            return output;
        }
    }
}