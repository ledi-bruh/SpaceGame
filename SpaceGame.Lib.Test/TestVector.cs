namespace SpaceGame.Lib.Test;
using Vector;

public class TestVector
{
    [Fact]
    public void TestIsEqualSizeTrue()
    {
        Vector A = new Vector(5, 1);
        Vector B = new Vector(0, 0);
        Assert.True(Vector.IsEqualSize(A, B));
    }

    [Fact]
    public void TestIsEqualSizeFalse()
    {
        Vector A = new Vector();
        Vector B = new Vector(0);
        Assert.False(Vector.IsEqualSize(A, B));
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

        Assert.Equal(Assert.Throws<ArgumentException>(() => A + B).Message, "Dimensions do not match");
    }
}