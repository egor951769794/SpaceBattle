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
    public void executeSagaSuccessInvocationsTest()
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
    public void executeSagaFailInvocationsTest()
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

        mockIWAdapter.VerifyGet(x => x.fuelLevel, Times.Exactly(2));
        mockIWAdapter.VerifyGet(x => x.fuelConsumption, Times.Exactly(2));
        mockIMAdapter.VerifyGet(x => x.position, Times.Exactly(1));
        mockIMAdapter.VerifyGet(x => x.speed, Times.Exactly(0));
    }
    [Fact]
    public void executeSagaSuccessValuesCheck()
    {
        var obj = new TestObject(new Dictionary<string, object>());
        obj.setProperty("position", new Vector(12, 5));
        obj.setProperty("speed", new Vector(-7, 3));
        obj.setProperty("fuelLevel", (float) 100);
        obj.setProperty("fuelConsumption", (float) 1);

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.MoveCommand", (object[] args) => new ActionCommand(
            () =>
            {
                obj.setProperty("position", (Vector) obj.getProperty("position") + (Vector) obj.getProperty("speed"));
            }
        )).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.WasteFuelCommand", (object[] args) => new ActionCommand(
            () =>
            {
                obj.setProperty("fuelLevel", (float) obj.getProperty("fuelLevel") - (float) obj.getProperty("fuelConsumption"));
            }
        )).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.Undo.MoveCommand", (object[] args) => new ActionCommand(
            () =>
            {
                obj.setProperty("position", (Vector) obj.getProperty("position") - (Vector) obj.getProperty("speed"));
            }
        )).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.Undo.WasteFuelCommand", (object[] args) => new ActionCommand(
            () =>
            {
                obj.setProperty("fuelLevel", (float) obj.getProperty("fuelLevel") + (float) obj.getProperty("fuelConsumption"));
            }
        )).Execute();

        SagaCommand sc = (SagaCommand) new CreateSaga().Run("WasteFuelCommand", "MoveCommand", obj);
        sc.Execute();

        Assert.Equal(99, (float) obj.getProperty("fuelLevel"));
        Assert.Equal(5, ((Vector) obj.getProperty("position"))[0]);
        Assert.Equal(8, ((Vector) obj.getProperty("position"))[1]);
    }
    [Fact]
    public void executeSagaFailValuesCheck()
    {
        var obj = new TestObject(new Dictionary<string, object>());
        obj.setProperty("position", new Vector(12, 5));
        obj.setProperty("speed", new Vector(-7, 3));
        obj.setProperty("fuelLevel", (float) 100);
        obj.setProperty("fuelConsumption", (float) 1);

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.MoveCommand", (object[] args) => new ActionCommand(
            () =>
            {
                throw new Exception();
            }
        )).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.WasteFuelCommand", (object[] args) => new ActionCommand(
            () =>
            {
                obj.setProperty("fuelLevel", (float) obj.getProperty("fuelLevel") - (float) obj.getProperty("fuelConsumption"));
            }
        )).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.Undo.MoveCommand", (object[] args) => new ActionCommand(
            () =>
            {
                obj.setProperty("position", (Vector) obj.getProperty("position") - (Vector) obj.getProperty("speed"));
            }
        )).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.Undo.WasteFuelCommand", (object[] args) => new ActionCommand(
            () =>
            {
                obj.setProperty("fuelLevel", (float) obj.getProperty("fuelLevel") + (float) obj.getProperty("fuelConsumption"));
            }
        )).Execute();

        SagaCommand sc = (SagaCommand) new CreateSaga().Run("WasteFuelCommand", "MoveCommand", obj);
        sc.Execute();

        Assert.Equal(100, (float) obj.getProperty("fuelLevel"));
        Assert.Equal(12, ((Vector) obj.getProperty("position"))[0]);
        Assert.Equal(5, ((Vector) obj.getProperty("position"))[1]);
    }
}
