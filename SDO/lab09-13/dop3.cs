using System;
using System.Collections.Generic;
using System.Linq;

public static class Dop3
{
    public class Order
    {
        public int Id { get; set; }
        public string Customer { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Status { get; set; } = "Новый";
    }

    public class OrderProcessor
    {
        private readonly Queue<Order> _queue = new Queue<Order>();
        private readonly Dictionary<int, Order> _registry = new Dictionary<int, Order>();
        private readonly Stack<Order> _history = new Stack<Order>();
        private readonly List<Order> _processed = new List<Order>();
        private readonly HashSet<int> _processedIds = new HashSet<int>();

        public void AddOrder(Order order)
        {
            _queue.Enqueue(order);
            _registry[order.Id] = order;
            Console.WriteLine($"Заказ {order.Id} добавлен в очередь.");
        }

        public void ProcessNext()
        {
            if (_queue.Count == 0)
            {
                Console.WriteLine("Очередь пуста.");
                return;
            }

            Order order = _queue.Dequeue();
            order.Status = "Обработан";
            _processed.Add(order);
            _processedIds.Add(order.Id);
            _history.Push(order);
            Console.WriteLine($"Заказ {order.Id} обработан.");
        }

        public void UndoLast()
        {
            if (_history.Count == 0)
            {
                Console.WriteLine("Нет операций для отмены.");
                return;
            }

            Order order = _history.Pop();
            order.Status = "Новый";
            _processed.Remove(order);
            _processedIds.Remove(order.Id);
            _queue.Enqueue(order);
            Console.WriteLine($"Обработка заказа {order.Id} отменена.");
        }

        public void PrintStatus()
        {
            Console.WriteLine($"В очереди: {_queue.Count}, обработано: {_processed.Count}, уникальных обработанных: {_processedIds.Count}");
            foreach (Order order in _queue)
            {
                Console.WriteLine($"  В очереди: заказ {order.Id} ({order.Customer}) на {order.Amount:F2} руб.");
            }
            foreach (Order order in _processed)
            {
                Console.WriteLine($"  Обработан: заказ {order.Id} ({order.Customer}) на {order.Amount:F2} руб.");
            }
        }

        public Order? FindOrder(int id) => _registry.TryGetValue(id, out Order? order) ? order : null;
    }

    public static void Run()
    {
        OrderProcessor processor = new OrderProcessor();
        processor.AddOrder(new Order { Id = 1, Customer = "Иван", Amount = 1500m });
        processor.AddOrder(new Order { Id = 2, Customer = "Анна", Amount = 2500m });
        processor.AddOrder(new Order { Id = 3, Customer = "Петр", Amount = 800m });

        processor.ProcessNext();
        processor.ProcessNext();

        Console.WriteLine();
        processor.PrintStatus();

        Console.WriteLine();
        processor.UndoLast();
        processor.PrintStatus();

        Console.WriteLine();
        Order? found = processor.FindOrder(3);
        Console.WriteLine(found != null ? $"Найден заказ 3: {found.Customer}, {found.Status}" : "Заказ 3 не найден.");
    }
}
