namespace SpaceGame.Lib;

public class GetHashCodeStrategy : IStrategy  // "GetHashCode"
{
    public object Invoke(params object[] args)
    {
        IEnumerable<object> data = (IEnumerable<object>)args[0];

        unchecked
        {
            return data.Aggregate((int)2166136261, (cur, next) => (cur * 16777619) ^ next.GetHashCode());
        }
    }
}
