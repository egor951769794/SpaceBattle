using Grpc.Core;
using SpaceBattle.Lib;
using IRouter = SpaceBattle.Lib.IRouter;

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
}
