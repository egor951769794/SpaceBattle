using Hwdtech;

namespace SpaceBattle.Lib;
public class CommandHandler : ICommand
{
    ICommand c;
    Exception e;
    public CommandHandler(ICommand cmd, Exception exc)
    {
        c = cmd;
        e = exc;
    }
    public void Execute()
    {
        try
        {
            c.Execute();
        } catch (Exception e)
        {
            IStrategy exceptionHandler = (IStrategy) (IoC.Resolve<IStrategy>("Strategies.FindExceptionHandler").Run(c, e));
            exceptionHandler.Run();
        }
    }
}
