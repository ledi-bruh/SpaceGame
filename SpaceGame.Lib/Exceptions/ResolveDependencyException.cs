namespace SpaceGame.Lib;

public class ResolveDependencyException : Exception
{
    public ResolveDependencyException() { }
    public ResolveDependencyException(string message) : base(message) { }
}
