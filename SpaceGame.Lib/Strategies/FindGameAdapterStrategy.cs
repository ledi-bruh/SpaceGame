namespace SpaceGame.Lib;
using System.Reflection;
using Hwdtech;

public class FindGameAdapterStrategy : IStrategy  // "Game.Adapter.Find"
{
    public object Invoke(params object[] args)
    {
        var uObject = (IUObject)args[0];
        var targetType = (Type)args[1];

        var assemblyMap = IoC.Resolve<IDictionary<KeyValuePair<Type, Type>, Assembly>>("Game.Adapter.Assembly.Map");
        var pair = new KeyValuePair<Type, Type>(uObject.GetType(), targetType);

        var assembly = assemblyMap[pair];
        var type = assembly.GetType(IoC.Resolve<string>("Game.Adapter.Name.Create", targetType))!;

        return Activator.CreateInstance(type, uObject)!;
    }
}
