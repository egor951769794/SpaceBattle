namespace SpaceBattle.Lib;
public class SagaCommand : ICommand
{
    List<Tuple<ICommand, ICommand>> cmds;
    public SagaCommand(List<Tuple<ICommand, ICommand>> _cmds)
    {
        cmds = _cmds;
    }      
    public void Execute()
    {
        int i = 0;
        try
        {
            for (; i < cmds.Count(); i++)
            {
                cmds[i].Item1.Execute();
            }
        } catch {
            i -= 1;
            for (; i >= 0; i--)
            {
                cmds[i].Item2.Execute();
            }
        }
    }
}
