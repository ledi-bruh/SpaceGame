namespace SpaceGame.Lib.Test;
using Moq;
using Hwdtech;
using Hwdtech.Ioc;

public class TestSetDefaultHandlerCommand
{
    public TestSetDefaultHandlerCommand()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        IoC.Resolve<ICommand>("Scopes.Current.Set",
            IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))
        ).Execute();
    }

    [Fact]
    public void SuccessfulSettingDefaultHandler()
    {
        var tree = new Dictionary<int, IHandler>();
        var mockHandler = new Mock<IHandler>();
        IHandler? checkHandler;

        IoC.Resolve<ICommand>("IoC.Register", "Exception.Handler.Tree", (object[] args) => tree).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Exception.Handler.Default.Set", (object[] args) => new SetDefaultHandlerCommand((IHandler)args[0])).Execute();

        Assert.True(tree.Count == 0);

        IoC.Resolve<SpaceGame.Lib.ICommand>("Exception.Handler.Default.Set", mockHandler.Object).Execute();

        Assert.True(tree.Count == 1);
        Assert.True(tree.TryGetValue(0, out checkHandler));
        Assert.Equal(mockHandler.Object, checkHandler);
    }
}
