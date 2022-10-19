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
        
        Assert.Equal("3.14286 deg", A.ToString());
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
    [InlineData(5, 125, 5)]
    [InlineData(532634542, 2, 2)]
    [InlineData(-7, 7000, 7)]
    [InlineData(90, -9, 9)]
    [InlineData(-110, -11, 11)]
    public void TestGCD(int a, int b, int c)
    {
        Assert.Equal(c, Angle.GCD(a, b));
    }
}