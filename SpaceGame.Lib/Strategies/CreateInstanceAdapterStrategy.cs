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
            IoC.Resolve<ICommand>("Compile.Adapter", targetType).Execute();
            IoC.Resolve<ICommand>("Game.Adapter.Register", uObject, targetType).Execute();
        }

        return IoC.Resolve<object>(gameAdapterMap[pair], uObject, targetType);
    }
}
