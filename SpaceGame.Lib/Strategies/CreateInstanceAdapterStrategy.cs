namespace SpaceGame.Lib;
using Hwdtech;

public class CreateInstanceAdapterStrategy : IStrategy  // "Game.Adapter"
{
    public object Invoke(params object[] args)
    {
        var uObject = (IUObject)args[0];
        var targetType = (Type)args[1];

        var gameAdapterMap = IoC.Resolve<IDictionary<KeyValuePair<Type, Type>, string>>("Game.Adapter.Map");
        var pair = new KeyValuePair<Type, Type>(uObject.GetType(), targetType);

        if (!gameAdapterMap.TryGetValue(pair, out var adapterStrategyName))
        {
            return IoC.Resolve<object>("Game.Adapter.Map.Default", uObject, targetType);
        }

        return IoC.Resolve<object>(adapterStrategyName, uObject, targetType);
    }
}
