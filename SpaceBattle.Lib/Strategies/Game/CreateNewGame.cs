using Hwdtech;


namespace SpaceBattle.Lib;

public class CreateNewGame : IStrategy
{
    int quantum = 500;
    public CreateNewGame(int _quantum)
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
