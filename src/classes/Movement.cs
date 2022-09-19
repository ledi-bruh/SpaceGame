public class Movement
{
    public static void Move(IMovable obj)
    {

        if (obj.getCoords().Length == 0)
        {
            throw new ArgumentException("No coordinates");
        }
        double[] speedVector = new double[] {
            Math.Round(obj.getNorma() * Math.Cos(obj.getAngleDirection())),
            Math.Round(obj.getNorma() * Math.Sin(obj.getAngleDirection()))
        };
        obj.setCoords(obj.getCoords().Zip(speedVector, (a, b) => a + b).ToArray());  // Math.Round(a + b)?
    }
}
