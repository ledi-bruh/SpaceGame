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
    public void TestToString()
    {
        Angle A = new Angle(22, 7);

        Assert.Equal("3.14286 deg (22 / 7)", A.ToString());
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

    [Theory]
    [MemberData(nameof(TestSumData))]
    public void TestSum(Angle A, Angle B, Angle C)
    {
        Assert.Equal(C, A + B);
    }

    public static IEnumerable<object[]> TestSumData => new List<object[]>
    {
        new object[] { new Angle(360), new Angle(180), new Angle(540) },
        new object[] { new Angle(1, 3), new Angle(1, 7), new Angle(10, 21) },
        new object[] { new Angle(1, 3), new Angle(-20, 60), new Angle(0, 1) },
        new object[] { new Angle(7154, 73), new Angle(3600, 1800), new Angle(100) },
    };

    [Theory]
    [MemberData(nameof(TestModData))]
    public void TestMod(Angle A, int k, Angle B)
    {
        Assert.Equal(B, A % k);
    }

    public static IEnumerable<object[]> TestModData => new List<object[]>
    {
        new object[] { new Angle(127, 3), 5, new Angle(7, 3) },
        new object[] { new Angle(-370), 360, new Angle(-10) },
        new object[] { new Angle(127, -3), 5, new Angle(-7, 3) },
        new object[] { new Angle(0), 3, new Angle(0) },
    };

    [Fact]
    public void TestRoundModZero()
    {
        Assert.Throws<DivideByZeroException>(() => new Angle(124, 7) % 0);
    }
}