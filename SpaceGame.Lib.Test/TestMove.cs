namespace SpaceGame.Lib.Test;
using Moq;
using Vector;

public class TestMove
{
    //* Для объекта, находящегося в точке (12, 5) и движущегося со скоростью (-7, 3) движение меняет положение объекта на (5, 8)
    [Fact]
    public void TestMoving()
    {
        Mock<IMovable> mock = new Mock<IMovable>();
        mock.Setup(x => x.Position).Returns(new Vector(12, 5)).Verifiable();
        mock.Setup(x => x.Velocity).Returns(new Vector(-7, 3)).Verifiable();

        MoveCommand moveCommand = new MoveCommand(mock.Object);
        moveCommand.Execute();

        var expected = new Vector(5, 8);
        mock.VerifySet(x => x.Position = expected, Times.Once);
    }

    //* Попытка сдвинуть объект, у которого невозможно прочитать положение объекта в пространстве, приводит к ошибке
    [Fact]
    public void CantReadPositionInMoving()
    {
        Mock<IMovable> mock = new Mock<IMovable>();
        mock
        .Setup(x => x.Position)
        .Throws(new Exception("Can't read object.Position"))
        .Verifiable();

        MoveCommand moveCommand = new MoveCommand(mock.Object);

        var expected = "Can't read object.Position";
        var error = Assert.Throws<Exception>(() => moveCommand.Execute());
        Assert.Equal(error.Message, expected);
    }

    //* Попытка сдвинуть объект, у которого невозможно прочитать значение мгновенной скорости, приводит к ошибке
    [Fact]
    public void CantReadVelocityInMoving()
    {
        Mock<IMovable> mock = new Mock<IMovable>();
        mock
        .Setup(x => x.Velocity)
        .Throws(new Exception("Can't read object.Velocity"))
        .Verifiable();

        MoveCommand moveCommand = new MoveCommand(mock.Object);

        var expected = "Can't read object.Velocity";
        var error = Assert.Throws<Exception>(() => moveCommand.Execute());
        Assert.Equal(error.Message, expected);
    }

    //* Попытка сдвинуть объект, у которого невозможно изменить положение в пространстве, приводит к ошибке
    [Fact]
    public void CantChangePositionInMoving()
    {
        Mock<IMovable> mock = new Mock<IMovable>();
        mock.SetupGet(x => x.Position).Returns(new Vector()).Verifiable();
        mock.SetupGet(x => x.Velocity).Returns(new Vector()).Verifiable();
        mock
        .SetupSet(x => x.Position = It.IsAny<Vector>())
        .Throws(new Exception("Can't change object.Position"))
        .Verifiable();

        MoveCommand moveCommand = new MoveCommand(mock.Object);

        var expected = "Can't change object.Position";
        var error = Assert.Throws<Exception>(() => moveCommand.Execute());
        Assert.Equal(error.Message, expected);
    }
}