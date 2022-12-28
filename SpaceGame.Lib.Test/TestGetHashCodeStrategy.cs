namespace SpaceGame.Lib.Test;

public class TestGetHashCodeStrategy
{
    [Fact]
    public void SuccesfullGetHashCode()
    {
        var data = new List<object> { typeof(Exception) };
        int hash;

        unchecked { hash = ((int)2166136261 * 16777619) ^ data[0].GetHashCode(); }

        Assert.Equal(hash, new GetHashCodeStrategy().Invoke(data));
    }

    [Fact]
    public void GetHashCodeSomeDataInSameOrderAreEqual()
    {
        var ghc = new GetHashCodeStrategy();
        var data1 = new List<object> { 1, "f2", typeof(Exception) };
        var data2 = new List<object> { 1, "f2", typeof(Exception) };

        Assert.Equal(ghc.Invoke(data1), ghc.Invoke(data2));
    }

    [Fact]
    public void GetHashCodeSomeDataInDifferentOrderAreNotEqual()
    {
        var ghc = new GetHashCodeStrategy();
        var data1 = new List<object> { 1, 0 };
        var data2 = new List<object> { 0, 1 };

        Assert.NotEqual(ghc.Invoke(data1), ghc.Invoke(data2));
    }
}
