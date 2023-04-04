namespace SpaceGame.Lib;
using Hwdtech;


public class GetGameQueueStrategy : IStrategy  // "Game.Get.Queue"
{
     public object Invoke(params object[] args)
    {
        int gameid = (int)args[0];

        Queue<ICommand>? queue;
        
        if (IoC.Resolve<IDictionary<int, Queue<ICommand>>>("Game.Queue.Map").TryGetValue(gameid, out queue))
        {
            return queue;
        }

        throw new Exception();
    }
}
