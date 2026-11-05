using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public static class Dop2
{
    public class Person
    {
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
    }

    private static bool CompareValues(object? first, object? second, out string difference)
    {
        difference = string.Empty;
        if (ReferenceEquals(first, second)) return true;
        if (first == null || second == null)
        {
            difference = "одно из значений null";
            return false;
        }

        if (first is IEnumerable firstEnumerable && second is IEnumerable secondEnumerable
            && first is not string && second is not string)
        {
            List<object?> firstItems = firstEnumerable.Cast<object?>().ToList();
            List<object?> secondItems = secondEnumerable.Cast<object?>().ToList();
            if (firstItems.Count != secondItems.Count)
            {
                difference = "разное количество элементов";
                return false;
            }
            for (int i = 0; i < firstItems.Count; i++)
            {
                if (!Equals(firstItems[i], secondItems[i]))
                {
                    difference = $"различаются элементы с индексом {i}";
                    return false;
                }
            }
            return true;
        }

        if (!Equals(first, second))
        {
            difference = $"\"{first}\" != \"{second}\"";
            return false;
        }

        return true;
    }

    public static bool CompareObjects(object first, object second)
    {
        if (first.GetType() != second.GetType())
            throw new ArgumentException("Объекты должны быть одного типа.");

        bool equal = true;
        foreach (PropertyInfo property in first.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            object? firstValue = property.GetValue(first);
            object? secondValue = property.GetValue(second);
            if (!CompareValues(firstValue, secondValue, out string difference))
            {
                equal = false;
                Console.WriteLine($"  Свойство {property.Name}: {difference}");
            }
        }
        return equal;
    }

    public static void Run()
    {
        Person first = new Person
        {
            Name = "Иван Петров",
            Age = 25,
            Tags = new List<string> { "друг", "коллега" }
        };

        Person second = new Person
        {
            Name = "Иван Петров",
            Age = 26,
            Tags = new List<string> { "друг", "коллега" }
        };

        Person third = new Person
        {
            Name = "Иван Петров",
            Age = 25,
            Tags = new List<string> { "друг", "коллега" }
        };

        Console.WriteLine("Сравнение первого и второго объекта:");
        Console.WriteLine($"Результат: {CompareObjects(first, second)}");

        Console.WriteLine("\nСравнение первого и третьего объекта:");
        Console.WriteLine($"Результат: {CompareObjects(first, third)}");
    }
}
