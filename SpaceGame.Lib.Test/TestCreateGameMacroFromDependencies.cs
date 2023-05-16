namespace SpaceGame.Lib.Test;
using Hwdtech;
using Hwdtech.Ioc;
using Moq;

public class TestCreateGameMacroFromDependencies
{
    public TestCreateGameMacroFromDependencies()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<ICommand>("Scopes.Current.Set",
            IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))
        ).Execute();
    }

    [Fact]
    public void SuccesfullCreateGameMacroTest()
    {
        IoC.Resolve<ICommand>("IoC.Register", "Game.Macro.Create.FromDependencies", (object[] args) => new CreateGameMacroFromDependenciesStrategy().Invoke(args)).Execute();

        List<string> testDependencies = new List<string>{"Test"};

        IoC.Resolve<ICommand>("IoC.Register", "Game.Dependencies.Get.Macro.Test", (object[] args) => testDependencies).Execute();
        
        var mockCmd = new Mock<SpaceGame.Lib.ICommand>();
        mockCmd.Setup(x => x.Execute()).Verifiable();
        var mockIUObject = new Mock<IUObject>();

        IoC.Resolve<ICommand>("IoC.Register", "Game.Command.Test", (object[] args) => mockCmd.Object).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Game.Command.Macro", (object[] args) => new MacroCommand((IEnumerable<SpaceGame.Lib.ICommand>)args[0])).Execute();

        var macroCmd = IoC.Resolve<SpaceGame.Lib.ICommand>("Game.Macro.Create.FromDependencies", mockIUObject.Object, "Test");
        macroCmd.Execute();

        mockCmd.Verify(x => x.Execute(), Times.Once);
    }
}
