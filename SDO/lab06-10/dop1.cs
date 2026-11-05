using System;
using System.Reflection;

public static class Dop1
{
    public class GenericComparator<T>
    {
        public int Compare(T a, T b, string criterion)
        {
            if (a == null || b == null) throw new ArgumentNullException(nameof(a));
            if (string.IsNullOrWhiteSpace(criterion)) throw new ArgumentException("Критерий не задан.");

            object? valueA = GetValue(a, criterion);
            object? valueB = GetValue(b, criterion);

            if (valueA is not IComparable comparable)
                throw new InvalidOperationException("Значение не поддерживает сравнение.");

            return comparable.CompareTo(valueB);
        }

        private static object? GetValue(T item, string criterion)
        {
            if (criterion.Equals("value", StringComparison.OrdinalIgnoreCase))
            {
                if (item is string text) return text.Length;
                if (item is DateTime date) return date.Year;
                return item;
            }

            if (criterion.Equals("length", StringComparison.OrdinalIgnoreCase))
            {
                if (item is string text) return text.Length;
                throw new InvalidOperationException("Критерий length применим только к строкам.");
            }

            if (criterion.Equals("year", StringComparison.OrdinalIgnoreCase))
            {
                if (item is DateTime date) return date.Year;
                throw new InvalidOperationException("Критерий year применим только к датам.");
            }

            PropertyInfo? property = typeof(T).GetProperty(
                criterion,
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

            if (property == null)
                throw new ArgumentException($"Свойство \"{criterion}\" не найдено у типа {typeof(T).Name}.");

            return property.GetValue(item);
        }
    }

    public class Person
    {
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
    }

    public static void Run()
    {
        GenericComparator<int> numberComparator = new GenericComparator<int>();
        Console.WriteLine($"5 vs 8 по значению: {numberComparator.Compare(5, 8, "value")}");

        GenericComparator<string> stringComparator = new GenericComparator<string>();
        Console.WriteLine($"\"кот\" vs \"собака\" по длине: {stringComparator.Compare("кот", "собака", "length")}");
        Console.WriteLine($"\"кот\" vs \"собака\" по значению: {stringComparator.Compare("кот", "собака", "value")}");

        GenericComparator<DateTime> dateComparator = new GenericComparator<DateTime>();
        Console.WriteLine($"2020 vs 2024 по году: {dateComparator.Compare(new DateTime(2020, 1, 1), new DateTime(2024, 1, 1), "year")}");

        GenericComparator<Person> personComparator = new GenericComparator<Person>();
        Person first = new Person { Name = "Иван", Age = 25 };
        Person second = new Person { Name = "Анна", Age = 30 };
        Console.WriteLine($"Люди по возрасту: {personComparator.Compare(first, second, "Age")}");

        try
        {
            personComparator.Compare(first, second, "Unknown");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }
}
