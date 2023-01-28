using ConwaysGame;
using FluentAssertions;
using System.Linq;

namespace ConwaysGameTests;

public class ConwaysGameTests
{
    [Fact]
    public void ReturnsTrueFunction_ReturnsTrue()
    {
        // Given
        bool alwaysTrue;

        // When
        alwaysTrue = ConwaysGameClass.ReturnsTrueAlways();

        // Then
        alwaysTrue.Should().BeTrue();
    }

    [Fact]
    public void Transition_ReturnsCorrectGrid()
    {
        // given
        var startGrid = new List<List<bool>>{
            new List<bool> { false, false, false},
            new List<bool> { true,  true,  true},
            new List<bool> { false, false, false},
        };

        var expectedGrid = new List<List<bool>>{
            new List<bool> { false, true, false},
            new List<bool> { false, true, false},
            new List<bool> { false, true, false},
        };

        // When
        var endGrid = ConwaysGameClass.Transition(startGrid);

        // Then
        endGrid.Should().BeEquivalentTo(expectedGrid);
    }
}