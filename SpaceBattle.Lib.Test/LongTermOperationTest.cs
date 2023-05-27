using Hwdtech;
using Hwdtech.Ioc;
using Xunit;
using SpaceBattle.Lib;
using Moq;
using System.Collections.Generic;

public class LongTermCommandTests
{
    public LongTermCommandTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        var mockCommand = new Mock<SpaceBattle.Lib.ICommand>();
        mockCommand.Setup(x => x.Execute());

        var mockStrategyWithParams = new Mock<IStrategy>();
        mockStrategyWithParams.Setup(x => x.Run(It.IsAny<object[]>())).Returns(mockCommand.Object);

        var mockIEnumString = new Mock<IEnumerable<string>>();
        var mockStrategyReturnString = new Mock<IStrategy>();
        mockStrategyReturnString.Setup(x => x.Run(It.IsAny<object[]>())).Returns(mockIEnumString.Object);


        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "CreateMacroCommandStrategy", (object[] args) => mockStrategyWithParams.Object.Run(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "LongTermOperationStrategy", (object[] args) => mockStrategyWithParams.Object.Run(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "SetUpOperation.Moving", (object[] args) => mockStrategyReturnString.Object.Run(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Create.MacroCommand", (object[] args) => mockStrategyWithParams.Object.Run(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command.Inject", (object[] args) => mockStrategyWithParams.Object.Run(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command.Repeat", (object[] args) => mockStrategyWithParams.Object.Run(args)).Execute();
    }

    [Fact]
    public void LongTermOperationStrategyTest()
    {
        IStrategy LongTermOperation = new LongTermOperationStrategy();
        string name = "Moving";
        var obj = new Mock<UObject>();
        Assert.NotNull(LongTermOperation.Run(name, obj.Object));
    }
}