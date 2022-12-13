namespace SpaceGame.Lib.Test;
using Moq;
using Hwdtech;
using Hwdtech.Ioc;

public class TestCheckCollisionCommand
{
    public TestCheckCollisionCommand()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        IoC.Resolve<ICommand>("Scopes.Current.Set",
            IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))
        ).Execute();
    }

    [Fact]
    public void CheckCollisionCommandCompleted()
    {
        var mockCommand = new Mock<SpaceGame.Lib.ICommand>();
        mockCommand.Setup(x => x.Execute()).Verifiable();

        var mockDictionary = new Mock<IDictionary<int, object>>();
        mockDictionary.SetupGet(x => x[It.IsAny<int>()]).Returns(mockDictionary.Object);
        mockDictionary.SetupGet(x => x.Keys).Returns(new List<int>{ 1 });

        IoC.Resolve<ICommand>("IoC.Register", "Game.UObject.GetProperty", (object[] args) => new List<int> {1, 1}).Execute();
        IoC.Resolve<ICommand>(
            "IoC.Register",
            "Game.Command.CheckCollision", 
            (object[] args) => new CheckCollisionCommand((IUObject)args[0], (IUObject)args[1])
        ).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Game.CollisionTree", (object[] args) => mockDictionary.Object).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Game.Event.Collision", (object[] args) => mockCommand.Object).Execute(); 

        var mockUObject = new Mock<IUObject>();

        var checkCollisionCommand = IoC.Resolve<SpaceGame.Lib.ICommand>("Game.Command.CheckCollision", mockUObject.Object, mockUObject.Object);

        checkCollisionCommand.Execute();

        mockCommand.VerifyAll();
    }

    [Fact]
    public void TryGetNewTreeThrowsException()
    {
        var mockCommand = new Mock<SpaceGame.Lib.ICommand>();
        mockCommand.Setup(x => x.Execute());

        var mockDictionary = new Mock<IDictionary<int, object>>();
        mockDictionary.SetupGet(x => x[It.IsAny<int>()]).Throws(new Exception()).Verifiable(); 

        IoC.Resolve<ICommand>("IoC.Register", "Game.UObject.GetProperty", (object[] args) => new List<int> {1, 1}).Execute();
        IoC.Resolve<ICommand>(
            "IoC.Register",
            "Game.Command.CheckCollision", 
            (object[] args) => new CheckCollisionCommand((IUObject)args[0], (IUObject)args[1])
        ).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Game.CollisionTree", (object[] args) => mockDictionary.Object).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Game.Event.Collision", (object[] args) => mockCommand.Object).Execute(); 

        var mockUObject = new Mock<IUObject>();

        var checkCollisionCommand = IoC.Resolve<SpaceGame.Lib.ICommand>("Game.Command.CheckCollision", mockUObject.Object, mockUObject.Object);

        Assert.Throws<Exception>(() => checkCollisionCommand.Execute());
        mockDictionary.VerifyAll();
    }
}
