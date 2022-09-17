using Xunit;
using SpaceGame;
using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
public class SpaceGameTests
{
    [Fact]
    public void MoveShip()
    {
        // Arrange
        double[] startPos = new double[] { 12, 5 };
        double[] speedVector = new double[] { -7, 3 };
        double[] expected = new double[] { 5, 8 };

        // Act
        Ship ship = new Ship(startPos, speedVector);
        Movement.Move(ship);
        double[] actual = ship.getCoords();

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void NoCoordinatsMove()
    {
        // Arrange
        double[] startPos = new double[] { };
        double[] speedVector = new double[] { -7, 3 };
        string actual = "No coordinates";

        // Act
        Ship ship = new Ship(startPos, speedVector);

        // Assert
        ArgumentException expected = Assert.Throws<ArgumentException>(() => Movement.Move(ship));
        Assert.Equal(expected.Message, actual);
    }

    // [Fact]
    // public void NoSpeedMove()
    // {
    //     // Arrange
    //     int[] startPos = new int[] { 12, 5 };
    //     int[] speedVector = new int[] { };

    //     // Act
    //     Ship ship = new Ship(startPos, speedVector);

    //     // Assert
    //     ArgumentException expected = Assert.Throws<ArgumentException>(() => Movement.Move(ship));
    //     Assert.Equal(expected.Message, "No speed vector");
    // }
}