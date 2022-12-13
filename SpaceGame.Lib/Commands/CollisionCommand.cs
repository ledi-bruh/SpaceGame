namespace SpaceGame.Lib;
using Hwdtech;
public class CollisionCommand : ICommand
{
    IUObject _obj1, _obj2;
    public CollisionCommand(IUObject obj1, IUObject obj2)
    {
        _obj1 = obj1;
        _obj2 = obj2;
    }
    public void Execute()
    {
        var collisiontree = IoC.Resolve<IDictionary<int, object>>("Game.CollisionTree");
        var coords1 = IoC.Resolve<List<int>>("Game.UObject.GetProperty",
            _obj1,
            "Position");
        var coords2 = IoC.Resolve<List<int>>("Game.UObject.GetProperty",
            _obj2,
            "Position");
        var velocity1 = IoC.Resolve<List<int>>("Game.UObject.GetProperty",
            _obj1,
            "Velocity");
        var velocity2 = IoC.Resolve<List<int>>("Game.UObject.GetProperty",
            _obj2,
            "Velocity");
        var characteristic = new List<int>{coords1[0]-coords2[0],
            coords1[1]-coords2[1],
            velocity1[0] - velocity2[0],
            velocity1[1] - velocity2[1]};
        characteristic.ForEach(x => 
        {
            collisiontree = (IDictionary<int, object>)collisiontree[x];
        });
        if ((bool)collisiontree.First().Key)
    }
}