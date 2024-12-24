namespace SpaceBattle.Endpoint;
using Grpc.Core;
using SpaceBattle.Lib;

class SpaceBattleGrpcApp
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        IRouter router = (IRouter) new CreateGameStrategy().Run(2);
        builder.Services.AddSingleton(new EndpointGameService((Lib.IRouter)router));
        builder.Services.AddGrpc();
        var app = builder.Build();
        app.MapGrpcService<EndpointGameService>();
        app.Run();
    }
}