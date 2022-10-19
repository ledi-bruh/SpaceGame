namespace SpaceGame.Lib.Test;
using Moq;
using Angle;

public class TestRotate
{
    //* Корабль, который находится под углом к горизонту в 45 градусов имеет угловую скорость 90 градусов.
    //* В результате поворота корабль оказывается под углом 135 градусов к горизонту.
    [Fact]
    public void TestRotating()
    {
        Mock<IRotatable> mock = new Mock<IRotatable>();
        mock.SetupGet(x => x.Direction).Returns(new Angle(45)).Verifiable();
        mock.SetupGet(x => x.AngularVelocity).Returns(new Angle(90)).Verifiable();
        RotateCommand rotateCommand = new RotateCommand(mock.Object);

        rotateCommand.Execute();

        mock.VerifySet(x => x.Direction = new Angle(135), Times.Once);
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
        mock.SetupGet(x => x.Direction).Returns(new Angle(0)).Verifiable();
        mock.SetupGet(x => x.AngularVelocity).Returns(new Angle(0)).Verifiable();
        mock.SetupSet(x => x.Direction = It.IsAny<Angle>()).Throws(new Exception()).Verifiable();
        RotateCommand rotateCommand = new RotateCommand(mock.Object);

        Assert.Throws<Exception>(() => rotateCommand.Execute());
    }
    
    [Fact]
    public void TestRotateAngle360()
    {
        Mock<IRotatable> mock = new Mock<IRotatable>();
        mock.SetupGet(x => x.Direction).Returns(new Angle(350)).Verifiable();
        mock.SetupGet(x => x.AngularVelocity).Returns(new Angle(390)).Verifiable();
        RotateCommand rotateCommand = new RotateCommand(mock.Object);

        rotateCommand.Execute();

        mock.VerifySet(x => x.Direction = new Angle(20), Times.Once);
    }
}