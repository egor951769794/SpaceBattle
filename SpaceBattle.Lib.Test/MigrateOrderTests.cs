using Moq;
using Hwdtech;
using Hwdtech.Ioc;
using System.Net.Http.Json;
using System.Collections.Concurrent;

namespace SpaceBattle.Lib.Test;

public class MigrateOrderTests
{
    public MigrateOrderTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
    }
    [Fact]
    public void sendOrderRequestTest()
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
    [Fact]
    public void sendGameRequestTest()
    {
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Threading.Get.GameRemover", (object[] args) => new Mock<ICommand>().Object).Execute();

        ConcurrentDictionary<string, ICommand> games = new ConcurrentDictionary<string, ICommand>();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.GetAll", (object[] args) => games).Execute();

        ICommand game = new GameCommand(new object(), new BlockingCollection<ICommand>());
        games["game0"] = game;

        ICommand prepareMigration = new PrepareGameMigrationCommand("http://localhost:5000", "th0", "game0", () => {});
        prepareMigration.Execute();
    }
}
