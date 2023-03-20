namespace SpaceGame.Lib.Test;
using Hwdtech;
using Hwdtech.Ioc;
using System.Collections.Concurrent;


public class TestSoftStopServerThread
{
    ConcurrentDictionary<int, ServerThread> serverThreadMap = new ConcurrentDictionary<int, ServerThread>();
    ConcurrentDictionary<int, ISender> serverThreadSenderMap = new ConcurrentDictionary<int, ISender>();

    public TestSoftStopServerThread()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Server.Thread.Map", (object[] args) => serverThreadMap).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Server.Thread.Sender.Map", (object[] args) => serverThreadSenderMap).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Server.Register.Thread", (object[] args) => new RegisterThreadStrategy().Invoke(args)).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Server.Register.Sender", (object[] args) => new RegisterThreadSenderStrategy().Invoke(args)).Execute();
    }

    [Fact]
    public void SuccessfulSoftStop()
    {
        IStrategy createAndStartStrategy = new CreateAndStartServerThreadStrategy();

        var isActivated = false;

        var key = 22;

        var are = new AutoResetEvent(true);

        var serverStartAndCreatecmd = (SpaceGame.Lib.ICommand)createAndStartStrategy.Invoke(key, () =>
        {
            are.WaitOne();
        });
        serverStartAndCreatecmd.Execute();

        Assert.True(serverThreadMap.Count() == 1);
        Assert.True(serverThreadSenderMap.Count() == 1);

        var softStopStrategy = new SoftStopServerThreadStrategy();
        var sendStrategy = new SendCommandStrategy();

        var softStopCommand  = (SpaceGame.Lib.ICommand)softStopStrategy.Invoke(key);

        softStopCommand.Execute();

        var sendCmd = (SpaceGame.Lib.ICommand)sendStrategy.Invoke(key, new ActionCommand(() =>
        {
            isActivated = true;
            are.WaitOne();
        }));

        sendCmd.Execute();

        are.Set();
        Thread.Sleep(1000);
        Assert.True(isActivated);
    }

    [Fact]
    public void FailedSoftStopCommand()
    {
        IoC.Resolve<ICommand>("IoC.Register", "Server.Thread.Create.Start", (object[] args) => new CreateAndStartServerThreadStrategy().Invoke(args)).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Server.Thread.Stop.Soft", (object[] args) => new SoftStopServerThreadStrategy().Invoke(args)).Execute();

        var key = 22;

        var are = new AutoResetEvent(true);

        var serverStartAndCreatecmd = IoC.Resolve<SpaceGame.Lib.ICommand>("Server.Thread.Create.Start", key);
        serverStartAndCreatecmd.Execute();

        var failedSoftStopCommand = new SoftStopServerThreadCommand(IoC.Resolve<ConcurrentDictionary<int, ServerThread>>("Server.Thread.Map")[key], () => { });

        Assert.Throws<Exception>(() => failedSoftStopCommand.Execute());

        var successfulSoftStopCommand = IoC.Resolve<SpaceGame.Lib.ICommand>("Server.Thread.Stop.Soft", key, () =>
        {
            are.WaitOne();
        });

        successfulSoftStopCommand.Execute();

        are.Set();
        Thread.Sleep(1000);
    }

    [Fact]
    public void FailedSoftStopStrategy()
    {
        IoC.Resolve<ICommand>("IoC.Register", "Server.Thread.Create.Start", (object[] args) => new CreateAndStartServerThreadStrategy().Invoke(args)).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Server.Thread.Stop.Soft", (object[] args) => new SoftStopServerThreadStrategy().Invoke(args)).Execute();

        var key = 22;

        var falsekey = 23;

        var are = new AutoResetEvent(true);

        var serverStartAndCreatecmd = IoC.Resolve<SpaceGame.Lib.ICommand>("Server.Thread.Create.Start", key, () =>
        {
            are.WaitOne();
        });
        serverStartAndCreatecmd.Execute();

        Assert.Throws<Exception>(
            () =>
            {
                var failedSoftStopCommand = IoC.Resolve<SpaceGame.Lib.ICommand>("Server.Thread.Stop.Soft", falsekey);
            }
        );

        var successfulSoftStopCommand = IoC.Resolve<SpaceGame.Lib.ICommand>("Server.Thread.Stop.Soft", key, () =>
        {
            are.WaitOne();
        });

        successfulSoftStopCommand.Execute();

        are.Set();
        Thread.Sleep(1000);
    }
}
