namespace SpaceGame.Lib;


public class SendCommandStrategy : IStrategy
{
    public object Invoke(params object[] args) //Server.Thread.Command.Send
    {
        var id = (int)args[0];
        var message = (ICommand)args[1];
        return new SendCommand(id, message);
    }
}
