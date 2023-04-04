namespace SpaceGame.Lib.Test;
using Moq;
using Hwdtech;
using Hwdtech.Ioc;

public class TestHttpOrderStrategy
{
    public TestHttpOrderStrategy()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        IoC.Resolve<ICommand>("Scopes.Current.Set",
            IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))
        ).Execute();

        var map = new Dictionary<int, int>();
        var mockCommand = new Mock<SpaceGame.Lib.ICommand>();

        IoC.Resolve<ICommand>("IoC.Register", "Server.Game.Thread.Map", (object[] args) => map).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Http.Order", (object[] args) => new HttpOrderStrategy().Invoke(args)).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Game.Command.Interpret", (object[] args) => mockCommand.Object).Execute();
    }

    [Fact]
    public void SuccessfulOrderSending()
    {
        var map = IoC.Resolve<IDictionary<int, int>>("Server.Game.Thread.Map");
        var mockCommand = new Mock<SpaceGame.Lib.ICommand>();
        mockCommand.Setup(c => c.Execute()).Verifiable();

        var messageMock = new Mock<IInterpretingMessage>();
        messageMock.SetupGet(x => x.GameID).Returns(0);

        map[0] = 0;

        IoC.Resolve<ICommand>("IoC.Register", "Server.Thread.Command.Send", (object[] args) => mockCommand.Object).Execute();

        mockCommand.Verify(c => c.Execute(), Times.Never());

        IoC.Resolve<SpaceGame.Lib.ICommand>("Http.Order", messageMock.Object).Execute();

        mockCommand.Verify(c => c.Execute(), Times.Exactly(1));
    }

    [Fact]
    public void TryGetGameIDThrowsException()
    {
        var map = IoC.Resolve<IDictionary<int, int>>("Server.Game.Thread.Map");
        var mockCommand = new Mock<SpaceGame.Lib.ICommand>();
        mockCommand.Setup(c => c.Execute()).Verifiable();

        var messageMock = new Mock<IInterpretingMessage>();
        messageMock.SetupGet(x => x.GameID).Throws(new Exception());

        IoC.Resolve<ICommand>("IoC.Register", "Server.Thread.Command.Send", (object[] args) => mockCommand.Object).Execute();

        mockCommand.Verify(c => c.Execute(), Times.Never());

        Assert.Throws<Exception>(() => IoC.Resolve<SpaceGame.Lib.ICommand>("Http.Order", messageMock.Object).Execute());

        mockCommand.Verify(c => c.Execute(), Times.Never());
    }
}
