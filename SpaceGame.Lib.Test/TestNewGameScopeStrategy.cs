namespace SpaceGame.Lib.Test;
using Hwdtech;
using Hwdtech.Ioc;

public class TestNewGameScopeStrategy
{
    public TestNewGameScopeStrategy()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        IoC.Resolve<ICommand>("Scopes.Current.Set",
            IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))
        ).Execute();
    }

    [Fact]
    public void SuccessfulCreatingNewGameScope()
    {
        var gameScopeMap = new Dictionary<string, object>();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Scope.New",
            (object[] args) => new NewGameScopeStrategy().Invoke(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Scope.Map",
            (object[] args) => gameScopeMap).Execute();

        var parentScope = IoC.Resolve<object>("Scopes.Current");
        var scope = IoC.Resolve<object>("Game.Scope.New", "gameId", parentScope, 400d);

        Assert.Throws<ArgumentException>(() => IoC.Resolve<object>("Game.Get.Time.Quantum"));

        IoC.Resolve<ICommand>("Scopes.Current.Set", scope).Execute();
        try
        {
            var quantum = IoC.Resolve<object>("Game.Get.Time.Quantum");
            Assert.Equal(400d, quantum);
        }
        catch (Exception e) { Assert.Fail(e.Message); }

        IoC.Resolve<ICommand>("Scopes.Current.Set", parentScope).Execute();
        Assert.Throws<ArgumentException>(() => IoC.Resolve<object>("Game.Get.Time.Quantum"));
    }
}
