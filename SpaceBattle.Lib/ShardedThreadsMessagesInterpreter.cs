using System.Collections.Concurrent;
using Hwdtech;


namespace SpaceBattle.Lib;

public class ShardedThreadsMessagesInterpreter
{
    private ConcurrentDictionary<string, List<string>> threadsGamesDict;

    public ShardedThreadsMessagesInterpreter(ConcurrentDictionary<string, List<string>> threadGames)
    {
        threadsGamesDict = threadGames;
    }

    public void sendMessage(IMessage msg)
    {
        string threadId = threadsGamesDict.First(x => x.Value.Contains(msg.Gameid)).Key;
        // interpretStrategy.Run(threadId, msg);
        // InterpretThreadMessageCommand intCmd = new InterpretThreadMessageCommand(msg);
        SenderAdapter threadMsgSender = IoC.Resolve<SenderAdapter>("Threading.Get.MessageSender", threadId);
        threadMsgSender.Send(msg);
    }

}
