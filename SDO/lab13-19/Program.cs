using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Лабораторная 19: многопоточность и класс Thread ===");
        Console.WriteLine("--- Основное задание (вариант 3) ---");
        MainTask.Run();

        Section("Дополнительное задание 1: параллельные числа Фибоначчи");
        Dop1.Run();

        Section("Дополнительное задание 2: пул потоков и очередь задач");
        Dop2.Run();

        Section("Дополнительное задание 3: параллельная обработка массива");
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
    private static int _counter;

    public static void Run()
    {
        const int iterations = 1_000_000;

        Thread first = new Thread(() =>
        {
            for (int i = 0; i < iterations; i++) _counter++;
        });
        Thread second = new Thread(() =>
        {
            for (int i = 0; i < iterations; i++) _counter++;
        });

        Stopwatch stopwatch = Stopwatch.StartNew();
        first.Start();
        second.Start();
        first.Join();
        second.Join();
        stopwatch.Stop();

        Console.WriteLine($"Без синхронизации: {_counter} (ожидалось {iterations * 2}), время {stopwatch.ElapsedMilliseconds} мс");

        _counter = 0;
        object locker = new object();
        first = new Thread(() =>
        {
            for (int i = 0; i < iterations; i++)
            {
                lock (locker) _counter++;
            }
        });
        second = new Thread(() =>
        {
            for (int i = 0; i < iterations; i++)
            {
                lock (locker) _counter++;
            }
        });

        stopwatch.Restart();
        first.Start();
        second.Start();
        first.Join();
        second.Join();
        stopwatch.Stop();

        Console.WriteLine($"С синхронизацией: {_counter} (ожидалось {iterations * 2}), время {stopwatch.ElapsedMilliseconds} мс");

        Console.WriteLine("\nПять потоков с именами и случайной задержкой:");
        List<Thread> threads = new List<Thread>();
        for (int i = 1; i <= 5; i++)
        {
            int number = i;
            Thread thread = new Thread(() =>
            {
                int delay = Random.Shared.Next(1000, 5001);
                Console.WriteLine($"  Поток {Thread.CurrentThread.Name} начал работу, задержка {delay} мс");
                Thread.Sleep(delay);
                Console.WriteLine($"  Поток {Thread.CurrentThread.Name} завершил работу");
            });
            thread.Name = $"Worker-{i}";
            threads.Add(thread);
            thread.Start();
        }

        foreach (Thread thread in threads) thread.Join();
        Console.WriteLine("Все пять потоков завершены.");
    }
}
