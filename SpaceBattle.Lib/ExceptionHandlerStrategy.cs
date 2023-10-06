namespace SpaceBattle.Lib;
using Hwdtech;

public class ExceptionLogCommand : ICommand
{
    ICommand command;
    Exception ex;
    public ExceptionLogCommand(ICommand command, Exception ex)
    {
        this.command = command;
        this.ex = ex;
    }
    public void Execute()
    {
        string logFileName = IoC.Resolve<string>("Exception.GetLogName");
        string errorMessage = $"Error in command '{command.GetType().Name}': {ex.Message}";

        using (StreamWriter writer = new StreamWriter(logFileName, true))
        {
            writer.WriteLine(errorMessage);
        }
    }
}
