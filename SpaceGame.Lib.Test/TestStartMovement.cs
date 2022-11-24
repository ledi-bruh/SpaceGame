namespace SpaceGame.Lib.Test;
using Moq;
using Vector;

public class TestStartMovement
{
    [Fact]
    public void TestStartMovementCommand()
    {
        var mockUObject = new Mock<IUObject>();

        var mockStartable = new Mock<IStartable>();
        mockStartable.Setup(x => x.Target).Returns(mockUObject.Object).Verifiable();
        mockStartable
        .Setup(x => x.Parameters)
        .Returns(new Dictionary<string, object> { { "Velocity", new Vector(1, 1) } })
        .Verifiable();

        var mockCommand = new Mock<ICommand>();
        mockCommand.Setup(x => x.Execute());

        var mockStrategyWithParams = new Mock<IStrategy>();
        mockStrategyWithParams.Setup(x => x.Invoke(It.IsAny<object[]>())).Returns(mockCommand.Object);
        IoC.Resolve<ICommand>("IoC.Register", "Game.UObject.SetProperty", mockStrategyWithParams.Object).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Game.Operation.Movement", mockStrategyWithParams.Object).Execute();

        var mockQueue = new Mock<Queue<ICommand>>();
        var mockQueueStrategy = new Mock<IStrategy>();
        mockQueueStrategy.Setup(x => x.Invoke()).Returns(mockQueue.Object);
        IoC.Resolve<ICommand>("IoC.Register", "Game.Queue", mockQueueStrategy.Object).Execute();

        var startMovementCommand = new StartMovementCommand(mockStartable.Object);
        startMovementCommand.Execute();

        mockStartable.VerifyAll();
    }

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
