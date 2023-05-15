using Hwdtech;

namespace SpaceGame.Lib;

public class CreateShootCommandStrategy: IStrategy
{
    public object Invoke(params object[] args)
    {
        var obj = (IUObject)args[0];
        return new ShootCommand(IoC.Resolve<IShootable>("Game.Adapter.Create", obj, typeof(IShootable)));
    }
}
