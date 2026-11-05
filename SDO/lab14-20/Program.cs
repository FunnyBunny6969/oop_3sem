using System;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Лабораторная 20: Task Parallel Library ===");
        Console.WriteLine("--- Основное задание (вариант 3) ---");
        MainTask.Run();

        Section("Дополнительное задание 1: параллельное умножение матриц");
        Dop1.Run();

        Section("Дополнительное задание 2: параллельный анализ текста");
        Dop2.Run();

        Section("Дополнительное задание 3: обработка заказов с приоритетами");
        Dop3.Run();
    }

    static void Section(string title)
    {
        Console.WriteLine();
        Console.WriteLine(new string('=', 80));
        Console.WriteLine("=== " + title + " ===");
        Console.WriteLine(new string('=', 80));
    }
}

public static class MainTask
{
    private static BigInteger Factorial(int n)
    {
        BigInteger result = 1;
        for (int i = 2; i <= n; i++) result *= i;
        return result;
    }

    private static long SumRange(int from, int to)
    {
        long sum = 0;
        for (int i = from; i <= to; i++) sum += i;
        return sum;
    }

    private static int CountPrimes(int limit)
    {
        int count = 0;
        for (int i = 2; i <= limit; i++)
        {
            bool prime = true;
            for (int d = 2; d * d <= i; d++)
            {
                if (i % d != 0) continue;
                prime = false;
                break;
            }
            if (prime) count++;
        }
        return count;
    }

    public static void Run()
    {
        const int count = 50;
        BigInteger[] sequential = new BigInteger[count];

        Stopwatch stopwatch = Stopwatch.StartNew();
        for (int i = 1; i <= count; i++) sequential[i - 1] = Factorial(i);
        stopwatch.Stop();
        long sequentialTime = stopwatch.ElapsedMilliseconds;

        BigInteger[] parallel = new BigInteger[count];
        stopwatch.Restart();
        Parallel.For(1, count + 1, i => parallel[i - 1] = Factorial(i));
        stopwatch.Stop();
        long parallelTime = stopwatch.ElapsedMilliseconds;

        Console.WriteLine("Факториалы 1..50:");
        Console.WriteLine($"  10! = {sequential[9]}");
        Console.WriteLine($"  20! = {sequential[19]}");
        Console.WriteLine($"  50! содержит {sequential[49].ToString().Length} цифр");
        Console.WriteLine($"  Последовательно: {sequentialTime} мс, параллельно: {parallelTime} мс");
        Console.WriteLine($"  Результаты совпадают: {sequential.SequenceEqual(parallel)}");

        Task<long> sumTask = Task.Run(() => SumRange(1, 10_000_000));
        Task<BigInteger> factorialTask = Task.Run(() => Factorial(25));
        Task<int> primesTask = Task.Run(() => CountPrimes(50_000));

        Task.WaitAll(sumTask, factorialTask, primesTask);
        Console.WriteLine("\nТри независимых вычисления через Task.Run:");
        Console.WriteLine($"  Сумма 1..10 000 000: {sumTask.Result}");
        Console.WriteLine($"  25!: {factorialTask.Result}");
        Console.WriteLine($"  Простых чисел до 50 000: {primesTask.Result}");
    }
}
