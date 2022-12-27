namespace SpaceGame.Lib.Test;

public class TestGetHashCodeStrategy
{
    [Fact]
    public void SuccesfullGetHashCode()
    {
        IEnumerable<object> data = new object[] { 1 };
        int hash;

        unchecked { hash = ((int)2166136261 * 16777619) ^ 1.GetHashCode(); }

        Assert.Equal(hash, new GetHashCodeStrategy().Invoke(data));
    }

    [Fact]
    public void GetHashCodeSomeDataInSameOrderAreEqual()
    {
        GetHashCodeStrategy ghc = new GetHashCodeStrategy();
        IEnumerable<object> data1 = new object[] { 1, "f2", typeof(Exception) };
        IEnumerable<object> data2 = new object[] { 1, "f2", typeof(Exception) };

        Assert.Equal(new GetHashCodeStrategy().Invoke(data1), new GetHashCodeStrategy().Invoke(data2));
    }

    [Fact]
    public void GetHashCodeSomeDataInDifferentOrderAreNotEqual()
    {
        GetHashCodeStrategy ghc = new GetHashCodeStrategy();
        IEnumerable<object> data1 = new object[] { 1, 0 };
        IEnumerable<object> data2 = new object[] { 0, 1 };

        Assert.NotEqual(new GetHashCodeStrategy().Invoke(data1), new GetHashCodeStrategy().Invoke(data2));
    }
}
