using System;
using System.Linq;

public static class Dop2
{
    public class PredicateComposer<T>
    {
        private Func<T, bool> _predicate = _ => true;
        private bool _hasFilter;

        public PredicateComposer<T> And(Func<T, bool> predicate)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            if (!_hasFilter)
            {
                _predicate = predicate;
                _hasFilter = true;
            }
            else
            {
                Func<T, bool> previous = _predicate;
                _predicate = item => previous(item) && predicate(item);
            }
            return this;
        }

        public PredicateComposer<T> Or(Func<T, bool> predicate)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            if (!_hasFilter)
            {
                _predicate = predicate;
                _hasFilter = true;
            }
            else
            {
                Func<T, bool> previous = _predicate;
                _predicate = item => previous(item) || predicate(item);
            }
            return this;
        }

        public PredicateComposer<T> Not()
        {
            Func<T, bool> previous = _predicate;
            _predicate = item => !previous(item);
            _hasFilter = true;
            return this;
        }

        public Predicate<T> Build() => item => _predicate(item);
    }

    public static void Run()
    {
        Predicate<int> predicate = new PredicateComposer<int>()
            .And(x => x > 5)
            .Or(x => x < 0)
            .Not()
            .Build();

        int[] values = { -5, 0, 3, 6, 10 };
        Console.WriteLine("Проверка чисел предикатом (x > 5 или x < 0), затем NOT:");
        foreach (int value in values)
        {
            Console.WriteLine($"  {value}: {predicate(value)}");
        }

        Console.WriteLine("\nФильтрация массива:");
        foreach (int value in values.Where(value => predicate(value)))
        {
            Console.WriteLine($"  {value}");
        }
    }
}
