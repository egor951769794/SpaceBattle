using Moq;
using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib.Test;

public class CheckCollisionCommandTests
{
    public CheckCollisionCommandTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

    }
    [Fact]
    public void successfulCollisionCheckObjectsCollided()
    {
        var MockObj1 = new Mock<UObject>();
        var MockObj2 = new Mock<UObject>();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "General.Collision.Check", (object[] args) => (object) true).Execute();
        ICommand checkCollisionCommand = new CheckCollisionCommand(MockObj1.Object, MockObj2.Object);
        try
        {
            checkCollisionCommand.Execute();
        }
        catch (Exception e)
        {
            Assert.Equal(String.Format("collision : [{0}, {1}]", MockObj1.Object.ToString(), MockObj2.Object.ToString()), e.Message);
        }
    }
    [Fact]
    public void successfulCollisionCheckObjectsNotCollided()
    {
        var MockObj1 = new Mock<UObject>();
        var MockObj2 = new Mock<UObject>();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "General.Collision.Check", (object[] args) => (object) false).Execute();
        ICommand checkCollisionCommand = new CheckCollisionCommand(MockObj1.Object, MockObj2.Object);
    }
}
