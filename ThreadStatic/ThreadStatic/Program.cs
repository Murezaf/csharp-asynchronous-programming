using System;
using System.Threading;

namespace ThreadStatic
{

    internal class Program
    {
        [ThreadStatic]
        static int threadSpecificValue;

        static void Main(string[] args)
        {
            threadSpecificValue = 100;

            Console.WriteLine($"Main thread - threadSpecificValue: {threadSpecificValue}");

            Thread thread1 = new Thread(ThreadMethod);
            Thread thread2 = new Thread(ThreadMethod);

            thread1.Start();
            thread2.Start();

            thread1.Join();
            thread2.Join();

            Console.WriteLine($"Main thread after threads finished - threadSpecificValue: {threadSpecificValue}");
        }

        static void ThreadMethod()
        {
            threadSpecificValue = new Random().Next(1, 100);

            Console.WriteLine($"Thread {Environment.CurrentManagedThreadId} {nameof(threadSpecificValue)}: {threadSpecificValue}");
        }
    }
}