namespace SpaceGame.Lib.Test;
using Moq;
using Vector;

public class TestStartMovement
{
    public TestStartMovement()
    {
        var mockCommand = new Mock<ICommand>();
        mockCommand.Setup(x => x.Execute());

        var mockStrategyWithParams = new Mock<IStrategy>();
        mockStrategyWithParams.Setup(x => x.Invoke(It.IsAny<object[]>())).Returns(mockCommand.Object);

        var mockStrategyWithoutParams = new Mock<IStrategy>();
        mockStrategyWithoutParams.Setup(x => x.Invoke()).Returns(new Queue<ICommand>());

        IoC.Resolve<ICommand>("IoC.Register", "Game.UObject.SetProperty", mockStrategyWithParams.Object).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Game.Operation.Movement", mockStrategyWithParams.Object).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Game.Queue", mockStrategyWithoutParams.Object).Execute();
    }

    //* Успешное выполнение StartMovementCommand
    [Fact]
    public void StartMovementCommand()
    {
        var mockUObject = new Mock<IUObject>();

        var mockStartable = new Mock<IStartable>();
        mockStartable.Setup(x => x.Target).Returns(mockUObject.Object).Verifiable();
        mockStartable.Setup(x => x.Parameters).Returns(
            new Dictionary<string, object> { { "Velocity", new Vector(1, 1) } }
        ).Verifiable();

        var startMovementCommand = new StartMovementCommand(mockStartable.Object);

        startMovementCommand.Execute();
        mockStartable.VerifyAll();
    }

    //* Попытка получить Target выбросит ошибку
    [Fact]
    public void TryGetTargetThrowsException()
    {
        var mockUObject = new Mock<IUObject>();

        var mockStartable = new Mock<IStartable>();
        mockStartable.Setup(x => x.Target).Throws(new Exception()).Verifiable();
        mockStartable.Setup(x => x.Parameters).Returns(
            new Dictionary<string, object> { { "Velocity", new Vector(1, 1) } }
        ).Verifiable();

        var startMovementCommand = new StartMovementCommand(mockStartable.Object);

        Assert.Throws<Exception>(() => startMovementCommand.Execute());
        mockStartable.VerifyAll();
    }

    //* Попытка получить Parameters выбросит ошибку
    [Fact]
    public void TryGetParametersThrowsException()
    {
        var mockUObject = new Mock<IUObject>();

        var mockStartable = new Mock<IStartable>();
        mockStartable.Setup(x => x.Target).Returns(mockUObject.Object);
        mockStartable.Setup(x => x.Parameters).Throws(new Exception());

        var startMovementCommand = new StartMovementCommand(mockStartable.Object);

        Assert.Throws<Exception>(() => startMovementCommand.Execute());
        mockStartable.VerifyGet(x => x.Target, Times.Never());
        mockStartable.VerifyGet(x => x.Parameters, Times.Once());
    }

    //! нужен для покрытия, пока не скинули IoC
    [Fact]
    public void IoCThrowsResolveDependencyException()
    {
        var mockThrowResolveDependencyExceptionStrategy = new Mock<IStrategy>();
        mockThrowResolveDependencyExceptionStrategy.Setup(x => x.Invoke()).Throws(new ResolveDependencyException());

        IoC.Resolve<ICommand>("IoC.Register", "ResolveDependencyException", mockThrowResolveDependencyExceptionStrategy.Object).Execute();

        Assert.Throws<ResolveDependencyException>(() => IoC.Resolve<ICommand>("ResolveDependencyException"));
        Assert.Throws<ResolveDependencyException>(() => IoC.Resolve<ICommand>("NaN"));
    }
}
