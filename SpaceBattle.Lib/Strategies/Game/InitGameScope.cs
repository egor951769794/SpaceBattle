using Hwdtech;
using Hwdtech.Ioc;


namespace SpaceBattle.Lib;

public class InitGameScope : IStrategy
{
    public object Run(params object[] args)
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetQuantum", (Func<object[], int>) (args => (int) args[0])).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "QueueDequeue", (Func<object[], ICommand>) (args => (ICommand) new QueueDequeue().Run((Queue<ICommand>) args[0]))).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "QueueEnqueue", (Func<object[], IStrategy>) (args => new QueueEnqueue())).Execute();

        Dictionary<string, UObject> gameObjects = new Dictionary<string, UObject>();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "General.Objects", (Func<object[], Dictionary<string, UObject>>) (args => gameObjects)).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "General.GetItem", (Func<object[], UObject>) (args => (UObject) new GetItem().Run(args[0]))).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "General.RemoveItem", (Func<object[], IStrategy>) (args => new RemoveItem())).Execute();

        return new object();
    }
}
