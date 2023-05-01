namespace SpaceGame.Lib;
using Hwdtech;

public class NewGameScopeStrategy : IStrategy  // "Game.Scope.New"
{
    public object Invoke(params object[] args)
    {
        string gameId = (string)args[0];
        var parentScope = (object)args[1];
        var quantum = (double)args[2];

        var gameScope = IoC.Resolve<object>("Scopes.New", parentScope);

        var gameScopeMap = IoC.Resolve<IDictionary<string, object>>("Game.Scope.Map");
        gameScopeMap.Add(gameId, gameScope);

        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", gameScope).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Get.Time.Quantum",
            (object[] args) => quantum
        ).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Queue.Push",
            (object[] args) => new GameQueuePushCommandStrategy().Invoke(args)
        ).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Queue.Pop",
            (object[] args) => new GameQueuePopStrategy().Invoke(args)
        ).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Get.UObject",
            (object[] args) => new GetGameUObjectStrategy().Invoke(args)
        ).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Delete.UObject",
            (object[] args) => new DeleteGameUObjectStrategy().Invoke(args)
        ).Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", parentScope).Execute();

        return gameScope;
    }
}
