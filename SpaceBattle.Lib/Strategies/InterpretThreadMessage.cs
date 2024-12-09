// namespace SpaceBattle.Lib;

// using System.Collections.Concurrent;
// using Hwdtech;


// public class InterpretThreadMessage: IStrategy  
// {
//      public object Run(params object[] args)
//     {
//         string threadId = (string) args[0];
//         IMessage msg = (IMessage) args[1];

//         ICommand cmd = IoC.Resolve<ICommand>("CreateCommand", msg);

//         ConcurrentQueue<ICommand> gameQueue = IoC.Resolve<ConcurrentQueue<ICommand>>("Game.Queue.Get", msg.Gameid);
//         gameQueue.Enqueue(cmd);
//         return new object();
//     }
// }
