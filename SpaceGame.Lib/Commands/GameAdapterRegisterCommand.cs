namespace SpaceGame.Lib;
using System.Reflection;
using Hwdtech;

public class GameAdapterRegisterCommand : ICommand
{
    private IUObject _uObject;
    private Type _targetType;

    public GameAdapterRegisterCommand(IUObject uObject, Type targetType)
    {
        _uObject = uObject;
        _targetType = targetType;
    }

    public void Execute()
    {
        var gameAdapterMap = IoC.Resolve<IDictionary<KeyValuePair<Type, Type>, string>>("Game.Adapter.Map");
        var pair = new KeyValuePair<Type, Type>(_uObject.GetType(), _targetType);
        var adapterStrategyName = _targetType.ToString() + "Adapter";

        gameAdapterMap.Add(pair, adapterStrategyName);
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", adapterStrategyName,
            (object[] args) => {
                var assembly = Assembly.Load("SpaceGame.Lib");
                var adapterClassType = assembly.GetType(adapterStrategyName);
                return Activator.CreateInstance(adapterClassType!, args[0]);
            }
        ).Execute();
    }
}
