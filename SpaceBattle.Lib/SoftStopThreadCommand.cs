using Hwdtech;


namespace SpaceBattle.Lib;
class SoftStopThreadCommand : ICommand
{
    ServerThread stoppingThread;
    Action finishingTask;
    int threadId;
    public SoftStopThreadCommand(ServerThread stoppingThread, Action finishingTask, int id)
    {
        this.stoppingThread = stoppingThread;
        this.finishingTask = finishingTask;
        this.threadId = id;
    }
    
    public void Execute()
    {
        ICommand finishingCommand = new HardStopThreadCommand(stoppingThread, finishingTask);
        IoC.Resolve<ICommand>("Commands.SendCommand", threadId, finishingCommand).Execute();
    }
}
