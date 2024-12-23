using Hwdtech;


namespace SpaceBattle.Lib;


[Serializable]
public class GameCommand : SpaceBattle.Lib.ICommand
{
    public IEnumerable<SpaceBattle.Lib.ICommand> queue { get; }

    public object scope { get; }

    public GameCommand(object scope, IEnumerable<SpaceBattle.Lib.ICommand> queue)
    {
        this.scope = scope;
        this.queue = queue;
    }

    public void Execute()
    {
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", scope).Execute();

        IoC.Resolve<SpaceBattle.Lib.ICommand>("StartQueue", queue).Execute();
    }
}
