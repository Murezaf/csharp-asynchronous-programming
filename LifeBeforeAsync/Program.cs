using LifeBeforeAsync;

Console.WriteLine("Hello, World!");

var turkey = new Turkey();
//turkey.Cook();
//Console.WriteLine("Main finished!");

turkey.Cook().ContinueWith(_ =>
{
    var gravy = new Gravy();
    gravy.Cook();
});

Console.ReadLine();