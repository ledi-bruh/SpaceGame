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

        IoC.Resolve<ICommand>("IoC.Register", "GetHashCode", (object[] args) => new GetHashCodeStrategy().Invoke(args)).Execute();
    }

    [Fact]
    public void SuccesfullRegisterHandlerCommand()
    {
        var mockHandler = new Mock<IHandler>();

        var mockExceptionHandleTree = new Mock<IDictionary<int, IHandler>>();
        mockExceptionHandleTree.Setup(x => x.Add(It.IsAny<int>(), It.IsAny<IHandler>())).Verifiable();

        IoC.Resolve<ICommand>("IoC.Register", "Exception.Handler.Register",
            (object[] args) => new RegisterHandlerCommand((IEnumerable<Type>)args[0], (IHandler)args[1])
        ).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Exception.Handler.Tree", (object[] args) => mockExceptionHandleTree.Object).Execute();

        IoC.Resolve<SpaceGame.Lib.ICommand>("Exception.Handler.Register", new Type[] { typeof(CommandTest), typeof(ArgumentException) }, mockHandler.Object).Execute();

        mockExceptionHandleTree.VerifyAll();
    }

    [Fact]
    public void SuccesfullRegisterSomeExceptionHandlers()
    {
        var mockHandler = new Mock<IHandler>();
        var dict = new Dictionary<int, IHandler>();

        IoC.Resolve<ICommand>("IoC.Register", "Exception.Handler.Register",
            (object[] args) => new RegisterHandlerCommand((IEnumerable<Type>)args[0], (IHandler)args[1])
        ).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Exception.Handler.Tree", (object[] args) => dict).Execute();

        IoC.Resolve<SpaceGame.Lib.ICommand>("Exception.Handler.Register", new Type[] { typeof(CommandTest), typeof(IOException) }, mockHandler.Object).Execute();
        IoC.Resolve<SpaceGame.Lib.ICommand>("Exception.Handler.Register", new List<Type> { typeof(ArgumentException), typeof(CommandTest) }, mockHandler.Object).Execute();
        IoC.Resolve<SpaceGame.Lib.ICommand>("Exception.Handler.Register", new List<Type> { typeof(CommandTest) }, mockHandler.Object).Execute();
        IoC.Resolve<SpaceGame.Lib.ICommand>("Exception.Handler.Register", new Type[] { typeof(ArgumentException) }, mockHandler.Object).Execute();
        var tree = IoC.Resolve<IDictionary<int, IHandler>>("Exception.Handler.Tree");

        Assert.Equal(4, tree.Count());
    }

    [Fact]
    public void RegisterExceptionHandlersWithSameHashCodesThrowException()
    {
        var mockHandler = new Mock<IHandler>();
        var dict = new Dictionary<int, IHandler>();

        IoC.Resolve<ICommand>("IoC.Register", "Exception.Handler.Register",
            (object[] args) => new RegisterHandlerCommand((IEnumerable<Type>)args[0], (IHandler)args[1])
        ).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Exception.Handler.Tree", (object[] args) => dict).Execute();

        IoC.Resolve<SpaceGame.Lib.ICommand>("Exception.Handler.Register", new Type[] { typeof(CommandTest), typeof(IOException) }, mockHandler.Object).Execute();

        Assert.Throws<ArgumentException>(() =>
            IoC.Resolve<SpaceGame.Lib.ICommand>(
                "Exception.Handler.Register", new Type[] { typeof(IOException), typeof(CommandTest) },
                mockHandler.Object
            ).Execute()
        );
    }
}
