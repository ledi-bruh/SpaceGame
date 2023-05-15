namespace SpaceGame.Lib;
using Hwdtech;

public class GameRegisterCommandsCommand : ICommand 
{
    public void Execute()
    {
        var dependencies = IoC.Resolve<IDictionary<string, IStrategy>>("Game.Dependencies.Get.Commands");

        dependencies.ToList().ForEach(x =>
        {
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command." + x.Key,(object[] args) => x.Value.Invoke(args)).Execute();
        });
    }
}
