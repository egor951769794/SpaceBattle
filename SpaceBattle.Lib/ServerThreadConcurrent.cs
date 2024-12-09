namespace SpaceBattle.Lib;
public class ServerThreadConcurrent
{
    public Thread thread { get; private set; }
    public IReceiver gamesQueue { get; private set; }
    public IReceiver messagesQueue { get; private set; }
    bool stop = false;
    Action strategy;
    Action finishingStrategy;
    public ServerThreadConcurrent(IReceiver gamesQueue, IReceiver messagesQueue)
    {
        this.gamesQueue = gamesQueue;
        this.messagesQueue = messagesQueue;

        strategy = () =>
        {
            _handleCommand();
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
    internal void _stop()
    {
        finishingStrategy();
        stop = true;
    }
    internal void _handleCommand()
    {
        if (!gamesQueue.isEmpty()) gamesQueue.Receive().Execute();
        if (!messagesQueue.isEmpty()) messagesQueue.Receive().Execute();
    }
    internal void _updateBehaviour(Action newBehaviour)
    {
        strategy = newBehaviour;
    }
    internal void _updateFinishingBehaviour(Action newBehaviour)
    {
        finishingStrategy = newBehaviour;
    }
    public void Start()
    {
        thread.Start();
    }
}
