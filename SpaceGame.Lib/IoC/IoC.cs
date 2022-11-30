namespace SpaceGame.Lib;

public class IoC
{
    private static IDictionary<string, IStrategy> _store;

    static IoC()
    {
        _store = new Dictionary<string, IStrategy>();
        _store["Ioc.Resolve"] = new IoCResolveStrategy(_store);
        _store["IoC.Register"] = new IoCRegisterStrategy(_store);
    }
    
    public static T Resolve<T>(string key, params object[] args)
    {
        try
        {
            return (T)_store["Ioc.Resolve"].Invoke(key, args);
        }
        catch (ResolveDependencyException rde)
        {
            throw rde;
        }
        catch (Exception e)
        {
            throw new ResolveDependencyException(e.Message);
        }
    }
}
