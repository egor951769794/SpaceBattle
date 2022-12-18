namespace SpaceBattle.Lib;

public class Check : IStrategy
{
    UObject obj1, obj2;
    public Check(UObject _obj1, UObject _obj2) 
    {
        obj1 = _obj1;
        obj2 = _obj2;
    }

    public object Run(object[] args)
    {
        return new object();
    }
}