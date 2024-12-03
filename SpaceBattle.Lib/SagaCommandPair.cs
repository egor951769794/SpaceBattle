namespace SpaceBattle.Lib;

public struct SagaCommandPair
{
    public ICommand command;
    public ICommand compensatingCommand;

    public SagaCommandPair(ICommand command, ICommand compensatingCommand)
    {
        this.command = command; 
        this.compensatingCommand = compensatingCommand;
    }

}
