namespace SpaceGame.Tests;

public class UnitTest1
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

    //! Q: Тесты     1) 7, 8;     2) 6, 7, 8;

    [Fact]
    public void RotateShip()
    {
        // Arrange
        double[] startPos = new double[] { 0, 0 };
        double angleDirection = Math.PI / 4;
        double angularSpeed = Math.PI / 2;
        double expected = Math.PI / 4 * 3;

        // Act
        Ship ship = new Ship(startPos, angleDirection, 0, angularSpeed);
        Rotation.Rotate(ship);
        double actual = ship.getAngleDirection();

        // Assert
        Assert.Equal(expected, actual);
    }
}
