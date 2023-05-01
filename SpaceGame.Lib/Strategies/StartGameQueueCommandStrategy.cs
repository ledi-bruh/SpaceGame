namespace SpaceGame.Lib;

public class StartGameQueueCommandStrategy : IStrategy  // "Game.Command.Queue.Start"
{
    public object Invoke(params object[] args)
    {
        var queue = (Queue<ICommand>)args[0];

        var quantum = (int)args[1];

        return new StartGameQueueCommand(queue, quantum);
    }
}
