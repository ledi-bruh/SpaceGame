namespace SpaceGame.Lib;

public class ResolveDependencyException : Exception
{
    public ResolveDependencyException(string message = "") : base(message) { }
}
