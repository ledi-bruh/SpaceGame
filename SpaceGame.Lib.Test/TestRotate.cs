namespace SpaceGame.Lib.Test;
using Moq;

public class TestRotate
{
    //* Корабль, который находится под углом к горизонту в 45 градусов имеет угловую скорость 90 градусов.
    //* В результате поворота корабль оказывается под углом 135 градусов к горизонту.
    [Fact]
    public void TestRotating()
    {
        Mock<IRotatable> mock = new Mock<IRotatable>();
        mock.Setup(x => x.Direction).Returns(45).Verifiable();
        mock.Setup(x => x.AngularVelocity).Returns(90).Verifiable();

        RotateCommand rotateCommand = new RotateCommand(mock.Object);
        rotateCommand.Execute();

        int expected = 135;
        mock.VerifySet(x => x.Direction = expected, Times.Once);
    }

    //* Попытка сдвинуть корабль, у которого невозможно прочитать значение угла наклона к горизонту, приводит к ошибке.
    [Fact]
    public void CantReadDirectionInRotating()
    {
        Mock<IRotatable> mock = new Mock<IRotatable>();
        mock
        .Setup(x => x.Direction)
        .Throws(new Exception("Can't read object.Direction"))
        .Verifiable();

        RotateCommand rotateCommand = new RotateCommand(mock.Object);

        var expected = "Can't read object.Direction";
        var error = Assert.Throws<Exception>(() => rotateCommand.Execute());
        Assert.Equal(error.Message, expected);
    }

    //* Попытка сдвинуть корабль, у которого невозможно прочитать значение угловой скорости, приводит к ошибке.
    [Fact]
    public void CantReadAngularVelocityInRotating()
    {
        Mock<IRotatable> mock = new Mock<IRotatable>();
        mock
        .Setup(x => x.AngularVelocity)
        .Throws(new Exception("Can't read object.AngularVelocity"))
        .Verifiable();

        RotateCommand rotateCommand = new RotateCommand(mock.Object);

        var expected = "Can't read object.AngularVelocity";
        var error = Assert.Throws<Exception>(() => rotateCommand.Execute());
        Assert.Equal(error.Message, expected);
    }

    //* Попытка сдвинуть корабль, у которого изменить угол наклона к горизонту, приводит к ошибке.
    [Fact]
    public void CantChangeDirectionInRotating()
    {
        Mock<IRotatable> mock = new Mock<IRotatable>();
        mock
        .SetupSet(x => x.Direction = It.IsAny<int>())
        .Throws(new Exception("Can't change object.Direction"))
        .Verifiable();

        RotateCommand rotateCommand = new RotateCommand(mock.Object);

        var expected = "Can't change object.Direction";
        var error = Assert.Throws<Exception>(() => rotateCommand.Execute());
        Assert.Equal(error.Message, expected);
    }
}