namespace Angle;

public class Angle
{
    private int _numerator, _denominator;
    
    public Angle(int numerator, int denominator = 1)
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
        b = Math.Abs(b);
        a = Math.Abs(a);
        while (a != 0 && b != 0) if (a >= b) a %= b; else b %= a;
        return a | b;
    }

    private Angle Round(int k = 360)
    {
        if (k == 0) throw new DivideByZeroException();
        return new Angle(_numerator % (k * _denominator), _denominator);
    }

    public override string ToString() => Math.Round(1d * _numerator / _denominator, 5).ToString().Replace(",", ".")
                                        + $" deg ({_numerator} / {_denominator})";

    public override bool Equals(object? obj) => obj is Angle angle &&
                                                this._numerator == angle._numerator &&
                                                this._denominator == angle._denominator;

    public override int GetHashCode() => HashCode.Combine(_numerator, _denominator);

    public static Angle operator +(Angle A, Angle B)
    {
        int numerator = A._numerator * B._denominator + B._numerator * A._denominator;
        int denominator = A._denominator * B._denominator;
        int gcd = Angle.GCD(numerator, denominator);
        return new Angle(numerator / gcd, denominator / gcd);
    }

    public static Angle operator %(Angle A, int b) => A.Round(b);
}
