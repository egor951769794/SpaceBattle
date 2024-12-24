namespace SpaceBattle.Lib;
public class ServerThread
{
    public Thread thread { get; private set; }
    public ReceiverAdapter queue { get; private set; }
    public ReceiverAdapter externalQueue { get; private set; }
    bool stop = false;
    Action strategy;
    Action finishingStrategy;
    public ServerThread(ReceiverAdapter queue, ReceiverAdapter externalQueue)
    {
        this.queue = queue;
        this.externalQueue = externalQueue;
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
        externalQueue.Receive().Execute();
        queue.Receive().Execute();
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
