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
        var adapterName = IoC.Resolve<string>("Game.Adapter.Name.Create", _targetType);

        gameAdapterMap.Add(pair, adapterName);
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", adapterName,
            (object[] args) => {
                var adapterClassType = IoC.Resolve<Assembly>("Assembly").GetType(adapterName);
                return Activator.CreateInstance(adapterClassType!, args[0]);
            }
        ).Execute();
    }
}
