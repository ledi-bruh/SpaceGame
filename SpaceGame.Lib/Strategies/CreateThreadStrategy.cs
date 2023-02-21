namespace SpaceGame.Lib;


public class CreateThreadStrategy : IStrategy
{
    public object Invoke(params object[] args)
    {
        IReceiver receiver = (IReceiver)args[1];
        return new ServerThread(receiver);
    }
}
