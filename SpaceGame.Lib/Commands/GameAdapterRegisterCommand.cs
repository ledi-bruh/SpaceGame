namespace SpaceGame.Lib;
using Hwdtech;

public class GameAdapterRegisterCommand : ICommand
{
    IUObject _uObject;
    Type _targetType;

    public GameAdapterRegisterCommand(IUObject uObject, Type targetType)
    {
        _uObject = uObject;
        _targetType = targetType;
    }

    public void Execute()
    {
        var adapterStrategyName = _targetType.ToString() + "Adapter";
        IoC.Resolve<Hwdtech.ICommand>("Register", adapterStrategyName, (object[] args) => Activator.CreateInstance((Type)args[1], args[0])).Execute();

        var gameAdapterMap = IoC.Resolve<IDictionary<KeyValuePair<Type, Type>, string>>("Game.Adapter.Map");
        var pair = new KeyValuePair<Type, Type>(_uObject.GetType(), _targetType);
        gameAdapterMap.Add(pair, adapterStrategyName);
    }
}
