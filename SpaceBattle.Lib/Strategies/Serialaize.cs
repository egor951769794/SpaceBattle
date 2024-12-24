using Hwdtech;

public class GameSerializer{

    public static string Serialize(string gameId){

        string serializedGame = "";

        Dictionary<string, object> gameObjects = IoC.Resolve<Dictionary<string, object>>("Game.Objects.GetAll", gameId);
        Queue<ICommand> gameQueue = IoC.Resolve<Queue<ICommand>>("Game.Queue.Get", gameId);
        TimeSpan timespan = IoC.Resolve<TimeSpan>("Game.Get.Timespan", gameId);

        foreach(KeyValuePair<string, object> entry in gameObjects)
        {
            serializedGame += entry.Key + " : " + IoC.Resolve<string>("ObjectToString", entry.Value) + ";";
        }

        serializedGame += " | ";

        foreach(ICommand cmd in gameQueue.ToArray()){
            serializedGame += IoC.Resolve<string>("SerializeCommand", cmd);
        }

        serializedGame += " | ";

        serializedGame += timespan.ToString();

        return serializedGame;
    }
}