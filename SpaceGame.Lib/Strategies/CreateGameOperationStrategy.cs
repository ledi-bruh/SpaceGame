using Hwdtech;

namespace SpaceGame.Lib;


public class CreateGameOperationStrategy: IStrategy //Game.Operation.Create
{
    public object Invoke(params object[] args)
    {
        var obj = (IUObject)args[0];
        var type = (string)args[1];

        var dependencies = IoC.Resolve<IEnumerable<string>>("Game.Dependencies.Get.Operation." + type);

        var commands = dependencies.ToList().Select(x => IoC.Resolve<ICommand>("Game.Command." + x, obj)).ToList();

        return IoC.Resolve<ICommand>("Game.Command.Macro", commands);
    }
}
