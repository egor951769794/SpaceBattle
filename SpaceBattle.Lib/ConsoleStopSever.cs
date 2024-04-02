namespace SpaceBattle.Lib;
using Hwdtech;

public class StopServerCommand : ICommand
{
    public void Execute()
    {
        Dictionary<string, string> myThreads = IoC.Resolve<Dictionary<string, string>>("Thread.GetDictionary");
        foreach (string threadId in myThreads.Keys)
        {
            var softStopCommand = IoC.Resolve<ICommand>("Thread.SoftStopTheThread");
            ISender sender = IoC.Resolve<ISender>("Thread.GetSender", threadId);
            sender.Send(softStopCommand);
        }
    }
}
