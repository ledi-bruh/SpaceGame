namespace SpaceGame.Lib.Test;
using Moq;
using Hwdtech;
using Hwdtech.Ioc;

public class TestEndMovement
{
    public TestEndMovement()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        IoC.Resolve<ICommand>("Scopes.Current.Set",
            IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))
        ).Execute();

        var mockCommand = new Mock<SpaceGame.Lib.ICommand>();
        mockCommand.Setup(x => x.Execute());

        var mockInjectable = new Mock<IInjectable>();
        mockInjectable.Setup(x => x.Inject(It.IsAny<object>()));

        IoC.Resolve<ICommand>("IoC.Register", "Game.UObject.DeleteProperty", (object[] args) => mockCommand.Object).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Game.UObject.GetProperty", (object[] args) => mockInjectable.Object).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Game.Command.EmptyCommand", (object[] args) => new EmptyCommand()).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Game.Command.EndMovement", (object[] args) => new EndMovementCommand((IEndable)args[0])).Execute();
    }

    [Fact]
    public void EndMovementCommandCompleted()
    {
        var mockUObject = new Mock<IUObject>();

        var mockEndable = new Mock<IEndable>();
        mockEndable.Setup(x => x.Target).Returns(mockUObject.Object).Verifiable();
        mockEndable.Setup(x => x.Keys).Returns(new List<string> { "Velocity" }).Verifiable();

        var endMovementCommand = IoC.Resolve<SpaceGame.Lib.ICommand>("Game.Command.EndMovement", mockEndable.Object);

        endMovementCommand.Execute();
        mockEndable.VerifyAll();
    }

    [Fact]
    public void InjectCommandTest()
    {
        var mockCommandFirst = new Mock<SpaceGame.Lib.ICommand>();
        mockCommandFirst.Setup(x => x.Execute()).Verifiable();

        var mockCommandSecond = new Mock<SpaceGame.Lib.ICommand>();
        mockCommandSecond.Setup(x => x.Execute()).Verifiable();

        var InjectCommand = new InjectCommand(mockCommandFirst.Object);

        InjectCommand.Execute();

        mockCommandFirst.Verify(x => x.Execute(), Times.Once);
        mockCommandSecond.Verify(x => x.Execute(), Times.Never);

        InjectCommand.Inject(mockCommandSecond.Object);

        InjectCommand.Execute();
        mockCommandFirst.Verify(x => x.Execute(), Times.Once);
        mockCommandSecond.Verify(x => x.Execute(), Times.Once);
    }

    [Fact]
    public void EmptyCommandTest()
    {
        var EmptyCommand = IoC.Resolve<SpaceGame.Lib.ICommand>("Game.Command.EmptyCommand");

        EmptyCommand.Execute();

        Assert.NotNull(EmptyCommand);
    }

    [Fact]
    public void TryGetTargetThrowsException()
    {
        var mockUObject = new Mock<IUObject>();

        var mockEndable = new Mock<IEndable>();
        mockEndable.Setup(x => x.Target).Throws(new Exception()).Verifiable();
        mockEndable.Setup(x => x.Keys).Returns(new List<string> { "Velocity" }).Verifiable();

        var endMovementCommand = IoC.Resolve<SpaceGame.Lib.ICommand>("Game.Command.EndMovement", mockEndable.Object);

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

        var endMovementCommand = IoC.Resolve<SpaceGame.Lib.ICommand>("Game.Command.EndMovement", mockEndable.Object);

        Assert.Throws<Exception>(() => endMovementCommand.Execute());
        mockEndable.VerifyGet(x => x.Target, Times.Never());
        mockEndable.VerifyGet(x => x.Keys, Times.Once());
    }
}
