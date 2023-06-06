using Hwdtech;


namespace SpaceBattle.Lib;

public class PlaceObject : IStrategy
{
    public object Run(params object[] args)
    {
        UObject obj = (UObject) args[0];
        obj.setProperty("position", (Vector) args[1]);
        return new object();
    }
}
