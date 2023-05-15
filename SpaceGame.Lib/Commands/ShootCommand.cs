using Hwdtech;

namespace SpaceGame.Lib;


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
            IoC.Resolve<SpaceGame.Lib.ICommand>("Game.IUObject.Shoot", _obj)).Execute();
    }   
}
