namespace SpaceBattle.Lib.Test;
using Moq;

public class RotateTest
{
    [Fact]
    public void Test1()
    {
        Mock<IRotatable> obj = new();
        obj.SetupProperty(x => x.Angle, 45);
        obj.SetupGet(x => x.AngleSpeed).Returns(90);
        RotateCommand rotate = new(obj.Object);

        rotate.Execute();

        Assert.Equal(135, obj.Object.Angle);
    }
}