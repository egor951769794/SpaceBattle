namespace SpaceBattle.Lib;
public class SagaCompensationCommand : ICommand
{
    List<ICommand> compensations;
    public SagaCompensationCommand(List<ICommand> _compensations)
    {
        compensations = _compensations;
    }      

    public void addCompensation(ICommand cmd)
    {
        compensations.Insert(0, cmd);
    }

    public void Execute()
    {
        
        compensations.ForEach(x => x.Execute());
    }
}
