using Moq;
using Hwdtech;
using Hwdtech.Ioc;
using System.Net.Http.Json;
using System.Collections.Concurrent;
using System.Text.Json;

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
    [Fact]
    public void serializingTests()
    {
        BlockingCollection<ICommand> gameQueue1 = new BlockingCollection<ICommand>();

        BlockingCollection<ICommand> msgQueue1 = new BlockingCollection<ICommand>();

        ThreadMessageSenderAdapter msa1 = new ThreadMessageSenderAdapter(msgQueue1);
        Dictionary<string, ThreadMessageSenderAdapter> msgSenders = new Dictionary<string, ThreadMessageSenderAdapter>();
        msgSenders["th0"] = msa1;


        ConcurrentDictionary<string, ICommand> games = new ConcurrentDictionary<string, ICommand>();
        
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.GetAll", (object[] args) => games).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Threading.Get.MessageSender", (object[] args) => msgSenders[(string) args[0]]).Execute();


        ReceiverAdapter gra1 = new ReceiverAdapter(gameQueue1);
        ISender sra1 = new ThreadMessageSenderAdapter(gameQueue1);
        Dictionary<string, ISender> gameSenders = new Dictionary<string, ISender>();
        gameSenders["th0"] = sra1;

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Threading.Get.GameSender", (object[] args) => gameSenders[(string) args[0]]).Execute();

        ReceiverAdapter mra1 = new ReceiverAdapter(msgQueue1);

        ServerThread thread1 = new ServerThread(gra1, mra1);

        Dictionary<string, ServerThread> threads = new Dictionary<string, ServerThread>();
        threads["th0"] = thread1;

        ICommand game = new GameCommand(new object(), new BlockingCollection<ICommand>());
        string serializedGame = JsonSerializer.Serialize(game);
        ICommand deserializeCmd = new DeserializeGameCommand(serializedGame, "th0");
        deserializeCmd.Execute();

        Assert.Equal(1, gameQueue1.Count);
    }
}
