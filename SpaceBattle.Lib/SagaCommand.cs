namespace SpaceBattle.Lib;

public class SagaCommand : ICommand
{
    private readonly ICollection<SagaCommandPair> commandPairs; // объект нельзя заменить на другой
    public SagaCommand(ICollection<SagaCommandPair> commands)
    {
        commandPairs = commands;
    }
    public void Execute()
    {
        int index = 0;
        try
        {
            for (; index < commandPairs.Count; index++)
            {
                commandPairs.ElementAt(index).command.Execute();
            }
        }
        catch (Exception)
        {
            for (index -= 1; index >= 0; index--)
            {
                commandPairs.ElementAt(index).compensatingCommand.Execute();
            }
            throw;
        }
    }
}
