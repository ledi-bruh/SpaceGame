namespace SpaceGame.Lib;


public class RegisterThreadSenderStrategy : IStrategy  //"Server.Register.Sender"
{
    public object Invoke(params object[] args)
    {
        int id = (int)args[0];
        ISender sender = (ISender)args[1];
        return new RegisterThreadSenderCommand(id, sender);
    }
}
