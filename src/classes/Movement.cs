public class Movement
{
    public static void Move(IMovable obj)
    {

        if (obj.getCoords().Length == 0)
        {
            throw new ArgumentException("No coordinates");
        }
        if (obj.getSpeed().Length == 0)
        {
            throw new ArgumentException("No speed vector");
        }
        obj.setCoords(obj.getCoords().Zip(obj.getSpeed(), (a, b) => a + b).ToArray());  // a + b не округляется

    }
}
