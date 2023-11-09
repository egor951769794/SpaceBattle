using Hwdtech;

namespace SpaceBattle.Lib;
public class CommandExceptionsHandler : ICommand
{
    ICommand cmd;
    public CommandExceptionsHandler(ICommand cmd)
    {
        this.cmd = cmd;
    }
    public void Execute()
    {
        try
        {
            cmd.Execute();
        } catch (Exception e)
        {
            IStrategy exceptionHandler = (IStrategy) (IoC.Resolve<IStrategy>("Strategies.FindExceptionHandler").Run(cmd, e));
            exceptionHandler.Run();
        }
    }
}
