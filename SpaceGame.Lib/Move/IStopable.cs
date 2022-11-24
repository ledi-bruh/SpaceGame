namespace SpaceGame.Lib;

public interface IEndable
{
    public IUObject Target { get; }
    public IDictionary<string, object> Parameters { get; }
}
