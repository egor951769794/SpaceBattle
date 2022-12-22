using Moq;
using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib.Test;

public class MacroCommandTests
{
    public MacroCommandTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Dependant_Command", (object[] args) => new Mock<SpaceBattle.Lib.ICommand>().Object).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.GetDependantCommandNames", (object[] args) => new List<string> {"Dependant_Command"}).Execute();
    }
    [Fact]
    public void successfulMacro()
    {
        var commandsList = IoC.Resolve<List<string>>("Commands.GetDependantCommandNames", new object());
        var commands = new List<ICommand>();
        commandsList.ForEach(cmdName => commands.Add(IoC.Resolve<ICommand>(cmdName, new object())));

        var macroCmd = new MacroCommand(commands);
        macroCmd.Execute();
    }
}