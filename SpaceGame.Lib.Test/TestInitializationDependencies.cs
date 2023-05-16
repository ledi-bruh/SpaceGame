namespace SpaceGame.Lib.Test;
using Hwdtech;
using Hwdtech.Ioc;
using Moq;


public class TestInitializationDependencies
{
    public TestInitializationDependencies()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<ICommand>("Scopes.Current.Set",
            IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))
        ).Execute();
    }

    [Fact]
    public void SuccessfullInitializationTest()
    {
        IoC.Resolve<ICommand>("IoC.Register", "Server.Thread.Game.Register.Dependencies", (object[] args) => new GameDependenciesRegisterStrategy().Invoke(args)).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Server.Thread.Game.Dependencies.Initialization", (object[] args) => new InitializationGameDependenciesStrategy().Invoke(args)).Execute();

        var gameQueue = new Queue<SpaceGame.Lib.ICommand>();

        IoC.Resolve<ICommand>("IoC.Register", "Game.Queue.Push", (object[] args) => new ActionCommand(() => { gameQueue.Enqueue((SpaceGame.Lib.ICommand)args[1]); })).Execute();

        IoC.Resolve<ICommand>("IoC.Register", "Game.Register.Commands", (object[] args) => new GameRegisterCommandsStrategy().Invoke(args)).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Game.Macro.Create.FromDependencies", (object[] args) => new CreateGameMacroFromDependenciesStrategy().Invoke(args)).Execute();

        var commandsDependencies = new Dictionary<string, IStrategy>();

        commandsDependencies.Add("Rotate", new CreateRotateCommandStrategy());

        IoC.Resolve<ICommand>("IoC.Register", "Game.Dependencies.Get.Commands",(object[] args) => commandsDependencies).Execute();

        IoC.Resolve<SpaceGame.Lib.ICommand>("Server.Thread.Game.Register.Dependencies", 1).Execute();

        Assert.True(gameQueue.Count == 1);

        gameQueue.Dequeue().Execute();

        Mock<IRotatable> mockRotatable = new Mock<IRotatable>();
        IoC.Resolve<ICommand>("IoC.Register","Game.UObject.Adapter.Create", (object[] args) => mockRotatable.Object).Execute();
        
        Mock<IUObject> mockIUObject = new Mock<IUObject>();

        var cmd = IoC.Resolve<SpaceGame.Lib.ICommand>("Game.Command.Rotate", mockIUObject.Object);

        Assert.True(typeof(RotateCommand) == cmd.GetType());
    }

    [Fact]
    public void ExceptionInitializationTest()
    {
        IoC.Resolve<ICommand>("IoC.Register", "Server.Thread.Game.Register.Dependencies", (object[] args) => new GameDependenciesRegisterStrategy().Invoke(args)).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Server.Thread.Game.Dependencies.Initialization", (object[] args) => new ActionCommand(() => {throw new Exception();})).Execute();

        var gameQueue = new Queue<SpaceGame.Lib.ICommand>();

        IoC.Resolve<ICommand>("IoC.Register", "Game.Queue.Push", (object[] args) => new ActionCommand(() => { gameQueue.Enqueue((SpaceGame.Lib.ICommand)args[1]); })).Execute();

        IoC.Resolve<ICommand>("IoC.Register", "Game.Register.Commands", (object[] args) => new GameRegisterCommandsStrategy().Invoke(args)).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Game.Macro.Create.FromDependencies", (object[] args) => new CreateGameMacroFromDependenciesStrategy().Invoke(args)).Execute();

        var commandsDependencies = new Dictionary<string, IStrategy>();

        commandsDependencies.Add("Rotate", new CreateRotateCommandStrategy());

        IoC.Resolve<ICommand>("IoC.Register", "Game.Dependencies.Get.Commands",(object[] args) => commandsDependencies).Execute();

        IoC.Resolve<SpaceGame.Lib.ICommand>("Server.Thread.Game.Register.Dependencies", 1).Execute();

        Assert.True(gameQueue.Count == 1);

        Assert.Throws<Exception>(() => {gameQueue.Dequeue().Execute();});
    }
}
