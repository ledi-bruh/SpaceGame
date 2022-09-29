namespace SpaceGame.Lib;

public class RotateCommand : ICommand
{
    private readonly IRotatable _rotatable;
    public RotateCommand(IRotatable obj)
    {
        this._rotatable = obj;
    }
    public void Execute() => this._rotatable.Direction += this._rotatable.AngularVelocity;
}