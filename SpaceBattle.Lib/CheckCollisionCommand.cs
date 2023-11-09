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
        if (IoC.Resolve<bool>("General.Collision.Check", obj1, obj2)) throw new Exception("Exceptions.Collision.Collided");
    }
}
