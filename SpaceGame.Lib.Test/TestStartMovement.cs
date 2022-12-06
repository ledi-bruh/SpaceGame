namespace SpaceGame.Lib.Test;
using Moq;
using Vector;
using Hwdtech;
using Hwdtech.Ioc;

public class TestStartMovement
{
    public TestStartMovement()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        IoC.Resolve<ICommand>("Scopes.Current.Set",
            IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))
        ).Execute();

        var mockCommand = new Mock<SpaceGame.Lib.ICommand>();
        mockCommand.Setup(x => x.Execute());

        IoC.Resolve<ICommand>("IoC.Register", "Game.UObject.SetProperty", (object[] args) => mockCommand.Object).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Game.Operation.Movement", (object[] args) => mockCommand.Object).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Game.Queue", (object[] args) => new Queue<SpaceGame.Lib.ICommand>()).Execute();
    }

    [Fact]
    public void SuccesfullStartMovementCommand()
    {
        var mockUObject = new Mock<IUObject>();

        var mockStartable = new Mock<IStartable>();
        mockStartable.SetupGet(x => x.Target).Returns(mockUObject.Object).Verifiable();
        mockStartable.SetupGet(x => x.Parameters).Returns(
            new Dictionary<string, object> { { "Velocity", new Vector(1, 1) } }
        ).Verifiable();

        var startMovementCommand = new StartMovementCommand(mockStartable.Object);

        startMovementCommand.Execute();
        mockStartable.VerifyAll();
    }

    [Fact]
    public void TryGetTargetThrowsException()
    {
        var mockStartable = new Mock<IStartable>();
        mockStartable.SetupGet(x => x.Target).Throws(new Exception()).Verifiable();
        mockStartable.SetupGet(x => x.Parameters).Returns(
            new Dictionary<string, object> { { "Velocity", new Vector(1, 1) } }
        ).Verifiable();

        var startMovementCommand = new StartMovementCommand(mockStartable.Object);

        Assert.Throws<Exception>(() => startMovementCommand.Execute());
        mockStartable.VerifyAll();
    }

    [Fact]
    public void TryGetParametersThrowsException()
    {
        var mockUObject = new Mock<IUObject>();

        var mockStartable = new Mock<IStartable>();
        mockStartable.SetupGet(x => x.Target).Returns(mockUObject.Object);
        mockStartable.SetupGet(x => x.Parameters).Throws(new Exception());

        var startMovementCommand = new StartMovementCommand(mockStartable.Object);

        Assert.Throws<Exception>(() => startMovementCommand.Execute());
        mockStartable.VerifyGet(x => x.Target, Times.Never());
        mockStartable.VerifyGet(x => x.Parameters, Times.Once());
    }
}
