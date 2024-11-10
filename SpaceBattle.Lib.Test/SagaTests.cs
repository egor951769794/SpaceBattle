using Moq;
using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib.Test;


public class SagaTests
{
    public SagaTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
    }  
    
    [Fact]
    public void executeSagaSuccessTest()
    {
        var obj = new TestObject(new Dictionary<string, object>());
        obj.setProperty("position", new Vector(12, 5));
        obj.setProperty("speed", new Vector(-7, 3));
        obj.setProperty("fuelLevel", (float) 100);
        obj.setProperty("fuelConsumption", (float) 1);

        var mockIMAdapter = new Mock<IMovable>();
        mockIMAdapter.SetupGet(x => x.position).Returns((Vector) obj.getProperty("position"));
        mockIMAdapter.SetupGet(x => x.speed).Returns((Vector) obj.getProperty("speed"));
 
        var mockIWAdapter = new Mock<IFuelChangable>();
        mockIWAdapter.SetupGet(x => x.fuelLevel).Returns((float) obj.getProperty("fuelLevel"));
        mockIWAdapter.SetupGet(x => x.fuelConsumption).Returns((float) obj.getProperty("fuelConsumption"));

        mockIWAdapter.SetupSet(x => x.fuelLevel = 99).Verifiable();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.MoveCommand", (object[] args) => new MoveCommand(mockIMAdapter.Object)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.WasteFuelCommand", (object[] args) => new WasteFuelCommand(mockIWAdapter.Object)).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.Undo.MoveCommand", (object[] args) => new ActionCommand(
            () => 
            {
                mockIMAdapter.Object.position -= mockIMAdapter.Object.speed;
            })
            ).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.Undo.WasteFuelCommand", (object[] args) => new ActionCommand(
            () =>
            {
                mockIWAdapter.Object.fuelLevel += mockIWAdapter.Object.fuelConsumption;
            })
            ).Execute();

        SagaCommand sc = (SagaCommand) new CreateSaga().Run("WasteFuelCommand", "MoveCommand", obj);
        sc.Execute();

        mockIWAdapter.VerifyGet(x => x.fuelLevel, Times.Once);
        mockIWAdapter.VerifyGet(x => x.fuelConsumption, Times.Once);
        mockIMAdapter.VerifyGet(x => x.position, Times.Once);
        mockIMAdapter.VerifyGet(x => x.speed, Times.Once);

        mockIWAdapter.Verify();
    }
    [Fact]
    public void executeSagaFailTest()
    {
        var obj = new TestObject(new Dictionary<string, object>());
        obj.setProperty("position", new Vector(12, 5));
        obj.setProperty("speed", new Vector(-7, 3));
        obj.setProperty("fuelLevel", (float) 100);
        obj.setProperty("fuelConsumption", (float) 1);

        var mockIMAdapter = new Mock<IMovable>();
        mockIMAdapter.SetupGet(x => x.position).Throws<Exception>();
        mockIMAdapter.SetupGet(x => x.speed).Returns((Vector) obj.getProperty("speed"));
 
        var mockIWAdapter = new Mock<IFuelChangable>();
        mockIWAdapter.SetupGet(x => x.fuelLevel).Returns((float) obj.getProperty("fuelLevel"));
        mockIWAdapter.SetupGet(x => x.fuelConsumption).Returns((float) obj.getProperty("fuelConsumption"));

        mockIWAdapter.SetupSet(x => x.fuelLevel = 100).Verifiable();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.MoveCommand", (object[] args) => new MoveCommand(mockIMAdapter.Object)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.WasteFuelCommand", (object[] args) => new WasteFuelCommand(mockIWAdapter.Object)).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.Undo.MoveCommand", (object[] args) => new ActionCommand(
            () => 
            {
                mockIMAdapter.Object.position -= mockIMAdapter.Object.speed;
            })
            ).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.Undo.WasteFuelCommand", (object[] args) => new ActionCommand(
            () =>
            {
                mockIWAdapter.Object.fuelLevel += 10 * mockIWAdapter.Object.fuelConsumption;
            })
            ).Execute();

        SagaCommand sc = (SagaCommand) new CreateSaga().Run("WasteFuelCommand", "MoveCommand", obj);
        sc.Execute();

        mockIWAdapter.VerifyGet(x => x.fuelLevel, Times.Exactly(2));
        mockIWAdapter.VerifyGet(x => x.fuelConsumption, Times.Exactly(2));
        mockIMAdapter.VerifyGet(x => x.position, Times.Exactly(1));
        mockIMAdapter.VerifyGet(x => x.speed, Times.Exactly(0));
    }
}
