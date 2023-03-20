namespace SpaceBattle.Lib;
class ServerThread
{
    public Thread thread
    {
        get; private set;
    }
    IReceiverAdapter queue;
    bool stop = false;
    Action strategy;
    Action finishingStrategy;

    internal void HardStop()
    {
        finishingStrategy();
        stop = true;
    }

    internal void HandleCommand()
    {
        var cmd = queue.Receive();
        cmd.Execute();
    }

    public ServerThread(IReceiverAdapter queue)
    {
        this.queue = queue;
        strategy = () =>
        {
            HandleCommand();
        };

        finishingStrategy = () =>
        {

        };

        thread = new Thread(() =>
        {
            while (!stop || !queue.isEmpty())
                strategy();
        });
    }

    internal void UpdateBehaviour(Action newBehaviour)
    {
        strategy = newBehaviour;
    }

    internal void setDefaultBehaviour()
    {
        strategy = () => 
        {
            HandleCommand();
        };
    }

    internal void UpdateFinishingBehaviour(Action newBehaviour)
    {
        finishingStrategy = newBehaviour;
    }

    public void Execute()
    {
        thread.Start();
    }
}
