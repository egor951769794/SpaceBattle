using Hwdtech;


namespace SpaceBattle.Lib;

public class DeleteGame : IStrategy
{
    public object Run(params object[] args)
    {
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
        return new object();
    }
}
