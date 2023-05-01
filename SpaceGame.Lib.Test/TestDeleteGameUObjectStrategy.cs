namespace SpaceGame.Lib.Test;
using Moq;
using Hwdtech;
using Hwdtech.Ioc;

public class TestDeleteGameUObjectStrategy
{
    public TestDeleteGameUObjectStrategy()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        IoC.Resolve<ICommand>("Scopes.Current.Set",
            IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))
        ).Execute();
    }

    [Fact]
    public void SuccessfulDeletingGameUObject()
    {
        var gameUObjectMap = new Dictionary<int, IUObject>();
        IoC.Resolve<ICommand>("IoC.Register", "Game.Delete.UObject", (object[] args) => new DeleteGameUObjectStrategy().Invoke(args)).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Game.UObject.Map", (object[] args) => gameUObjectMap).Execute();

        var mockUObject = new Mock<IUObject>();

        Assert.Empty(gameUObjectMap);
        gameUObjectMap.Add(0, mockUObject.Object);
        Assert.Single(gameUObjectMap);
        IoC.Resolve<Lib.ICommand>("Game.Delete.UObject", 0).Execute();
        Assert.Empty(gameUObjectMap);
    }
}
