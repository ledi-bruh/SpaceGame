namespace SpaceGame.Lib.Test;
using Moq;
using Hwdtech;
using Hwdtech.Ioc;

public class TestArrangeShipCommand
{
    public TestArrangeShipCommand()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        IoC.Resolve<ICommand>("Scopes.Current.Set",
            IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))
        ).Execute();
    }

    [Fact]
    public void SuccessfulArrangeShip()
    {
        var mockUObject = new Mock<IUObject>();

        var mockPositionIterator = new Mock<IEnumerator<object>>();
        mockPositionIterator.SetupGet(x => x.Current).Verifiable();
        mockPositionIterator.Setup(x => x.MoveNext()).Verifiable();

        var mockCommand = new Mock<Lib.ICommand>();
        mockCommand.Setup(x => x.Execute()).Verifiable();

        IoC.Resolve<ICommand>("IoC.Register", "Game.UObject.SetProperty", (object[] args) => mockCommand.Object).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Game.Arrange.Ship",
            (object[] args) => new ArrangeShipCommand((IUObject)args[0], (IEnumerator<object>)args[1])
        ).Execute();

        mockCommand.Verify(x => x.Execute(), Times.Never());

        IoC.Resolve<Lib.ICommand>("Game.Arrange.Ship", mockUObject.Object, mockPositionIterator.Object).Execute();

        mockCommand.Verify(x => x.Execute(), Times.Once());
        mockPositionIterator.VerifyAll();
    }
}
