namespace SpaceGame.Lib.Test;
using Hwdtech;
using Hwdtech.Ioc;
using System.Collections.Concurrent;


public class TestSendCommand
{
    ConcurrentDictionary<int, ServerThread> serverThreadMap = new ConcurrentDictionary<int, ServerThread>();
    ConcurrentDictionary<int, ISender> serverThreadSenderMap = new ConcurrentDictionary<int, ISender>();

    public TestSendCommand()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Server.Thread.Map", (object[] args) => serverThreadMap).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Server.Thread.Sender.Map", (object[] args) => serverThreadSenderMap).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Server.Register.Thread", (object[] args) => new RegisterThreadStrategy().Invoke(args)).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Server.Register.Sender", (object[] args) => new RegisterThreadSenderStrategy().Invoke(args)).Execute();
    }

    [Fact]
    public void FailedSendCommandException()
    {
        IoC.Resolve<ICommand>("IoC.Register", "Server.Thread.Create.Start", (object[] args) => new CreateAndStartServerThreadStrategy().Invoke(args)).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Server.Thread.Command.Send", (object[] args) => new SendCommandStrategy().Invoke(args)).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Server.Thread.Stop.Hard", (object[] args) => new HardStopServerThreadStrategy().Invoke(args)).Execute();

        var key = 22;
        var falsekey = 23;

        var are = new AutoResetEvent(true);

        var serverStartAndCreatecmd = IoC.Resolve<SpaceGame.Lib.ICommand>("Server.Thread.Create.Start", key);
        serverStartAndCreatecmd.Execute();

        var sendCmd = IoC.Resolve<SpaceGame.Lib.ICommand>("Server.Thread.Command.Send", falsekey, new ActionCommand(() =>
        {
            are.WaitOne();
        }));

        Assert.Throws<Exception>(
            () =>
            {
                sendCmd.Execute();
            }
        );

        var hardStopCommand = IoC.Resolve<SpaceGame.Lib.ICommand>("Server.Thread.Stop.Hard", key, () =>
        {
            are.WaitOne();
        });

        hardStopCommand.Execute();

        are.Set();
        Thread.Sleep(1000);
    }
}
