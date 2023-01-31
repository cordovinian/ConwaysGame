using ConwaysGame;
using FluentAssertions;

namespace ConwaysGameTests;

public class ConwaysGameTests : ConwaysGameClass {}

public class GridTests : Grid
{
    [Fact]
    public void Grid_ReturnsEmptyGrid()
    {
        // Given & when
        var newGrid = new Grid();
        var gridAs2DList = newGrid.As2DList();

        // Then
        newGrid.Width.Should().Be(0);
        newGrid.Length.Should().Be(0);
        gridAs2DList.Should().BeEmpty();
    }

    [Fact]
    public void Grid_ReturnsGridMatchingInput()
    {
        // Given
        var expectedGrid = new List<List<int>>{
            new List<int>{ 0, 0, 0 },
            new List<int>{ 1, 1, 1 },
            new List<int>{ 0, 0, 0 }
        };

        // When
        var actualGrid = new Grid(expectedGrid);
        var actualAs2DList = actualGrid.As2DList();

        // Then
        actualGrid.Width.Should().Be(3);
        actualGrid.Length.Should().Be(3);
        actualAs2DList.Should().BeEquivalentTo(expectedGrid);
    }

    /// Note: This test has the potential to be very flakey
    [Theory]
    [InlineData(10, 10, 10)]
    [InlineData(10, 10, 3)]
    [InlineData(10, 10, 8)]
    [InlineData(100, 100, 80)]
    public void Grid_WithSeedValues_ReturnsRandomizedGrid(uint width, uint length, uint aliveCells)
    {
        // Given
        // NOTE: Starting values set in theory
        var twentyPercent = (int)Math.Round(aliveCells*0.2);
        if (twentyPercent < 5)
        {
            twentyPercent = 5;
        }
        var aliveLowerEnd = aliveCells - twentyPercent;
        var aliveUpperEnd = aliveCells + twentyPercent;

        // When
        var actualGrid = new Grid(width, length, aliveCells);
        var actualGridAs2DList = actualGrid.As2DList();
        var allGridValues = actualGridAs2DList.SelectMany(x => x).ToList();

        // Then
        actualGrid.Width.Should().Be((int)width);
        actualGrid.Length.Should().Be((int)length);
        actualGridAs2DList.Should().BeOfType<List<List<int>>?>();
        allGridValues.Count(i => i == 1).Should().BeInRange((int)aliveLowerEnd, (int)aliveUpperEnd);
    }

    [Fact]
    public void TryParseGrid_ReturnsGridAndSucess()
    {
        // Given
        var actualGrid = new Grid();
        var file = new FileInfo("GridSamples/Oscillator-Blinker.txt");
        var expectedGrid = new List<List<int>>{
            new List<int>{ 0, 0, 0 },
            new List<int>{ 1, 1, 1 },
            new List<int>{ 0, 0, 0 }
        };

        // When
        var result = Grid.TryParseGrid(out actualGrid, file.FullName, null);
        int expectedWidth = 0, expectedLength = 0;

        foreach (var line in System.IO.File.ReadLines(file.FullName))
        {
            if (expectedLength <= 0)
            {
                var cells = line.Split(',', StringSplitOptions.RemoveEmptyEntries & StringSplitOptions.TrimEntries);
                expectedWidth = cells.Length;
            }
            expectedLength++;
        }
        var actualAs2DList = actualGrid?.As2DList();

        // Then
        result.Should().BeTrue("TryParseGrid did not return success.");
        actualGrid.Should().NotBeNull();
        actualGrid?.Width.Should().Be(expectedWidth);
        actualGrid?.Length.Should().Be(expectedLength);
        actualAs2DList.Should().BeEquivalentTo(expectedGrid);
    }

    [Fact]
    public void Transition_UpdatesGridCorrectly()
    {
        // given
        var startGrid = new List<List<int>>{
            new List<int> { 0, 0, 0},
            new List<int> { 1, 1, 1},
            new List<int> { 0, 0, 0},
        };
        var grid = new Grid(startGrid);

        var expectedGrid = new List<List<int>>{
            new List<int> { 0, 1, 0},
            new List<int> { 0, 1, 0},
            new List<int> { 0, 1, 0},
        };

        // When
        grid.Transition();

        // Then
        var endGrid = grid.As2DList();
        endGrid.Should().BeEquivalentTo(expectedGrid);
    }
}