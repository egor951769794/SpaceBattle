using Hwdtech;
using System.Collections.Concurrent;
using System.Text;
using System.Text.Json;


namespace SpaceBattle.Lib;

public class PrepareGameMigrationCommand : ICommand
{
    private string serverUrl;
    private string gameId;
    private string threadId;
    private Action finishingBehaviour;

    public PrepareGameMigrationCommand(string serverUrl, string threadId, string gameId, Action finishBeh)
    {
        this.serverUrl = serverUrl;
        this.gameId = gameId;
        this.threadId = threadId;
        this.finishingBehaviour = finishBeh;
    }

    public void Execute()
    {
        GameCommand targetGame = (GameCommand) IoC.Resolve<ConcurrentDictionary<string, ICommand>>("Game.GetAll")[gameId];
        IoC.Resolve<ICommand>("Threading.Get.GameRemover", threadId, gameId).Execute();
        finishingBehaviour();
        string serializedGame = JsonSerializer.Serialize(targetGame);

        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("Thread-Id", threadId);
            HttpContent game = new StringContent(serializedGame, Encoding.UTF8, "application/json");
            client.PostAsync(serverUrl, game).GetAwaiter().GetResult();
        }
    }
}
