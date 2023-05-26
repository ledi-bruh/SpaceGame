namespace SpaceGame.Lib;
using Hwdtech;

public class GameAdapterMapDefaultStrategy : IStrategy  // "Game.Adapter.Map.Default"
{
    public object Invoke(params object[] args)
    {
        var uObject = (IUObject)args[0];
        var targetType = (Type)args[1];

        IoC.Resolve<ICommand>("Compile.Adapter", targetType).Execute();
        IoC.Resolve<ICommand>("Game.Adapter.Register", uObject, targetType).Execute();

        return IoC.Resolve<object>("Game.Adapter", uObject, targetType);
    }
}
