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
        var ghcAnyOrder = new GetHashCodeInAnyOrderStrategy();
        var data1 = new List<object> { -5123, typeof(Exception), "ooaip" };
        var data2 = new List<object> { typeof(Exception), "ooaip", -5123 };

        Assert.Equal(ghcAnyOrder.Invoke(data1), ghcAnyOrder.Invoke(data2));
    }
}
