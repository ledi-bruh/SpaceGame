namespace SpaceGame.Lib;

public class IoCRegisterCommand : ICommand
{
    private IDictionary<string, IStrategy> _store;
    private string _key;
    private IStrategy _strategy;

    public IoCRegisterCommand(IDictionary<string, IStrategy> store, string key, IStrategy strategy)
    {
        _store = store;
        _key = key;
        _strategy = strategy;
    }

    public void Execute() => _store[_key] = _strategy;
}
