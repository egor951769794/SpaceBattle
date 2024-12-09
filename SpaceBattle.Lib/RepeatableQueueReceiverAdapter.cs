namespace SpaceBattle.Lib;
using System.Collections.Concurrent;

public class RepeatableQueueReceiverAdapter : IReceiver
{
    BlockingCollection<ICommand> queue;

    public RepeatableQueueReceiverAdapter(BlockingCollection<ICommand> queue) => this.queue = queue;

    public ICommand Receive()
    {
        ICommand cmd = queue.Take();
        queue.Add(cmd);
        return cmd;
    }

    public bool isEmpty()
    {
        return queue.Count() == 0;
    }
}
