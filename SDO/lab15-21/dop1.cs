using System;
using System.Diagnostics;
using System.Threading.Tasks;

public static class Dop1
{
    private static async Task<long> FactorialAsync(int number)
    {
        await Task.Delay(300);
        long result = 1;
        for (int i = 2; i <= number; i++) result *= i;
        return result;
    }

    private static async Task<double> PowerAsync(double value, int exponent)
    {
        await Task.Delay(400);
        return Math.Pow(value, exponent);
    }

    private static async Task<double> SeriesSumAsync(int terms)
    {
        await Task.Delay(500);
        double sum = 0;
        for (int i = 1; i <= terms; i++) sum += 1.0 / (i * i);
        return sum;
    }

    private static async Task<(T Result, long Elapsed)> MeasureAsync<T>(string name, Func<Task<T>> operation)
    {
        Stopwatch stopwatch = Stopwatch.StartNew();
        T result = await operation();
        stopwatch.Stop();
        Console.WriteLine($"  {name}: {result}, время {stopwatch.ElapsedMilliseconds} мс");
        return (result, stopwatch.ElapsedMilliseconds);
    }

    public static async Task RunAsync()
    {
        Stopwatch total = Stopwatch.StartNew();
        Task<(long Result, long Elapsed)> factorial = MeasureAsync("15!", () => FactorialAsync(15));
        Task<(double Result, long Elapsed)> power = MeasureAsync("2^20", () => PowerAsync(2, 20));
        Task<(double Result, long Elapsed)> series = MeasureAsync("Сумма 1/n^2 (1000 членов)", () => SeriesSumAsync(1000));

        await Task.WhenAll(factorial, power, series);
        total.Stop();

        Console.WriteLine($"  Общее время: {total.ElapsedMilliseconds} мс");
    }
}
