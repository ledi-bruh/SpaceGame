namespace SpaceGame.Lib.Test;
using Moq;

internal class ThrowResolveDependencyExceptionStrategy : IStrategy
{
    public object Invoke(params object[] args) => throw new ResolveDependencyException();
}

public class TestIoC
{
    [Fact]
    public void IoCThrowsResolveDependencyException()
    {
        IoC.Resolve<ICommand>("IoC.Register", "ResolveDependencyException", new ThrowResolveDependencyExceptionStrategy()).Execute();

        Assert.Throws<ResolveDependencyException>(() => IoC.Resolve<ICommand>("ResolveDependencyException"));
        Assert.Throws<ResolveDependencyException>(() => IoC.Resolve<ICommand>("NaN"));
    }
}
