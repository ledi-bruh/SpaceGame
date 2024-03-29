namespace SpaceGame.Lib.Test;
using Hwdtech;
using Hwdtech.Ioc;
using System.Threading;
using Moq;


public class TestGameCommand
{
    public TestGameCommand()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<ICommand>("Scopes.Current.Set",
            IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))
        ).Execute();
    }

    [Fact]
    public void SuccessfullGameQueueDequeue()
    {
        Queue<SpaceGame.Lib.ICommand> queue = new Queue<SpaceGame.Lib.ICommand>();
        queue.Enqueue(new EmptyCommand());

        IoC.Resolve<ICommand>("IoC.Register", "Game.Queue.Dequeue", (object[] args) => new GameQueueDequeueStrategy().Invoke(args)).Execute();

        IoC.Resolve<SpaceGame.Lib.ICommand>("Game.Queue.Dequeue", queue).Execute();

        Assert.True(queue.Count() == 0);
    }

    [Fact]
    public void SuccessfullGameCommandExecute()
    {
        var scope = IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"));

        IoC.Resolve<ICommand>("Scopes.Current.Set", scope).Execute();
        var mockStrategy = new Mock<IStrategy>();
        mockStrategy.Setup(x => x.Invoke()).Returns(400);

        var cmd = new ActionCommand(() =>
        {
            mockStrategy.Setup(x => x.Invoke()).Returns(0);
        });

        IoC.Resolve<ICommand>("IoC.Register", "Game.Command.Queue.Start", (object[] args) => new StartGameQueueCommandStrategy().Invoke(args)).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Game.Queue.Dequeue", (object[] args) => new GameQueueDequeueStrategy().Invoke(args)).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Game.Get.Time.Quantum", (object[] args) => mockStrategy.Object.Invoke(args)).Execute();
        Queue<SpaceGame.Lib.ICommand> queue = new Queue<SpaceGame.Lib.ICommand>();

        queue.Enqueue(cmd);
        queue.Enqueue(new ActionCommand(() => { }));
        queue.Enqueue(new ActionCommand(() => { }));

        var scopeNew = IoC.Resolve<object>("Scopes.New", scope);

        var gameCmd = new GameCommand(scopeNew, queue);

        gameCmd.Execute();

        Assert.True(queue.Count == 2);
    }

    [Fact]
    public void TestEmptyQueueExeption()
    {
        var scope = IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"));


        IoC.Resolve<ICommand>("Scopes.Current.Set", scope).Execute();
        var mockStrategy = new Mock<IStrategy>();
        mockStrategy.Setup(x => x.Invoke()).Returns(400);

        IoC.Resolve<ICommand>("IoC.Register", "Game.Command.Queue.Start", (object[] args) => new StartGameQueueCommandStrategy().Invoke(args)).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Game.Queue.Dequeue", (object[] args) => new GameQueueDequeueStrategy().Invoke(args)).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Game.Get.Time.Quantum", (object[] args) => mockStrategy.Object.Invoke(args)).Execute();
        Queue<SpaceGame.Lib.ICommand> queue = new Queue<SpaceGame.Lib.ICommand>();
        queue.Enqueue(new ActionCommand(() => { }));

        var scopeNew = IoC.Resolve<object>("Scopes.New", scope);

        var gameCmd = new GameCommand(scopeNew, queue);
        Assert.Throws<Exception>(
            () =>
            {
                gameCmd.Execute();
            }
        );
    }

    [Fact]
    public void TestSuccesfullHandleException()
    {
        var scope = IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"));

        var mockGoodHandler = new Mock<IHandler>();
        mockGoodHandler.Setup(x => x.Handle()).Verifiable();

        IoC.Resolve<ICommand>("Scopes.Current.Set", scope).Execute();

        var mockStrategy = new Mock<IStrategy>();
        mockStrategy.Setup(x => x.Invoke()).Returns(400);




        IoC.Resolve<ICommand>("IoC.Register", "Game.Command.Queue.Start", (object[] args) => new StartGameQueueCommandStrategy().Invoke(args)).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Game.Queue.Dequeue", (object[] args) => new GameQueueDequeueStrategy().Invoke(args)).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Exception.Handler.Find", (object[] args) => mockGoodHandler.Object).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Game.Get.Time.Quantum", (object[] args) => mockStrategy.Object.Invoke(args)).Execute();

        Queue<SpaceGame.Lib.ICommand> queue = new Queue<SpaceGame.Lib.ICommand>();
        queue.Enqueue(new ActionCommand(() => { }));
        queue.Enqueue(new ActionCommand(() => { throw new Exception(); }));
        queue.Enqueue(new ActionCommand(() => { mockStrategy.Setup(x => x.Invoke()).Returns(0); }));

        var scopeNew = IoC.Resolve<object>("Scopes.New", scope);

        var gameCmd = new GameCommand(scopeNew, queue);

        gameCmd.Execute();

        mockGoodHandler.Verify(x => x.Handle(), Times.Once);
    }

    [Fact]
    public void TestDefaultHandler()
    {
        var scope = IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"));

        IoC.Resolve<ICommand>("Scopes.Current.Set", scope).Execute();

        var mockStrategy = new Mock<IStrategy>();
        mockStrategy.Setup(x => x.Invoke()).Returns(400);

        IoC.Resolve<ICommand>("IoC.Register", "Game.Command.Queue.Start", (object[] args) => new StartGameQueueCommandStrategy().Invoke(args)).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Game.Queue.Dequeue", (object[] args) => new GameQueueDequeueStrategy().Invoke(args)).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Exception.Handler.Find", (object[] args) => new DefaultHandler((Exception)args[0])).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Game.Get.Time.Quantum", (object[] args) => mockStrategy.Object.Invoke(args)).Execute();

        Queue<SpaceGame.Lib.ICommand> queue = new Queue<SpaceGame.Lib.ICommand>();
        queue.Enqueue(new ActionCommand(() => { }));
        queue.Enqueue(new ActionCommand(() => { throw new Exception(); }));
        queue.Enqueue(new ActionCommand(() => { mockStrategy.Setup(x => x.Invoke()).Returns(0); }));

        var scopeNew = IoC.Resolve<object>("Scopes.New", scope);

        var gameCmd = new GameCommand(scopeNew, queue);

        Assert.Throws<Exception>(
            () =>
            {
                gameCmd.Execute();
            }
        );
    }
}
