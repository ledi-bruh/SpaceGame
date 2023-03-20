namespace SpaceGame.Lib;
using Hwdtech;
using System.Collections.Concurrent;

public class RegisterThreadSenderCommand : ICommand //Server.Register.Thread.Sender
{
    int _id;
    ISender _sender;

    public RegisterThreadSenderCommand(int id, ISender sender)
    {
        _id = id;
        _sender = sender;
    }
    public void Execute()
    {
        var queue_dictionary = IoC.Resolve<ConcurrentDictionary<int, ISender>>("Server.Thread.Sender.Map");
        queue_dictionary[_id] = _sender;
    }
}
