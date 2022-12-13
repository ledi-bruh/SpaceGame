namespace SpaceGame.Lib.Test;
using Moq;
using Hwdtech;
using Hwdtech.Ioc;

public class TestCollisionTreeBuilder
{
    public TestCollisionTreeBuilder()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        IoC.Resolve<ICommand>("Scopes.Current.Set",
            IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))
        ).Execute();
    }

    [Fact]
    public void SuccesfullCreateCollisionTreeCommand()
    {
        var mockCollisionTreeBuilder = new Mock<ICollisionTreeBuilder>();
        mockCollisionTreeBuilder.Setup(x => x.BuildFromFile(It.IsAny<string>())).Verifiable();

        IoC.Resolve<ICommand>("IoC.Register", "Game.CollisionTree.Builder", (object[] args) => mockCollisionTreeBuilder.Object).Execute();

        var createCollisionTreeCommand = new BuildCollisionTreeCommand("anyPath");

        createCollisionTreeCommand.Execute();

        mockCollisionTreeBuilder.Verify(x => x.BuildFromFile("anyPath"), Times.Once());
    }

    [Fact]
    public void TryExecuteCreateCollisionTreeCommandThrowFileNotFoundException()
    {
        var collisionTreeBuilder = new CollisionTreeBuilder();

        IoC.Resolve<ICommand>("IoC.Register", "Game.CollisionTree.Builder", (object[] args) => collisionTreeBuilder).Execute();

        var createCollisionTreeCommand = new BuildCollisionTreeCommand("pijuba");

        Assert.Throws<FileNotFoundException>(() => createCollisionTreeCommand.Execute());
    }

    [Fact]
    public void BuildTreeFromFileWithSomeBranhes()
    {
        var tree = new Dictionary<int, object>();
        IoC.Resolve<ICommand>("IoC.Register", "Game.CollisionTree", (object[] args) => tree).Execute();

        var collisionTreeBuilder = new CollisionTreeBuilder();
        collisionTreeBuilder.BuildFromFile("../../../Data/collisionData.txt");

        Assert.Equal(3, IoC.Resolve<IDictionary<int, object>>("Game.CollisionTree").Count);
        Assert.Equal(2, ((IDictionary<int, object>)IoC.Resolve<IDictionary<int, object>>("Game.CollisionTree")[1]).Count);
    }
}
