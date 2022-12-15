using Hwdtech;


namespace SpaceBattle.Lib;
public class StartMoveCommand : ICommand
{
    IStartingMoveCommand initializer;
    public StartMoveCommand(IStartingMoveCommand obj)
    {
        initializer = obj;
    }
    public void Execute()
    {
        initializer.order.ToList().ForEach(o => IoC.Resolve<ICommand>("General.SetProperty", initializer.objToMove, o.Key, o.Value).Execute());
        ICommand cmd = IoC.Resolve<ICommand>("Commands.Movement", initializer.objToMove);
        IoC.Resolve<ICommand>("General.SetProperty", initializer.objToMove, "Commands.Movement", cmd).Execute();
        IoC.Resolve<ICommand>("Queue.Push", IoC.Resolve<Queue<ICommand>>("Queue.Main"), cmd).Execute();
    }
}
