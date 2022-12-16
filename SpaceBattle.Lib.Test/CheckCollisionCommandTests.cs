using Moq;
using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib.Test;

public class CheckCollisionCommandTests
{
    public CheckCollisionCommandTests()
    {
        new InitScopeBasedIoCImplementationCommand();

        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "General.SetProperty", (object[] args) => new Mock<ICommand>().Object).Execute();


    }
    [Fact]
    public void successfulCollisionCheck()
    {
        var MockObj1 = new Mock<UObject>();
        var MockObj2 = new Mock<UObject>();

        ICommand checkCollisionCommand = new CheckCollisionCommand(MockObj1.Object, MockObj2.Object);
        checkCollisionCommand.Execute();
    }
}