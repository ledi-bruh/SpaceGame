namespace SpaceGame.Lib;

public class RotateCommand : ICommand
{
    private readonly IRotatable _rotatable;

    public RotateCommand(IRotatable obj) => _rotatable = obj;

    public void Execute() => this._rotatable.Direction = (this._rotatable.Direction + this._rotatable.AngularVelocity) % 360;
    //! Direction = (int)Round(Direction * Матрица поворота от AngleVelocity * Math.PI/180)
}
