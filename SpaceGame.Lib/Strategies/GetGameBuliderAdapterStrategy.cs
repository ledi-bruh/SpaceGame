namespace SpaceGame.Lib;

using Hwdtech;


public class GetGameBuliderAdapterStrategy : IStrategy //"Game.Bulider.Adapter"
{
    public object Invoke(params object[] args)
    {
        var typeOld = (Type)args[0];
        var typeNew = (Type)args[1];

        return new AdapterBulider(typeOld, typeNew);
    }
}
