using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

public static class Dop3
{
    private static async Task<(long Inside, long Total)> CountChunkAsync(int iterations, int seed)
    {
        return await Task.Run(() =>
        {
            Random random = new Random(seed);
            long inside = 0;
            for (int i = 0; i < iterations; i++)
            {
                double x = random.NextDouble();
                double y = random.NextDouble();
                if (x * x + y * y <= 1.0) inside++;
            }
            return (inside, (long)iterations);
        });
    }

    public static async Task RunAsync()
    {
        int[] iterationCounts = { 100_000, 1_000_000, 5_000_000 };
        int chunkCount = 4;

        foreach (int iterations in iterationCounts)
        {
            int chunkSize = iterations / chunkCount;
            Stopwatch stopwatch = Stopwatch.StartNew();

            Task<(long Inside, long Total)>[] tasks = Enumerable.Range(0, chunkCount)
                .Select(i => CountChunkAsync(chunkSize, 42 + i))
                .ToArray();

            (long Inside, long Total)[] results = await Task.WhenAll(tasks);
            stopwatch.Stop();

            long inside = results.Sum(r => r.Inside);
            long total = results.Sum(r => r.Total);
            double pi = 4.0 * inside / total;
            double error = Math.Abs(Math.PI - pi);

            Console.WriteLine($"Итераций: {total:N0}, частей: {chunkCount}");
            Console.WriteLine($"  Пи = {pi:F8}, ошибка = {error:F8}, время = {stopwatch.ElapsedMilliseconds} мс");
        }
    }
}
