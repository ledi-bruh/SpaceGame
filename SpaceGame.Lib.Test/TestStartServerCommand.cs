namespace SpaceGame.Lib.Test;
using Moq;
using Hwdtech;
using Hwdtech.Ioc;

public class TestStartServerCommand
{
    public TestStartServerCommand()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        IoC.Resolve<ICommand>("Scopes.Current.Set",
            IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))
        ).Execute();
    }

    [Fact]
    public void SuccessfulStartingServer()
    {
        var threadCount = 73;
        var mockCommand = new Mock<SpaceGame.Lib.ICommand>();
        mockCommand.Setup(c => c.Execute()).Verifiable();

        IoC.Resolve<ICommand>("IoC.Register", "Server.Thread.Create.Start", (object[] args) => mockCommand.Object).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Server.Start", (object[] args) => new StartServerCommand((int)args[0])).Execute();

        mockCommand.Verify(c => c.Execute(), Times.Never());

        IoC.Resolve<SpaceGame.Lib.ICommand>("Server.Start", threadCount).Execute();

        mockCommand.Verify(c => c.Execute(), Times.Exactly(threadCount));
    }
}
