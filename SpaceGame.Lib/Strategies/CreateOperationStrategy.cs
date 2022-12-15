namespace SpaceGame.Lib;
using Hwdtech;

public class CreateOperationStrategy : IStrategy  // "Game.Operation.Create"
{
    public object Invoke(params object[] args)
    {
        string name = (string)args[0];
        IUObject obj = (IUObject)args[1];

        ICommand command = IoC.Resolve<ICommand>("Game.Command.Macro.Create", name, obj);

        List<ICommand> commands = new List<ICommand> { command };

        ICommand macroCommand = IoC.Resolve<ICommand>("Game.Command.Macro", commands);

        ICommand injectCommand = IoC.Resolve<ICommand>("Game.Command.Inject", macroCommand);

        ICommand repeatCommand = IoC.Resolve<ICommand>("Game.Command.Repeat", injectCommand);
        commands.Add(repeatCommand);

        return injectCommand;
    }
}
