namespace SpaceGame.Lib.Test;
using Hwdtech;
using Hwdtech.Ioc;
using Moq;


public class TestRegisterGameCommands
{
    public TestRegisterGameCommands()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<ICommand>("Scopes.Current.Set",
            IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))
        ).Execute();
    }

    [Fact]
    public void SuccesfullInitializationCommandsTest()
    {

        var commandsDependencies = new Dictionary<string, IStrategy>();
        commandsDependencies.Add("StartMovement", new CreateStartMovementCommandStrategy());
        commandsDependencies.Add("EndMovement", new CreateEndMovementCommandStrategy());
        commandsDependencies.Add("Shoot", new CreateShootCommandStrategy());

        IoC.Resolve<ICommand>("IoC.Register", "Game.Dependencies.Get.Commands",(object[] args) => commandsDependencies).Execute();

        new GameRegisterCommandsCommand().Execute();

        var mockStartable = new Mock<IStartable>();
        IoC.Resolve<ICommand>("IoC.Register","Game.UObject.Adapter.Create", (object[] args) => mockStartable.Object).Execute();
        
        Mock<IUObject> mockIUObject = new Mock<IUObject>();

        var cmd = IoC.Resolve<SpaceGame.Lib.ICommand>("Game.Command.StartMovement", mockIUObject.Object);

        Assert.True(typeof(StartMovementCommand) == cmd.GetType());

        IoC.Resolve<ICommand>("Scopes.Current.Set",
            IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Current"))
        ).Execute();

        var mockEndable = new Mock<IEndable>();
        IoC.Resolve<ICommand>("IoC.Register","Game.UObject.Adapter.Create", (object[] args) => mockEndable.Object).Execute();

        cmd = IoC.Resolve<SpaceGame.Lib.ICommand>("Game.Command.EndMovement", mockIUObject.Object);

        Assert.True(typeof(EndMovementCommand) == cmd.GetType());


        IoC.Resolve<ICommand>("Scopes.Current.Set",
            IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Current"))
        ).Execute();

        var mockShootable = new Mock<IShootable>();
        IoC.Resolve<ICommand>("IoC.Register","Game.UObject.Adapter.Create", (object[] args) => mockShootable.Object).Execute();

        cmd = IoC.Resolve<SpaceGame.Lib.ICommand>("Game.Command.Shoot", mockIUObject.Object);

        Assert.True(typeof(ShootCommand) == cmd.GetType());
    }


    [Fact]
    public void ShootCommandTest()
    {
        var queue = new Queue<SpaceGame.Lib.ICommand>();

        var mockCmd = new Mock<SpaceGame.Lib.ICommand>();
        
        var mockShootable = new Mock<IShootable>();

        var mockGetId = new Mock<IStrategy>();

        var mockObject = new Mock<object>();
        mockGetId.Setup(x => x.Invoke()).Returns(1).Verifiable();

        IoC.Resolve<ICommand>("IoC.Register","Game.Queue.Push", (object[] args) => new ActionCommand( () => queue.Enqueue((SpaceGame.Lib.ICommand)args[1]))).Execute();
        IoC.Resolve<ICommand>("IoC.Register","Game.Get.GameId", (object[] args) => mockGetId.Object.Invoke(args)).Execute();
        IoC.Resolve<ICommand>("IoC.Register","Game.Create.Projectile", (object[] args) => mockObject.Object).Execute();
        IoC.Resolve<ICommand>("IoC.Register","Game.Create.Projectile.Command.Move", (object[] args) => mockCmd.Object).Execute();

        new ShootCommand(mockShootable.Object).Execute();

        Assert.True(queue.Count == 1);
        
        mockGetId.Verify(x => x.Invoke(), Times.Once);
    }
}
