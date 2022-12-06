namespace SpaceGame.Lib;
using Hwdtech;

public class OperationMovementStrategy : IStrategy  // "Game.Operation.Movement"
{
    public object Invoke(params object[] args)
    {
        IUObject uObject = (IUObject)args[0];

        List<ICommand> commands = new List<ICommand> { };

        IoC.Resolve<List<string>>("Config.Commands").ForEach(commandName =>
            commands.Add(IoC.Resolve<ICommand>(commandName, uObject))
        );

        ICommand macroCommand = IoC.Resolve<ICommand>("Game.Command.Macro", commands);

        ICommand injectCommand = IoC.Resolve<ICommand>("Game.Command.Inject", macroCommand);

        ICommand repeatCommand = IoC.Resolve<ICommand>("Game.Command.Repeat", injectCommand);
        commands.Add(repeatCommand);

        return injectCommand;
    }
}
