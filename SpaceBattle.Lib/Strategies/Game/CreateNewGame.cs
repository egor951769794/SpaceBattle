using Hwdtech;


namespace SpaceBattle.Lib;

public class CreateNewGame : IStrategy
{
    int quantum;
    public CreateNewGame(int _quantum = 500)
    {
        quantum = _quantum;
    }
    public object Run(params object[] args)
    {
        Queue<ICommand> queue = new Queue<ICommand>();
        object scope = new InitGameScope().Run(quantum);
        return IoC.Resolve<ICommand>("Commands.GameCommand", scope, queue);
    }
}
