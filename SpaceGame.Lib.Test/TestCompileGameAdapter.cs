namespace SpaceGame.Lib.Test;
using System.Reflection;
using Hwdtech;
using Hwdtech.Ioc;

public class TestCompileGameAdapter
{
    public TestCompileGameAdapter()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        IoC.Resolve<ICommand>("Scopes.Current.Set",
            IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))
        ).Execute();
    }

    [Fact]
    public void SuccessfulAddingAssemblyAfterGameAdapterCompilation()
    {
        var assembly = Assembly.Load("SpaceGame.Lib.Test");
        var assemblyMap = new Dictionary<KeyValuePair<Type, Type>, Assembly>();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Adapter.Code", (object[] args) => ";").Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Compile", (object[] args) => assembly).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Adapter.Assembly.Map", (object[] args) => assemblyMap).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Adapter.Compile", (object[] args) => new CompileGameAdapterStrategy().Invoke(args)).Execute();

        Assert.Empty(assemblyMap);

        IoC.Resolve<Lib.ICommand>("Game.Adapter.Compile", typeof(Type), typeof(Type)).Execute();

        Assert.Single(assemblyMap);
        Assert.Equal(new KeyValuePair<Type, Type>(typeof(Type), typeof(Type)), assemblyMap.First().Key);
        Assert.Equal(assembly, assemblyMap.First().Value);
    }
}
