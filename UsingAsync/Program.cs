using UsingAsync;

Console.WriteLine("Hello World!");

var turkey = new Turkey();
await turkey.Cook();

var gravy = new Gravy();
await gravy.Cook();

Console.WriteLine("Ready to eat");