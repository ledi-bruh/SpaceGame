namespace SpaceGame.Lib;
using System.Collections.Concurrent;
using Hwdtech;

public class CreateAndStartServerStrategy : IStrategy  //"Server.Thread.Create.Start"
{
    public object Invoke(params object[] args)
    {

        Action action;
        int id = (int)args[0];
        if (args[1] is null)
        {
            action = () => { };
        }
        else
        {
            action = (Action)args[1];
        }
        var queue = new BlockingCollection<ICommand>();

        var sender = new SenderAdapter(queue);
        var register_sender_cmd = IoC.Resolve<ICommand>("Server.Register.Sender", id, sender);

        var receiver = new ReceiverAdapter(queue);
        var register_thread_cmd = IoC.Resolve<ICommand>("Server.Register.Thread", id, receiver);

        var thread = IoC.Resolve<Dictionary<int, ServerThread>>("Server.Thread.Map")[id];
        return new ActionCommand(() =>
        {
            register_sender_cmd.Execute();
            register_thread_cmd.Execute();
            thread.Start();
            action();
        });
        // Action<object> action = Action    
    }
}
