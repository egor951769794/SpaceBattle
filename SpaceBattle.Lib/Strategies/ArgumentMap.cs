namespace SpaceBattle.Lib;
using Google.Protobuf.Collections;
using System.Collections.Generic;
public class ArgumentMapStrategy: IStrategy{

   public object Run(params object[] args){

        MapField<string, string> protoMap = (MapField<string, string>)args[0];

        Dictionary<string, object> newDict = new();

        foreach(string key in protoMap.Keys)
        {
            newDict[key] = (object) protoMap[key];
        }

        return newDict;
   }
}