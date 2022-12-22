using Moq;

namespace SpaceBattle.Lib.Test;

public class MacroCommandTests
{
    [Fact]
    public void successfulMacro()
    {
        var mockCommands = new Mock<List<ICommand>>();
        var macroCmd = new MacroCommand(mockCommands.Object);
        macroCmd.Execute();
    }
}