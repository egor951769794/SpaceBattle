namespace SpaceBattle.Lib;
using System.Collections.Generic;

public class Router : IRouter{

    private Dictionary<string, ISender> routeDict;

    public Router(Dictionary<string, ISender> dict){
        this.routeDict = dict;
    }

    public bool Route(string id, string command, Dictionary<string, object> args){
        try{
            routeDict[id].Send(new DeserializeCommand(id, command, args));
            return true;
        }
        catch {
            return false;
        }
    }
}