class Rotation
{
    public static void Rotate(IRotatable obj)
    {
        obj.setAngleDirection((obj.getAngleDirection() + obj.getAngularSpeed()) % (2 * Math.PI));  // в радианах
    }
    //! Q: кол-во направлений?
    // int maxDirections = 12;
    // int curDirection = (obj.getAngle()) / (360 / maxDirections);
}
