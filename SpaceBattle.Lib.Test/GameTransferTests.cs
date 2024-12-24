using Hwdtech;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using Moq;
using SpaceBattle.Lib;

namespace Spaceship.Lib.Test;

public class GameTransitionTests
{
    public void SerializationTest()
    {
        new Hwdtech.Ioc.InitScopeBasedIoCImplementationCommand().Execute();

        var scope = Hwdtech.IoC.Resolve<object>("Scopes.New", Hwdtech.IoC.Resolve<object>("Scopes.Root"));

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", scope).Execute();
        bool exceptionWasHandled = false;
        bool commandExecuted = false;

        Dictionary<string, object> gameObjects = new();

        Queue<SpaceBattle.Lib.ICommand> queue = new();

        TimeSpan ts = new TimeSpan(0, 0, 0, 0, 100);

        Mock<UObject> _obj = new();

        Mock<SpaceBattle.Lib.ICommand> mcmd = new();

        mcmd.Setup(c => c.Execute()).Callback(() => {commandExecuted = true;});

        SpaceBattle.Lib.ICommand cmd = mcmd.Object;

        gameObjects.Add("obj123", _obj);

        SpaceBattle.Lib.ICommand command = new ActionCommand(() => {});

        queue.Enqueue(command);

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register" , "Game.Objects.GetAll", (object[] args) => 
        {
            return gameObjects;
        }).Execute();

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register" , "Game.Queue.Get", (object[] args) => 
        {
            return queue;
        }).Execute();

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register" , "Game.Get.Timespan", (object[] args) => 
        {
            return (object)ts;
        }).Execute();


        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register" , "Game.Current.Timespan", (object[] args) => 
        {
            return (object) ts;
        }).Execute();

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register" , "Game.Current.Queue", (object[] args) => 
        {
            return queue;
        }).Execute();

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register" , "Game.Current.HandleCommand", (object[] args) => 
        {
            Hwdtech.IoC.Resolve<Queue<SpaceBattle.Lib.ICommand>>("Game.Current.Queue").TryDequeue(out cmd!);
            return cmd;
        }).Execute();

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Get.Exception.Source", (object[] args) => 
        {
        
            Exception ex = (Exception)args[0];
            var a = (new StackTrace(ex).GetFrame(0)!.GetMethod()!.ReflectedType)!.FullName;
            return a;
            
        }).Execute();

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Command.GetProps", (object[] args) => 
        {
            Dictionary<string, object> dict = new(){{"key", "string value"}};    
            return dict;        
        }).Execute();

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Command.GetAction", (object[] args) => 
        {
            return "{}";      
        }).Execute();

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "StringifyObject", (object[] args) => 
        {
            string result_string = "";
            object obj = args[0];

            result_string += obj.ToString();

            result_string += Hwdtech.IoC.Resolve<string>("StringifyObjectProps", obj);

            return result_string;      
        }).Execute();

         Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "StringifyObjectProps", (object[] args) => 
        {
            string result_string = "{key=string value, key2=int 123}";
            return result_string;      
        }).Execute();

         Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register" , "SerializeCommand", (object[] args) => 
        {
            SpaceBattle.Lib.ICommand cmd = (SpaceBattle.Lib.ICommand) args[0];

            string return_string = "Command ";

            if(cmd is MoveCommand)
            {
                return_string += "type=move, ";
                foreach(var prop in Hwdtech.IoC.Resolve<Dictionary<string, object>>("Command.GetProps", cmd)){
                    return_string += prop.Key + " : " + prop.Value.ToString();
                }
            }
            if(cmd is ActionCommand){
                return_string += "type=action, ";
                return_string += "action=" + Hwdtech.IoC.Resolve<string>("Command.GetAction", cmd);
            }

            return return_string;
        }).Execute();

        SpaceBattle.Lib.ICommand gameCommand = new GameCommand(scope, queue);

        string serializedGame = GameSerializer.Serialize("1");

        Assert.Equal("obj123 : UObject{key=string value, key2=int 123}; | Command type=action, action={} | 00:00:00.1000000", serializedGame);

    }

    public void DeserializationTest()
    {

        new Hwdtech.Ioc.InitScopeBasedIoCImplementationCommand().Execute();

        var scope = Hwdtech.IoC.Resolve<object>("Scopes.New", Hwdtech.IoC.Resolve<object>("Scopes.Root"));

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", scope).Execute();

        Dictionary<string, object> deserializedGameObjects = new();

        Queue<SpaceBattle.Lib.ICommand> deserializedQueue = new();

        TimeSpan dts = new();

        bool exceptionWasHandled = false;
        bool commandExecuted = false;

        Dictionary<string, object> gameObjects = new();

        Queue<SpaceBattle.Lib.ICommand> queue = new();

        TimeSpan ts = new TimeSpan(0, 0, 0, 0, 100);

        Mock<SpaceBattle.Lib.ICommand> mcmd = new();

        mcmd.Setup(c => c.Execute()).Callback(() => {commandExecuted = true;});

        SpaceBattle.Lib.ICommand cmd = mcmd.Object;

        Mock<UObject> _obj = new();

        gameObjects.Add("obj123", _obj);

        SpaceBattle.Lib.ICommand command = new ActionCommand(() => {});

        queue.Enqueue(command);

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "DeserializeValue", (object[] args) => {
            string serializedString = (string) args[0];
            return (object) _obj;
        }).Execute();

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "DeserializeCommand", (object[] args) => {
            string serializedCommand = (string) args[0];
            return (object) command;
        }).Execute();

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "DeserializeTimespan", (object[] args) => {
            string serializedTimespan = (string) args[0];
            return (object) ts;
        }).Execute();


        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "CreateNewGameCommand", (object[] args) => {
            deserializedGameObjects = (Dictionary<string, object>) args[0];
            deserializedQueue = (Queue<SpaceBattle.Lib.ICommand>) args[1];
            dts = (TimeSpan)args[2];
            return (object)new ActionCommand(() => {}); 
        }).Execute();

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register" , "Game.Objects.GetAll", (object[] args) => 
        {
            return gameObjects;
        }).Execute();

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register" , "Game.Queue.Get", (object[] args) => 
        {
            return queue;
        }).Execute();

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register" , "Game.Get.Timespan", (object[] args) => 
        {
            return (object)ts;
        }).Execute();


        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register" , "Game.Current.Timespan", (object[] args) => 
        {
            return (object) ts;
        }).Execute();

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register" , "Game.Current.Queue", (object[] args) => 
        {
            return queue;
        }).Execute();

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register" , "Game.Current.HandleCommand", (object[] args) => 
        {
            Hwdtech.IoC.Resolve<Queue<SpaceBattle.Lib.ICommand>>("Game.Current.Queue").TryDequeue(out cmd!);
            return cmd;
        }).Execute();

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Get.Exception.Source", (object[] args) => 
        {
        
            Exception ex = (Exception)args[0];
            var a = (new StackTrace(ex).GetFrame(0)!.GetMethod()!.ReflectedType)!.FullName;
            return a;
            
        }).Execute();

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Command.GetProps", (object[] args) => 
        {
            Dictionary<string, object> dict = new(){{"key", "string value"}};    
            return dict;        
        }).Execute();

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Command.GetAction", (object[] args) => 
        {
            return "{}";      
        }).Execute();

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "StringifyObject", (object[] args) => 
        {
            string result_string = "";
            object obj = args[0];

            result_string += obj.ToString();

            result_string += Hwdtech.IoC.Resolve<string>("StringifyObjectProps", obj);

            return result_string;      
        }).Execute();

         Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "StringifyObjectProps", (object[] args) => 
        {
            string result_string = "{key=string value, key2=int 123}";
            return result_string;      
        }).Execute();

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register" , "SerializeCommand", (object[] args) => 
        {
            SpaceBattle.Lib.ICommand cmd = (SpaceBattle.Lib.ICommand) args[0];

            string return_string = "Command ";

            if(cmd is MoveCommand)
            {
                return_string += "type=move, ";
                foreach(var prop in Hwdtech.IoC.Resolve<Dictionary<string, object>>("Command.GetProps", cmd)){
                    return_string += prop.Key + " : " + prop.Value.ToString();
                }
            }

            return return_string;
        }).Execute();

        SpaceBattle.Lib.ICommand gameCommand = new GameCommand(scope, queue);

        string serializedGame = GameSerializer.Serialize("1");

        SpaceBattle.Lib.ICommand deserializedGameCommand = GameDeserializer.Deserialize(serializedGame);

        Assert.Equal("obj123 : UObject{key=string value, key2=int 123}; | Command type=action, action={} | 00:00:00.1000000", serializedGame);
        Assert.Equal(queue, deserializedQueue);
        Assert.Equal(gameObjects, deserializedGameObjects);
        Assert.Equal(ts, dts);
    }
}