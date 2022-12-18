namespace SpaceBattle.Lib;
using Hwdtech;

public class Check : IStrategy
{
    UObject obj1, obj2;
    public Check(UObject _obj1, UObject _obj2) 
    {
        obj1 = _obj1;
        obj2 = _obj2;
    }

    public object Run(object[] args)
    {
        Vector property1 = IoC.Resolve<Vector>("General.Collision.GetCollisionPropertiesVector", obj1);
        Vector property2 = IoC.Resolve<Vector>("General.Collision.GetCollisionPropertiesVector", obj2);

        Vector properties = IoC.Resolve<Vector>("General.Collision.GetCollisionPropertiesPrepared", property1, property2);

        BinaryTree<float> tree = IoC.Resolve<BinaryTree<float>>("General.Collision.BuildSolutionTree");

        bool collided = IoC.Resolve<bool>("General.Collision.SolutionTreeTraverse", properties);

        return collided;
    }
}
