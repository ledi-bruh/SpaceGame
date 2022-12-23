namespace SpaceGame.Lib.Test;
using Moq;
using Hwdtech;
using Hwdtech.Ioc;

public class TestFindHandlerStrategy
{
    public TestFindHandlerStrategy()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        IoC.Resolve<ICommand>("Scopes.Current.Set",
            IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))
        ).Execute();
    }

    [Fact]
    public void SuccesfullFindHandler()
    {
        var tree = new Dictionary<int, IHandler>();
        var mockHandler = new Mock<IHandler>();

        IoC.Resolve<ICommand>("IoC.Register", "GetHashCode", (object[] args) => new GetHashCodeStrategy().Invoke(args)).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Exception.Handler.Tree", (object[] args) => tree).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Exception.Handler.Find",
            (object[] args) => new FindHanlderStrategy().Invoke(args)
        ).Execute();

        var types = new List<Type>() { typeof(ICommand), typeof(ArgumentException) };

        tree.Add(IoC.Resolve<int>("GetHashCode", types.OrderBy(x => x.GetHashCode())), mockHandler.Object);

        Assert.Equal(mockHandler.Object, IoC.Resolve<IHandler>("Exception.Handler.Find", types));
    }

    [Fact]
    public void FindDefaultHandler()
    {
        var tree = new Dictionary<int, IHandler>();
        var mockHandler = new Mock<IHandler>();

        tree.Add(0, mockHandler.Object);

        IoC.Resolve<ICommand>("IoC.Register", "GetHashCode", (object[] args) => new GetHashCodeStrategy().Invoke(args)).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Exception.Handler.Tree", (object[] args) => tree).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Exception.Handler.Find", 
            (object[] args) => new FindHanlderStrategy().Invoke(args)
        ).Execute();

        var types = new List<Type>() { typeof(ICommand), typeof(ArgumentException) };

        Assert.Equal(mockHandler.Object, IoC.Resolve<IHandler>("Exception.Handler.Find", types));
    }
}
