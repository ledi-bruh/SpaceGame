namespace SpaceGame.Lib;
using Hwdtech;
using System.Collections.Concurrent;


public class SoftStopServerStrategy : IStrategy //Server.Stop.Soft
{
    public object Invoke(params object[] args)
    {
        int id = (int)args[0];
        Action action;
        if (args[1] is null)
        {
            action = () => { };
        }
        else
        {
            action = (Action)args[1];
        }

        ServerThread? thread;
        if (IoC.Resolve<ConcurrentDictionary<int, ServerThread>>("Server.Thread.Map").TryGetValue(id, out thread))
        {
            var cmd = new SoftStopServerCommand(thread, action);
            return new SendCommand(id, new ActionCommand(() =>
            {
                cmd.Execute();
            }));
        }
        throw new Exception();
    }
}
