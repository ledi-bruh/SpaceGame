namespace SpaceGame.Lib;
using Hwdtech;

public class RegisterHandlerCommand : ICommand
{
    private IEnumerable<Type> _types;
    private IHandler _handler;

    public RegisterHandlerCommand(IEnumerable<Type> types, IHandler handler)
    {
        _types = types;
        _handler = handler;
    }

    public void Execute()
    {
        unchecked
        {
            int hash = (int)2166136261;

            _types.Select(x => x.GetHashCode()).OrderBy(x => x).ToList().ForEach(
                hashcode => hash = (hash * 16777619) ^ hashcode
            );

            IoC.Resolve<IDictionary<int, IHandler>>("Exception.Handler.Tree").Add(hash, _handler);
        }
    }
}
