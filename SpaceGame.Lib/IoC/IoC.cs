namespace SpaceGame.Lib;

public static class IoC
{
    private static IDictionary<string, IStrategy> _store;

    static IoC()
    {
        _store = new Dictionary<string, IStrategy>();
        _store["IoC.Resolve"] = new IoCResolveStrategy(_store);
        _store["IoC.Register"] = new IoCRegisterStrategy(_store);
        _store["IoC.Root.Store"] = new IoCRootStoreStrategy(_store);
    }

    public static T Resolve<T>(string key, params object[] args)
    {
        try
        {
            return (T)_store["IoC.Resolve"].Invoke(key, args);
        }
        catch (ResolveDependencyException rde)
        {
            throw rde;
        }
        catch (Exception)
        {
            throw new ResolveDependencyException();
        }
    }
}
