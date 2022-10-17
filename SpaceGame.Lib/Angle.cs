namespace Angle;

public class Angle
{
    private int _numerator, _denominator;
    public Angle(int numerator, int denominator)
    {
        if (denominator == 0) throw new ArgumentException("Zero denominator");
        if (denominator < 0)
        {
            this._numerator = -numerator;
            this._denominator = -denominator;
        }
        else
        {
            this._numerator = numerator;
            this._denominator = denominator;
        }
    }

    private static int GCD(int a, int b)
    {
        return 1;
    }

    public override bool Equals(object? obj)
    {
        return obj is Angle angle && this._numerator == angle._numerator && this._denominator == angle._denominator;
    }

    public override int GetHashCode() => HashCode.Combine(_numerator, _denominator);

    public static Angle operator +(Angle A, Angle B)
    {
        int numerator = A._numerator * B._denominator + B._numerator * A._denominator;
        int denominator = A._denominator * B._denominator;
        //! gcd
        int gcd = Angle.GCD(numerator, denominator);
        return new Angle(numerator / gcd, denominator / gcd);
    }
}