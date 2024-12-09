using System.Collections.Concurrent;
using Hwdtech;


namespace SpaceBattle.Lib;

public class ShardedThreadsMessagesInterpreter
{
    private ConcurrentDictionary<string, List<string>> threadsGamesDict;
    private IStrategy interpretStrategy; 
    // private Thread messageThread;
    // private BlockingCollection<IMessage> messages;
 
    public ShardedThreadsMessagesInterpreter(ConcurrentDictionary<string, List<string>> threadGames, IStrategy interpretStrategy)
    {
        threadsGamesDict = threadGames;
        this.interpretStrategy = interpretStrategy;
        // messageThread = new Thread(() => interpretMessage());
        // messages = new BlockingCollection<IMessage>();
    }
    // public void Send(IMessage msg)
    // {
    //     messages.Add(msg);
    // }

    // public void initializeNewShardedThread()
    // {
    //     ServerThreadConcurrent thread = (ServerThreadConcurrent) IoC.Resolve<IStrategy>("Threading.ConcurrentThread.New").Run();
    //     string threadId = IoC.Resolve<string>("Threading.ConcurrentThread.GetId", thread);
    //     threadsGamesDict[threadId] = new List<string>();
    // }
    public void interpretMessage(IMessage msg)
    {
        // if (!messages.IsEmpty())
        // {
        //     IMessage msg = 
            // InterpretCommand intprCmd = new InterpretCommand(msg);
            // string threadId = threadsGamesDict.ElementAt(rnd.Next(0, threadsGamesDict.Count())).Key;
            // IoC.Resolve<IStrategy>("Threading.ConcurrentThread.Send", threadId).Run(intprCmd);
        // }
        // foreach (var msg in messages.GetConsumingEnumerable())
        // {
            // InterpretCommand intprCmd = new InterpretCommand(msg);
            // intprCmd.Execute();/
            string threadId = threadsGamesDict.First(x => x.Value.Contains(msg.Gameid)).Key;
            // InterpretCommand intprCmd = new InterpretCommand(msg);
            // string threadId = threadsGamesDict.ElementAt(new Random().Next(0, threadsGamesDict.Count())).Key;
            interpretStrategy.Run(threadId, msg);
            // IoC.Resolve<IStrategy>("Threading.ConcurrentThread.SendMessage", threadId).Run(intprCmd);
        // }
    }
    // public void Start()
    // {
    //     messageThread.Start();
    // }
    // public void Stop()
    // {
    //     messages.CompleteAdding();
    //     messageThread.Join();
    // }
}
