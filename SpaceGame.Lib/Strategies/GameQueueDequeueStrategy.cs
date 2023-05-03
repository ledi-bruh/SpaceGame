namespace SpaceGame.Lib;


public class GameQueueDequeueStrategy : IStrategy  // "Game.Queue.Dequeue"
{
    public object Invoke(params object[] args)
    {
        var queue = (Queue<ICommand>)args[0];
        if(queue.TryDequeue(out ICommand? cmd))
        {
            return cmd;
        }
        throw new Exception();
    }
}
