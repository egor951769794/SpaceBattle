using Hwdtech;
using Hwdtech.Ioc;
using Moq;

namespace SpaceBattle.Lib.Test;
public class FindExceptionsTest
{
    public FindExceptionsTest()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        var mockHandlerStrategy = new Mock<IStrategy>();
        mockHandlerStrategy.Setup(x => x.Run()).Returns("handler executed");

        var mockHandlers = new Mock<IDictionary<string, IDictionary<string, IStrategy>>>();
        mockHandlers.Setup(x => x["MoveCommand"][It.IsAny<string>()]).Returns(mockHandlerStrategy.Object);
        
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Exceptions.Handlers", (object[] args) =>
            mockHandlers.Object
        ).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.ToString", (object[] args) => "MoveCommand").Execute();
    }
    [Fact]
    public void successfulFindException()
    {
        var badObjToMove = new Mock<IMovable>();
        badObjToMove.SetupProperty(x => x.position);
        badObjToMove.SetupGet(x => x.speed).Throws<Exception>();

        IStrategy findHandler = new FindExceptionHandler();
        ICommand moveCmd = new MoveCommand(badObjToMove.Object);
        var mockExc = new Mock<Exception>();
        IStrategy handler = (IStrategy) findHandler.Run(new object[] {moveCmd, mockExc.Object});
        Assert.Equal("handler executed", handler.Run());
    }
}
