namespace SpaceGame.Lib;
using Hwdtech;

public class SetFuelForUObjectsCommand : ICommand  // "Game.UObjects.Set.Fuel"
{
    private IEnumerable<IUObject> _gameUObjects;
    private double _fuelVolume;

    public SetFuelForUObjectsCommand(IEnumerable<IUObject> gameUObjects, double fuelVolume)
    {
        _gameUObjects = gameUObjects;
        _fuelVolume = fuelVolume;
    }

    public void Execute()
    {
        _gameUObjects.ToList().ForEach(
            x => IoC.Resolve<ICommand>("Game.UObject.SetProperty", x, "Fuel", _fuelVolume).Execute()
        );
    }
}
