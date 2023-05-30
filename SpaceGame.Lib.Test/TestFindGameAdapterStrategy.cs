namespace SpaceGame.Lib.Test;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Moq;
using Hwdtech;
using Hwdtech.Ioc;

public class TestFindGameAdapterStrategy
{
    public TestFindGameAdapterStrategy()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        IoC.Resolve<ICommand>("Scopes.Current.Set",
            IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))
        ).Execute();

        var references = new List<MetadataReference> {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            MetadataReference.CreateFromFile(Assembly.Load("SpaceGame.Lib").Location)
        };

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Assembly.Name.Create", (object[] args) => Guid.NewGuid().ToString()).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Compile.References", (object[] args) => references).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Compile", (object[] args) => new CompileStrategy().Invoke(args)).Execute();
    }

    [Fact]
    public void SuccessfulFindingGameAdapter()
    {
        var mockUObject = new Mock<IUObject>();
        var targetType = typeof(Type);
        var adapterAssemblyMap = new Dictionary<KeyValuePair<Type, Type>, Assembly>();
        var adapterCode = @"namespace SpaceGame.Lib;
public class IMovableTestAdapter
{
    public IMovableTestAdapter(IUObject uObject) { }
}
";

        var assembly = IoC.Resolve<Assembly>("Compile", adapterCode);
        adapterAssemblyMap[new KeyValuePair<Type, Type>(mockUObject.Object.GetType(), targetType)] = assembly;

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Adapter.Assembly.Map", (object[] args) => adapterAssemblyMap).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Adapter.Name.Create", (object[] args) => "SpaceGame.Lib.IMovable" + "TestAdapter").Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Adapter.Find", (object[] args) => new FindGameAdapterStrategy().Invoke(args)).Execute();

        object adapter = IoC.Resolve<object>("Game.Adapter.Find", mockUObject.Object, targetType);

        Assert.NotNull(adapter);
        Assert.Equal("SpaceGame.Lib.IMovableTestAdapter", adapter.GetType().ToString());
    }
}
