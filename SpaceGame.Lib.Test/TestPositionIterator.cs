namespace SpaceGame.Lib.Test;
using Vector;
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
    public void SuccessfulIterating()
    {
        var positions = new List<Vector>{
            new Vector(0, 0),
            new Vector(1, 0),
        };

        IoC.Resolve<ICommand>("IoC.Register", "Game.Positions", (object[] args) => positions).Execute();
        var iterator = new PositionIterator();

        Assert.Equal(positions[0], iterator.Current);
        Assert.True(iterator.MoveNext());
        Assert.Equal(positions[1], iterator.Current);
        Assert.False(iterator.MoveNext());

        iterator.Reset();
        Assert.Equal(positions[0], iterator.Current);

        Assert.Throws<NotImplementedException>(() => iterator.Dispose());
    }

    [Fact]
    public void IteratingThrowsOutOfRangeException()
    {
        var positions = new List<Vector>{
            new Vector(0, 0),
        };

        IoC.Resolve<ICommand>("IoC.Register", "Game.Positions", (object[] args) => positions).Execute();
        var iterator = new PositionIterator();

        Assert.Equal(positions[0], iterator.Current);
        Assert.False(iterator.MoveNext());
        Assert.Throws<ArgumentOutOfRangeException>(() => iterator.Current);
    }
}
