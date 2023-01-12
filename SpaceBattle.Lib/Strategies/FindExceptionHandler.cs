using Hwdtech;

namespace SpaceBattle.Lib;
public class FindExceptionHandler : IStrategy
{
    public object Run(object[] args)
    {
        string cmdName = IoC.Resolve<string>("Commands.ToString", (ICommand)args[0]);
        string excName = ((Exception)args[1]).Message;

        var handlers = IoC.Resolve<IDictionary<string, IDictionary<string, IStrategy>>>("Exceptions.Handlers");
        IStrategy handler = handlers[cmdName][excName];
        return handler;
    }
}
