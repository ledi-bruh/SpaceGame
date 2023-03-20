namespace SpaceGame.Lib.Test;

using Hwdtech;
using Hwdtech.Ioc;
using System.Collections.Concurrent;

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
    public void SuccessfulHardStopWithoutParams()
    {
        IoC.Resolve<ICommand>("IoC.Register", "Server.Thread.Create.Start", (object[] args) => new CreateAndStartServerThreadStrategy().Invoke(args)).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Server.Thread.Command.Send", (object[] args) => new SendCommandStrategy().Invoke(args)).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Server.Thread.Stop.Hard", (object[] args) => new HardStopServerThreadStrategy().Invoke(args)).Execute();

        var isActive = false;

        var key = 22;

        var are = new AutoResetEvent(true);

        var serverStartAndCreatecmd = IoC.Resolve<SpaceGame.Lib.ICommand>("Server.Thread.Create.Start", key);
        serverStartAndCreatecmd.Execute();

        var sendCmd = IoC.Resolve<SpaceGame.Lib.ICommand>("Server.Thread.Command.Send", key, new ActionCommand(() =>
        {
            isActive = true;
            are.WaitOne();
        }));

        sendCmd.Execute();

        are.Set();
        Thread.Sleep(1000);

        Assert.True(isActive);
        Assert.True(serverThreadMap.Count() == 1);
        Assert.True(serverThreadSenderMap.Count() == 1);

        var hardStopCommand = IoC.Resolve<SpaceGame.Lib.ICommand>("Server.Thread.Stop.Hard", key);

        hardStopCommand.Execute();
        are.Set();
        Thread.Sleep(1000);
    }

    [Fact]
    public void SuccessfulHardStopWithParams()
    {
        IoC.Resolve<ICommand>("IoC.Register", "Server.Thread.Create.Start", (object[] args) => new CreateAndStartServerThreadStrategy().Invoke(args)).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Server.Thread.Command.Send", (object[] args) => new SendCommandStrategy().Invoke(args)).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Server.Thread.Stop.Hard", (object[] args) => new HardStopServerThreadStrategy().Invoke(args)).Execute();

        var isActive = false;
        var createAndStartFlag = false;
        var hsFlag = false;

        var key = 22;

        var are = new AutoResetEvent(true);

        var serverStartAndCreatecmd = IoC.Resolve<SpaceGame.Lib.ICommand>("Server.Thread.Create.Start", key, () =>
        {
            createAndStartFlag = true;
            are.WaitOne();
        });
        serverStartAndCreatecmd.Execute();

        var sendCmd = IoC.Resolve<SpaceGame.Lib.ICommand>("Server.Thread.Command.Send", key, new ActionCommand(() =>
        {
            isActive = true;
            are.WaitOne();
        }));

        sendCmd.Execute();

        are.Set();
        Thread.Sleep(1000);

        Assert.True(isActive);
        Assert.True(serverThreadMap.Count() == 1);
        Assert.True(serverThreadSenderMap.Count() == 1);
        Assert.True(createAndStartFlag);

        var hardStopCommand = IoC.Resolve<SpaceGame.Lib.ICommand>("Server.Thread.Stop.Hard", key, () =>
        {
            hsFlag = true;
            are.WaitOne();
        });

        hardStopCommand.Execute();

        are.Set();
        Thread.Sleep(1000);

        Assert.True(hsFlag);
    }

    [Fact]
    public void failedHardStopCommand()
    {
        IoC.Resolve<ICommand>("IoC.Register", "Server.Thread.Create.Start", (object[] args) => new CreateAndStartServerThreadStrategy().Invoke(args)).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Server.Thread.Stop.Hard", (object[] args) => new HardStopServerThreadStrategy().Invoke(args)).Execute();

        var key = 22;

        var are = new AutoResetEvent(true);

        var serverStartAndCreatecmd = IoC.Resolve<SpaceGame.Lib.ICommand>("Server.Thread.Create.Start", key, () =>
        {
            are.WaitOne();
        });
        serverStartAndCreatecmd.Execute();

        var failedHardStopCommand = new HardStopServerThreadCommand(IoC.Resolve<ConcurrentDictionary<int, ServerThread>>("Server.Thread.Map")[key]);

        Assert.Throws<Exception>(() => failedHardStopCommand.Execute());

        var successfulHardStopCommand = IoC.Resolve<SpaceGame.Lib.ICommand>("Server.Thread.Stop.Hard", key, () =>
        {
            are.WaitOne();
        });

        successfulHardStopCommand.Execute();

        are.Set();
        Thread.Sleep(1000);
    }

    [Fact]
    public void FailedHardStopStrategy()
    {
        IoC.Resolve<ICommand>("IoC.Register", "Server.Thread.Create.Start", (object[] args) => new CreateAndStartServerThreadStrategy().Invoke(args)).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Server.Thread.Stop.Hard", (object[] args) => new HardStopServerThreadStrategy().Invoke(args)).Execute();

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
                var failedHardStopCommand = IoC.Resolve<SpaceGame.Lib.ICommand>("Server.Thread.Stop.Hard", falsekey);
            }
        );

        var successfulHardStopCommand = IoC.Resolve<SpaceGame.Lib.ICommand>("Server.Thread.Stop.Hard", key, () =>
        {
            are.WaitOne();
        });

        successfulHardStopCommand.Execute();

        are.Set();
        Thread.Sleep(1000);
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
    
    [Fact]
    public void SuccessfulSoftStopWithParams()
    {
        IoC.Resolve<ICommand>("IoC.Register", "Server.Thread.Create.Start", (object[] args) => new CreateAndStartServerThreadStrategy().Invoke(args)).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Server.Thread.Command.Send", (object[] args) => new SendCommandStrategy().Invoke(args)).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Server.Thread.Stop.Soft", (object[] args) => new SoftStopServerThreadStrategy().Invoke(args)).Execute();

        var isActive = false;
        var createAndStartFlag = false;
        var ssFlag = false;

        var key = 22;

        var are = new AutoResetEvent(true);

        var serverStartAndCreatecmd = IoC.Resolve<SpaceGame.Lib.ICommand>("Server.Thread.Create.Start", key, () =>
        {
            createAndStartFlag = true;
            are.WaitOne();
        });
        serverStartAndCreatecmd.Execute();
        are.Set();
        Thread.Sleep(1000);

        Assert.True(serverThreadMap.Count() == 1);
        Assert.True(serverThreadSenderMap.Count() == 1);
        Assert.True(createAndStartFlag);

        var softStopCommand = IoC.Resolve<SpaceGame.Lib.ICommand>("Server.Thread.Stop.Soft", key, () =>
        {
            ssFlag = true;
            are.WaitOne();
        });

        softStopCommand.Execute();
        Assert.False(ssFlag);

        var sendCmd = IoC.Resolve<SpaceGame.Lib.ICommand>("Server.Thread.Command.Send", key, new ActionCommand(() =>
        {
            isActive = true;
        }));

        sendCmd.Execute();

        are.Set();
        Thread.Sleep(1000);
        Assert.True(isActive);
        Assert.True(ssFlag);
    }

    [Fact]
    public void FailedSoftStopCommand()
    {
        IoC.Resolve<ICommand>("IoC.Register", "Server.Thread.Create.Start", (object[] args) => new CreateAndStartServerThreadStrategy().Invoke(args)).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Server.Thread.Stop.Soft", (object[] args) => new SoftStopServerThreadStrategy().Invoke(args)).Execute();

        var key = 22;

        var are = new AutoResetEvent(true);

        var serverStartAndCreatecmd = IoC.Resolve<SpaceGame.Lib.ICommand>("Server.Thread.Create.Start", key, () =>
        {
            are.WaitOne();
        });
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
