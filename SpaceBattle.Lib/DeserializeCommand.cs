namespace SpaceBattle.Lib;
using Hwdtech;
using System.Collections.Generic;

public class DeserializeCommand: ICommand{

    private Dictionary<string, object> args;

    public DeserializeCommand(string id, string command, Dictionary<string, object> args)
    {
        this.args = args;
    }
    public void Execute()
    {
        ICommand cmd = IoC.Resolve<ICommand>("Create" + (string)args["command"], (string)args["id"], args);

        IoC.Resolve<ICommand>("Threading.SendCommand", cmd).Execute();
    }
}