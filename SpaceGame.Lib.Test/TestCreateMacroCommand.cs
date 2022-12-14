namespace SpaceGame.Lib.Test;
using Moq;
using Hwdtech;
using Hwdtech.Ioc;

public class TestCreateMacroCommand
{
    public TestCreateMacroCommand()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        IoC.Resolve<ICommand>("Scopes.Current.Set",
            IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))
        ).Execute();
    }

    [Fact]
    public void SuccessfullCreateMacroCommand()
    {
        var mockCommand = new Mock<SpaceGame.Lib.ICommand>();
        mockCommand.Setup(x => x.Execute()).Verifiable();

        string operationName = "Movement";
        var mockUObject = new Mock<IUObject>();

        IoC.Resolve<ICommand>("IoC.Register", "Config." + operationName, (object[] args) => new List<string> { "Game.Command.Move" }).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Game.Command.Macro.Create",
            (object[] args) => new CreateMacroCommandStrategy().Invoke(args[0], args[1])
        ).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Game.Command.Move", (object[] args) => mockCommand.Object).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Game.Command.Macro", (object[] args) => mockCommand.Object).Execute();

        IoC.Resolve<SpaceGame.Lib.ICommand>("Game.Command.Macro.Create", operationName, mockUObject.Object).Execute();

        mockCommand.VerifyAll();
    }

    [Fact]
    public void SuccessfullWorkingMacroCommand()
    {
        var mockCommand = new Mock<SpaceGame.Lib.ICommand>();
        mockCommand.Setup(x => x.Execute()).Verifiable();

        new MacroCommand(
            new List<SpaceGame.Lib.ICommand> { mockCommand.Object, mockCommand.Object, mockCommand.Object }
        ).Execute();

        mockCommand.Verify(x => x.Execute(), Times.Exactly(3));
    }

    [Fact]
    public void TryExecuteCommandsInMacroCommandThrowException()
    {
        var mockCommand = new Mock<SpaceGame.Lib.ICommand>();
        mockCommand.Setup(x => x.Execute()).Throws(new Exception());

        var commands = new List<SpaceGame.Lib.ICommand> { mockCommand.Object };

        Assert.Throws<Exception>(() => new MacroCommand(commands).Execute());
    }
}
