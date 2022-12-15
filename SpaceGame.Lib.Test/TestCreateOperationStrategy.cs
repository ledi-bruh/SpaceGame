namespace SpaceGame.Lib.Test;
using Moq;
using Hwdtech;
using Hwdtech.Ioc;

public class TestCreateOperationStrategy
{
    public TestCreateOperationStrategy()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        IoC.Resolve<ICommand>("Scopes.Current.Set",
            IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))
        ).Execute();
        IoC.Resolve<ICommand>(
            "IoC.Register",
            "Game.Operation.Create",
            (object[] args) => new CreateOperationStrategy().Invoke(args)
        ).Execute();
    }

    [Fact]
    public void SuccessfullCreateOperationStrategy()
    {
        var mockCommand = new Mock<SpaceGame.Lib.ICommand>();
        mockCommand.Setup(x => x.Execute()).Verifiable();

        string operationName = "Movement";
        var mockUObject = new Mock<IUObject>();

        IoC.Resolve<ICommand>("IoC.Register", "Game.Command.Macro.Create", (object[] args) => mockCommand.Object).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Game.Command.Macro", (object[] args) => mockCommand.Object).Execute();
        IoC.Resolve<ICommand>(
            "IoC.Register",
            "Game.Command.Inject",
            (object[] args) => new InjectCommand((SpaceGame.Lib.ICommand)args[0])
        ).Execute();
        IoC.Resolve<ICommand>(
            "IoC.Register",
            "Game.Command.Repeat",
            (object[] args) => new RepeatCommand((SpaceGame.Lib.ICommand)args[0])
        ).Execute();

        IoC.Resolve<SpaceGame.Lib.ICommand>("Game.Operation.Create", operationName, mockUObject.Object).Execute();
        mockCommand.VerifyAll();
    }

    [Fact]
    public void RepeatCommandTest()
    {
        var mockCommand = new Mock<SpaceGame.Lib.ICommand>();

        var queue = new Queue<SpaceGame.Lib.ICommand>();

        var repeatCommand = new RepeatCommand(mockCommand.Object);

        IoC.Resolve<ICommand>(
            "IoC.Register",
            "Game.Queue",
            (object[] args) => queue
        ).Execute();

        repeatCommand.Execute();

        Assert.Equal(mockCommand.Object, queue.Dequeue());
    }
}
