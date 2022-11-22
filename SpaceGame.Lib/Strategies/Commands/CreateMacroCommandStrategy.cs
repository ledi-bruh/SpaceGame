namespace SpaceGame.Lib;

public class CreateMacroCommandStrategy : IStrategy  // "Game.Command.Macro"
{
    public object Invoke(params object[] args)
    {
        IEnumerable<ICommand> commands = (IEnumerable<ICommand>)args[0];

        return new MacroCommand(commands);
    }
}
