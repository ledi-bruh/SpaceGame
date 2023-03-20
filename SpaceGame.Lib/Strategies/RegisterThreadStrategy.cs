namespace SpaceGame.Lib;


public class RegisterThreadStrategy : IStrategy  //"Server.Register.Thread"
{
    public object Invoke(params object[] args)
    {
        int id = (int)args[0];
        IReceiver receiver = (IReceiver)args[1];
        return new RegisterThreadCommand(id, receiver);
    }
}
