namespace SpaceGame.Lib;

public interface IStrategy
{
    public object Invoke(params object[] args);
}
