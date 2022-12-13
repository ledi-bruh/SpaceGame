namespace SpaceGame.Lib;
using Hwdtech;

public class CollisionTreeBuilder : ICollisionTreeBuilder
{
    private IEnumerable<IEnumerable<int>> ReadDataFromFile(string dataPath) =>
        File.ReadAllLines(dataPath).Select(line => line.Split().Select(int.Parse)
    );

    private void BuildTree(IEnumerable<IEnumerable<int>> data)
    {
        data.ToList().ForEach(line =>
        {
            var subtree = IoC.Resolve<IDictionary<int, object>>("Game.CollisionTree");
            line.ToList().ForEach(number =>
            {
                subtree.TryAdd(number, new Dictionary<int, object>());
                subtree = (IDictionary<int, object>)subtree[number];
            });
        });
    }

    public void BuildFromFile(string path) => BuildTree(ReadDataFromFile(path));
}
