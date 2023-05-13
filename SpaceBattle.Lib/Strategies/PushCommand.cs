using Hwdtech;


namespace SpaceBattle.Lib;

public class PushCommand : IStrategy 
{   
    public object Run(params object[] args)
    {
        int id = (int)args[0];
        ICommand command = (ICommand)args[1];

        var queue = IoC.Resolve<Queue<ICommand>>("GetQueue", id);
        
        return new ActionCommand(() => { queue.Enqueue(command); });
    }
}
