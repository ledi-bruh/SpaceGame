namespace SpaceGame.Lib.Test;
using Moq;
using Hwdtech;
using Hwdtech.Ioc;

public class TestArrangeShipsCommand
{
    public TestArrangeShipsCommand()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        IoC.Resolve<ICommand>("Scopes.Current.Set",
            IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))
        ).Execute();
    }

    [Fact]
    public void SuccessfulArrangeShips()
    {
        var mockGameUObjects = Enumerable.Repeat((new Mock<IUObject>()).Object, 3).ToList();

        var mockPositionIterator = new Mock<IEnumerator<object>>();
        mockPositionIterator.Setup(x => x.Reset()).Verifiable();

        var mockCommand = new Mock<Lib.ICommand>();
        mockCommand.Setup(x => x.Execute()).Verifiable();

        IoC.Resolve<ICommand>("IoC.Register", "Game.Iterator.Position", (object[] args) => mockPositionIterator.Object).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Game.Arrange.Ship", (object[] args) => mockCommand.Object).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Game.Arrange.Ships", (object[] args) => new ArrangeShipsCommand((IEnumerable<IUObject>)args[0])).Execute();

        mockPositionIterator.Verify(x => x.Reset(), Times.Never());
        mockCommand.Verify(x => x.Execute(), Times.Never());

        IoC.Resolve<Lib.ICommand>("Game.Arrange.Ships", mockGameUObjects).Execute();

        mockPositionIterator.Verify(x => x.Reset(), Times.Once());
        mockCommand.Verify(x => x.Execute(), Times.Exactly(3));
    }
}
