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
        var projectile = IoC.Resolve<object>("Game.Create.Projectile", _obj.AmmoType);
        var cmd = IoC.Resolve<ICommand>("Game.Create.Projectile.Command.Move", projectile, _obj.projectilePosition, _obj.projectileVelocity);
        IoC.Resolve<SpaceGame.Lib.ICommand>("Game.Queue.Push", IoC.Resolve<int>("Game.Get.GameId"), cmd).Execute();
    }   
}
