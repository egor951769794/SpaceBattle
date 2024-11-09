namespace SpaceBattle.Lib;
public class WasteFuelCommand : ICommand
{
    IFuelChangable obj;
    public WasteFuelCommand(IFuelChangable _obj)
    {
        obj = _obj;
    }      
    public void Execute()
    {
        obj.fuelLevel -= obj.fuelConsumption;
    }
}
