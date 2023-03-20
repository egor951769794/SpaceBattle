namespace SpaceBattle.Lib;
public class HardStopThreadCommand : ICommand
{
    ServerThread stoppingThread;
    Action finishingTask;
    public HardStopThreadCommand(ServerThread stoppingThread, Action finishingTask)
    {
        this.stoppingThread = stoppingThread;
        this.finishingTask = finishingTask;
    }
    public void Execute()
    {
        if (Thread.CurrentThread == stoppingThread.thread)
        {
            stoppingThread.UpdateFinishingBehaviour(finishingTask);
            stoppingThread.HardStop();
        }
        else
        {
            throw new Exception();
        }
    }
}
