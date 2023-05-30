namespace SpaceGame.Lib;
using System.Reflection;
using Hwdtech;

public class CompileGameAdapterCommand : ICommand
{
    private Type _objectType;
    private Type _targetType;

    public CompileGameAdapterCommand(Type objectType, Type targetType)
    {
        _objectType = objectType;
        _targetType = targetType;
    }

    public void Execute()
    {
        var adapterCode = IoC.Resolve<string>("Game.Adapter.Code", _objectType, _targetType);
        var assembly = IoC.Resolve<Assembly>("Compile", adapterCode);

        var assemblyMap = IoC.Resolve<IDictionary<KeyValuePair<Type, Type>, Assembly>>("Game.Adapter.Assembly.Map");
        var pair = new KeyValuePair<Type, Type>(_objectType, _targetType);

        assemblyMap[pair] = assembly;
    }
}
