using Grpc.Core;
using Hwdtech;
using SpaceBattle.Lib;
using IRouter = SpaceBattle.Lib.IRouter;
using Grpc.Net.Client;

namespace SpaceBattle.Endpoint;

public class EndpointGameService : Endpoint.EndpointBase
{
    private IRouter _router;

    public EndpointGameService(IRouter router)
    {
        _router = router;
    }

    public override Task<EndpointReply> EndpointReceiver(EndpointRequest request, ServerCallContext context)
    {
        bool status = false;

        Dictionary<string, object> data = (Dictionary<string, object>) new ArgumentMapStrategy().Run(request.Args);

        string gameId = (string)data["gameid"];
        string command = (string)data["command"];

        if (_router.Route(gameId, command, data))
        {
            status = true;
        }

        return Task.FromResult(new EndpointReply
        {
            Status = status
        });
    }

    public async override Task<TransferReply> Transfer(TransferRequest request, ServerCallContext context)
    {
        string gameId = request.GameId;
        string serializedGame = (string)GameSerializer.Serialize(gameId);

        Client.Client.Call(request.NewServerId, serializedGame);

        return await Task.FromResult(new TransferReply
        {
            GameStatus = true
        });
    }

    public override Task<LoadReply> Load(LoadRequest request, ServerCallContext context)
    {
        string serializedGame = request.SerializedGame;
        Lib.ICommand newGameCommand = (Lib.ICommand)GameDeserializer.Deserialize(serializedGame);
        IoC.Resolve<Hwdtech.ICommand>("AddGame", "1", newGameCommand).Execute();
        return Task.FromResult(new LoadReply
        {
            UploadStatus = true
        });
    }
}
