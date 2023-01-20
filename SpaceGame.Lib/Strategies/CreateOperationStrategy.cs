namespace SpaceGame.Lib;
using Hwdtech;

public class CreateOperationStrategy : IStrategy  // "Game.Operation." + _name
{
    string _name;

    public CreateOperationStrategy(string name) => _name = name;

    public object Invoke(params object[] args)
    {
        IUObject obj = (IUObject)args[0];

        ICommand command = IoC.Resolve<ICommand>("Game.Command." + _name, obj);

        List<ICommand> commandsList = new List<ICommand> { command };

        ICommand macroCommand = IoC.Resolve<ICommand>("Game.Command.Macro", commandsList);

        ICommand injectCommand = IoC.Resolve<ICommand>("Game.Command.Inject", macroCommand);

        ICommand repeatCommand = IoC.Resolve<ICommand>("Game.Command.Repeat", injectCommand);
        commandsList.Add(repeatCommand);

        return injectCommand;
    }
}
