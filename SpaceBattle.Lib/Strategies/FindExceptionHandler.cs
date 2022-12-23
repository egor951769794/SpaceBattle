using Hwdtech;

namespace SpaceBattle.Lib;
public class FindExceptionHandler : IStrategy
{
    public object Run(object[] args)
    {
        var Handlers = IoC.Resolve<IDictionary<ICommand, IDictionary<Exception, IStrategy>>>("Exceptions.Handlers");
        IStrategy handler = Handlers[(ICommand)args[0]][(Exception)args[1]];
        return handler;
    }
}
