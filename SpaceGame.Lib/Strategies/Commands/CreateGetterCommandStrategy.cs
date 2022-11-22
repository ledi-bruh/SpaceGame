namespace SpaceGame.Lib;

public class CreateGetterCommandStrategy : IStrategy  // "Game.Command.Getter"
{
    public object Invoke(params object[] args)
    {
        IUObject uObject = (IUObject)args[0];
        string commandName = (string)args[1];

        return new GetterCommand(uObject, commandName);
    }
}
