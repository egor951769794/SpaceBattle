using Hwdtech;
namespace SpaceBattle.Lib;

public class LongTermOperationStrategy : IStrategy
{
    public object Run(params object[] args)
    {
        string name = (string)args[0];
        UObject uobj = (UObject)args[1];

        var macro = IoC.Resolve<SpaceBattle.Lib.ICommand>("CreateMacroCommandStrategy", name, uobj);

        SpaceBattle.Lib.ICommand repeatCommand = IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Command.Repeat", macro);
        SpaceBattle.Lib.ICommand inject_command = IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Command.Inject", repeatCommand);


        return inject_command;
    }
}