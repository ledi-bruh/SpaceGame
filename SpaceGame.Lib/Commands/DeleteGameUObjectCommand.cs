namespace SpaceGame.Lib;
using Hwdtech;

public class DeleteGameUObjectCommand : ICommand
{
    private int _uObjectId;

    public DeleteGameUObjectCommand(int uObjectId) => _uObjectId = uObjectId;

    public void Execute()
    {
        IoC.Resolve<IDictionary<int, IUObject>>("Game.UObject.Map").Remove(_uObjectId);
    }
}
