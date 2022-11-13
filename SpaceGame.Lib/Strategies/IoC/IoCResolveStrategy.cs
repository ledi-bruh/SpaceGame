namespace SpaceGame.Lib;

public class IoCResolveStrategy : IStrategy  // "IoC.Resolve"
{
    private IDictionary<string, IStrategy> _store;

    public IoCResolveStrategy(IDictionary<string, IStrategy> store) => _store = store;

    public object Invoke(params object[] args)
    {
        // try catch
        string key = (string)args[0];
        object[] args_to_invoke = (object[])args[1];

        return _store[key].Invoke(args_to_invoke);
    }
}
