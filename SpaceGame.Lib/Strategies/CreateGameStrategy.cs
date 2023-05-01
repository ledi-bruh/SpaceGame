namespace SpaceGame.Lib;
using Hwdtech;

public class CreateGameStrategy : IStrategy  // "Game.Create"
{
    public object Invoke(params object[] args)
    {
        string gameId = (string)args[0];
        var parentScope = (object)args[1];
        var quantum = (double)args[2];

        var gameScope = IoC.Resolve<object>("Game.Scope.New", gameId, parentScope, quantum);
        var gameQueue = IoC.Resolve<object>("Game.Queue.New");
        var gameCommand = IoC.Resolve<ICommand>("Game.Command", gameQueue, gameScope);

        var commandsList = new List<ICommand> { gameCommand };
        var macroCommand = IoC.Resolve<ICommand>("Game.Command.Macro", commandsList);
        var injectCommand = IoC.Resolve<ICommand>("Game.Command.Inject", macroCommand);
        var repeatCommand = IoC.Resolve<ICommand>("Game.Command.Repeat", injectCommand);
        commandsList.Add(repeatCommand);

        var gameMap = IoC.Resolve<IDictionary<string, ICommand>>("Game.Map");
        gameMap.Add(gameId, injectCommand);

        return injectCommand;
    }
}
