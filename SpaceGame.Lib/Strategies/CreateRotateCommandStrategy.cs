using Hwdtech;

namespace SpaceGame.Lib;

public class CreateRotateCommandStrategy: IStrategy
{
    public object Invoke(params object[] args)
    {
        var obj = (IUObject)args[0];
        return new RotateCommand(IoC.Resolve<IRotatable>("Game.UObject.Adapter.Create", obj, typeof(IRotatable)));
    }
}
