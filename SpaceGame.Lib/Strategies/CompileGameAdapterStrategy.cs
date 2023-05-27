namespace SpaceGame.Lib;

public class CompileGameAdapterStrategy : IStrategy  // "Game.Adapter.Compile"
{
    public object Invoke(params object[] args)
    {
        var objectType = (Type)args[0];
        var targetType = (Type)args[1];

        return new CompileGameAdapterCommand(objectType, targetType);
    }
}
