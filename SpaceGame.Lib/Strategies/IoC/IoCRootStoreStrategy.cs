namespace SpaceGame.Lib;

public class IoCRootStoreStrategy : IStrategy  // "IoC.Root.Store"
{
    private IDictionary<string, IStrategy> _store;

    public IoCRootStoreStrategy(IDictionary<string, IStrategy> store) => _store = store;

    public object Invoke(params object[] args)
    {
        // try catch
        return _store;
    }
}
