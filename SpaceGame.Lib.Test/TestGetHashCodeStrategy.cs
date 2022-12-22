namespace SpaceGame.Lib.Test;

public class TestGetHashCodeStrategy
{
    [Fact]
    public void SuccesfullGetHashCode()
    {
        IEnumerable<object> types = new Type[] { typeof(Exception) };
        int hash;

        unchecked { hash = ((int)2166136261 * 16777619) ^ typeof(Exception).GetHashCode(); }

        Assert.Equal(hash, new GetHashCodeStrategy().Invoke(types));
    }
}
