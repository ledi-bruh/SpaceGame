using Hwdtech;

namespace SpaceGame.Lib;


public class InitializationGameDependenciesStrategy : IStrategy //Server.Thread.Game.Dependencies.Initialization
{
    public object Invoke(params object[] args)
    {
        var cmdRegisterCommands = IoC.Resolve<SpaceGame.Lib.ICommand>("Game.Register.Commands");

        var cmdRegisterOperationCreation = new ActionCommand(() =>
        {
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Macro.Create.FromDependencies", (object[] args) => new CreateGameMacroFromDependenciesStrategy().Invoke(args));
        });

        return new ActionCommand( () =>
        {
            cmdRegisterCommands.Execute();

            cmdRegisterOperationCreation.Execute();
        }
        );
    }
}
