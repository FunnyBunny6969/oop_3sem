using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public static class Dop3
{
    private class Order
    {
        public int Id { get; init; }
        public decimal Amount { get; init; }
        public int Priority { get; init; }
    }

    private static void Process(Order order)
    {
        Thread.Sleep(20 + order.Priority * 5);
        Console.WriteLine($"  Обработан заказ {order.Id}: сумма {order.Amount:F2}, приоритет {order.Priority}, поток {Thread.CurrentThread.ManagedThreadId}");
    }

    public static void Run()
    {
        Random random = new Random(42);
        List<Order> orders = new List<Order>();
        for (int i = 1; i <= 120; i++)
        {
            orders.Add(new Order
            {
                Id = i,
                Amount = random.Next(100, 10000),
                Priority = random.Next(1, 6)
            });
        }

        int[] processed = new int[6];
        Stopwatch stopwatch = Stopwatch.StartNew();

        Console.WriteLine("Заказы с приоритетом 5 обрабатываются немедленно:");
        foreach (Order order in orders.Where(o => o.Priority == 5))
        {
            Process(order);
            Interlocked.Increment(ref processed[5]);
        }

        for (int priority = 4; priority >= 1; priority--)
        {
            int currentPriority = priority;
            Console.WriteLine($"\nОбработка заказов с приоритетом {currentPriority}:");
            Parallel.ForEach(
                orders.Where(o => o.Priority == currentPriority),
                new ParallelOptions { MaxDegreeOfParallelism = 4 },
                order =>
                {
                    Process(order);
                    Interlocked.Increment(ref processed[currentPriority]);
                });
        }

        stopwatch.Stop();

        Console.WriteLine($"\nСтатистика, время выполнения: {stopwatch.ElapsedMilliseconds} мс");
        for (int priority = 1; priority <= 5; priority++)
        {
            Console.WriteLine($"  Приоритет {priority}: обработано {processed[priority]}");
        }
        Console.WriteLine($"  Всего обработано: {processed.Skip(1).Sum()}");
    }
}
