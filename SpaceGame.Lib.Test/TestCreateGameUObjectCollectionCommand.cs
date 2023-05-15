namespace SpaceGame.Lib.Test;
using Moq;
using Hwdtech;
using Hwdtech.Ioc;

public class TestCreateGameUObjectCollectionCommand
{
    public TestCreateGameUObjectCollectionCommand()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        IoC.Resolve<ICommand>("Scopes.Current.Set",
            IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))
        ).Execute();
    }

    [Fact]
    public void SuccessfulCreatingGameUObjects()
    {
        var gameUObjectMap = new Dictionary<int, IUObject>();

        IoC.Resolve<ICommand>("IoC.Register", "Game.UObject.Map", (object[] args) => gameUObjectMap).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Game.UObject.Create", (object[] args) => new Mock<IUObject>().Object).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Game.UObject.Collection.Create",
            (object[] args) => new CreateGameUObjectCollectionCommand((int)args[0])
        ).Execute();

        Assert.Empty(gameUObjectMap);
        IoC.Resolve<Lib.ICommand>("Game.UObject.Collection.Create", 10).Execute();
        Assert.Equal(gameUObjectMap.Count, 10);
    }
}
