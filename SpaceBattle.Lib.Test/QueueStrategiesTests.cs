using Moq;
using Hwdtech;
using Hwdtech.Ioc;


namespace SpaceBattle.Lib.Test;

public class QueueStrategiesTests
{
    public QueueStrategiesTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.GameCommand", (object[] args) => new ActionCommand(
            () =>
            {
                IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", args[0]).Execute();
            }
        )).Execute();
    }
    [Fact]
    public void enqueueTest()
    {
        var queue = new Queue<ICommand>();
        var cmd = new Mock<ICommand>();

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

        IoC.Resolve<ICommand>("QueueEnqueue", queue, cmd.Object).Execute();
        Assert.True(queue.Count() == 1);
    }
    [Fact]
    public void dequeueTest()
    {
        var queue = new Queue<ICommand>();
        var cmd = new Mock<ICommand>();

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

        IoC.Resolve<ICommand>("QueueEnqueue", queue, cmd.Object).Execute();

        var cmd1 = IoC.Resolve<ICommand>("QueueDequeue", queue);
        Assert.Equal(cmd.Object, cmd1);
    }
}
