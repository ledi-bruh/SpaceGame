namespace SpaceGame.Lib.Test;
using Moq;
using Hwdtech;
using Hwdtech.Ioc;

internal class CommandTest : ICommand
{
    public void Execute() { }
}

public class TestRegisterHandlerCommand
{
    public TestRegisterHandlerCommand()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        IoC.Resolve<ICommand>("Scopes.Current.Set",
            IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))
        ).Execute();
    }

    [Fact]
    public void SuccesfullRegisterHandlerCommand()
    {
        var mockHandler = new Mock<IHandler>();

        var mockExceptionLevelDict = new Mock<IDictionary<object, IHandler>>();
        mockExceptionLevelDict.Setup(x => x.Add(typeof(ArgumentException), It.IsAny<IHandler>())).Verifiable();

        var mockCommandLevelDict = new Mock<IDictionary<object, IDictionary<object, IHandler>>>();
        mockCommandLevelDict.Setup(x => x.Add(typeof(CommandTest), It.IsAny<IDictionary<object, IHandler>>())).Verifiable();
        mockCommandLevelDict.Setup(x => x[typeof(CommandTest)]).Returns(mockExceptionLevelDict.Object).Verifiable();

        IoC.Resolve<ICommand>("IoC.Register", "Exception.Handle.Register",
            (object[] args) => new RegisterHandlerCommand(args[0], args[1], (IHandler)args[2])
        ).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Exception.Handle.Tree", (object[] args) => mockCommandLevelDict.Object).Execute();

        IoC.Resolve<SpaceGame.Lib.ICommand>("Exception.Handle.Register", typeof(CommandTest), typeof(ArgumentException), mockHandler.Object).Execute();
        
        mockExceptionLevelDict.VerifyAll();
        mockCommandLevelDict.VerifyAll();
    }

    [Fact]
    public void SuccesfullRegisterSomeExceptionHandlers()
    {
        var mockHandler = new Mock<IHandler>();
        var dict = new Dictionary<object, IDictionary<object, IHandler>>();

        IoC.Resolve<ICommand>("IoC.Register", "Exception.Handle.Register",
            (object[] args) => new RegisterHandlerCommand(args[0], args[1], (IHandler)args[2])
        ).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Exception.Handle.Tree", (object[] args) => dict).Execute();

        IoC.Resolve<SpaceGame.Lib.ICommand>("Exception.Handle.Register", typeof(CommandTest), typeof(ArgumentException), mockHandler.Object).Execute();
        IoC.Resolve<SpaceGame.Lib.ICommand>("Exception.Handle.Register", typeof(CommandTest), typeof(ArithmeticException), mockHandler.Object).Execute();
        IoC.Resolve<SpaceGame.Lib.ICommand>("Exception.Handle.Register", typeof(CommandTest), "Exception.Handle.Exception.Default", mockHandler.Object).Execute();
        IoC.Resolve<SpaceGame.Lib.ICommand>("Exception.Handle.Register", "Exception.Handle.Command.Default", typeof(ArgumentException), mockHandler.Object).Execute();
        IoC.Resolve<SpaceGame.Lib.ICommand>("Exception.Handle.Register", "Exception.Handle.Command.Default", typeof(FormatException), mockHandler.Object).Execute();
        var tree = IoC.Resolve<IDictionary<object, IDictionary<object, IHandler>>>("Exception.Handle.Tree");

        Assert.Equal(2, tree.Count());
        Assert.Equal(3, tree[typeof(CommandTest)].Count());
        Assert.Equal(2, tree["Exception.Handle.Command.Default"].Count());
    }
}
