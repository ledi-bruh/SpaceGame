namespace SpaceGame.Lib.Test;
using Hwdtech;
using Hwdtech.Ioc;

public class TestGameCommand
{
    Dictionary<int, IHandler> exceptionHandlerTree = new Dictionary<int, IHandler>();
    public TestGameCommand()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        IoC.Resolve<ICommand>("IoC.Register", "Exception.Handler.Tree", (object[] args) => exceptionHandlerTree).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Game.Command.Queue.Start", (object[] args) => new StartGameQueueCommandStrategy().Invoke(args)).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Game.Queue.Dequeue", (object[] args) => new GameQueueDequeueStrategy().Invoke(args)).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Exception.Handler.Find", (object[] args) => new DefaultHandler()).Execute();
        IoC.Resolve<ICommand>("Scopes.Current.Set",
            IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))
        ).Execute();
    }

    [Fact]
    public void SuccessfullGameCommandExecute()
    {




        
    }

}
