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

    public static int GCD(int a, int b)
    {
        if (b < 0) b *= -1;
        if (a < 0) a *= -1;
        while (a != 0 && b != 0) if (a >= b) a %= b; else b %= a;
        return a | b;
    }

    public override string ToString() => $"{Math.Round(1d * _numerator / _denominator, 5)} deg";

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
}