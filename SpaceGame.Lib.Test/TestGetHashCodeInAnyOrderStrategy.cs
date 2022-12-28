namespace SpaceGame.Lib.Test;
using Hwdtech;
using Hwdtech.Ioc;

public class TestGetHashCodeInAnyOrderStrategy
{
    public TestGetHashCodeInAnyOrderStrategy()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        IoC.Resolve<ICommand>("Scopes.Current.Set",
            IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))
        ).Execute();

        IoC.Resolve<ICommand>("IoC.Register", "GetHashCode", (object[] args) => new GetHashCodeStrategy().Invoke(args)).Execute();
    }

    [Fact]
    public void GetHashCodeSomeDataInAnyOrderAreEqual()
    {
        IEnumerable<object> data1 = new object[] { -5123, typeof(Exception), "ooaip" };
        IEnumerable<object> data2 = new object[] { typeof(Exception), "ooaip", -5123 };

        GetHashCodeInAnyOrderStrategy ghcAnyOrder = new GetHashCodeInAnyOrderStrategy();

        Assert.Equal(ghcAnyOrder.Invoke(data1), ghcAnyOrder.Invoke(data2));
    }
}
