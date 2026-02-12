using CreatingTaskFromScratch;

Console.WriteLine($"Starting Thread Id: {Environment.CurrentManagedThreadId}");

//DomeTrainTask.Run(() => Console.WriteLine($"First DomeTrainTask Id: {Environment.CurrentManagedThreadId}"));
//DomeTrainTask.Run(() => Console.WriteLine($"Second DomeTrainTask Id: {Environment.CurrentManagedThreadId}"));

//Console.ReadLine(); 

//------------------------------------------

//DomeTrainTask.Run(() => Console.WriteLine($"First DomeTrainTask Id: {Environment.CurrentManagedThreadId}"))
//    .ContinueWith(() => Console.WriteLine($"Second DomeTrainTask Id: {Environment.CurrentManagedThreadId}"));

//Console.ReadLine();

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

//DomeTrainTask.Run(() => Console.WriteLine($"First DomeTrainTask Id: {Environment.CurrentManagedThreadId}")).Wait();

//Console.WriteLine($"Second DomeTrainTask Id: {Environment.CurrentManagedThreadId}");

//DomeTrainTask.Run(() => Console.WriteLine($"Third DomeTrainTask Id: {Environment.CurrentManagedThreadId}")).Wait();
////No need for Console.ReadLine(); because of Wait method

//----------------------------------------

//DomeTrainTask.Run(() => Console.WriteLine($"First DomeTrainTask Id: {Environment.CurrentManagedThreadId}")).Wait();

////DomeTrainTask.Delay(TimeSpan.FromSeconds(1)); //There is going to be a task that will be completed after 1 second, but it doesn't effect on anything basically ...
//DomeTrainTask.Delay(TimeSpan.FromSeconds(1)).Wait();

//Console.WriteLine($"Second DomeTrainTask Id: {Environment.CurrentManagedThreadId}");

//DomeTrainTask.Delay(TimeSpan.FromSeconds(1)).Wait();

//DomeTrainTask.Run(() => Console.WriteLine($"Third DomeTrainTask Id: {Environment.CurrentManagedThreadId}")).Wait();

//----------------------------------------

await DomeTrainTask.Run(() => Console.WriteLine($"First DomeTrainTask Id: {Environment.CurrentManagedThreadId}"));

await DomeTrainTask.Delay(TimeSpan.FromSeconds(1));

Console.WriteLine($"Second DomeTrainTask Id: {Environment.CurrentManagedThreadId}");

await DomeTrainTask.Delay(TimeSpan.FromSeconds(1));

await DomeTrainTask.Run(() => Console.WriteLine($"Third DomeTrainTask Id: {Environment.CurrentManagedThreadId}"));
