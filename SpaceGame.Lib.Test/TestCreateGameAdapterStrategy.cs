namespace SpaceGame.Lib.Test;
using System.Reflection;
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
        var mockUObject = new Mock<IUObject>();
        var targetType = typeof(Type);
        var assembly = Assembly.Load("SpaceGame.Lib.Test");

        var adapterAssemblyMap = new Dictionary<KeyValuePair<Type, Type>, Assembly>();
        adapterAssemblyMap[new KeyValuePair<Type, Type>(mockUObject.Object.GetType(), targetType)] = assembly;

        var mockCommand = new Mock<Lib.ICommand>();
        mockCommand.Setup(x => x.Execute()).Verifiable();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Adapter.Assembly.Map", (object[] args) => adapterAssemblyMap).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Adapter.Compile", (object[] args) => mockCommand.Object).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Adapter.Find", (object[] args) => (object)0).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Adapter.Create", (object[] args) => new CreateGameAdapterStrategy().Invoke(args)).Execute();

        Assert.NotEmpty(adapterAssemblyMap);
        mockCommand.Verify(x => x.Execute(), Times.Never());

        var adapter = IoC.Resolve<object>("Game.Adapter.Create", mockUObject.Object, targetType);

        Assert.Equal(0, adapter);
        mockCommand.Verify(x => x.Execute(), Times.Never());
    }

    [Fact]
    public void SuccessfulCreatingGameAdapterWithCompilation()
    {
        var mockUObject = new Mock<IUObject>();
        var targetType = typeof(Type);
        var assembly = Assembly.Load("SpaceGame.Lib.Test");

        var adapterAssemblyMap = new Dictionary<KeyValuePair<Type, Type>, Assembly>();

        var mockCommand = new Mock<Lib.ICommand>();
        mockCommand.Setup(x => x.Execute()).Callback(
            () => adapterAssemblyMap[new KeyValuePair<Type, Type>(mockUObject.Object.GetType(), targetType)] = assembly
        ).Verifiable();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Adapter.Assembly.Map", (object[] args) => adapterAssemblyMap).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Adapter.Compile", (object[] args) => mockCommand.Object).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Adapter.Find", (object[] args) => (object)1).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Adapter.Create", (object[] args) => new CreateGameAdapterStrategy().Invoke(args)).Execute();

        Assert.Empty(adapterAssemblyMap);
        mockCommand.Verify(x => x.Execute(), Times.Never());

        var adapter = IoC.Resolve<object>("Game.Adapter.Create", mockUObject.Object, targetType);

        Assert.Single(adapterAssemblyMap);
        Assert.Equal(1, adapter);
        mockCommand.Verify(x => x.Execute(), Times.Once());
    }
}
