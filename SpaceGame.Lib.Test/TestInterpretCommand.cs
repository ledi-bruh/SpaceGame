namespace SpaceGame.Lib.Test;
using Hwdtech;
using Hwdtech.Ioc;
using Moq;


public class TestInterpretCommand
{
    Dictionary<int, Queue<SpaceGame.Lib.ICommand>> gameQueueMap = new Dictionary<int, Queue<SpaceGame.Lib.ICommand>>();
    Dictionary<int, IUObject> gameUObjectMap = new Dictionary<int, IUObject>();

    public TestInterpretCommand()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        Mock<IUObject> mockUObject = new Mock<IUObject>();
        mockUObject.Setup(x => x.SetProperty(It.IsAny<string>(), It.IsAny<object>()));

        gameQueueMap.Add(1, new Queue<SpaceGame.Lib.ICommand>());

        gameUObjectMap.Add(1, mockUObject.Object);

        IoC.Resolve<ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Game.Queue.Map", (object[] args) => gameQueueMap).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Game.UObject.Map", (object[] args) => gameUObjectMap).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Game.Get.Queue", (object[] args) => new GetGameQueueStrategy().Invoke(args)).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Game.Get.UObject", (object[] args) => new GetGameUObjectStrategy().Invoke(args)).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Game.Command.Create.FromMessage", (object[] args) => new CreateGameCommandFromMessageStrategy().Invoke(args)).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Game.Queue.Push", (object[] args) => new GameQueuePushCommandStrategy().Invoke(args)).Execute();
    }

    [Fact]
    public void SuccessfulPush()
    {
        Mock<SpaceGame.Lib.ICommand> mockCommand = new Mock<SpaceGame.Lib.ICommand>();

        Mock<IInterpretingMessage> mockMessage = new Mock<IInterpretingMessage>();
        mockMessage.SetupGet(x => x.GameID).Returns(1);
        mockMessage.SetupGet(x => x.TypeCommand).Returns("Test");
        mockMessage.SetupGet(x => x.Parameters).Returns(new Dictionary<string, object> { { "Test", 1 } });
        mockMessage.SetupGet(x => x.ObjectID).Returns(1);

        IoC.Resolve<ICommand>("IoC.Register", "Game.Command.Test", (object[] args) => mockCommand.Object).Execute();

        var intepretcmd = new InterpretCommand(mockMessage.Object);
        intepretcmd.Execute();

        Assert.True(gameQueueMap[1].Count() == 1);
    }

    [Fact]
    public void GetGameQueueThrowsException()
    {
        Mock<SpaceGame.Lib.ICommand> mockCommand = new Mock<SpaceGame.Lib.ICommand>();

        Mock<IUObject> mockUObject = new Mock<IUObject>();
        mockUObject.Setup(x => x.SetProperty(It.IsAny<string>(), It.IsAny<object>())).Verifiable();

        Mock<IInterpretingMessage> mockMessage = new Mock<IInterpretingMessage>();
        mockMessage.SetupGet(x => x.GameID).Returns(22);
        mockMessage.SetupGet(x => x.TypeCommand).Returns("Test");
        mockMessage.SetupGet(x => x.Parameters).Returns(new Dictionary<string, object> { { "Test", 1 } });
        mockMessage.SetupGet(x => x.ObjectID).Returns(1);

        IoC.Resolve<ICommand>("IoC.Register", "Game.Command.Test", (object[] args) => mockCommand.Object).Execute();

        var intepretcmd = new InterpretCommand(mockMessage.Object);
        Assert.Throws<Exception>(() => { intepretcmd.Execute(); });
    }

    [Fact]
    public void GetGameUObjectThrowsException()
    {
        Mock<SpaceGame.Lib.ICommand> mockCommand = new Mock<SpaceGame.Lib.ICommand>();

        Mock<IUObject> mockUObject = new Mock<IUObject>();
        mockUObject.Setup(x => x.SetProperty(It.IsAny<string>(), It.IsAny<object>())).Verifiable();

        Mock<IInterpretingMessage> mockMessage = new Mock<IInterpretingMessage>();
        mockMessage.SetupGet(x => x.GameID).Returns(1);
        mockMessage.SetupGet(x => x.TypeCommand).Returns("Test");
        mockMessage.SetupGet(x => x.Parameters).Returns(new Dictionary<string, object> { { "Test", 1 } });
        mockMessage.SetupGet(x => x.ObjectID).Returns(22);

        IoC.Resolve<ICommand>("IoC.Register", "Game.Command.Test", (object[] args) => mockCommand.Object).Execute();

        var intepretcmd = new InterpretCommand(mockMessage.Object);
        Assert.Throws<Exception>(() => { intepretcmd.Execute(); });
    }

    [Fact]
    public void GetPropertyFromMessageThrowsException()
    {
        Mock<SpaceGame.Lib.ICommand> mockCommand = new Mock<SpaceGame.Lib.ICommand>();

        Mock<IUObject> mockUObject = new Mock<IUObject>();
        mockUObject.Setup(x => x.SetProperty(It.IsAny<string>(), It.IsAny<object>())).Verifiable();

        Mock<IInterpretingMessage> mockMessage = new Mock<IInterpretingMessage>();
        mockMessage.SetupGet(x => x.GameID).Throws(new Exception());
        mockMessage.SetupGet(x => x.TypeCommand).Returns("Test");
        mockMessage.SetupGet(x => x.Parameters).Returns(new Dictionary<string, object> { { "Test", 1 } });
        mockMessage.SetupGet(x => x.ObjectID).Returns(1);

        IoC.Resolve<ICommand>("IoC.Register", "Game.Command.Test", (object[] args) => mockCommand.Object).Execute();

        var intepretcmd = new InterpretCommand(mockMessage.Object);
        Assert.Throws<Exception>(() => { intepretcmd.Execute(); });
    }
}
