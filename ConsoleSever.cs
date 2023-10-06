namespace SpaceBattle.Lib;
using Hwdtech;
using System.Collections.Concurrent;

public class ServerProgram
{
    public static void Main(string[] args){
        int numOfThread = int.Parse(args[0]);

        Console.WriteLine("Процедура запуска сервера...");

        IoC.Resolve<ICommand>("Thread.ConsoleStartServer", numOfThread).Execute();
        
        Console.WriteLine("Все потоки успешно запущены");

        Console.WriteLine("Нажмите любую клавишу для остановки сервера...");
        Console.Read();

        Console.WriteLine("Процедура остановки сервера...");

        IoC.Resolve<ICommand>("Thread.ConsoleStopServer").Execute();

        Console.WriteLine("Завершение программы. Нажмите любую клавишу для выхода...");
        Console.Read();
    }
}
