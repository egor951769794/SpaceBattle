using Hwdtech;
using Hwdtech.Ioc;


namespace SpaceBattle.Lib;

public class InitGameScope : IStrategy
{
    public object Run(params object[] _args)
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        object scopePrev = IoC.Resolve<object>("Scopes.Current");
        
        object scope = IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"));
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", scope).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Scopes.Outer", (Func<object[], object>) (args => scopePrev)).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetQuantum", (Func<object[], object>) (args => (int) _args[0])).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "QueueDequeue", (Func<object[], ICommand>) (args => (ICommand) new QueueDequeue().Run((Queue<ICommand>) args[0]))).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "QueueEnqueue", (Func<object[], ICommand>) (args => new QueueEnqueueCommand((Queue<ICommand>) args[0], (ICommand) args[1]))).Execute();

        Dictionary<string, UObject> gameObjects = new Dictionary<string, UObject>();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "General.Objects", (Func<object[], Dictionary<string, UObject>>) (args => gameObjects)).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "General.GetItem", (Func<object[], UObject>) (args => (UObject) new GetItem().Run(args[0]))).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "General.RemoveItem", (Func<object[], ICommand>) (args => new RemoveItemCommand((string)args[0]))).Execute();

        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.Outer")).Execute();

        return scope;
    }
}
