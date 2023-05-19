using Moq;
using Hwdtech;
using Hwdtech.Ioc;


namespace SpaceBattle.Lib.Test;

public class GameObjectsTests
{
    public GameObjectsTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
    }
    [Fact]
    public void getItemTest()
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

        var mockObj = new Mock<UObject>();

        IoC.Resolve<Dictionary<string, UObject>>("General.Objects").Add("0", mockObj.Object);

        var resolvedObj = IoC.Resolve<UObject>("General.GetItem", "0");

        Assert.Equal(mockObj.Object, resolvedObj);
    }
    [Fact]
    public void removeItemTest()
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

        var mockObj = new Mock<UObject>();

        IoC.Resolve<Dictionary<string, UObject>>("General.Objects").Add("0", mockObj.Object);
        Assert.True( IoC.Resolve<Dictionary<string, UObject>>("General.Objects").Count() == 1);

        IoC.Resolve<ICommand>("General.RemoveItem", "0").Execute();
        Assert.True( IoC.Resolve<Dictionary<string, UObject>>("General.Objects").Count() == 0);
    }
}