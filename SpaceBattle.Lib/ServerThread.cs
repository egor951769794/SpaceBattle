namespace SpaceBattle.Lib;
public class ServerThread
{
    public Thread thread { get; private set; }
    public ReceiverAdapter queue { get; private set; }
    bool stop = false;
    Action strategy;
    Action finishingStrategy;
    public ServerThread(ReceiverAdapter queue)
    {
        this.queue = queue;
        strategy = () =>
        {
            HandleCommand();
        };

        finishingStrategy = new Action(() =>
        {

        });
    
        thread = new Thread(() =>
        {
            while (!stop)
            {
                strategy();
            }
        });
    }
    internal void Stop()
    {
        finishingStrategy();
        stop = true;
    }
    internal void HandleCommand()
    {
        queue.Receive().Execute();
    }
    internal void UpdateBehaviour(Action newBehaviour)
    {
        strategy = newBehaviour;
    }
    internal void UpdateFinishingBehaviour(Action newBehaviour)
    {
        finishingStrategy = newBehaviour;
    }
    public void Start()
    {
        thread.Start();
    }
}
