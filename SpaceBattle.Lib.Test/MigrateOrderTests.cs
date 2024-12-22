using Moq;
using Hwdtech;
using Hwdtech.Ioc;
using System.Text;
using System.Net.Http;
using System.Net.Http.Json;

namespace SpaceBattle.Lib.Test;

public class MigrateOrderTests
{
    public MigrateOrderTests()
    {
        
    }
    [Fact]
    public void sendOrderTest()
    {
        string gameId = "game0";
        string serverUrl = "http://localhost:5000";
        string threadId = "th0";

        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("Game-Id", gameId);
            MigrateOrderBody order = new MigrateOrderBody(serverUrl, threadId);
            HttpContent orderContent = JsonContent.Create(order);
            client.PostAsync(serverUrl + "/game/migrateOrder", orderContent).GetAwaiter().GetResult();
            
        }
    }
}
