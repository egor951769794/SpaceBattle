namespace SpaceBattle.Lib;

public class SagaCommandStrategy : IStrategy
{
    public object Run(params object[] args)
    {
        ICollection<Tuple<string, object[]>> commands = (ICollection<Tuple<string, object[]>>) args[0];
        ICollection<SagaCommandPair> commandPairs = new List<SagaCommandPair>();
        foreach (var item in commands)
        {
            string commandName = item.Item1;
            object[] commandArgs = item.Item2;
            ICommand command = Hwdtech.IoC.Resolve<ICommand>(commandName, commandArgs);
            ICommand commandCompensating = Hwdtech.IoC.Resolve<ICommand>($"{commandName}.Compensating", commandArgs);
            commandPairs.Add(new SagaCommandPair(command, commandCompensating));
        }
        return new SagaCommand(commandPairs);
    }
}
