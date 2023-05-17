using Hwdtech;


namespace SpaceBattle.Lib;

public class RemoveItem : IStrategy
{
    public object Run(params object[] args)
    {
        IoC.Resolve<Dictionary<string, UObject>>("General.Objects").Remove((string) args[0]);
        return new object();
    }
}
