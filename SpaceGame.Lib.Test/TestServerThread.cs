namespace SpaceGame.Lib.Test;
using Hwdtech;
using Hwdtech.Ioc;
using System.Collections.Concurrent;
using Moq;


public class TestServerThread
{
    ConcurrentDictionary<int, ServerThread> serverThreadMap = new ConcurrentDictionary<int, ServerThread>();
    ConcurrentDictionary<int, ISender> serverThreadSenderMap = new ConcurrentDictionary<int, ISender>();

    public TestServerThread()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Server.Thread.Map", (object[] args) => serverThreadMap).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Server.Thread.Sender.Map", (object[] args) => serverThreadSenderMap).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Server.Register.Thread", (object[] args) => new RegisterThreadStrategy().Invoke(args)).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Server.Register.Sender", (object[] args) => new RegisterThreadSenderStrategy().Invoke(args)).Execute();
    }

    [Fact]
    public void ExceptionHandleCommand()
    {
        var isHandled = false;
        IoC.Resolve<ICommand>("IoC.Register", "Server.Thread.Create.Start", (object[] args) => new CreateAndStartServerThreadStrategy().Invoke(args)).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Server.Thread.Stop.Hard", (object[] args) => new HardStopServerThreadStrategy().Invoke(args)).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Server.Thread.Command.Send", (object[] args) => new SendCommandStrategy().Invoke(args)).Execute();

        var key = 22;

        var are = new AutoResetEvent(true);

        var serverStartAndCreatecmd = IoC.Resolve<SpaceGame.Lib.ICommand>("Server.Thread.Create.Start", key, () =>
        {
            are.WaitOne();
        });
        serverStartAndCreatecmd.Execute();
        var sendCmd = IoC.Resolve<SpaceGame.Lib.ICommand>("Server.Thread.Command.Send", key, new ActionCommand(() =>
        {
            new InitScopeBasedIoCImplementationCommand().Execute();
            IoC.Resolve<ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

            var mockHandler = new Mock<IHandler>();
            mockHandler.Setup(x => x.Handle()).Callback(() => isHandled = true);

            IoC.Resolve<ICommand>("IoC.Register", "Exception.Handler.Find", (object[] args) => mockHandler.Object).Execute();

            throw new Exception();
        }));

        sendCmd.Execute();
        are.Set();
        Thread.Sleep(1000);

        Assert.True(isHandled);

        var successfulHardStopCommand = IoC.Resolve<SpaceGame.Lib.ICommand>("Server.Thread.Stop.Hard", key, () =>
        {
            are.WaitOne();
        });

        successfulHardStopCommand.Execute();

        are.Set();
        Thread.Sleep(1000);
    }
}
