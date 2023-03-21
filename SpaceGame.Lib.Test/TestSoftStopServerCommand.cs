namespace SpaceGame.Lib.Test;
using System.Collections.Concurrent;
using Moq;
using Hwdtech;
using Hwdtech.Ioc;

public class TestSoftStopServerCommand
{
    public TestSoftStopServerCommand()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        IoC.Resolve<ICommand>("Scopes.Current.Set",
            IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))
        ).Execute();

        var map = new ConcurrentDictionary<int, object>();

        IoC.Resolve<ICommand>("IoC.Register", "Server.Thread.Map", (object[] args) => map).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Server.Stop.Soft", (object[] args) => new SoftStopServerCommand()).Execute();
    }

    [Fact]
    public void SuccessfulSoftStoppingServer()
    {
        var map = IoC.Resolve<ConcurrentDictionary<int, object>>("Server.Thread.Map");
        var mockCommand = new Mock<SpaceGame.Lib.ICommand>();
        mockCommand.Setup(c => c.Execute()).Verifiable();

        IoC.Resolve<ICommand>("IoC.Register", "Server.Thread.Command.Send", (object[] args) => mockCommand.Object).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Server.Thread.Stop.Soft", (object[] args) => mockCommand.Object).Execute();

        map[1] = 1;
        map[2] = 2;
        map[3] = 3;

        mockCommand.Verify(c => c.Execute(), Times.Never());

        IoC.Resolve<SpaceGame.Lib.ICommand>("Server.Stop.Soft").Execute();

        mockCommand.Verify(c => c.Execute(), Times.Exactly(3));
    }
}
