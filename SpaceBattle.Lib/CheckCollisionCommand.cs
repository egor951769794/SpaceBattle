namespace SpaceBattle.Lib;
using Hwdtech;

public class CheckCollisionCommand : ICommand
{
    private UObject obj1, obj2;

    public CheckCollisionCommand(UObject _obj1, UObject _obj2)
    {
        obj1 = _obj1;
        obj2 = _obj2;
    }

    public void Execute()
    {
        bool collided = IoC.Resolve<bool>("General.Collision.Check", obj1, obj2);
        if (collided) IoC.Resolve<ICommand>("Commands.CollisionHandle", obj1, obj2).Execute();
    }
}