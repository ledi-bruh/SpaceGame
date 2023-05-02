namespace SpaceGame.Lib.Test;
using Moq;
using Hwdtech;
using Hwdtech.Ioc;

public class TestGameQueuePopStrategy
{
    public TestGameQueuePopStrategy()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        IoC.Resolve<ICommand>("Scopes.Current.Set",
            IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))
        ).Execute();
    }

    [Fact]
    public void SuccessfulGameQueuePopping()
    {
        var gameQueueMap = new Dictionary<int, Queue<Lib.ICommand>>();
        IoC.Resolve<ICommand>("IoC.Register", "Game.Queue.Pop", (object[] args) => new GameQueuePopStrategy().Invoke(args)).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Game.Get.Queue", (object[] args) => new GetGameQueueStrategy().Invoke(args)).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Game.Queue.Map", (object[] args) => gameQueueMap).Execute();

        var mockCommand = new Mock<Lib.ICommand>();
        var queue = new Queue<Lib.ICommand>();
        queue.Enqueue(mockCommand.Object);
        gameQueueMap.Add(0, queue);

        Assert.Single(queue);
        IoC.Resolve<Lib.ICommand>("Game.Queue.Pop", 0).Execute();
        Assert.Empty(queue);
    }
}
