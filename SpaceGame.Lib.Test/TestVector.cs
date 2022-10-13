namespace SpaceGame.Lib.Test;
using Vector;

public class TestVector
{
    [Fact]
    public void TestNotEmptyVector()
    {
        Assert.Equal("Invalid params", Assert.Throws<ArgumentException>(() => new Vector()).Message);
    }

    [Fact]
    public void TestEqualsTrue()
    {
        Vector A = new Vector(5, 8, 1);
        Vector B = new Vector(5, 8, 1);
        Assert.Equal(A, B);
    }

    [Fact]
    public void TestEqualsFalse()
    {
        Vector A = new Vector(1, 2, 3);
        Vector B = new Vector(1, 2);
        int b = 1;

        Assert.NotEqual(A, B);
        Assert.False(A.Equals(b));
    }

    [Fact]
    public void TestGetHashCodeTrue()
    {
        Vector A = new Vector(547, 212, 589123893, -901230000, 17000999);
        Vector B = new Vector(547, 212, 589123893, -901230000, 17000999);
        Assert.Equal(A.GetHashCode(), B.GetHashCode());
    }

    [Fact]
    public void TestGetHashCodeFalse()
    {
        Vector A = new Vector(547, 212, 589123893, -901230000, 17000999);
        Vector B = new Vector(-901230000, 17000999, 547, 589123893, 212);
        Assert.NotEqual(A.GetHashCode(), B.GetHashCode());
    }

    [Fact]
    public void TestSumTrue()
    {
        Vector A = new Vector(1, 2, 3);
        Vector B = new Vector(-1, -2, -3);
        Vector C = new Vector(0, 0, 0);
        Assert.Equal(A + B, C);
    }

    [Fact]
    public void TestSumThrowsExceptionDim()
    {
        Vector A = new Vector(1, 2, 3);
        Vector B = new Vector(-1, -2);

        Assert.Equal("Dimensions do not match", Assert.Throws<ArgumentException>(() => A + B).Message);
    }
}