using System;
using System.Diagnostics;
using System.Threading;

public static class Dop1
{
    private static readonly object ConsoleLock = new object();

    private static long Fibonacci(int n) => n < 2 ? n : Fibonacci(n - 1) + Fibonacci(n - 2);

    public static void Run()
    {
        int[] numbers = { 20, 22, 24, 26, 28 };
        long[] results = new long[numbers.Length];
        Thread[] threads = new Thread[numbers.Length];

        Stopwatch parallelWatch = Stopwatch.StartNew();
        for (int i = 0; i < numbers.Length; i++)
        {
            int index = i;
            threads[i] = new Thread(() =>
            {
                long value = Fibonacci(numbers[index]);
                results[index] = value;
                lock (ConsoleLock)
                {
                    Console.WriteLine($"  Поток {Thread.CurrentThread.ManagedThreadId}: F({numbers[index]}) = {value}");
                }
            });
            threads[i].Start();
        }

        foreach (Thread thread in threads) thread.Join();
        parallelWatch.Stop();

        Stopwatch sequentialWatch = Stopwatch.StartNew();
        for (int i = 0; i < numbers.Length; i++)
        {
            results[i] = Fibonacci(numbers[i]);
        }
        sequentialWatch.Stop();

        Console.WriteLine($"\nПараллельное время: {parallelWatch.ElapsedMilliseconds} мс");
        Console.WriteLine($"Последовательное время: {sequentialWatch.ElapsedMilliseconds} мс");
    }
}
