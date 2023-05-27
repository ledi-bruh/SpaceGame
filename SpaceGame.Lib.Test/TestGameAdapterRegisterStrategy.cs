namespace SpaceGame.Lib.Test;
using Moq;
using Hwdtech;
using Hwdtech.Ioc;

public class UObject : IUObject
    {
        object IUObject.GetProperty(string name)
        {
            throw new NotImplementedException();
        }

        void IUObject.SetProperty(string name, object value)
        {
            throw new NotImplementedException();
        }
    }

public class TestGameAdapterRegisterStrategy
{
    public TestGameAdapterRegisterStrategy()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        IoC.Resolve<ICommand>("Scopes.Current.Set",
            IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))
        ).Execute();
    }

    [Fact]
    public void SuccessfulRegisterAdapter()
    {
        var gameAdapterMap = new Dictionary<KeyValuePair<Type, Type>, string> {};
        var mockUObject = new Mock<IUObject>();
        var targetType = typeof(IMovable);
        var adapterName = "SpaceGame.Lib.IMovableAdapter";

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Adapter.Map", (object[] args) => gameAdapterMap).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Adapter.Register", (object[] args) => new GameAdapterRegisterStrategy().Invoke(args)).Execute();

        Assert.Empty(gameAdapterMap);
        Assert.Throws<ArgumentException>(() => IoC.Resolve<IMovable>(adapterName, mockUObject.Object, targetType));

        IoC.Resolve<Lib.ICommand>("Game.Adapter.Register", mockUObject.Object, targetType).Execute();

        Assert.Single(gameAdapterMap);
        Assert.Equal(new KeyValuePair<Type, Type> (mockUObject.Object.GetType(), targetType ), gameAdapterMap.First().Key);
        Assert.Equal(adapterName, gameAdapterMap.First().Value);

        var movable = IoC.Resolve<IMovable>(adapterName, mockUObject.Object);
        Assert.Equal(adapterName, movable.GetType().ToString());
    }
}
