namespace SpaceGame.Lib.Test;
using Angle;

public class TestAngle
{
    [Fact]
    public void TestNotZeroAngle()
    {
        Assert.Equal("Zero denominator", Assert.Throws<ArgumentException>(() => new Angle(1, 0)).Message);
    }

    [Fact]
    public void TestEqualsTrue()
    {
        Angle A = new Angle(-25, 11);
        Angle B = new Angle(25, -11);

        Assert.Equal(A, B);
    }

    [Fact]
    public void TestEqualsFalse()
    {
        Angle A = new Angle(25, 11);
        Angle B = new Angle(11, 25);
        int b = 1;

        Assert.NotEqual(A, B);
        Assert.False(A.Equals(b));
    }

    [Fact]
    public void TestGetHashCodeTrue()
    {
        Angle A = new Angle(589123893, -901230000);
        Angle B = new Angle(-589123893, 901230000);

        Assert.Equal(A.GetHashCode(), B.GetHashCode());
    }

    [Fact]
    public void TestGetHashCodeFalse()
    {
        Angle A = new Angle(589123893, 901230000);
        Angle B = new Angle(901230000, 589123893);

        Assert.NotEqual(A.GetHashCode(), B.GetHashCode());
    }

    [Fact]
    public void TestSumTrue()
    {
        Angle A = new Angle(1, 3);
        Angle B = new Angle(1, 7);
        Angle C = new Angle(10, 21);

        Assert.Equal(C, A + B);
    }
}