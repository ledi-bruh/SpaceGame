namespace SpaceGame.Lib.Test;
using Vector;

public class TestVector
{
    [Fact]
    public void TestEqualSizeTrue()
    {
        Vector A = new Vector(5, 1);
        Vector B = new Vector(0, 0);
        Assert.True(Vector.EqualSize(A, B));
    }

    [Fact]
    public void TestEqualSizeFalse()
    {
        Vector A = new Vector();
        Vector B = new Vector(0);
        Assert.False(Vector.EqualSize(A, B));
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
        Assert.NotEqual(A, B);
    }

    [Fact]
    public void TestSum()
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

        var expected = "Dimensions do not match";
        var error = Assert.Throws<ArgumentException>(() => A + B);
        Assert.Equal(error.Message, expected);
    }
}