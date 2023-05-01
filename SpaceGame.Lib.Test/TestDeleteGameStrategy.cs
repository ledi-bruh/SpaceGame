namespace SpaceGame.Lib.Test;
using Moq;
using Hwdtech;
using Hwdtech.Ioc;

public class TestDeleteGameStrategy
{
    public TestDeleteGameStrategy()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        IoC.Resolve<ICommand>("Scopes.Current.Set",
            IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))
        ).Execute();
    }

    [Fact]
    public void SuccessfulDeletingGame()
    {
        var gameMap = new Dictionary<string, IInjectable>();
        var gameScopeMap = new Dictionary<string, object>();

        IoC.Resolve<ICommand>("IoC.Register", "Game.Delete", (object[] args) => new DeleteGameStrategy().Invoke(args)).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Game.Map", (object[] args) => gameMap).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Game.Command.Empty", (object[] args) => new EmptyCommand()).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Game.Scope.Map", (object[] args) => gameScopeMap).Execute();

        var mockGame = new Mock<IInjectable>();
        var gameId = "gameId";
        var scope = "scope";

        Assert.Empty(gameMap);
        Assert.Empty(gameScopeMap);

        gameMap.Add(gameId, mockGame.Object);
        gameScopeMap.Add(gameId, scope);
        Assert.Single(gameMap);
        Assert.Single(gameScopeMap);

        IoC.Resolve<Lib.ICommand>("Game.Delete", gameId).Execute();
        Assert.Single(gameMap);
        Assert.Empty(gameScopeMap);
    }
}
