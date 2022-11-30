namespace SpaceGame.Lib.Test;
using Moq;

public class TestEndMovement
{
    public TestEndMovement()
    {
        var mockCommand = new Mock<ICommand>();
        mockCommand.Setup(x => x.Execute());

        var mockStrategyWithParams = new Mock<IStrategy>();
        mockStrategyWithParams.Setup(x => x.Invoke(It.IsAny<object[]>())).Returns(mockCommand.Object);

        var mockStrategyEmptyCommand = new Mock<IStrategy>();
        mockStrategyEmptyCommand.Setup(x => x.Invoke()).Returns(mockCommand.Object);

        IoC.Resolve<ICommand>("IoC.Register", "Game.UObject.DeleteProperty", mockStrategyWithParams.Object).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Game.UObject.SetProperty", mockStrategyWithParams.Object).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Game.Command.Empty", mockStrategyEmptyCommand.Object).Execute();
    }

    [Fact]
    public void EndMovementCommandCompleted()
    {
        var mockUObject = new Mock<IUObject>();
        
        var mockEndable = new Mock<IEndable>();
        mockEndable.Setup(x => x.Target).Returns(mockUObject.Object).Verifiable();
        mockEndable.Setup(x => x.Keys).Returns(
            new List<string> { "Velocity" }
        ).Verifiable();

        var endMovementCommand = new EndMovementCommand(mockEndable.Object);

        endMovementCommand.Execute();
        mockEndable.VerifyAll();
    }

    [Fact]
    public void TryGetTargetThrowsException()
    {
        var mockUObject = new Mock<IUObject>();

        var mockEndable = new Mock<IEndable>();
        mockEndable.Setup(x => x.Target).Throws(new Exception()).Verifiable();
        mockEndable.Setup(x => x.Keys).Returns(
            new List<string> { "Velocity" }
        ).Verifiable();

        var endMovementCommand = new EndMovementCommand(mockEndable.Object);

        Assert.Throws<Exception>(() => endMovementCommand.Execute());
        mockEndable.VerifyAll();
    }

    [Fact]
    public void TryGetKeysThrowsException()
    {
        var mockUObject = new Mock<IUObject>();

        var mockEndable = new Mock<IEndable>();
        mockEndable.Setup(x => x.Target).Returns(mockUObject.Object);
        mockEndable.Setup(x => x.Keys).Throws(new Exception());

        var startMovementCommand = new EndMovementCommand(mockEndable.Object);

        Assert.Throws<Exception>(() => startMovementCommand.Execute());
        mockEndable.VerifyGet(x => x.Target, Times.Never());
        mockEndable.VerifyGet(x => x.Keys, Times.Once());
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