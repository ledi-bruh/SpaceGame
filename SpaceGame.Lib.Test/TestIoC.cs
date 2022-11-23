namespace SpaceGame.Lib.Test;
using Moq;
using Vector;

internal class ThrowResolveDependencyExceptionStrategy : IStrategy
{
    public object Invoke(params object[] args) => throw new ResolveDependencyException();
}

public class TestIoC
{
    [Fact]
    public void TestStartMovementCommand()
    {
        IoC.Resolve<ICommand>("IoC.Register", "Game.CreateUObject", new CreateNewObjectStrategy()).Execute();

        var uObject = IoC.Resolve<IUObject>("Game.CreateUObject");

        var mockStartable = new Mock<IStartable>();
        mockStartable.SetupGet(x => x.Target).Returns(uObject).Verifiable();
        mockStartable.SetupGet(x => x.Parameters).Returns(new Dictionary<string, object> { { "Velocity", new Vector(1, 1) } }).Verifiable();

        var queue = new QueueAdapter<ICommand>(new Queue<ICommand>());

        var startMovementCommand = new StartMovementCommand(mockStartable.Object);

        IoC.Resolve<ICommand>("IoC.Register", "Game.Adapter", new AdapterStrategy()).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Game.Command.Move", new CreateMoveCommandStrategy()).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Game.Command.Macro", new CreateMacroCommandStrategy()).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Game.Command.Getter", new CreateGetterCommandStrategy()).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Game.Command.Repeat", new CreateRepeatCommandStrategy()).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Game.UObject.SetProperty", new SetPropertyStrategy()).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Game.UObject.GetProperty", new GetPropertyStrategy()).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Game.Operation.Movement", new OperationMovementStrategy()).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Game.Queue", new QueueStrategy(queue)).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Config.Properties", new ConfigPropertiesStrategy(new List<string> { "Game.Command.Move" })).Execute();

        IoC.Resolve<ICommand>("Game.UObject.SetProperty", uObject, "Position", new Vector(0, 0)).Execute();

        startMovementCommand.Execute();

        var cmd = IoC.Resolve<ICommand>("Game.UObject.GetProperty", uObject, "MovementCommand");
        Assert.Equal(typeof(MacroCommand), cmd.GetType());

        queue.Pop().Execute();
        queue.Pop().Execute();
        queue.Pop().Execute();

        Assert.Equal(typeof(GetterCommand), queue.Pop().GetType());
    }

    [Fact]
    public void TestCreateAdapter()
    {
        IoC.Resolve<ICommand>("IoC.Register", "Game.Adapter", new AdapterStrategy()).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Game.CreateUObject", new CreateNewObjectStrategy()).Execute();

        IUObject uobj = IoC.Resolve<IUObject>("Game.CreateUObject");
        IMovable adapter = IoC.Resolve<IMovable>("Game.Adapter", uobj, typeof(MovableAdapter));

        Assert.Equal(typeof(MovableAdapter), adapter.GetType());
    }

    [Fact]
    public void IoCThrowsResolveDependencyException()
    {
        IoC.Resolve<ICommand>("IoC.Register", "ResolveDependencyException", new ThrowResolveDependencyExceptionStrategy()).Execute();

        Assert.Throws<ResolveDependencyException>(() => IoC.Resolve<ICommand>("ResolveDependencyException"));
        Assert.Throws<ResolveDependencyException>(() => IoC.Resolve<ICommand>("NaN"));
    }
}
