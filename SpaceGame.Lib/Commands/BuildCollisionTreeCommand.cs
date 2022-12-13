namespace SpaceGame.Lib;
using Hwdtech;

public class BuildCollisionTreeCommand : ICommand
{
    private string _path;

    public BuildCollisionTreeCommand(string path) => _path = path;

    public void Execute() => IoC.Resolve<ICollisionTreeBuilder>("Game.CollisionTree.Builder").BuildFromFile(_path);
}
