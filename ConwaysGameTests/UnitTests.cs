using ConwaysGame;
using FluentAssertions;

namespace ConwaysGameTests;

public class ConwaysGameTests : ConwaysGameClass
{
    [Fact]
    public void Transition_ReturnsCorrectGrid()
    {
        // given
        var startGrid = new List<List<int>>{
            new List<int> { 0, 0, 0},
            new List<int> { 1, 1, 1},
            new List<int> { 0, 0, 0},
        };

        var expectedGrid = new List<List<int>>{
            new List<int> { 0, 1, 0},
            new List<int> { 0, 1, 0},
            new List<int> { 0, 1, 0},
        };

        // When
        var endGrid = ConwaysGameClass.Transition(startGrid);

        // Then
        endGrid.Should().BeEquivalentTo(expectedGrid);
    }
}