namespace SpaceGame.Lib.Test;
using Moq;
using Angle;

public class UnitTest1
{
    //* Корабль, который находится под углом к горизонту в 45 градусов имеет угловую скорость 90 градусов.
    //* В результате поворота корабль оказывается под углом 135 градусов к горизонту.
    [Theory]
    [MemberData(nameof(TestRotatingData))]
    public void TestRotating(Angle direction, Angle angularVelocity, Angle expected)
    {
        Mock<IRotatable> mock = new Mock<IRotatable>();
        mock.Setup(x => x.Direction).Returns(direction).Verifiable();
        mock.Setup(x => x.AngularVelocity).Returns(angularVelocity).Verifiable();

        RotateCommand rotateCommand = new RotateCommand(mock.Object);
        rotateCommand.Execute();

        mock.VerifySet(x => x.Direction = expected, Times.Once);
    }
    public static TheoryData<Angle, Angle, Angle> TestRotatingData => new()
    {
        {new Angle(1, 1), new Angle(0, 1), new Angle(-1, 1)},
        {new Angle(0, 1), new Angle(0, 1), new Angle(-1, 0)},
        {new Angle(1, 0), new Angle(1, 0), new Angle(1, 0)},
        {new Angle(0, 0), new Angle(0, 0), new Angle(0, 0)},
        {new Angle(0, 0), new Angle(1, 1), new Angle(1, 1)},
        {new Angle(1, 2), new Angle(0, 0), new Angle(1, 2)},
    };

    //* Попытка сдвинуть корабль, у которого невозможно прочитать значение угла наклона к горизонту, приводит к ошибке.
    [Fact]
    public void CantReadDirectionInRotating()
    {
        Mock<IRotatable> mock = new Mock<IRotatable>();
        mock
        .Setup(x => x.Direction)
        .Throws(new ArgumentException("Can't read object.Direction"))
        .Verifiable();

        RotateCommand rotateCommand = new RotateCommand(mock.Object);

        var expected = "Can't read object.Direction";
        var error = Assert.Throws<ArgumentException>(() => rotateCommand.Execute());
        Assert.Equal(error.Message, expected);
    }

    //* Попытка сдвинуть корабль, у которого невозможно прочитать значение угловой скорости, приводит к ошибке.
    [Fact]
    public void CantReadAngularVelocityInRotating()
    {
        Mock<IRotatable> mock = new Mock<IRotatable>();
        mock
        .Setup(x => x.AngularVelocity)
        .Throws(new ArgumentException("Can't read object.AngularVelocity"))
        .Verifiable();

        RotateCommand rotateCommand = new RotateCommand(mock.Object);

        var expected = "Can't read object.AngularVelocity";
        var error = Assert.Throws<ArgumentException>(() => rotateCommand.Execute());
        Assert.Equal(error.Message, expected);
    }

    //* Попытка сдвинуть корабль, у которого изменить угол наклона к горизонту, приводит к ошибке.
    [Fact]
    public void CantChangeDirectionInRotating()
    {
        Mock<IRotatable> mock = new Mock<IRotatable>();
        mock.SetupGet(x => x.Direction).Returns(new Angle(0, 0)).Verifiable();
        mock.SetupGet(x => x.AngularVelocity).Returns(new Angle(0, 0)).Verifiable();
        mock
        .SetupSet(x => x.Direction = It.IsAny<Angle>())
        .Throws(new ArithmeticException("Can't change object.Direction"))
        .Verifiable();

        RotateCommand rotateCommand = new RotateCommand(mock.Object);

        var expected = "Can't change object.Direction";
        var error = Assert.Throws<ArithmeticException>(() => rotateCommand.Execute());
        Assert.Equal(error.Message, expected);
    }
}