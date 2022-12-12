namespace SpaceGame.Lib;

public class RotateCommand : ICommand
{
    private readonly IRotatable _rotatable;

    public RotateCommand(IRotatable rotatable) => _rotatable = rotatable;

    public void Execute() => _rotatable.Direction = (_rotatable.Direction + _rotatable.AngularVelocity) % 360;
}
