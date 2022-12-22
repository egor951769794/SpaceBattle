using Hwdtech;
using Hwdtech.Ioc;
using Moq;

namespace SpaceBattle.Lib.Test;


public class CreateMacroTests
{
    public CreateMacroTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.GetDependantCommandNames", (object[] args) => new List<string> {"Dependant_Command"}).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Dependant_Command", (object[] args) => new Mock<SpaceBattle.Lib.ICommand>().Object).Execute();
    }
    [Fact]
    public void successfulCreateMacro()
    {
        var newMacro = new Mock<CreateMacro>();
        newMacro.Object.Run(new object[] {It.IsAny<string>(), new object()});
    }
}
