using System;
using System.Diagnostics;
using System.Threading.Tasks;

public static class Dop1
{
    public class ParallelMatrixMultiplier
    {
        public int Threshold { get; }

        public ParallelMatrixMultiplier(int threshold = 500) => Threshold = threshold;

        public static int[][] CreateMatrix(int size, Random random)
        {
            int[][] matrix = new int[size][];
            for (int i = 0; i < size; i++)
            {
                matrix[i] = new int[size];
                for (int j = 0; j < size; j++) matrix[i][j] = random.Next(0, 10);
            }
            return matrix;
        }

        public static int[][] MultiplySequential(int[][] a, int[][] b)
        {
            int size = a.Length;
            int[][] result = CreateEmpty(size);
            for (int i = 0; i < size; i++)
            {
                for (int k = 0; k < size; k++)
                {
                    int value = a[i][k];
                    if (value == 0) continue;
                    for (int j = 0; j < size; j++) result[i][j] += value * b[k][j];
                }
            }
            return result;
        }

        public static int[][] MultiplyParallel(int[][] a, int[][] b)
        {
            int size = a.Length;
            int[][] result = CreateEmpty(size);
            Parallel.For(0, size, i =>
            {
                for (int k = 0; k < size; k++)
                {
                    int value = a[i][k];
                    if (value == 0) continue;
                    for (int j = 0; j < size; j++) result[i][j] += value * b[k][j];
                }
            });
            return result;
        }

        private static int[][] CreateEmpty(int size)
        {
            int[][] matrix = new int[size][];
            for (int i = 0; i < size; i++) matrix[i] = new int[size];
            return matrix;
        }

        public string Strategy(int size) => size < Threshold ? "последовательная" : "параллельная";

        public int[][] Multiply(int[][] a, int[][] b) =>
            a.Length < Threshold ? MultiplySequential(a, b) : MultiplyParallel(a, b);
    }

    private static bool AreEqual(int[][] first, int[][] second)
    {
        for (int i = 0; i < first.Length; i++)
        {
            for (int j = 0; j < first[i].Length; j++)
            {
                if (first[i][j] != second[i][j]) return false;
            }
        }
        return true;
    }

    public static void Run()
    {
        Random random = new Random(42);

        int testSize = 80;
        int[][] smallA = ParallelMatrixMultiplier.CreateMatrix(testSize, random);
        int[][] smallB = ParallelMatrixMultiplier.CreateMatrix(testSize, random);
        int[][] expected = ParallelMatrixMultiplier.MultiplySequential(smallA, smallB);
        int[][] actual = ParallelMatrixMultiplier.MultiplyParallel(smallA, smallB);
        Console.WriteLine($"Проверка корректности {testSize}x{testSize}: {AreEqual(expected, actual)}");

        const int size = 520;
        int[][] a = ParallelMatrixMultiplier.CreateMatrix(size, random);
        int[][] b = ParallelMatrixMultiplier.CreateMatrix(size, random);
        ParallelMatrixMultiplier multiplier = new ParallelMatrixMultiplier();

        Stopwatch stopwatch = Stopwatch.StartNew();
        int[][] sequential = ParallelMatrixMultiplier.MultiplySequential(a, b);
        stopwatch.Stop();
        long sequentialTime = stopwatch.ElapsedMilliseconds;

        stopwatch.Restart();
        int[][] parallel = multiplier.Multiply(a, b);
        stopwatch.Stop();
        long parallelTime = stopwatch.ElapsedMilliseconds;

        Console.WriteLine($"Матрица {size}x{size}, выбранная стратегия: {multiplier.Strategy(size)}");
        Console.WriteLine($"Последовательно: {sequentialTime} мс");
        Console.WriteLine($"Параллельно: {parallelTime} мс");
        Console.WriteLine($"Ускорение: {(parallelTime == 0 ? 0 : sequentialTime / (double)parallelTime):F2}x");
        Console.WriteLine($"Результаты совпадают: {AreEqual(sequential, parallel)}");
    }
}
