namespace SpaceGame.Lib;

public class SoftStopServerStrategy : IStrategy  // "Server.Stop.Soft"
{
    public object Invoke(params object[] args)
    {
        return new SoftStopServerCommand();
    }
}
