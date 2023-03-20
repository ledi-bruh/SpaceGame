namespace SpaceGame.Lib;
using Hwdtech;
using System.Collections.Concurrent;

public class RegisterThreadCommand : ICommand //Server.Thread.Register
{
    int _id;
    IReceiver _receiver;

    public RegisterThreadCommand(int id, IReceiver receiver)
    {
        _id = id;
        _receiver = receiver;
    }
    public void Execute()
    {
        var thread_dictionary = IoC.Resolve<ConcurrentDictionary<int, ServerThread>>("Server.Thread.Map");
        thread_dictionary[_id] = new ServerThread(_receiver);
    }
}
