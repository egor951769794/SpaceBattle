namespace SpaceBattle.Lib;


public class QueueEnqueue : IStrategy  
{
    public object Run(params object[] args)
    {
        var queue = (Queue<ICommand>)args[0];
        var cmd = (ICommand) args[1];
        queue.Enqueue(cmd);
        return new object();
    }
}
