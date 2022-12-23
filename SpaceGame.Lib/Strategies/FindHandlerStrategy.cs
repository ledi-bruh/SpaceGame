namespace SpaceGame.Lib;
using Hwdtech;

public class FindHanlderStrategy : IStrategy  // "Exception.Handler.Find"
{
    public object Invoke(params object[] args)
    {
        var types = (IEnumerable<Type>)args[0];

        var tree = IoC.Resolve<IDictionary<int, IHandler>>("Exception.Handler.Tree");

        if (tree.TryGetValue(IoC.Resolve<int>("GetHashCode", types.OrderBy(x => x.GetHashCode())), out IHandler? handler))
        {
            return handler;
        }
        return tree[0];
    }
}
