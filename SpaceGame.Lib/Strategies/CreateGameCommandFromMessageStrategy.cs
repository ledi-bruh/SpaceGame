namespace SpaceGame.Lib;
using Hwdtech;

public class CreateGameCommandFromMessageStrategy : IStrategy  // "Game.Command.Create.FromMessage"
{
    public object Invoke(params object[] args)
    {
        IInterpretingMessage message = (IInterpretingMessage)args[0];

        var obj = IoC.Resolve<IUObject>("Game.Get.UObject", message.ObjectID);

        message.Parameters.ToList().ForEach(x => obj.SetProperty(x.Key, x.Value));
        
        return IoC.Resolve<SpaceGame.Lib.ICommand>("Game.Command." + message.TypeCommand, obj);
    }
}
