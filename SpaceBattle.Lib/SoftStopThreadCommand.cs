using Hwdtech;


namespace SpaceBattle.Lib;
public class SoftStopThreadCommand : ICommand
{
    ServerThread stoppingThread;
    Action finishingTask;
    // Action softStopStrategy;
    public SoftStopThreadCommand(ServerThread stoppingThread, Action finishingTask)
    {
        this.stoppingThread = stoppingThread;
        this.finishingTask = finishingTask;
        // softStopStrategy = () => 
        // {
        //     if (stoppingThread.queue.isEmpty())
        //     {
        //         int threadId = IoC.Resolve<int>("Threading.GetThreadId", this.stoppingThread);
        //         ICommand hardStopThread = IoC.Resolve<ICommand>("Threading.HardStopThread", threadId, this.finishingTask);
        //         IoC.Resolve<ICommand>("Threading.SendCommand", hardStopThread).Execute();
        //         stoppingThread.updated = true;
        //     }
        //     else
        //     {
        //         stoppingThread.HandleCommand();
        //         stoppingThread.updated = true;
        //     }
        // };
    }
    
    public void Execute()
    {
        // new UpdateBehaviourCommand(stoppingThread, softStopStrategy).Execute();


        int threadId = IoC.Resolve<int>("Threading.GetThreadId", stoppingThread);
        var sender = IoC.Resolve<Dictionary<int, (ServerThread, SenderAdapter)>>("Threading.ServerThreads")[threadId].Item2;

        HardStopThreadCommand hardStop = new HardStopThreadCommand(stoppingThread, finishingTask, sender);

        IoC.Resolve<ICommand>("Threading.SendCommand", threadId, hardStop).Execute();
    }
}
