using System;
using System.Collections.Generic;

public static class Dop2
{
    public class ChainBuilder<T> where T : class
    {
        private readonly List<(T Item, T Next)> _links = new List<(T, T)>();

        public bool AllowCircular { get; set; }

        public void AddNext(T item, T next)
        {
            if (item == null || next == null) throw new ArgumentNullException();

            if (!AllowCircular && CreatesCycle(item, next))
                throw new InvalidOperationException("Добавление создаст цикл в цепочке.");

            int index = _links.FindIndex(link => ReferenceEquals(link.Item, item));
            if (index >= 0) _links[index] = (item, next);
            else _links.Add((item, next));
        }

        public T GetNext(T item)
        {
            (T Item, T Next) link = _links.Find(link => ReferenceEquals(link.Item, item));
            if (link.Item == null) throw new InvalidOperationException("Элемент не найден в цепочке.");
            return link.Next;
        }

        public bool IsCircular()
        {
            if (_links.Count == 0) return false;
            T current = _links[0].Item;
            HashSet<T> visited = new HashSet<T>(ReferenceEqualityComparer.Instance);
            while (current != null)
            {
                if (!visited.Add(current)) return true;
                (T Item, T Next) link = _links.Find(l => ReferenceEquals(l.Item, current));
                if (link.Item == null) return false;
                current = link.Next;
            }
            return false;
        }

        private bool CreatesCycle(T item, T next)
        {
            T? current = next;
            HashSet<T> visited = new HashSet<T>(ReferenceEqualityComparer.Instance);
            while (current != null)
            {
                if (ReferenceEquals(current, item)) return true;
                if (!visited.Add(current)) return false;
                (T Item, T Next) link = _links.Find(l => ReferenceEquals(l.Item, current));
                if (link.Item == null) return false;
                current = link.Next;
            }
            return false;
        }
    }

    public class Person
    {
        public string Name { get; }
        public Person(string name) => Name = name;
    }

    public static void Run()
    {
        ChainBuilder<Person> chain = new ChainBuilder<Person>();
        Person first = new Person("Иван");
        Person second = new Person("Анна");
        Person third = new Person("Петр");

        chain.AddNext(first, second);
        chain.AddNext(second, third);

        Console.WriteLine($"{first.Name} -> {chain.GetNext(first).Name}");
        Console.WriteLine($"{second.Name} -> {chain.GetNext(second).Name}");
        Console.WriteLine($"Цикл: {chain.IsCircular()}");

        try
        {
            chain.AddNext(third, first);
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }

        chain.AllowCircular = true;
        chain.AddNext(third, first);
        Console.WriteLine($"После разрешения цикла: {chain.IsCircular()}");
    }
}
