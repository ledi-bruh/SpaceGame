namespace SpaceGame.Lib;

public class IoCRegisterStrategy : IStrategy  // "IoC.Register"
{
    private IDictionary<string, IStrategy> _store;

    public IoCRegisterStrategy(IDictionary<string, IStrategy> store) => _store = store;

    public object Invoke(params object[] args)
    {
        // try catch
        string key = (string)args[0];
        IStrategy strategy = (IStrategy)args[1];
        
        return new IoCRegisterCommand(_store, key, strategy);
    }
}
