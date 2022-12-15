using Moq;
using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib.Test;

public class StartMoveCommandTests
{
    public StartMoveCommandTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
        
        var mockCommand = new Mock<ICommand>();
        mockCommand.Setup(x => x.Execute());

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "General.SetProperty", (object[] args) => new Mock<ICommand>().Object).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.Movement", (object[] args) => new Mock<ICommand>().Object).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Queue.Push", (object[] args) => new Mock<ICommand>().Object).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Queue.Main", (object[] args) => new Queue<ICommand>()).Execute();
    }

    [Fact]
    public void succesfulStartMoveCommand()
    {
        var InitMock = new Mock<IStartingMoveCommand>();
        InitMock.SetupGet(a => a.objToMove).Returns(new Mock<UObject>().Object).Verifiable();
        InitMock.SetupGet(a => a.order).Returns(new Dictionary<string, object>() { { "speed", new Vector(It.IsAny<int>(), It.IsAny<int>()) } }).Verifiable();
        ICommand startMoveCommand = new StartMoveCommand(InitMock.Object);
        startMoveCommand.Execute();
        InitMock.Verify();
    }
}