using System.Collections.Concurrent;
using Moq;
using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib.Test;

public class ShardedThreadsTests
{

    public ShardedThreadsTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
        int numberOfGames = 0;

        ConcurrentDictionary<string, ConcurrentQueue<SpaceBattle.Lib.ICommand>> gamesQueues = new ConcurrentDictionary<string, ConcurrentQueue<SpaceBattle.Lib.ICommand>>();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Queue.GetAll", (object[] args) => gamesQueues).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.MakeNewId", (object[] args) => "game" + numberOfGames++.ToString()).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.GameCommand", (object[] args) => new GameCommand(args[0], (IEnumerable<ICommand>) args[1])).Execute();
    }
    
    [Fact]
    public void test1()
    {
        BlockingCollection<ICommand> gameQueue1 = new BlockingCollection<ICommand>();
        BlockingCollection<ICommand> gameQueue2 = new BlockingCollection<ICommand>();

        BlockingCollection<ICommand> msgQueue1 = new BlockingCollection<ICommand>();
        BlockingCollection<ICommand> msgQueue2 = new BlockingCollection<ICommand>();

        ReceiverAdapter gra1 = new ReceiverAdapter(gameQueue1);
        ReceiverAdapter gra2 = new ReceiverAdapter(gameQueue2);

        ReceiverAdapter mra1 = new ReceiverAdapter(msgQueue1);
        ReceiverAdapter mra2 = new ReceiverAdapter(msgQueue2);

        ServerThreadConcurrent thread1 = new ServerThreadConcurrent(gra1, mra1);
        ServerThreadConcurrent thread2 = new ServerThreadConcurrent(gra2, mra2);

        ConcurrentDictionary<string, List<string>> threadsGames = new ConcurrentDictionary<string, List<string>>();
        threadsGames["th1"] = new List<string>();
        threadsGames["th2"] = new List<string>();

        new CreateNewGameConcurrent().Run();
        
        Assert.Equal(1, IoC.Resolve<ConcurrentDictionary<string, ConcurrentQueue<SpaceBattle.Lib.ICommand>>>("Game.Queue.GetAll").Count);
        Assert.Equal(0, IoC.Resolve<ConcurrentDictionary<string, ConcurrentQueue<SpaceBattle.Lib.ICommand>>>("Game.Queue.GetAll")["game0"].Count);

        new CreateNewGameConcurrent().Run();

        Assert.Equal(0, IoC.Resolve<ConcurrentDictionary<string, ConcurrentQueue<SpaceBattle.Lib.ICommand>>>("Game.Queue.GetAll")["game1"].Count);
        Assert.Equal(2, IoC.Resolve<ConcurrentDictionary<string, ConcurrentQueue<SpaceBattle.Lib.ICommand>>>("Game.Queue.GetAll").Count);


        threadsGames["th1"].Add("game0");
        threadsGames["th2"].Add("game1");

        Mock<IMessage> mockMsg = new Mock<IMessage>();
        mockMsg.Setup(x => x.Gameid).Returns("game0");
        Mock<IStrategy> mockStrat = new Mock<IStrategy>();
        mockStrat.Setup(x => x.Run("th1", mockMsg.Object)).Verifiable();

        ShardedThreadsMessagesInterpreter shThreads = new ShardedThreadsMessagesInterpreter(threadsGames, mockStrat.Object);

        shThreads.interpretMessage(mockMsg.Object);

        mockStrat.Verify();
    }
}