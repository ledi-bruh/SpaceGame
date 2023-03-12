namespace SpaceGame.Lib;

public class StartServerStrategy : IStrategy  // "Server.Start"
{
    public object Invoke(params object[] args)
    {
        var threadCount = (int)args[0];

        return new StartServerCommand(threadCount);
    }
}
