using System.Collections.Concurrent;
using Hwdtech;


namespace SpaceBattle.Lib;

public class CreateNewGameConcurrent : IStrategy
{
    int quantum;
    public CreateNewGameConcurrent(int _quantum = 500)
    {
        quantum = _quantum;
    }
    public object Run(params object[] args)
    {
        ConcurrentQueue<ICommand> queue = new ConcurrentQueue<ICommand>();
        object scope = new InitGameScope().Run(quantum);
        string gameId = IoC.Resolve<string>("Game.MakeNewId");
        ConcurrentDictionary<string, ConcurrentQueue<SpaceBattle.Lib.ICommand>> gamesQueues = IoC.Resolve<ConcurrentDictionary<string, ConcurrentQueue<SpaceBattle.Lib.ICommand>>>("Game.Queue.GetAll");
        gamesQueues[gameId] = queue;
        ConcurrentDictionary<string, ICommand> games = IoC.Resolve<ConcurrentDictionary<string, ICommand>>("Game.GetAll");
        ICommand newGame = IoC.Resolve<ICommand>("Commands.GameCommand", scope, queue);
        games[gameId] = newGame;
        return IoC.Resolve<ICommand>("Commands.GameCommand", scope, queue);
    }
}
