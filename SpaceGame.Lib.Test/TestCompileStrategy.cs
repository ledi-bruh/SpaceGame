namespace SpaceGame.Lib.Test;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Hwdtech;
using Hwdtech.Ioc;

public class TestCompileStrategy
{
    public TestCompileStrategy()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        IoC.Resolve<ICommand>("Scopes.Current.Set",
            IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))
        ).Execute();
    }

    [Fact]
    public void SuccessfulCompiling()
    {
        Assembly? assembly = null;
        var references = new List<MetadataReference> {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            MetadataReference.CreateFromFile(Assembly.Load("SpaceGame.Lib").Location)
        };
        var adapterCode = @"namespace SpaceGame.Lib;
public class TestFoo {
    public TestFoo() {}
}
";

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Assembly.Name.Create", (object[] args) => Guid.NewGuid().ToString()).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Compile.References", (object[] args) => references).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Compile", (object[] args) => new CompileStrategy().Invoke(args)).Execute();

        try
        {
            assembly = IoC.Resolve<Assembly>("Compile", adapterCode);
        }
        catch (Exception e) { Assert.Fail(e.Message); }
        
        var foo = Activator.CreateInstance(assembly.GetType("SpaceGame.Lib.TestFoo")!)!;

        Assert.Equal("SpaceGame.Lib.TestFoo", foo.GetType().ToString());
    }
}
