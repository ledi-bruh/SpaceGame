using Hwdtech;

namespace SpaceGame.Lib;


public class InitializationGameDependenciesStrategy : IStrategy //Server.Thread.Game.Dependencie.Initialization
{
    public object Invoke(params object[] args)
    {
        var cmdRegisterCommands = IoC.Resolve<SpaceGame.Lib.ICommand>("Game.Register.Commands");

        var cmdRegisterOperationCreation = new ActionCommand(() =>
        {
            IoC.Resolve<ICommand>("IoC.Register", "Game.Operation.Create", new CreateGameOperationStrategy());
        });

        return new ActionCommand( () =>
        {
            cmdRegisterCommands.Execute();

            cmdRegisterOperationCreation.Execute();
        }
        );
    }
}
