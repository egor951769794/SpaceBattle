namespace SpaceBattle.Lib;
public class SagaCommand : ICommand
{
    List<Tuple<ICommand, ICommand>> cmds;
    SagaCompensationCommand saga;
    public SagaCommand(List<Tuple<ICommand, ICommand>> _cmds)
    {
        cmds = _cmds;
        saga = new SagaCompensationCommand(new List<ICommand>());
    }      
    public void Execute()
    {
        int i = 0;
        bool br = false;
        while (!br)
        {
            try 
            {
                cmds[i].Item1.Execute();
                saga.addCompensation(cmds[i].Item2);
                i++;
                if (i == cmds.Count) br = true;
            } 
            catch
            {
                br = true;
                saga.Execute();
            }
        }
    }
}
