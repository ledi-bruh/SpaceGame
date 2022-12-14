namespace SpaceGame.Lib;
using Hwdtech;

public class CheckCollisionCommand : ICommand
{
    IUObject _objFirst, _objSecond;

    public CheckCollisionCommand(IUObject objFirst, IUObject objSecond)
    {
        _objFirst = objFirst;
        _objSecond = objSecond;
    }

    public void Execute()
    {
        var positionFirst = IoC.Resolve<List<int>>("Game.UObject.GetProperty", _objFirst, "Position");
        var positionSecond = IoC.Resolve<List<int>>("Game.UObject.GetProperty", _objSecond, "Position");
        var velocityFirst = IoC.Resolve<List<int>>("Game.UObject.GetProperty", _objFirst, "Velocity");
        var velocitySecond = IoC.Resolve<List<int>>("Game.UObject.GetProperty", _objSecond, "Velocity");

        var attributes = new List<int>{
            positionFirst[0] - positionSecond[0],
            positionFirst[1] - positionSecond[1],
            velocityFirst[0] - velocitySecond[0],
            velocityFirst[1] - velocitySecond[1]
        };

        var collisionTree = IoC.Resolve<IDictionary<int, object>>("Game.CollisionTree");

        attributes.ForEach(attribute => collisionTree = (IDictionary<int, object>)collisionTree[attribute]);

        if (collisionTree.Keys.First() != 0)
        {
            IoC.Resolve<ICommand>("Game.Event.Collision", _objFirst, _objSecond).Execute();
        }
    }
}
