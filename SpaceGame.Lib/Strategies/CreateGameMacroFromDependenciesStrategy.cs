using Hwdtech;

namespace SpaceGame.Lib;


public class CreateGameMacroFromDependenciesStrategy: IStrategy //Game.Macro.Create.FromDependencies
{
    public object Invoke(params object[] args)
    {
        var obj = (IUObject)args[0];
        var type = (string)args[1];

        var dependencies = IoC.Resolve<IEnumerable<string>>("Game.Dependencies.Get.Macro." + type);

        var commands = dependencies.ToList().Select(x => IoC.Resolve<ICommand>("Game.Command." + x, obj)).ToList();

        return IoC.Resolve<ICommand>("Game.Command.Macro", commands);
    }
}
