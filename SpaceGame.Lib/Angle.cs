namespace Angle;

public class Angle
{
    private int x, y, part;  //!    arctan(y/x) + part*pi/2    [-pi/2, 3pi/2]

    public Angle(int x, int y)
    {
        this.x = 1; this.y = 0;
        if (x == 0 && y == 0) this.part = 0;
        else if (x == 0) this.part = y > 0 ? 1 : 3;
        else if (y == 0) this.part = x > 0 ? 0 : 2;
        else if (y > 0 && x > 0) { this.y = y; this.x = x; this.part = 0; }
        else { this.y = y; this.x = x; this.part = 2; }
    }
    private static int GCD(int a, int b)
    {
        if (a == 0) return b;
        return GCD(b % a, a);
    }
    private static Angle ZeroForm(Angle Obj)
    {
        if (Obj.x == 1 && Obj.y == 0) return Obj;
        Angle Buf = new Angle(Obj.x, Obj.y);
        Buf.part = Obj.part;
        while (Buf.part > 0)
        {
            Buf.part -= 1;
            (Buf.x, Buf.y) = (Buf.y, -Buf.x);
        }
        return Buf;
    }
    public static bool operator ==(Angle a, Angle b)
    {
        Angle A = ZeroForm(a), B = ZeroForm(b);
        return A.y == B.y && A.x == B.x && A.part == B.part;
    }
    public static bool operator !=(Angle a, Angle b) => !(a == b);
    public override bool Equals(object? obj)
    {
        return obj is Angle angle &&
               this == angle;
    }
    public override int GetHashCode()
    {
        return HashCode.Combine(x, y, part);
    }
    public override string ToString() => $"Angle({x}, {y}): arctan({y}/{x}) + {part}*PI/2";
    public static Angle operator +(Angle A, Angle B)
    {
        int x = A.x * B.x - A.y * B.y;
        int y = A.y * B.x + B.y * A.x;
        int gcd = GCD(y, x);
        Angle Buf = new Angle(x / gcd, y / gcd);
        if (x == B.x && y == B.y) Buf.part += A.part;
        if (x == A.x && y == A.y) Buf.part += B.part;
        if (Buf.part > 3) Buf.part %= 4;
        return Buf;
    }
}