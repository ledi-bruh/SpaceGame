namespace SpaceGame.Lib;
using Hwdtech;
using System.Collections.Concurrent;

public class SendCommand : ICommand
{
    private int _id;
    private ICommand _message;

    public SendCommand(int id, ICommand message)
    {
        _id = id;
        _message = message;
    }

    public void Execute()
    {
        ISender? sender;
        if (IoC.Resolve<ConcurrentDictionary<int, ISender>>("Server.Thread.Sender.Map").TryGetValue(_id, out sender))
        {
            sender.Send(_message);
        }
        else
        {
            throw new Exception();
        }
    }
}
