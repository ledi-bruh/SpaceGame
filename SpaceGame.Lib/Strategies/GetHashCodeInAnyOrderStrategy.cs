namespace SpaceGame.Lib;
using Hwdtech;

public class GetHashCodeInAnyOrderStrategy : IStrategy  // "GetHashCode.AnyOrder"
{
    public object Invoke(params object[] args)
    {
        IEnumerable<object> data = (IEnumerable<object>)args[0];

        return IoC.Resolve<int>("GetHashCode", data.OrderBy(x => x.GetHashCode()));
    }
}
