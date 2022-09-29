namespace Angle;

public class Angle
{
    private readonly int y, x;  //!    y/x => arctan(y/x)
    private int part; //!    part * pi/2

    public Angle(int y, int x)
    {
        this.y = 0;
        this.x = 1;
        if (x == 0) this.part = y > 0 ? 1 : 3;
        else if (y == 0) this.part = x > 0 ? 0 : 2;
        else if (y > 0 && x > 0) { this.y = y; this.x = x; this.part = 0; }
        else if (y < 0 && x > 0) { this.y = y; this.x = x; this.part = 4; }
        else { this.y = y; this.x = x; this.part = 2; }
    }
    private static int GCD(int a, int b)
    {
        if (a == 0) return b;
        return GCD(b % a, a);
    }

    public override string ToString() => $"{y}/{x} + {part} * PI/2";
    public static Angle operator +(Angle A, Angle B)
    {
        int y = A.y * B.x + B.y * A.x;
        int x = A.x * B.x - A.y * B.y;
        int gcd = GCD(y, x);
        Angle Buf = new Angle(y / gcd, x / gcd);
        Buf.part = (A.part + B.part + Buf.part) % 8;
        return Buf;
    }
}