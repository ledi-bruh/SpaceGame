namespace SpaceGame.Lib.Test;
using System.Collections.Concurrent;
using Moq;
using Hwdtech;
using Hwdtech.Ioc;

public class TestCreateGameStrategy
{
    public TestCreateGameStrategy()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        IoC.Resolve<ICommand>("Scopes.Current.Set",
            IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))
        ).Execute();
    }

    [Fact]
    public void SuccessfulCreatingNewGame()
    {
        var mockCommand = new Mock<Lib.ICommand>();
        mockCommand.Setup(x => x.Execute()).Verifiable();
        var gameMap = new Dictionary<string, Lib.ICommand>();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Create",
            (object[] args) => new CreateGameStrategy().Invoke(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Scope.New", (object[] args) => (object)0).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Queue.New", (object[] args) => new Queue<Lib.ICommand>()).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command", (object[] args) => mockCommand.Object).Execute();


        var concurrentQueue = new BlockingCollection<Lib.ICommand>();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.Thread.Queue", (object[] args) => concurrentQueue).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command.Macro",
            (object[] args) => new MacroCommand((List<Lib.ICommand>)args[0])).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command.Inject",
            (object[] args) => new InjectCommand((Lib.ICommand)args[0])).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Command.Concurrent.Repeat",
            (object[] args) => new RepeatConcurrentCommand((Lib.ICommand)args[0])).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Map", (object[] args) => gameMap).Execute();


        Assert.Empty(gameMap);

        var gameId = "gameId";
        var game = IoC.Resolve<Lib.ICommand>(
            "Game.Create",
            gameId,
            IoC.Resolve<object>("Scopes.Current"),
            400d
        );

        Assert.Single(gameMap);
        Assert.Equal(typeof(InjectCommand), gameMap[gameId].GetType());
        Assert.Equal(typeof(InjectCommand), game.GetType());
        Assert.Equal(game, gameMap[gameId]);

        Assert.Empty(concurrentQueue);
        concurrentQueue.Add(game);
        Assert.Single(concurrentQueue);

        concurrentQueue.Take().Execute();
        concurrentQueue.Take().Execute();
        concurrentQueue.Take().Execute();
        mockCommand.Verify(x => x.Execute(), Times.Exactly(3));

        Assert.Single(concurrentQueue);
        concurrentQueue.Take();
        Assert.Empty(concurrentQueue);
    }
}
