namespace SpaceGame.Lib;
using System.Reflection;
using Hwdtech;

public class CreateGameAdapterStrategy : IStrategy  // "Game.Adapter.Create"
{
    public object Invoke(params object[] args)
    {
        var uObject = (IUObject)args[0];
        var targetType = (Type)args[1];

        var adapterAssemblyMap = IoC.Resolve<IDictionary<KeyValuePair<Type, Type>, Assembly>>("Game.Adapter.Assembly.Map");
        var pair = new KeyValuePair<Type, Type>(uObject.GetType(), targetType);

        if (!adapterAssemblyMap.TryGetValue(pair, out var assembly))
        {
            IoC.Resolve<ICommand>("Game.Adapter.Compile", uObject.GetType(), targetType).Execute();
        }

        return IoC.Resolve<object>("Game.Adapter.Find", uObject, targetType);
    }
}
