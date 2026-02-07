using ParallelProgramming;

Console.WriteLine("Hello World!");

var turkey = new Turkey();
var gravy = new Gravy();

await Task.WhenAll(turkey.Cook(), gravy.Cook());

Console.WriteLine("Ready to eat");