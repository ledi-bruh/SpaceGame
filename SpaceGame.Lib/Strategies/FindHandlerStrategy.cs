namespace SpaceGame.Lib;
using Hwdtech;

public class FindHanlderStrategy : IStrategy  // "Exception.Handler.Find"
{
    public object Invoke(params object[] args)
    {
        var types = (IEnumerable<Type>)args[0];

        var tree = IoC.Resolve<IDictionary<int, IHandler>>("Exception.Handler.Tree");

        if (tree.TryGetValue(IoC.Resolve<int>("GetHashCode.AnyOrder", types), out IHandler? handler))
        {
            return handler;
        }
        return tree[0];
    }
}
