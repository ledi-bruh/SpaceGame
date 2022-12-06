namespace SpaceGame.Lib;

public interface IStartable
{
    public IUObject Target { get; }
    public IDictionary<string, object> Parameters { get; }
}
