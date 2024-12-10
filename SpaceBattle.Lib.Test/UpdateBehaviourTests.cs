using System.Collections.Concurrent;
using Moq;

namespace SpaceBattle.Lib.Test;

public class UpdateBehaviourTests
{
    [Fact]
    public void successfulUpdateBehaviour()
    {
        var queue = new BlockingCollection<ICommand>();
        
        var objToMove = new Mock<IMovable>();
        objToMove.SetupProperty(x => x.position);
        objToMove.SetupGet(x => x.speed).Returns(new Vector(-7, 3));
        objToMove.Object.position = new Vector(12, 5);
        var cmd = new MoveCommand(objToMove.Object);
        
        queue.Add(cmd);

        var ra = new ReceiverAdapter(queue);

        Mock<IReceiver> mockMsgReceiver = new Mock<IReceiver>();
        mockMsgReceiver.Setup(x => x.isEmpty()).Returns(true);

        var st = new ServerThread(ra, mockMsgReceiver.Object);

        var HandleNothing = () => 
        {
            
        };

        new UpdateBehaviourCommand(st, HandleNothing).Execute();

        st.Start();
        
        Assert.True(objToMove.Object.position == new Vector(12, 5));
    }
}
