namespace SpaceGame.Lib;
using Hwdtech;


public class ShootCommand : ICommand
{
    private IShootable _obj;
    
    public ShootCommand(IShootable obj)
    {
        _obj = obj;
    }

    public void Execute()
    {
        IoC.Resolve<SpaceGame.Lib.ICommand>("Game.Queue.Push", IoC.Resolve<int>("Game.Get.GameId"),
            IoC.Resolve<SpaceGame.Lib.ICommand>("Game.UObject.Shoot", _obj)).Execute();
    }   
}
