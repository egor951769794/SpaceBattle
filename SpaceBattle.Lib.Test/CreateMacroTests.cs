
namespace SpaceBattle.Lib.Test;
using Hwdtech;
using Hwdtech.Ioc;
using Moq;

public class CreateMacroTests
{
    public CreateMacroTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.GetDependantCommandNames", (object[] args) => new Mock<List<string>>().Object).Execute();
    }
    [Fact]
    public void successfulCreateMacro()
    {
        var newMacro = new Mock<CreateMacro>();
        newMacro.Object.Run(new object[] {It.IsAny<string>(), new object()});
    }
}