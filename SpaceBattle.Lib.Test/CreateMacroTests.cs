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
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.GetCommand.WithArgs", (object[] args) => new Mock<IStrategy>().Object).Execute();
    }
    [Fact]
    public void successfulCreateMacro()
    {
        var newMacro = new Mock<CreateMacro>();
        newMacro.Object.Run(new object[] {It.IsAny<string>(), new object()});
    }
    // [Fact]
    // public void unsuccessfulCreateMacroNameThrowsException()
    // {
    //     var newMacro = new Mock<CreateMacro>();
    //     var cmdName = new Mock<List<string>>();
    //     cmdName.SetupGet(x => x[It.IsAny<int>()]).Throws<Exception>();
    //     Assert.Throws<Exception>(() => newMacro.Object.Run(new object[] {cmdName.Object[0], new object()}));
    // }
}
