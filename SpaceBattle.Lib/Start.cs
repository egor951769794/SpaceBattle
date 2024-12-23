using Microsoft.AspNetCore.Server.Kestrel.Core;
using Hwdtech;
using Hwdtech.Ioc;
using System.Collections.Concurrent;


namespace SpaceBattle.Lib;


public class Start
{
    public static void Main(string[] args)
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
		IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set",IoC.Resolve<object>("Scopes.Root")).Execute();
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.Configure<KestrelServerOptions>(options =>
		{
			options.AllowSynchronousIO = true;
		});

        BlockingCollection<ICommand> gameQueue1 = new BlockingCollection<ICommand>();

        BlockingCollection<ICommand> msgQueue1 = new BlockingCollection<ICommand>();

        ThreadMessageSenderAdapter msa1 = new ThreadMessageSenderAdapter(msgQueue1);
        Dictionary<string, ThreadMessageSenderAdapter> msgSenders = new Dictionary<string, ThreadMessageSenderAdapter>();
        msgSenders["th0"] = msa1;

        ConcurrentDictionary<string, ICommand> games = new ConcurrentDictionary<string, ICommand>();
        
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.GetAll", (object[] args) => games).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Threading.Get.MessageSender", (object[] args) => msgSenders[(string) args[0]]).Execute();

        ReceiverAdapter gra1 = new ReceiverAdapter(gameQueue1);

        ReceiverAdapter mra1 = new ReceiverAdapter(msgQueue1);

        ServerThread thread1 = new ServerThread(gra1, mra1);

        Dictionary<string, ServerThread> threads = new Dictionary<string, ServerThread>();
        threads["th0"] = thread1;

        ConcurrentDictionary<string, List<string>> threadsGames = new ConcurrentDictionary<string, List<string>>();
        threadsGames["th0"] = new List<string>();

        var threadsInterpreter = new ShardedThreadsMessagesInterpreter(threadsGames);

        builder.Services.AddSingleton(threadsInterpreter);
        builder.Services.AddRazorPages();
		builder.Services.AddControllers();

        var app = builder.Build();

		app.UseRouting();
		app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        app.Use((context, next) =>
		{
			context.Request.EnableBuffering();
			return next();
		});


		app.Run();
    }
}