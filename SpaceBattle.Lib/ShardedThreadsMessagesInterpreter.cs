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
        ISender threadMsgSender = IoC.Resolve<ThreadMessageSenderAdapter>("Threading.Get.MessageSender", threadId);
        ICommand intCmd = new InterpretThreadMessageCommand(msg);
        threadMsgSender.Send(intCmd);
    }

    public void sendSerializedGame(string gameJson, string threadId)
    {
        ISender threadMsgSender = IoC.Resolve<ThreadMessageSenderAdapter>("Threading.Get.MessageSender", threadId);
        ICommand deserealizeCmd = new DeserializeGameCommand(gameJson, threadId);
        threadMsgSender.Send(deserealizeCmd);
    }
    public void sendGameMigration(string threadId, string gameId, string serverUrl)
    {
        ISender threadMsgSender = IoC.Resolve<ThreadMessageSenderAdapter>("Threading.Get.MessageSender", threadId);
        ICommand migrateCmd = new PrepareGameMigrationCommand(serverUrl, threadId, gameId, () => {
            threadsGamesDict.First(x => x.Value.Contains(gameId)).Value.Remove(gameId);
        });
        threadMsgSender.Send(migrateCmd);
    }
}
