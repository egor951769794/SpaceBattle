// using System.Collections.Concurrent;
// using Moq;
// using Hwdtech;
// using Hwdtech.Ioc;

// namespace SpaceBattle.Lib.Test;

// public class SoftStopTests
// {
//     public SoftStopTests()
//     {
//         new InitScopeBasedIoCImplementationCommand().Execute();

//         IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
//         IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Dependant_Command", (object[] args) => new Mock<SpaceBattle.Lib.ICommand>().Object).Execute();
//         IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.GetDependantCommandNames", (object[] args) => new List<string> {"MoveCommand"}).Execute();
//         IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Threading.SendCommand", (object[] args) => new MoveCommand((IMovable)args[0])).Execute();
//     }
//     [Fact]
//     public void successfulSoftStop()
//     {
//         var queue = new BlockingCollection<ICommand>();
        
//         var objToMove = new Mock<IMovable>();
//         objToMove.SetupProperty(x => x.position);
//         objToMove.SetupGet(x => x.speed).Returns(new Vector(-7, 3));
//         objToMove.Object.position = new Vector(12, 5);
//         var cmd = new MoveCommand(objToMove.Object);
        
//         queue.Add(cmd);

//         var ra = new ReceiverAdapter(queue);

//         var st = new ServerThread(ra);

//         var softStopThread = new HardStopThreadCommand(st, () => {

//         });

//         softStopThread.Execute();

//     }
// }
