namespace SpaceGame.Lib.Test;
using Moq;
using Hwdtech;
using Hwdtech.Ioc;

public class TestSetFuelForUObjectsCommand
{
    public TestSetFuelForUObjectsCommand()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        IoC.Resolve<ICommand>("Scopes.Current.Set",
            IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))
        ).Execute();
    }

    [Fact]
    public void SuccessfulSettingFuel()
    {
        IoC.Resolve<ICommand>("IoC.Register", "Game.UObject.SetProperty", (object[] args) => new ActionCommand(
            () => ((IUObject)args[0]).SetProperty((string)args[1], (object)args[2])
        )).Execute();

        IoC.Resolve<ICommand>("IoC.Register", "Game.UObject.Collection.Set.Fuel",
            (object[] args) => new SetFuelForUObjectsCommand((IEnumerable<IUObject>)args[0], (double)args[1])
        ).Execute();

        var mockUObjects = Enumerable.Range(0, 3).Select(x =>
        {
            var mock = new Mock<IUObject>();
            mock.Setup(x => x.SetProperty(It.IsAny<string>(), It.IsAny<object>())).Verifiable();
            return mock;
        }).ToList();

        mockUObjects.ForEach(mock => mock.Verify(x => x.SetProperty(It.IsAny<string>(), It.IsAny<object>()), Times.Never()));
        IoC.Resolve<Lib.ICommand>("Game.UObject.Collection.Set.Fuel", mockUObjects.Select(x => x.Object), 100.0).Execute();
        mockUObjects.ForEach(mock => mock.Verify(x => x.SetProperty(It.IsAny<string>(), It.IsAny<object>()), Times.Once()));
    }
}
