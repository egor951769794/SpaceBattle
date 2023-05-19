using Hwdtech;

namespace SpaceBattle.Lib;

public class GameQuantum : IStrategy
{
    int quantum;
    public GameQuantum(int _quatum)
    {
        quantum = _quatum;
    }
    public object Run(object[] args)
    {
        return quantum;
    }
}
