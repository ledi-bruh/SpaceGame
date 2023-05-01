namespace SpaceGame.Lib;
using Hwdtech;

public class GameQueuePopStrategy : IStrategy  // "Game.Queue.Pop"
{
    public object Invoke(params object[] args)
    {
        int gameQueueId = (int)args[0];

        var queue = IoC.Resolve<Queue<ICommand>>("Game.Get.Queue", gameQueueId);

        return new ActionCommand(() => { queue.Dequeue(); });
    }
}
