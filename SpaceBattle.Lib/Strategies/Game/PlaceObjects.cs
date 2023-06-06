using Hwdtech;


namespace SpaceBattle.Lib;

public class PlacePlayerObjects : IStrategy
{
    public object Run(params object[] args)
    {
        List<UObject> obj = (List<UObject>) args[0];
        
        return new object();
    }
}
