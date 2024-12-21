using Hwdtech;
using System.Text.Json;


namespace SpaceBattle.Lib;

public class DeserializeGameCommand : ICommand
{
    private string gameJson;
    private string threadId;

    public DeserializeGameCommand(string gameJson, string threadId)
    {
        this.gameJson = gameJson;
        this.threadId = threadId;
    }

    public void Execute()
    {
        GameCommand? gameDeserialized = JsonSerializer.Deserialize<GameCommand>(gameJson);
        if (gameDeserialized != null)
        {
            IoC.Resolve<ISender>("Threading.Get.GameSender", threadId).Send(gameDeserialized);
        }
    }
}
