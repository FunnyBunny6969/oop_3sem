using System;
using System.Collections.Generic;
using System.Threading;

public static class Dop2
{
    public sealed class WorkQueue : IDisposable
    {
        private readonly Queue<Action> _queue = new Queue<Action>();
        private readonly object _locker = new object();
        private readonly AutoResetEvent _signal = new AutoResetEvent(false);
        private readonly Thread[] _workers;
        private bool _stopping;

        public WorkQueue(int workerCount)
        {
            _workers = new Thread[workerCount];
            for (int i = 0; i < workerCount; i++)
            {
                _workers[i] = new Thread(WorkerLoop)
                {
                    IsBackground = true,
                    Name = $"Worker-{i + 1}"
                };
                _workers[i].Start();
            }
        }

        public void Enqueue(Action job)
        {
            lock (_locker)
            {
                if (_stopping) throw new InvalidOperationException("Очередь уже остановлена.");
                _queue.Enqueue(job);
            }
            _signal.Set();
        }

        private void WorkerLoop()
        {
            while (true)
            {
                _signal.WaitOne();

                while (true)
                {
                    Action? job = null;
                    lock (_locker)
                    {
                        if (_queue.Count > 0)
                        {
                            job = _queue.Dequeue();
                        }
                        else if (_stopping)
                        {
                            return;
                        }
                    }

                    if (job == null) break;
                    job();
                }
            }
        }

        public void Complete()
        {
            lock (_locker) _stopping = true;
            for (int i = 0; i < _workers.Length; i++) _signal.Set();
            foreach (Thread worker in _workers) worker.Join();
        }

        public void Dispose() => _signal.Dispose();
    }

    private static long Factorial(int n)
    {
        long result = 1;
        for (int i = 2; i <= n; i++) result *= i;
        return result;
    }

    private static long Sum(int from, int to)
    {
        long result = 0;
        for (int i = from; i <= to; i++) result += i;
        return result;
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
        using WorkQueue queue = new WorkQueue(3);

        queue.Enqueue(() => Console.WriteLine($"  [{Thread.CurrentThread.Name}] 10! = {Factorial(10)}"));
        queue.Enqueue(() => Console.WriteLine($"  [{Thread.CurrentThread.Name}] Сумма 1..1000 = {Sum(1, 1000)}"));
        queue.Enqueue(() => Console.WriteLine($"  [{Thread.CurrentThread.Name}] Простых чисел до 5000: {CountPrimes(5000)}"));
        queue.Enqueue(() => Console.WriteLine($"  [{Thread.CurrentThread.Name}] 15! = {Factorial(15)}"));
        queue.Enqueue(() => Console.WriteLine($"  [{Thread.CurrentThread.Name}] Сумма 1..10000 = {Sum(1, 10000)}"));

        queue.Complete();
        Console.WriteLine("Все задачи очереди выполнены.");
    }
}
