namespace SpaceGame.Lib.Test;
using System.Reflection;
using Moq;
using Hwdtech;
using Hwdtech.Ioc;

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
        var gameAdapterMap = new Dictionary<KeyValuePair<Type, Type>, string> { };
        var assembly = Assembly.Load("SpaceGame.Lib.Test");
        var targetType = typeof(IMovable);
        var adapterName = "SpaceGame.Lib.Test.IMovable" + "TestAdapter";
        var mockUObject = new Mock<IUObject>();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Adapter.Map", (object[] args) => gameAdapterMap).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Adapter.Name.Create", (object[] args) => adapterName).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Assembly", (object[] args) => assembly).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Adapter.Register", (object[] args) => new GameAdapterRegisterStrategy().Invoke(args)).Execute();

        Assert.Empty(gameAdapterMap);
        Assert.Throws<ArgumentException>(() => IoC.Resolve<object>(adapterName, mockUObject.Object, targetType));

        IoC.Resolve<Lib.ICommand>("Game.Adapter.Register", mockUObject.Object, targetType).Execute();

        Assert.Single(gameAdapterMap);
        Assert.Equal(new KeyValuePair<Type, Type>(mockUObject.Object.GetType(), targetType), gameAdapterMap.First().Key);
        Assert.Equal(adapterName, gameAdapterMap.First().Value);

        var testAdapter = IoC.Resolve<object>(adapterName, mockUObject.Object);
        Assert.Equal(adapterName, testAdapter.GetType().ToString());
    }
}
