using Moq;
using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib.Test;

public class SagaCommandTest 
{
    [Fact]
    public void SuccessfulSagaCommand()
    {
        Mock<ICommand> command = new(MockBehavior.Strict);
        command.Setup(x => x.Execute()).Verifiable();

        Mock<ICommand> commandCompensating = new(MockBehavior.Strict);
        commandCompensating.Setup(x => x.Execute()).Verifiable();

        List<SagaCommandPair> pairs = new() {
            new(command.Object, commandCompensating.Object)
        };

        SagaCommand transaction = new(pairs);

        transaction.Execute();

        command.Verify();
        commandCompensating.VerifyNoOtherCalls();
    }

    [Fact]
    public void FailedSagaCommand()
    {
        Mock<ICommand> command = new(MockBehavior.Strict);
        command.Setup(x => x.Execute()).Verifiable();
        Mock<ICommand> failedCommand = new(MockBehavior.Strict);
        failedCommand.Setup(x => x.Execute()).Throws<Exception>();
        Mock<ICommand> ignoredCommand = new(MockBehavior.Strict);
        ignoredCommand.Setup(x => x.Execute()).Verifiable();

        Mock<ICommand> commandCompensating = new(MockBehavior.Strict);
        commandCompensating.Setup(x => x.Execute()).Verifiable();
        Mock<ICommand> ignoredCommandCompensating = new(MockBehavior.Strict);
        ignoredCommandCompensating.Setup(x => x.Execute()).Verifiable();

        List<SagaCommandPair> pairs = new() {
            new(command.Object, commandCompensating.Object),
            new(failedCommand.Object, ignoredCommandCompensating.Object),
            new(ignoredCommand.Object, ignoredCommandCompensating.Object)
        };

        SagaCommand transaction = new(pairs);

        Assert.Throws<Exception>(transaction.Execute);

        command.Verify();
        commandCompensating.Verify();
        ignoredCommand.VerifyNoOtherCalls();
        ignoredCommandCompensating.VerifyNoOtherCalls();
    }
    [Fact]
    public void SuccessfulSagaStrategy()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set",
            IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))
        ).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "SagaCommand", (object[] args) => new SagaCommandStrategy().Run(args)).Execute();

        Mock<ICommand> command = new(MockBehavior.Strict);
        command.Setup(x => x.Execute()).Verifiable();

        Mock<ICommand> commandCompensating = new(MockBehavior.Strict);
        commandCompensating.Setup(x => x.Execute()).Verifiable();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Command", (object[] args) => command.Object).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Command.Compensating", (object[] args) => commandCompensating.Object).Execute();

        ICommand transaction = IoC.Resolve<ICommand>("SagaCommand", new List<Tuple<string, object[]>>() {
            Tuple.Create("Command", Array.Empty<object>())
        });

        transaction.Execute();

        command.Verify();
        commandCompensating.VerifyNoOtherCalls();
    }
    [Fact]
    public void FailedSagaStrategy()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set",
            IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))
        ).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "SagaCGIommand", (object[] args) => new SagaCommandStrategy().Run(args)).Execute();

        Mock<ICommand> command = new(MockBehavior.Strict);
        command.Setup(x => x.Execute()).Verifiable();
        Mock<ICommand> failedCommand = new(MockBehavior.Strict);
        failedCommand.Setup(x => x.Execute()).Throws<Exception>();
        Mock<ICommand> ignoredCommand = new(MockBehavior.Strict);
        ignoredCommand.Setup(x => x.Execute()).Verifiable();

        Mock<ICommand> commandCompensating = new(MockBehavior.Strict);
        commandCompensating.Setup(x => x.Execute()).Verifiable();
        Mock<ICommand> ignoredCommandCompensating = new(MockBehavior.Strict);
        ignoredCommandCompensating.Setup(x => x.Execute()).Verifiable();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Command1", (object[] args) => command.Object).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Command1.Compensating", (object[] args) => commandCompensating.Object).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Command2", (object[] args) => failedCommand.Object).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Command2.Compensating", (object[] args) => ignoredCommandCompensating.Object).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Command3", (object[] args) => ignoredCommand.Object).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Command3.Compensating", (object[] args) => ignoredCommandCompensating.Object).Execute();

        ICommand transaction = IoC.Resolve<ICommand>("SagaCommand", new List<Tuple<string, object[]>>() {
            Tuple.Create("Command1", Array.Empty<object>()),
            Tuple.Create("Command2", Array.Empty<object>()),
            Tuple.Create("Command3", Array.Empty<object>())
        });

        Assert.Throws<Exception>(transaction.Execute);

        command.Verify();
        commandCompensating.Verify();
        ignoredCommand.VerifyNoOtherCalls();
        ignoredCommandCompensating.VerifyNoOtherCalls();
    }
}
