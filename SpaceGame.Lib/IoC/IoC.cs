namespace SpaceGame.Lib;

public static class IoC
{
    private static IDictionary<string, IStrategy> _store;

    static IoC()
    {
        _store = new Dictionary<string, IStrategy>();
        _store["IoC.Register"] = new IoCRegisterStrategy(_store);
        //? "IoC.Resolve"
        //? "IoC.Root"
    }

    public static T Resolve<T>(string key, params object[] args)
    {
        return (T)_store[key].Invoke(args);
        // try catch
    }
}
