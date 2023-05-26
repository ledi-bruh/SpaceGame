namespace SpaceGame.Lib;
using Hwdtech;

public class GameAdapterMapDefaultStrategy : IStrategy  // "Game.Adapter.Map.Default"
{
    public object Invoke(params object[] args)
    {
        var uObject = (IUObject)args[0];
        var targetType = (Type)args[1];

        var gameAdapterMap = IoC.Resolve<IDictionary<KeyValuePair<Type, Type>, string>>("Game.Adapter.Map");
        var pair = new KeyValuePair<Type, Type>(uObject.GetType(), targetType);


        var codeString = IoC.Resolve<ICommand>("Game.Adapter.Code", targetType);  // ! команда из 1 части
        IoC.Resolve<ICommand>("Compile", codeString).Execute();

        var adapterStrategyName = targetType.ToString() + "Adapter";
        IoC.Resolve<Hwdtech.ICommand>("Register", adapterStrategyName, (object[] args) => Activator.CreateInstance((Type)args[1], args[0])).Execute();
        gameAdapterMap.Add(pair, adapterStrategyName);

        return Activator.CreateInstance(targetType, uObject)!;
    }
}
