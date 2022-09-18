class Rotation
{
    public static void Rotate(IRotatable obj)
    {
        obj.setAngle((obj.getAngle() + obj.getAngularSpeed()) % (2 * Math.PI));  // в радианах

        //! Q: кол-во направлений?
        // int maxDirections = 12;
        // int curDirection = (obj.getAngle()) / (360 / maxDirections);
        if (obj is IChangeableSpeed)
        {
            double modSpeed = Math.Sqrt(((IMovable)obj).getSpeed().Select(xi => xi * xi).Sum());
            ((IChangeableSpeed)obj).setSpeed(new double[] { Math.Round(modSpeed * Math.Cos(obj.getAngle())), Math.Round(modSpeed * Math.Sin(obj.getAngle())) });
        }
    }
}
