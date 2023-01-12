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

        var mockHandlerStrategy = new Mock<IStrategy>();
        mockHandlerStrategy.Setup(x => x.Run()).Returns("handler executed");

        var mockHandler = new Mock<IStrategy>();
        mockHandler.Setup(x => x.Run(It.IsAny<object[]>())).Returns(mockHandlerStrategy.Object);

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Strategies.FindExceptionHandler", (object[] args) => 
        mockHandler.Object
        ).Execute();

        var mockHandlers = new Mock<IDictionary<string, IDictionary<string, IStrategy>>>();
        mockHandlers.Setup(x => x["MoveCommand"][It.IsAny<string>()]).Returns(mockHandlerStrategy.Object);
        
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Exceptions.Handlers", (object[] args) =>
            mockHandlers.Object
        ).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.ToString", (object[] args) => "MoveCommand").Execute();
    }
    [Fact]
    public void regularCommandThrowsException()
    {
        var badObjToMove = new Mock<IMovable>();
        badObjToMove.SetupProperty(x => x.position);
        badObjToMove.SetupGet(x => x.speed).Throws<Exception>();
        badObjToMove.Object.position = new Vector(12, -7);

        ICommand moveCmd = new MoveCommand(badObjToMove.Object);
        Assert.Throws<Exception>(() => moveCmd.Execute());
    }
    [Fact]
    public void successfulHandlerCatchesException()
    {
        var badObjToMove = new Mock<IMovable>();
        badObjToMove.SetupProperty(x => x.position);
        badObjToMove.SetupGet(x => x.speed).Throws<Exception>();
        badObjToMove.Object.position = new Vector(12, -7);

        ICommand moveCmd = new MoveCommand(badObjToMove.Object);
        CommandExceptionsHandler moveCmdExc = new CommandExceptionsHandler(moveCmd);
        moveCmdExc.Execute();
    }
}
