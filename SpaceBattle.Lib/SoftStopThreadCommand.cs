using Hwdtech;


namespace SpaceBattle.Lib;
public class SoftStopThreadCommand : ICommand
{
    ServerThread stoppingThread;
    Action finishingTask;
    public SoftStopThreadCommand(ServerThread stoppingThread, Action finishingTask)
    {
        this.stoppingThread = stoppingThread;
        this.finishingTask = finishingTask;
    }
    
    public void Execute()
    {
        int threadId = IoC.Resolve<int>("Threading.GetThreadId", stoppingThread);
        var sender = IoC.Resolve<Dictionary<int, (ServerThread, SenderAdapter)>>("Threading.ServerThreads")[threadId].Item2;

        HardStopThreadCommand hardStop = new HardStopThreadCommand(stoppingThread, finishingTask, sender);

        IoC.Resolve<ICommand>("Threading.SendCommand", threadId, hardStop).Execute();
    }
}
