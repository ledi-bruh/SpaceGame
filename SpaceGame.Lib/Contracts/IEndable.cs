namespace SpaceGame.Lib;

public interface IEndable
{
    public IUObject Target { get; }
    public IEnumerable<string> Keys { get; }
}
