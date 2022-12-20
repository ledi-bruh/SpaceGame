namespace SpaceGame.Lib.Test;
using Moq;
using Hwdtech;
using Hwdtech.Ioc;

public class TestRegisterCommand
{
    public TestRegisterCommand()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        IoC.Resolve<ICommand>("Scopes.Current.Set",
            IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))
        ).Execute();
    }

    [Fact]
    public void SuccesfullRegisterCommand()
    {
        // // var mockCommand = new Mock<ICommand>();
        // // var mockException = new Mock<Exception>();
        // // var mockHandler = new Mock<IHandler>();

        // var mockEDict = new Mock<IDictionary<Exception, IHandler>>();
        // mockEDict.Setup(x => x.Add(It.IsAny<Exception>(), It.IsAny<IHandler>())).Verifiable();

        // var mockCDict = new Mock<IDictionary<SpaceGame.Lib.ICommand, IDictionary<Exception, IHandler>>>();
        // mockCDict.Setup(x => x.Add(It.IsAny<SpaceGame.Lib.ICommand>(), It.IsAny<IDictionary<Exception, IHandler>>())).Verifiable();
        // mockCDict.Setup(x => x[It.IsAny<SpaceGame.Lib.ICommand>()]).Returns(mockEDict.Object).Verifiable();

        // IoC.Resolve<ICommand>("IoC.Register", "Exception.Handle.Register",
        //     (object[] args) => new RegisterHandlerCommand((SpaceGame.Lib.ICommand)args[0], (Exception)args[1], (IHandler)args[2])
        // ).Execute();
        // IoC.Resolve<ICommand>("IoC.Register", "Exception.Handle.Tree", mockCDict.Object).Execute();

        // mockEDict.VerifyAll();
        // mockCDict.VerifyAll();
    }

    [Fact]
    public void T123()
    {
        var a = new Dictionary<object, IDictionary<object, IHandler>>();
        var mockHandler = new Mock<IHandler>();

        IoC.Resolve<ICommand>("IoC.Register", "Exception.Handle.Register",
            (object[] args) => new RegisterHandlerCommand((object)args[0], (object)args[1], (IHandler)args[2])
        ).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Exception.Handle.Tree", (object[] args) => a).Execute();

        IoC.Resolve<SpaceGame.Lib.ICommand>("Exception.Handle.Register", typeof(MoveCommand), typeof(ArgumentNullException), mockHandler.Object).Execute();
        IoC.Resolve<SpaceGame.Lib.ICommand>("Exception.Handle.Register", "Exception.Handle.Command.Default", typeof(ArgumentNullException), mockHandler.Object).Execute();
        IoC.Resolve<SpaceGame.Lib.ICommand>("Exception.Handle.Register", typeof(MoveCommand), "Exception.Handle.Exception.Default", mockHandler.Object).Execute();

        var imovable = new Mock<IMovable>();

        object b = new MoveCommand(imovable.Object).GetType();
        var c = a[b];

    }
}
