using Hwdtech;

namespace SpaceBattle.Lib;

public class CreateMacro : IStrategy
{
    public object Run(object[] args)
    {
        string cmdName = (string)args[0];
        List<string> dependantCommands = IoC.Resolve<List<string>>("Commands.GetDependantCommandNames", args[0]);
        List<ICommand> commands = new List<ICommand>();
        dependantCommands.ForEach(cmd => commands.Add((ICommand)(IoC.Resolve<IStrategy>("Commands.GetCommand.WithArgs", cmd, args[1]).Run())));
        return new MacroCommand(commands);
    }
}
