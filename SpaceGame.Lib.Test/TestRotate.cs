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
        mock.SetupGet(x => x.Direction).Returns(45).Verifiable();
        mock.SetupGet(x => x.AngularVelocity).Returns(90).Verifiable();
        RotateCommand rotateCommand = new RotateCommand(mock.Object);

        rotateCommand.Execute();

        mock.VerifySet(x => x.Direction = 135, Times.Once);
    }

    //* Попытка сдвинуть корабль, у которого невозможно прочитать значение угла наклона к горизонту, приводит к ошибке.
    [Fact]
    public void CantReadDirectionInRotating()
    {
        Mock<IRotatable> mock = new Mock<IRotatable>();
        mock.Setup(x => x.Direction).Throws(new Exception()).Verifiable();
        RotateCommand rotateCommand = new RotateCommand(mock.Object);

        Assert.Throws<Exception>(() => rotateCommand.Execute());
    }

    //* Попытка сдвинуть корабль, у которого невозможно прочитать значение угловой скорости, приводит к ошибке.
    [Fact]
    public void CantReadAngularVelocityInRotating()
    {
        Mock<IRotatable> mock = new Mock<IRotatable>();
        mock.Setup(x => x.AngularVelocity).Throws(new Exception()).Verifiable();
        RotateCommand rotateCommand = new RotateCommand(mock.Object);

        Assert.Throws<Exception>(() => rotateCommand.Execute());
    }

    //* Попытка сдвинуть корабль, у которого невозможно изменить угол наклона к горизонту, приводит к ошибке.
    [Fact]
    public void CantChangeDirectionInRotating()
    {
        Mock<IRotatable> mock = new Mock<IRotatable>();
        mock.SetupSet(x => x.Direction = It.IsAny<int>()).Throws(new Exception()).Verifiable();
        RotateCommand rotateCommand = new RotateCommand(mock.Object);

        Assert.Throws<Exception>(() => rotateCommand.Execute());
    }
    
    [Fact]
    public void TestRotateAngle360()
    {
        Mock<IRotatable> mock = new Mock<IRotatable>();
        mock.SetupGet(x => x.Direction).Returns(350).Verifiable();
        mock.SetupGet(x => x.AngularVelocity).Returns(390).Verifiable();
        RotateCommand rotateCommand = new RotateCommand(mock.Object);

        rotateCommand.Execute();

        mock.VerifySet(x => x.Direction = 20, Times.Once);
    }
}