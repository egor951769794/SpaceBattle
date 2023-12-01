namespace SpaceBattle.Lib.Test;
using Xunit;
using Moq;
using System;
using System.IO;
using System.Collections.Generic;
using Hwdtech;

public class Test_ServerStart
{
    public object globalScope;
    int threadsStartCount = 0;
    int threadsStopCount = 0;
    bool start = false;
    bool stop = false;
    public Test_ServerStart()
    {

        new Hwdtech.Ioc.InitScopeBasedIoCImplementationCommand().Execute();
        var scope = IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"));
        IoC.Resolve<ICommand>("Scopes.Current.Set", scope).Execute();

        IoC.Resolve<ICommand>("IoC.Register", "Thread.CreateAndStartThread", (object[] args) => {
            return new ActionCommand(() => { threadsStartCount++; });
        }).Execute();

        IoC.Resolve<ICommand>("IoC.Register", "Thread.GetDictionary", (object[] args) => {
            Dictionary<string, string> threads = new Dictionary<string, string>() {
                {"1", "Thread1"},
                {"2", "Thread2"},
                {"3", "Thread3"}
            };
            return threads;
        }).Execute();

        IoC.Resolve<ICommand>("IoC.Register", "Thread.SoftStopTheThread", (object[] args) => {
            return new EmptyCommand();
        }).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Threading.SendCommand", (object[] args) => {
            return new ActionCommand(() => { threadsStopCount++; });
        }).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Thread.ConsoleStartServer", (object[] args) => {
            start = true;
            return new EmptyCommand();
        }).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Thread.ConsoleStopServer", (object[] args) => {
            stop = true;
            return new EmptyCommand();
        }).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Exception.GetLogName", (object[] args) => {
            return "error.log";

        }).Execute();

        globalScope = scope;
    }
    [Fact]
    public void CreateAndStartThreadTest()
    {
        int numOfThread = 5;

        var startServerCommand = new StartServerCommand(numOfThread);
        startServerCommand.Execute();

        Assert.Equal(numOfThread, threadsStartCount);
    }
    [Fact]
    public void StopThreadTest()
    {
        Dictionary<string, string> myThreads = IoC.Resolve<Dictionary<string, string>>("Thread.GetDictionary");

        var StopServerCommand = new StopServerCommand();
        StopServerCommand.Execute();

        Assert.Equal(myThreads.Count, threadsStopCount);
    }
    [Fact]
    public void ConsoleTest()
    {
        int numOfThread = 3;
        var args = new[] { "3" };
        var consoleInput = new StringReader("a");
        var consoleOutput = new StringWriter();
        var originalInput = Console.In;
        var originalOutput = Console.Out;
        Console.SetIn(consoleInput);
        Console.SetOut(consoleOutput);

        ServerProgram.Main(args);

        var output = consoleOutput.ToString();
        Console.SetIn(originalInput);
        Console.SetOut(originalOutput);

        Assert.Contains("Процедура запуска сервера...", output);
        IoC.Resolve<SpaceBattle.Lib.ICommand>("Thread.ConsoleStartServer", numOfThread).Execute();
        Assert.True(true == start);
        Assert.Contains("Все потоки успешно запущены", output);
        Assert.Contains("Процедура остановки сервера...", output);
        IoC.Resolve<SpaceBattle.Lib.ICommand>("Thread.ConsoleStopServer").Execute();
        Assert.True(true == stop);
        Assert.Contains("Завершение программы. Нажмите любую клавишу для выхода...", output);
    }
    [Fact]
    public void ExceptionHandlerStrategyTest()
    {
        SpaceBattle.Lib.ICommand command = Mock.Of<SpaceBattle.Lib.ICommand>();
        Exception exception = new Exception("Test exception");
        string logFileName = "error.log";
        string errorMessage = $"Error in command '{command.GetType().Name}': {exception.Message}";

        var strategy = new ExceptionLogCommand(command, exception);

        strategy.Execute();

        Assert.True(File.Exists(logFileName));
        string[] lines = File.ReadAllLines(logFileName);
        Assert.Contains(errorMessage, lines[0]);
    }
}
