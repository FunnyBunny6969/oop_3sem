using System;
using System.Diagnostics;
using System.Threading;

public static class Dop3
{
    private class PartResult
    {
        public long Sum { get; set; }
        public int EvenCount { get; set; }
        public int AboveThreshold { get; set; }
    }

    private static PartResult Process(int[] data, int from, int to, int threshold)
    {
        PartResult result = new PartResult();
        for (int i = from; i < to; i++)
        {
            int value = data[i];
            result.Sum += value;
            if (value % 2 == 0) result.EvenCount++;
            if (value > threshold) result.AboveThreshold++;
        }
        return result;
    }

    public static void Run()
    {
        const int size = 5_000_000;
        const int threshold = 700;
        int[] data = new int[size];
        Random random = new Random(42);
        for (int i = 0; i < size; i++) data[i] = random.Next(0, 1001);

        Stopwatch sequentialWatch = Stopwatch.StartNew();
        PartResult sequential = Process(data, 0, size, threshold);
        sequentialWatch.Stop();

        int partCount = Math.Min(Environment.ProcessorCount, 8);
        int chunkSize = size / partCount;
        Thread[] threads = new Thread[partCount];
        PartResult[] results = new PartResult[partCount];

        Stopwatch parallelWatch = Stopwatch.StartNew();
        for (int i = 0; i < partCount; i++)
        {
            int index = i;
            int from = index * chunkSize;
            int to = index == partCount - 1 ? size : from + chunkSize;
            threads[i] = new Thread(() => results[index] = Process(data, from, to, threshold));
            threads[i].Start();
        }

        foreach (Thread thread in threads) thread.Join();
        parallelWatch.Stop();

        long parallelSum = 0;
        int parallelEven = 0;
        int parallelAbove = 0;
        foreach (PartResult result in results)
        {
            parallelSum += result.Sum;
            parallelEven += result.EvenCount;
            parallelAbove += result.AboveThreshold;
        }

        Console.WriteLine($"Размер массива: {size:N0}, частей: {partCount}");
        Console.WriteLine($"Последовательно: сумма {sequential.Sum:N0}, чётных {sequential.EvenCount:N0}, больше {threshold}: {sequential.AboveThreshold:N0}, время {sequentialWatch.ElapsedMilliseconds} мс");
        Console.WriteLine($"Параллельно:     сумма {parallelSum:N0}, чётных {parallelEven:N0}, больше {threshold}: {parallelAbove:N0}, время {parallelWatch.ElapsedMilliseconds} мс");
        Console.WriteLine($"Результаты совпадают: {sequential.Sum == parallelSum && sequential.EvenCount == parallelEven && sequential.AboveThreshold == parallelAbove}");
    }
}
