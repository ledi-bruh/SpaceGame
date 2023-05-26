namespace SpaceGame.Lib;

using Hwdtech;



public class CreateGameAdapterCodeStrategy : IStrategy //"Game.Adapter.Code"
{
    public object Invoke(params object[] args)
    {
        var typeOld = (Type)args[0];
        var typeNew = (Type)args[1];

        var bulider = IoC.Resolve<IBulider>("Game.Bulider.Adapter", typeOld, typeNew);
        return bulider.Bulid();
    }
}
