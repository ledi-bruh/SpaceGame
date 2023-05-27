namespace SpaceGame.Lib.Test;
using Moq;
using Hwdtech;
using Hwdtech.Ioc;

public class TestCreateGameAdapterStrategy
{
    public TestCreateGameAdapterStrategy()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        IoC.Resolve<ICommand>("Scopes.Current.Set",
            IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))
        ).Execute();
    }

    [Fact]
    public void SuccessfulCreatingGameAdapter()
    {
        var gameAdapterMap = new Dictionary<KeyValuePair<Type, Type>, string>();
        var mockUObject = new Mock<IUObject>();
        var targetType = typeof(object);

        var mockCommand = new Mock<Lib.ICommand>();
        mockCommand.Setup(x => x.Execute()).Verifiable();

        var pair = new KeyValuePair<Type, Type>(mockUObject.Object.GetType(), targetType);
        var adapterName = "SpaceGame.Lib.IFoo" + "Adapter";
        gameAdapterMap[pair] = adapterName;

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Adapter.Map", (object[] args) => gameAdapterMap).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Compile.Adapter", (object[] args) => mockCommand.Object).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Adapter.Register", (object[] args) => mockCommand.Object).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", adapterName, (object[] args) => (object)0).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Adapter.Create", (object[] args) => new CreateGameAdapterStrategy().Invoke(args)).Execute();

        Assert.NotEmpty(gameAdapterMap);
        mockCommand.Verify(x => x.Execute(), Times.Never());

        var adapter = IoC.Resolve<object>("Game.Adapter.Create", mockUObject.Object, targetType);

        Assert.Equal(0, adapter);
        mockCommand.Verify(x => x.Execute(), Times.Never());
    }

    [Fact]
    public void SuccessfulCreatingGameAdapterWithRegistration()
    {
        var gameAdapterMap = new Dictionary<KeyValuePair<Type, Type>, string>();
        var mockUObject = new Mock<IUObject>();
        var targetType = typeof(object);

        var mockCommand = new Mock<Lib.ICommand>();
        mockCommand.Setup(x => x.Execute()).Verifiable();

        var pair = new KeyValuePair<Type, Type>(mockUObject.Object.GetType(), targetType);
        var adapterName = "SpaceGame.Lib.IFoo" + "Adapter";

        var mockRegisterCommand = new Mock<Lib.ICommand>();
        mockRegisterCommand.Setup(x => x.Execute()).Callback(
            () => gameAdapterMap[pair] = adapterName
        ).Verifiable();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Adapter.Map", (object[] args) => gameAdapterMap).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Compile.Adapter", (object[] args) => mockCommand.Object).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Adapter.Register", (object[] args) => mockRegisterCommand.Object).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", adapterName, (object[] args) => (object)1).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Adapter.Create", (object[] args) => new CreateGameAdapterStrategy().Invoke(args)).Execute();

        Assert.Empty(gameAdapterMap);
        mockCommand.Verify(x => x.Execute(), Times.Never());
        mockRegisterCommand.Verify(x => x.Execute(), Times.Never());

        var adapter = IoC.Resolve<object>("Game.Adapter.Create", mockUObject.Object, targetType);

        Assert.Single(gameAdapterMap);
        Assert.Equal(1, adapter);
        mockCommand.Verify(x => x.Execute(), Times.Once());
        mockRegisterCommand.Verify(x => x.Execute(), Times.Once());
    }
}
