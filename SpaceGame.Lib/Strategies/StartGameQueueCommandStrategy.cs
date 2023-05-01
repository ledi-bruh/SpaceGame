namespace SpaceGame.Lib;
using Hwdtech;

public class StartGameQueueCommandStrategy : IStrategy  // "Game.Command.Macro.Create"
{
    public object Invoke(params object[] args)
    {
        var queue = (Queue<ICommand>)args[0];

        var quantum = (double)args[1];

        return new StartGameQueueCommand(queue, quantum);
    }
}
