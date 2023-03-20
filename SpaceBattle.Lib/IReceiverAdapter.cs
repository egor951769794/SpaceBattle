namespace SpaceBattle.Lib;
using System.Collections.Concurrent;

class IReceiverAdapter : IReceiver
{
    BlockingCollection<ICommand> queue;

    public IReceiverAdapter(BlockingCollection<ICommand> queue) => this.queue = queue;

    public ICommand Receive()
    {
        return queue.Take();
    }

    public bool isEmpty()
    {
        return queue.Count() == 0;
    }
}
