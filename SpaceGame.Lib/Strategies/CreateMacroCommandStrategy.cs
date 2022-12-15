namespace SpaceGame.Lib;
using Hwdtech;

public class CreateMacroCommandStrategy : IStrategy  // "Game.Command.Macro.Create"
{
    public object Invoke(params object[] args)
    {
        string operationName = (string)args[0];
        IUObject uObject = (IUObject)args[1];

        List<ICommand> commands = new List<ICommand> { };
        IoC.Resolve<List<string>>("Config." + operationName).ForEach(commandName =>
            commands.Add(IoC.Resolve<ICommand>(commandName, uObject))
        );

        return IoC.Resolve<ICommand>("Game.Command.Macro", commands);
    }
}
