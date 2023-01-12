using Moq;
using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib.Test;

public class CheckStrategyTests
{
    public CheckStrategyTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "General.Collision.GetCollisionPropertiesVector", (object[] args) => new Mock<Vector>().Object).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "General.Collision.GetCollisionPropertiesPrepared", (object[] args) => new Mock<Vector>().Object).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "General.Collision.BuildSolutionTree", (object[] args) => new Mock<BinaryTree<float>>().Object).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "General.Collision.TreeTraverse", (object[] args) => (object) new bool()).Execute();
    }
    [Fact]
    public void successfulCheckStrategyRun()
    {
        var MockObj1 = new Mock<UObject>();
        var MockObj2 = new Mock<UObject>();

        var check = new Check(MockObj1.Object, MockObj2.Object);
        check.Run(new object[0]);
    }
}
