using Hwdtech;
using Hwdtech.Ioc;
using Moq;

namespace SpaceBattle.Lib.Test;

public class CommandHandlerTests
{
    public CommandHandlerTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        var mockHandler = new Mock<IStrategy>();
        mockHandler.Setup(x => x.Run(It.IsAny<object[]>())).Returns(new Mock<IStrategy>().Object);

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Strategies.FindExceptionHandler", (object[] args) => 
        mockHandler.Object
        ).Execute();
    }
    [Fact]
    public void successfulHandlerInit()
    {
        var mockCmd = new Mock<ICommand>();
        CommandExceptionsHandler mockCmdHandler = new CommandExceptionsHandler(mockCmd.Object);
    }
    [Fact]
    public void successfulHandlerExecutingCommand()
    {
        var mockCmd = new Mock<ICommand>();
        CommandExceptionsHandler mockCmdHandler = new CommandExceptionsHandler(mockCmd.Object);
        mockCmdHandler.Execute();
    }
    [Fact]
    public void successfulHandlerCatchesException()
    {
        var mockCmd = new Mock<ICommand>();
        mockCmd.Setup(x => x.Execute()).Throws<Exception>();
        CommandExceptionsHandler cmdHandler = new CommandExceptionsHandler(mockCmd.Object);
        cmdHandler.Execute();
    }
}
