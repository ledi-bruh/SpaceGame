namespace SpaceGame.Lib;
using Hwdtech;
using System.Collections.Concurrent;


public class RegisterThreadStrategy : IStrategy  //"Server.Register.Thread"
{
    public object Invoke(params object[] args)
    {
        int id = (int)args[0];
        IReceiver receiver = (IReceiver)args[1];
        return new ActionCommand(() =>
        {
            var thread_dictionary = IoC.Resolve<ConcurrentDictionary<int, ServerThread>>("Server.Thread.Map");
            thread_dictionary[id] = new ServerThread(receiver);
        });
    }
}
