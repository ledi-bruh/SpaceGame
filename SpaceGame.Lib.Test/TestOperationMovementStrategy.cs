namespace SpaceGame.Lib.Test;
using Moq;
using Hwdtech;
using Hwdtech.Ioc;

public class TestOperaionMovementStrategy
{
    public TestOperaionMovementStrategy()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        IoC.Resolve<ICommand>("Scopes.Current.Set",
            IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))
        ).Execute();

        var mockCommand = new Mock<SpaceGame.Lib.ICommand>();
        mockCommand.Setup(x => x.Execute());

        var mockConfigCommandsStrategy = new Mock<IStrategy>();
        mockConfigCommandsStrategy.Setup(x => x.Invoke()).Returns(new List<string>() {"Game.Command.Move"});

        IoC.Resolve<ICommand>("IoC.Register", "Game.Command.Move", (object[] args) => mockCommand.Object).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Game.Command.Macro", (object[] args) => mockCommand.Object).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Game.Command.Inject", (object[] args) => mockCommand.Object).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Game.Command.Repeat", (object[] args) => mockCommand.Object).Execute();        
        IoC.Resolve<ICommand>("IoC.Register", "Config.Commands", (object[] args) => mockConfigCommandsStrategy.Object.Invoke(args)).Execute();
    }

    [Fact]
    public void SuccesfullOperationMovement()
    {
        var mockUObject = new Mock<IUObject>();
        var operationMovementStrategy = new OperationMovementStrategy();
        
        Assert.NotNull(operationMovementStrategy.Invoke(mockUObject.Object));
    }
}
