using Hwdtech;
using Hwdtech.Ioc;
using Moq;


namespace SpaceBattle.Lib.Test;

public class ScopeTests
{
    public ScopeTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
    }
    [Fact]
    public void currentScopeIsEmpty()
    {
        object scope = new InitGameScope().Run(500);

        Assert.Throws<ArgumentException>(
            () =>
            {
                IoC.Resolve<int>("GetQuantum");
            }
        );
        Assert.Throws<ArgumentException>(
            () =>
            {
                IoC.Resolve<ICommand>("QueueDequeue");
            }
        );
        Assert.Throws<ArgumentException>(
            () =>
            {
                IoC.Resolve<ICommand>("QueueEnqueue");
            }
        );
    }
    [Fact]
    public void CreateNewGameTest()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.GameCommand", (object[] args) => new ActionCommand(
            () =>
            {
                IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", args[0]).Execute();
            }
        )).Execute();

        ICommand gameCommand = (ICommand) new CreateNewGame().Run();
        gameCommand.Execute();

        Assert.Equal(500, IoC.Resolve<int>("GetQuantum"));
        Assert.True(IoC.Resolve<Dictionary<string, UObject>>("General.Objects").Count() == 0);
        Assert.Throws<Exception>(
            () =>
            {
                IoC.Resolve<ICommand>("QueueDequeue", new Queue<ICommand>());
            }
        );
    }
}
