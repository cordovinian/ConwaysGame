using System.Text;

namespace ConwaysGame
{
    /// Grid object
    public class Grid
    {
        private int[,] _grid { get; set; }

        /// Length of the grid.
        public int Length { get { return _grid.GetLength(0); } }

        /// Width of the grid.
        public int Width { get { return _grid.GetLength(1); } }

        private int[] GetRow(int rowIndex) {

            var row = new int[this.Width];

            for (int col = 0; col < this.Width; col++)
            {
                row[col] = _grid[rowIndex, col];
            }

            return row;
        }

        /// Grid as a multidimentional array.
        public int[,] As2DArray()
        {
            return _grid;
        }

        /// Grid as a list of lists.
        public List<List<int>> As2DList() {
            return Enumerable.Range(0, this.Length)
                    .Select(row => Enumerable.Range(0, this.Width)
                        .Select(col => _grid[row, col]).ToList())
                    .ToList();
            }

        /// <summary>Default constructor for Grid.</summary>
        public Grid()
        {
            _grid = new int[0,0];
        }

        /// <summary>Grid constructor with custom width, length, and seeded cells.</summary>
        public Grid(uint width, uint length, uint aliveCells)
        {
            _grid = RandomValuesGrid(width, length, aliveCells);
        }

        /// <summary>Grid constructor using two dimentional list of int`.</summary>
        /// <param name="grid">Two dimentional list of int".</param>
        public Grid(List<List<int>> grid)
        {
            var gridArray = new int[grid.Count,grid[0].Count];

            for (int row = 0; row < grid.Count; row++)
            {
                for (int column = 0; column < grid[0].Count; column++)
                {
                    gridArray[row,column] = grid[row][column];
                }
            }

            _grid = gridArray;
        }

        /// <summary>Grid constructor using two dimentional array of int`.</summary>
        /// <param name="grid">Two dimentional array of int".</param>
        public Grid(int[,] grid)
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

        /// <summary>Generate a grid of randomly seeded cells.</summary>
        private static int[,] RandomValuesGrid(uint width, uint length, uint alive)
        {
            var randomCellsGrid = new int[width, length];

            // Short circuit special cases
            if (alive == 0)
            {
                for (int row = 0; row < length; row++)
                {
                    for (int column = 0; column < width; column++)
                    {
                        randomCellsGrid[row,column] = 0;
                    }
                }
                return randomCellsGrid;
            }
            else if (alive == (width * length))
            {
                for (int row = 0; row < length; row++)
                {
                    for (int column = 0; column < width; column++)
                    {
                        randomCellsGrid[row,column] = 1;
                    }
                }
                return randomCellsGrid;
            }

            // Calculate the seconds between now and an arbitrary date to use as the seed value.
            var timeSeed = (int)Math.Round((DateTime.Now - DateTime.Today).TotalSeconds);

            var rando = new Random(timeSeed);

            var gridCellCount = width * length;
            var randoUpperBound = (int)(gridCellCount / alive);

            for (int rr = 0; rr < length; rr++)
            {
                for (int cc = 0; cc < width; cc++)
                {
                    var randomNumber = rando.Next(0, randoUpperBound);
                    var cellValue = (int)Math.Round((double)((randomNumber + 1) / randoUpperBound));
                    randomCellsGrid[rr,cc] = cellValue;
                }
            }

            return randomCellsGrid;
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
            // ASSUME: width is same for all rows
            var finalGridArr = new int[this.Width, this.Length];

            for (int row = 0; row < this.Length; row++)
            {
                for (int column = 0; column < this.Width; column++)
                {
                    var peers = new List<int>(8);

                    // TODO: Unused right now
                    int leftOfCell = row > 0 ? row - 1 : row;
                    int rightOfCell = row < this.Width ? row + 1 : row;

                    // TODO: Unused right now
                    int topOfCell = column > 0 ? column - 1 : column;
                    int bottomOfCell = column < this.Length ? column + 1 : column;

                    if (row - 1 >= 0)
                    {
                        if (column - 1 >= 0)
                        {
                            peers.Add(_grid[row - 1,column - 1]);
                        }
                        peers.Add(_grid[row - 1,column]);
                        if (column + 1 < this.Width)
                            peers.Add(_grid[row - 1,column + 1]);
                    }

                    if (column - 1 >= 0)
                    {
                        peers.Add(_grid[row,column - 1]);
                    }
                    if (column + 1 < this.Width)
                    {
                        peers.Add(_grid[row,column + 1]);
                    }

                    if (row + 1 < this.Length)
                    {
                        if (column - 1 >= 0)
                        {
                            peers.Add(_grid[row + 1,column - 1]);
                        }
                        peers.Add(_grid[row + 1,column]);
                        if (column + 1 < this.Width)
                        {
                            peers.Add(_grid[row + 1,column + 1]);
                        }
                    }

                    var peerCount = peers.Count(x => x == 1);

                    if (_grid[row,column] == 1 && peerCount < 2)
                    {
                        finalGridArr[row,column] = 0;
                    }
                    else if (_grid[row,column] == 1 && (peerCount == 2 || peerCount == 3))
                    {
                        finalGridArr[row,column] = 1;
                    }
                    else if (_grid[row,column] == 1 && (peerCount >= 3))
                    {
                        finalGridArr[row,column] = 0;
                    }
                    else if (_grid[row,column] == 0 && (peerCount == 3))
                    {
                        finalGridArr[row,column] = 1;
                    }
                    else
                    {
                        finalGridArr[row,column] = 0;
                    }
                }
            }

            _grid = finalGridArr;
        }

        /// <summary>Returns a string representation of the grid.</summary>
        public string ToString(bool gridLines = false, bool boxLines = false)
        {
            string output = string.Empty;

            var lineLength = gridLines ? this.Width * 4 : this.Width * 2;

            var columnSeparator = gridLines ? " | " : " ";
            var boxVerticalLine = boxLines ? "|" : string.Empty;
            var boxHorizontalLine = '-';

            for (var rr = 0; rr < this.Length; rr++)
            {
                var joinedDataRow = String.Join(columnSeparator, this.GetRow(rr));
                var formattedDataRow = String.Concat(boxVerticalLine, ' ', joinedDataRow, ' ', boxVerticalLine, '\n');
                // TODO: string replace not optimal for large grids
                formattedDataRow = formattedDataRow.Replace('0', ' ');
                formattedDataRow = formattedDataRow.Replace('1', '■');

                output += string.Concat(formattedDataRow);

                if (gridLines && rr < (this.Length - 1))
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