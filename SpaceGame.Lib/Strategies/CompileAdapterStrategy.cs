namespace SpaceGame.Lib;
using Hwdtech;

public class CompileAdapterStrategy : IStrategy  // "Compile.Adapter"
{
    public object Invoke(params object[] args)
    {
        var targetType = (Type)args[0];

        // ! команда из 1 части
        return IoC.Resolve<ICommand>("Compile", IoC.Resolve<ICommand>("Game.Adapter.Code", targetType));
    }
}
