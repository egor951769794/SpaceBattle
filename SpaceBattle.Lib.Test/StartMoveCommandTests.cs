using Moq;
using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib.Test;

public class StartMoveCommandTests {

    public StartMoveCommandTests() {
        
        new InitScopeBasedIoCImplementationCommand().Execute();

        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        var mockCommand = new Mock<ICommand>();
        mockCommand.Setup(x => x.Execute());

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "NotExisting.GetSomePropertyMethod", (object[] args) => new Mock<UObject>()).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "General.UObj.SetProperty", (object[] args) => mockCommand.Object).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "General.UObj.GetProperty", (object[] args) => new Mock<ICommand>().Object).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.Movement", (object[] args) => mockCommand.Object).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Queue.Push", (object[] args) => mockCommand.Object).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Queue.Main", (object[] args) => new Queue<ICommand>()).Execute();

    }

    [Fact]

    public void success() 
    {
        
        var newOrder = new Mock<IStartingMoveCommand>();
        var obj = new Mock<UObject>();

        newOrder.SetupGet(x => x.objToMove).Returns(obj.Object).Verifiable();
        newOrder.SetupGet(x => x.order).Returns(new Dictionary<string, object> {
            // {"id", 0},
            {"speed", new Vector(0, 0)},
        }).Verifiable();

        ICommand startMoveCommand = new StartMoveCommand(newOrder.Object);

        startMoveCommand.Execute();

        newOrder.VerifyAll();
    }
}