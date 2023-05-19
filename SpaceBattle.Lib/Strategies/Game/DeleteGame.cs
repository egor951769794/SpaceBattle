using Hwdtech;


namespace SpaceBattle.Lib;

public class DeleteGame : IStrategy
{
    public object Run(params object[] args)
    {
        object scope = IoC.Resolve<object>("Scopes.Outer");
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", scope).Execute();
        return new object();
    }
}
