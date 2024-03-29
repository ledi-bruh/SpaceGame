using Hwdtech;

namespace SpaceGame.Lib;

public class CreateEndMovementCommandStrategy: IStrategy
{
    public object Invoke(params object[] args)
    {
        var obj = (IUObject)args[0];
        return new EndMovementCommand(IoC.Resolve<IEndable>("Game.UObject.Adapter.Create", obj, typeof(IEndable)));
    }
}
