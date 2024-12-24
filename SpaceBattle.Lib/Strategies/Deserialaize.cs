using Hwdtech;
namespace SpaceBattle.Lib;

public class GameDeserializer{

    public static ICommand Deserialize(string serializedGame){
        string[] serializedData = serializedGame.Split('|');
        Dictionary<string, object> gameObjects = new();
        Queue<ICommand> gameQueue = new();
        foreach(string propertyData in serializedData[0].Split(';')){
            if (propertyData.Contains(':'))
            {
                string key = propertyData.Split(" : ")[0];
                string stringValue = propertyData.Split(" : ")[1];

                object objectValue = Hwdtech.IoC.Resolve<object>("DeserializeValue", stringValue);

                gameObjects[key] = objectValue;
            }
        }
        foreach(string commandData in serializedData[1].Split(',')){
            if(commandData.Contains("type"))
            {
                ICommand deserializedCommand = Hwdtech.IoC.Resolve<ICommand>("DeserializeCommand", commandData);

                gameQueue.Enqueue(deserializedCommand);            
            }
        }
        TimeSpan timespan = Hwdtech.IoC.Resolve<TimeSpan>("DeserializeTimespan", serializedData[2]);
        ICommand newGameCommand = Hwdtech.IoC.Resolve<ICommand>("CreateNewGameCommand", gameObjects, gameQueue, timespan);
        return newGameCommand;
    }
}