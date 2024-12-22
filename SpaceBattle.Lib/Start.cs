using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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
        BlockingCollection<ICommand> gameQueue2 = new BlockingCollection<ICommand>();

        BlockingCollection<ICommand> msgQueue1 = new BlockingCollection<ICommand>();
        BlockingCollection<ICommand> msgQueue2 = new BlockingCollection<ICommand>();

        ThreadMessageSenderAdapter msa1 = new ThreadMessageSenderAdapter(msgQueue1);
        ThreadMessageSenderAdapter msa2 = new ThreadMessageSenderAdapter(msgQueue2);
        Dictionary<string, ThreadMessageSenderAdapter> msgSenders = new Dictionary<string, ThreadMessageSenderAdapter>();
        msgSenders["th0"] = msa1;
        msgSenders["th1"] = msa2;

        ConcurrentDictionary<string, ICommand> games = new ConcurrentDictionary<string, ICommand>();
        
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.GetAll", (object[] args) => games).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Threading.Get.MessageSender", (object[] args) => msgSenders[(string) args[0]]).Execute();

        ReceiverAdapter gra1 = new ReceiverAdapter(gameQueue1);
        ReceiverAdapter gra2 = new ReceiverAdapter(gameQueue2);

        ReceiverAdapter mra1 = new ReceiverAdapter(msgQueue1);
        ReceiverAdapter mra2 = new ReceiverAdapter(msgQueue2);

        ServerThread thread1 = new ServerThread(gra1, mra1);
        ServerThread thread2 = new ServerThread(gra2, mra2);

        Dictionary<string, ServerThread> threads = new Dictionary<string, ServerThread>();
        threads["th0"] = thread1;
        threads["th1"] = thread2;

        ConcurrentDictionary<string, List<string>> threadsGames = new ConcurrentDictionary<string, List<string>>();
        threadsGames["th0"] = new List<string>();
        threadsGames["th1"] = new List<string>();

        var threadsInterpreter = new ShardedThreadsMessagesInterpreter(threadsGames);

        builder.Services.AddSingleton<ShardedThreadsMessagesInterpreter>(threadsInterpreter);
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