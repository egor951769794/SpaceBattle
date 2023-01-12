using Hwdtech;

namespace SpaceBattle.Lib;
public class FindExceptionHandler : IStrategy
{
    public object Run(object[] args)
    {
        var handlers = IoC.Resolve<IDictionary<ICommand, IDictionary<Exception, IStrategy>>>("Exceptions.Handlers");
        IStrategy handler = handlers[(ICommand)args[0]][(Exception)args[1]];
        return handler;
    }
}
