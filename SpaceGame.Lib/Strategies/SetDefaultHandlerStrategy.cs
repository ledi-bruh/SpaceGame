namespace SpaceGame.Lib;

public class SetDefaultHandlerStrategy : IStrategy  // "Exception.Handler.Default.Set"
{
    public object Invoke(params object[] args)
    {
        var handler = (IHandler)args[0];
        
        return new SetDefaultHandlerCommand(handler);
    }
}
