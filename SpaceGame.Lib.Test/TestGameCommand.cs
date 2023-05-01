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
        IoC.Resolve<ICommand>("IoC.Register", "Game.Queue.Dequeue", (object[] args) => new GameQueueDequeueStrategy().Invoke(args)).Execute();
    }

    [Fact]
    public void SuccessfullGameQueueDequeue()
    {
        Queue<SpaceGame.Lib.ICommand> queue = new Queue<SpaceGame.Lib.ICommand>();
        queue.Enqueue(new EmptyCommand());
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

        IoC.Resolve<ICommand>("IoC.Register", "Game.Command.Queue.Start", (object[] args) => new StartGameQueueCommandStrategy().Invoke(args)).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Game.Queue.Dequeue", (object[] args) => new GameQueueDequeueStrategy().Invoke(args)).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Game.Get.Time.Quantum", (object[] args) => mockStrategy.Object.Invoke(args)).Execute();
        Queue<SpaceGame.Lib.ICommand> queue = new Queue<SpaceGame.Lib.ICommand>();
        queue.Enqueue(new ActionCommand(() => {Thread.Sleep(300);}));
        queue.Enqueue(new ActionCommand(() => {Thread.Sleep(200);}));

        var scopeNew = IoC.Resolve<object>("Scopes.New", scope);

        var gameCmd = new GameCommand(scopeNew, queue);

        gameCmd.Execute();

        Assert.True(queue.Count() == 0);
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
        queue.Enqueue(new ActionCommand(() => {Thread.Sleep(300);}));

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
    public void TestDefaultHandlerException()
    {
        var scope = IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"));

        Dictionary<int, IHandler> exceptionHandlerTree = new Dictionary<int, IHandler>();
        exceptionHandlerTree.Add(0, new DefaultHandler());

        var mockStrategy = new Mock<IStrategy>();
        mockStrategy.Setup(x => x.Invoke()).Returns(400);

        IoC.Resolve<ICommand>("Scopes.Current.Set", scope).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "GetHashCode.AnyOrder", (object[] args) => new GetHashCodeStrategy().Invoke(args)).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Exception.Handler.Tree", (object[] args) => exceptionHandlerTree).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Game.Command.Queue.Start", (object[] args) => new StartGameQueueCommandStrategy().Invoke(args)).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Game.Queue.Dequeue", (object[] args) => new GameQueueDequeueStrategy().Invoke(args)).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Exception.Handler.Find", (object[] args) => new FindHanlderStrategy().Invoke(args)).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Game.Get.Time.Quantum", (object[] args) => mockStrategy.Object.Invoke(args)).Execute();

        Queue<SpaceGame.Lib.ICommand> queue = new Queue<SpaceGame.Lib.ICommand>();
        queue.Enqueue(new ActionCommand(() => {Thread.Sleep(300);}));
        queue.Enqueue(new ActionCommand(() => {throw new Exception();}));

        var scopeNew = IoC.Resolve<object>("Scopes.New", scope);

        var gameCmd = new GameCommand(scopeNew, queue);
        Assert.Throws<Exception>(
            () =>
            {
                gameCmd.Execute();
            }
        );
        Assert.True(IoC.Resolve<object>("Scopes.Current") == scopeNew);
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
        queue.Enqueue(new ActionCommand(() => {Thread.Sleep(300);}));
        queue.Enqueue(new ActionCommand(() => {throw new Exception();}));
        queue.Enqueue(new ActionCommand(() => {Thread.Sleep(300);}));

        var scopeNew = IoC.Resolve<object>("Scopes.New", scope);

        var gameCmd = new GameCommand(scopeNew, queue);

        gameCmd.Execute();

        mockGoodHandler.Verify(x => x.Handle(), Times.Once);
    }
}
