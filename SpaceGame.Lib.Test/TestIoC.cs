namespace SpaceGame.Lib.Test;
using Moq;

internal class ThrowResolveDependencyExceptionStrategy : IStrategy
{
    public object Invoke(params object[] args) => throw new ResolveDependencyException();
}

public class TestIoC
{
    [Fact]
    public void TestCreateAdapter()
    {
        IoC.Resolve<ICommand>("IoC.Register", "Game.Adapter", new AdapterStrategy()).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Game.CreateUObject", new CreateNewObjectStrategy()).Execute();

        IUObject uobj = IoC.Resolve<IUObject>("Game.CreateUObject");
        IMovable adapter = IoC.Resolve<IMovable>("Game.Adapter", uobj, typeof(MovableAdapter));

        Assert.Equal(typeof(MovableAdapter), adapter.GetType());
    }

    [Fact]
    public void IoCThrowsResolveDependencyException()
    {
        IoC.Resolve<ICommand>("IoC.Register", "ResolveDependencyException", new ThrowResolveDependencyExceptionStrategy()).Execute();

        Assert.Throws<ResolveDependencyException>(() => IoC.Resolve<ICommand>("ResolveDependencyException"));
        Assert.Throws<ResolveDependencyException>(() => IoC.Resolve<ICommand>("NaN"));
    }
}
