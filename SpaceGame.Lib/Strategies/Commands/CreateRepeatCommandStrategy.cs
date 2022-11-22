namespace SpaceGame.Lib;

public class CreateRepeatCommandStrategy : IStrategy  // "Game.Command.Repeat"
{
    public object Invoke(params object[] args)
    {
        ICommand command = (ICommand)args[0];

        return new RepeatCommand(command);
    }
}
