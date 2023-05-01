namespace SpaceGame.Lib;


public class GameQueueDequeueStrategy : IStrategy  // "Game.Command.Create.FromMessage"
{
    public object Invoke(params object[] args)
    {
        var queue = (Queue<ICommand>)args[0];

        return queue.Dequeue();
    }
}
