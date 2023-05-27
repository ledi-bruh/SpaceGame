namespace SpaceGame.Lib.Test;
using Microsoft.CodeAnalysis;
using Hwdtech;
using Hwdtech.Ioc;

public class TestCompiling
{
    public TestCompiling()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        IoC.Resolve<ICommand>("Scopes.Current.Set",
            IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))
        ).Execute();
    }

    [Fact]
    public void SuccessfulCompiling()
    {
        var codeAdapter = @"";  // ! TODO: Добавить код адаптера
        var references = new MetadataReference[] { MetadataReference.CreateFromFile(typeof(object).Assembly.Location) };

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Adapter.Code", (object[] args) => codeAdapter).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Compile.References", (object[] args) => references).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Compile", (object[] args) => new CompileStrategy().Invoke(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Compile.Adapter", (object[] args) => new CompileAdapterStrategy().Invoke(args)).Execute();

        try
        {
            IoC.Resolve<Lib.ICommand>("Compile.Adapter", typeof(Type)).Execute();
        }
        catch (Exception e) { Assert.Fail(e.Message); }
    }
}
