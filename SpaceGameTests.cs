using Xunit;
using SpaceGame;

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
}