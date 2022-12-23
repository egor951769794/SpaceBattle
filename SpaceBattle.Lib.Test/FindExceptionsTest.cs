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

        var mockHandlers = new Mock<IDictionary<ICommand, IDictionary<Exception, IStrategy>>>();
        mockHandlers.Setup(x => x[It.IsAny<ICommand>()][It.IsAny<Exception>()]).Returns(new Mock<IStrategy>().Object);
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Exceptions.Handlers", (object[] args) =>
            mockHandlers.Object
        ).Execute();
    }
    [Fact]
    public void successfulFindException()
    {
        IStrategy findHandler = new FindExceptionHandler();
        var mockCmd = new Mock<ICommand>();
        var mockExc = new Mock<Exception>();
        IStrategy handler = (IStrategy) findHandler.Run(new object[] {mockCmd.Object, mockExc.Object});
    }
}
