using Hwdtech;

namespace SpaceGame.Lib;

public class CreateStartMovementCommandStrategy: IStrategy
{
    public object Invoke(params object[] args)
    {
        var obj = (IUObject)args[0];
        return new StartMovementCommand(IoC.Resolve<IStartable>("Game.UObject.Adapter.Create", obj, typeof(IStartable)));
    }
}
