using Hwdtech;


namespace SpaceBattle.Lib;

public class CreateSaga: IStrategy  
{
    public object Run(params object[] args)
    {
        List<string> cmdNames = new List<string>();
        int i = 0;
        while (args[i] as string != null) {
            cmdNames.Add((string) args[i]);
            i++;
        }
        UObject obj = (UObject) args[i];

        List<Tuple<ICommand, ICommand>> cmds = new List<Tuple<ICommand, ICommand>>();

        cmdNames.ForEach(name => 
            {
                cmds.Add(new Tuple<ICommand, ICommand>(IoC.Resolve<ICommand>("Commands." + name, obj), IoC.Resolve<ICommand>("Commands.Undo." + name, obj)));
            }
        );

        return new SagaCommand(cmds);
    }
    
}
