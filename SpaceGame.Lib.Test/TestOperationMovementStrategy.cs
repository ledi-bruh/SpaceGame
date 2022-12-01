namespace SpaceGame.Lib.Test;
using Moq;

public class TestOperaionMovementStrategy
{
    public TestOperaionMovementStrategy()
    {
        var mockCommand = new Mock<ICommand>();
        mockCommand.Setup(x => x.Execute());

        var mockStrategyWithParams = new Mock<IStrategy>();
        mockStrategyWithParams.Setup(x => x.Invoke(It.IsAny<object[]>())).Returns(mockCommand.Object);

        var mockConfigPropertiesStrategy = new Mock<IStrategy>();
        mockConfigPropertiesStrategy.Setup(x => x.Invoke()).Returns(new List<string>() {"Game.Command.Move"});

        IoC.Resolve<ICommand>("IoC.Register", "Game.Command.Move", mockStrategyWithParams.Object).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Game.Command.Macro", mockStrategyWithParams.Object).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Game.Command.Inject", mockStrategyWithParams.Object).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Game.Command.Repeat", mockStrategyWithParams.Object).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Config.Commands", mockConfigPropertiesStrategy.Object).Execute();
    }

    [Fact]
    public void SuccesfullOperationMovement()
    {
        var mockUObject = new Mock<IUObject>();
        var operationMovementStrategy = new OperationMovementStrategy();
        
        Assert.NotNull(operationMovementStrategy.Invoke(mockUObject.Object));
    }
}
