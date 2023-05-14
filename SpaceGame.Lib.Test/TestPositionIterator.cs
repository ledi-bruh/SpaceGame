namespace SpaceGame.Lib.Test;
using Vector;
using Moq;
using Hwdtech;
using Hwdtech.Ioc;

public class TestPositionIterator
{
    public TestPositionIterator()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        IoC.Resolve<ICommand>("Scopes.Current.Set",
            IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))
        ).Execute();
    }
    [Fact]
    public void Test1()
    {
        IoC.Resolve<ICommand>("IoC.Register", "Game.Positions", (object[] args) => new List<Vector>{
            new Vector(0, 0),
            new Vector(1, 0),
            new Vector(2, 0),
        }).Execute();
        var iterator = new PositionIterator();
        var a = iterator.Current;
        var b = iterator.MoveNext();
        a = iterator.Current;
        b = iterator.MoveNext();
        a = iterator.Current;
        b = iterator.MoveNext();
        b = iterator.MoveNext();
        b = iterator.MoveNext();
        // a = iterator.Current;
    }
}
