namespace SpaceBattle.Lib;

using System.Collections.Concurrent;
using Hwdtech;


public class InterpretThreadMessageCommand: ICommand
{
    private IMessage msg;
    public InterpretThreadMessageCommand(IMessage msg)
    {
        this.msg = msg;
    }
    public void Execute()
    {
        ICommand cmd = IoC.Resolve<ICommand>("CreateCommand", msg);

        ConcurrentQueue<ICommand> gameQueue = IoC.Resolve<ConcurrentQueue<ICommand>>("Game.Queue.Get", msg.Gameid);
        gameQueue.Enqueue(cmd);
    }
}
