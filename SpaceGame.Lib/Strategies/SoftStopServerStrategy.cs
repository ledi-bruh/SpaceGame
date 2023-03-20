namespace SpaceGame.Lib;
using Hwdtech;
using System.Collections.Concurrent;


public class SoftStopServerStrategy : IStrategy //Server.Stop.Soft
{
    public object Invoke(params object[] args)
    {
        Action action = () => { };
        int id = (int)args[0];
        if (args.Length == 2)
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
