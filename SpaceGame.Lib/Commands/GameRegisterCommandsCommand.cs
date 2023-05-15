using Hwdtech;

namespace SpaceGame.Lib;

public class GameRegisterCommandsCommand : ICommand 
{
    public void Execute()
    {
        var dependencies = IoC.Resolve<IDictionary<string, IStrategy>>("Game.Dependencies.Get.Commands");

        dependencies.ToList().ForEach(x =>
        {
            IoC.Resolve<ICommand>("IoC.Register", "Game.Command." + x.Key, x.Value).Execute();
        });
    }
}
