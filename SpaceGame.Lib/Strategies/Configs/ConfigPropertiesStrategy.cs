namespace SpaceGame.Lib;

public class ConfigPropertiesStrategy : IStrategy  // "Config.Properties"
{
    private IEnumerable<string> _store;

    public ConfigPropertiesStrategy(IEnumerable<string> store) => _store = store;

    public object Invoke(params object[] args)
    {
        return _store;
    }
}
