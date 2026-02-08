using CreatingTaskFromScratch;

Console.WriteLine($"Starting Thread Id: {Environment.CurrentManagedThreadId}");

//DomeTrainTask.Run(() => Console.WriteLine($"First DomeTrainTask Id: {Environment.CurrentManagedThreadId}"));
//DomeTrainTask.Run(() => Console.WriteLine($"Second DomeTrainTask Id: {Environment.CurrentManagedThreadId}"));

//DomeTrainTask.Run(() => Console.WriteLine($"First DomeTrainTask Id: {Environment.CurrentManagedThreadId}"))
//    .ContinueWith(() => Console.WriteLine($"Second DomeTrainTask Id: {Environment.CurrentManagedThreadId}"));

//------------------------------------------

//DomeTrainTask task = DomeTrainTask.Run(() => Console.WriteLine($"First DomeTrainTask Id: {Environment.CurrentManagedThreadId}"));
//task.ContinueWith(() =>
//{
//    DomeTrainTask.Run(() =>
//    {
//        Console.WriteLine($"Third DomeTrainTask Id: {Environment.CurrentManagedThreadId}");
//    });

//    Console.WriteLine($"Second DomeTrainTask Id: {Environment.CurrentManagedThreadId}");
//});

//Console.ReadLine();

//----------------------------------------

DomeTrainTask.Run(() => Console.WriteLine($"First DomeTrainTask Id: {Environment.CurrentManagedThreadId}")).Wait();
Console.WriteLine($"Second DomeTrainTask Id: {Environment.CurrentManagedThreadId}");
DomeTrainTask.Run(() => Console.WriteLine($"Third DomeTrainTask Id: {Environment.CurrentManagedThreadId}")).Wait();
//No need for Console.ReadLine(); because of Wait method
