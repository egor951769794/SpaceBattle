using System.Threading.Tasks;
using System;
using Grpc.Net.Client;

namespace SpaceBattle.Client;

public class Client
{
    public static void Call(string ip, string message)
    {
        using var channel = GrpcChannel.ForAddress(ip);
        var client = new Endpoint.EndpointClient(channel);
        var response = client.Load(new LoadRequest { SerializedGame = message });
    }
}