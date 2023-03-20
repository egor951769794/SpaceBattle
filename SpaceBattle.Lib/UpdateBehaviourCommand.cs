namespace SpaceBattle.Lib;
class UpdateBehaviourCommand : ICommand
{
    Action behaviour;
    ServerThread thread;

    public UpdateBehaviourCommand(ServerThread thread, Action newBehaviour)
    {
        this.behaviour = newBehaviour;
        this.thread = thread;
    }
    public void Execute()
    {
        thread.UpdateBehaviour(behaviour);
    }
}
