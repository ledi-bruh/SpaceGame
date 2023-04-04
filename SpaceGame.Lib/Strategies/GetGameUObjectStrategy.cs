namespace SpaceGame.Lib;
using Hwdtech;


public class GetGameUObjectStrategy : IStrategy  // "Game.Get.UObject"
{
     public object Invoke(params object[] args)
    {
        int objectid = (int)args[0];

        IUObject? uObject;
        
        if (IoC.Resolve<IDictionary<int, IUObject>>("Game.UObject.Map").TryGetValue(objectid, out uObject))
        {
            return uObject;
        }
        throw new Exception();
    }
}
