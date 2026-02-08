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

//DomeTrainTask.Run(() => Console.WriteLine($"First DomeTrainTask Id: {Environment.CurrentManagedThreadId}")).Wait();

//Console.WriteLine($"Second DomeTrainTask Id: {Environment.CurrentManagedThreadId}");

//DomeTrainTask.Run(() => Console.WriteLine($"Third DomeTrainTask Id: {Environment.CurrentManagedThreadId}")).Wait();
//No need for Console.ReadLine(); because of Wait method

//----------------------------------------

DomeTrainTask.Run(() => Console.WriteLine($"First DomeTrainTask Id: {Environment.CurrentManagedThreadId}")).Wait();

//DomeTrainTask.Delay(TimeSpan.FromSeconds(1)); //There is going to be a task that will be completed after 1 second, but it doesn't effect on threads or ...
DomeTrainTask.Delay(TimeSpan.FromSeconds(1)).Wait();

Console.WriteLine($"Second DomeTrainTask Id: {Environment.CurrentManagedThreadId}");

DomeTrainTask.Delay(TimeSpan.FromSeconds(1)).Wait();

DomeTrainTask.Run(() => Console.WriteLine($"Third DomeTrainTask Id: {Environment.CurrentManagedThreadId}")).Wait();

/*It works but using Wait is dangerous, because we are blocking our thread until the background task is completed.
We prefer replace Wait with await so blocking won't happen but, our next code won't execute until Delay is completely finished.(definition of await)*/

