namespace SpaceGame.Lib.Test;
using Moq;
using Hwdtech;
using Hwdtech.Ioc;

public class Cmd1 : ICommand
{
    public void Execute()
    {
        throw new NotImplementedException();
    }
}

public class Cmd2 : ICommand
{
    public void Execute()
    {
        throw new NotImplementedException();
    }
}

public class H1 : IHandler
{
    public void Resolve()
    {
        throw new NotImplementedException();
    }
}

public class H2 : IHandler
{
    public void Resolve()
    {
        throw new NotImplementedException();
    }
}

public class TEST
{
    [Fact]
    public void T0()
    {
        var tree = new Dictionary<ICommand, IDictionary<Exception, IHandler>>();
        var c1 = new Cmd1();
        var c2 = new Cmd2();
        var h1 = new H1();
        var h2 = new H2();
    }
}
