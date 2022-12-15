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
        UObject objToMove = IoC.Resolve<UObject>("General.GetByID", IoC.Resolve<int>("General.UObj.GetProperty", initializer, "id"));
        IoC.Resolve<ICommand>("General.UObj.SetProperty", objToMove, "speed", IoC.Resolve<int>("General.UObj.GetProperty", initializer, "speed")).Execute();
        ICommand moveCommand = IoC.Resolve<ICommand>("Commands.Movement", objToMove);
        IoC.Resolve<ICommand>("General.UObj.SetProperty", objToMove, "Movement", moveCommand).Execute();
        IoC.Resolve<ICommand>("Queue.Push", IoC.Resolve<Queue<ICommand>>("Queue.Main"), moveCommand).Execute();
    }
}
