using Hwdtech;
using Hwdtech.Ioc;
using SpaceBattle.Lib;
using Moq;
using System.Collections.Concurrent;
using Google.Protobuf.Collections;

namespace SpaceBattle.Lib.Test;

public class EndpointGameTests
{
    [Fact]
    public void PositiveRoutingTest()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        BlockingCollection<ICommand> queue = new();
        BlockingCollection<ICommand> orderQueue = new();

        Mock<UObject> obj = new();
        Dictionary<string, Dictionary<string, UObject>> GamesObjects = new();
        Dictionary<string, UObject> game1 = new();
        game1.Add("obj123", obj.Object);
        GamesObjects.Add("1.1", game1);

        ISender snd = new SenderAdapter(orderQueue);
        ISender internalSnd = new SenderAdapter(queue);

        IReceiver rec = new ReceiverAdapter(queue);
        IReceiver orderRec = new ReceiverAdapter(orderQueue);

        Dictionary<string, ISender> internalDicts = new(){{"1", internalSnd}};
        Dictionary<string, ISender> routeDict = new(){{"1", snd}};

        IRouter router = new Router(routeDict);

        Dictionary<string, object> ValueDictionary = new(){{"objid", "obj123"}, {"thread", "1"}, {"velocity", 1}};

        Assert.Empty(orderQueue);

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetOrderSenderByThreadId", (object[] args) => {
            return routeDict[(string)args[0]];
        }).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetInternalSenderByThreadId", (object[] args) => {
            return internalDicts[(string)args[0]];
        }).Execute();

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "CurrentGameId", (object[] args) => {
                return "1";
        }).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Threading.SendCommand", (object[] args) => new ActionCommand(new Action(
            () =>
            {
                int threadId = (int)args[0];
                ICommand cmd = (ICommand)args[1];
                var threads = IoC.Resolve<Dictionary<int, (ServerThread, SenderAdapter)>>("Threading.ServerThreads");
                threads[threadId].Item2.Send((object)cmd);
            }
        ))).Execute();

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Get Object by ids", (object[] args) =>
        {
            string GameID = (string) args[0];
            string ObjectID = (string) args[1];
            UObject obj = GamesObjects[GameID][ObjectID];
            return obj;
        }).Execute();

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "CreateMoveCommand", (object[] args) =>
        {
            Dictionary<string, object> MessageContent = (Dictionary<string, object>) args[1];
            UObject obj = IoC.Resolve<UObject>("Get Object by ids", (string) args[0], MessageContent["objid"]);
            Mock<ICommand> cmd = new();
            return cmd.Object;
        }).Execute();

        router.Route("1", "MoveCommand", ValueDictionary);

        Assert.Empty(orderQueue);
    }

    [Fact]
    public void NegativeRoutingThrowsTest()
    {
        Mock<ISender> snd = new();

        snd.Setup(s => s.Send(It.IsAny<ICommand>())).Throws(new Exception());

        Dictionary<string, ISender> routeDict = new(){{"1", snd.Object}};

        IRouter router = new Router(routeDict);

        Dictionary<string, object> ValueDictionary1 = new(){{"objid", "obj123"}, {"thread", "1"}, {"velocity", 1}};

        Assert.False(router.Route("1", "MoveCommand", ValueDictionary1));
    }

    [Fact]
    public void PositiveArgumentMapStrategyTest()
    {
        MapField<string, string> protoMap = new(){
            ["prop1"] = "propertyValue"
        };

        Dictionary<string, object> dict = (Dictionary<string, object>) new ArgumentMapStrategy().Run(protoMap);

        Assert.True(((string)dict["prop1"]) == protoMap["prop1"]);
    }
}