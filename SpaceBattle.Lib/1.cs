// using System.Collections.Concurrent;
// using Hwdtech;

// namespace SpaceBattle.Lib;

// class Program
// {
//     // Количество рабочих потоков
//     static int numberOfThreads = 3;

//     // Массив рабочих потоков
//     static ServerThreadConcurrent[] threads;

//     // Массив очередей задач для каждого потока
//     static IReceiver[] messageQueues;

//     static void Main()
//     {
//         // Инициализируем массивы
//         threads = new ServerThreadConcurrent[numberOfThreads];
//         messageQueues = new IReceiver[numberOfThreads];

//         // Создаем очереди задач и стартуем потоки
//         for (int i = 0; i < numberOfThreads; i++)
//         {
//             int threadIndex = i;
//             messageQueues[i] = new ReceiverAdapter(new BlockingCollection<ICommand>());

//             // Создаем и запускаем рабочий поток
//             // можно и так данные передавать в поток (здесь передается индекс)
//             // threads[i] = new Thread(() => ProcessTasks(threadIndex));
//             threads[i] = new ServerThreadConcurrent(IoC.Resolve<IReceiver>("Threading.New.GameCommandsReceiver"), messageQueues[i]);
//             threads[i].Start();

//             Console.WriteLine($"Поток {threadIndex} запущен.");
//         }

//         // Добавляем задачи
//         // for (int i = 0; i < 10; i++)
//         // {
//         //     var threadIndex = i % numberOfThreads; // распределянм задачи по потокам
//         //     var Number = i; // Номер задачи для отображения

//         //     // Добавляем задачу в очередь потока
//         //     taskQueues[threadIndex].Add(() =>
//         //     {
//         //         Console.WriteLine($"Задача {taskNumber} выполняется в потоке {threadIndex}");
//         //         // Имитируем работу
//         //         Thread.Sleep(500);
//         //     });

//         //     Console.WriteLine($"Задача {taskNumber} добавлена в очередь потока {threadIndex}");
//         // }

//         // Пусть задачи выполняются 6 секунд
//         Thread.Sleep(6000);

//         // Сообщаем потокам, что больше не будет задач
//         foreach (var queue in taskQueues)
//         {
//             queue.CompleteAdding();
//         }
// // 
//         // Ожидаем завершения всех рабочих потоков
//         foreach (var thread in workerThreads)
//         {
//             thread.Join();
//         }

//         Console.WriteLine("Все задачи выполнены.");
//     }

//     // Метод обработки задач для рабочего потока
//     static void ProcessTasks(int threadIndex)
//     {
//         Console.WriteLine($"Поток {threadIndex} начинает обработку задач.");

//         // Получаем очередь задач для текущего потока
//         var queue = taskQueues[threadIndex];

//         // Получаем задачи из очереди и обрабатываем их
//         foreach (var task in queue.GetConsumingEnumerable())
//         {
//             task();
//         }

//         Console.WriteLine($"Поток {threadIndex} завершил обработку задач.");
//     }
// }