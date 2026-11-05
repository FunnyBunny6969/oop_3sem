using System;
using System.Collections.Generic;
using System.Reflection;

public static class Dop1
{
    [AttributeUsage(AttributeTargets.Property)]
    public class TrackChangesAttribute : Attribute
    {
    }

    public class Employee
    {
        [TrackChanges]
        public string Name { get; set; } = "Иван Петров";

        [TrackChanges]
        public string Position { get; set; } = "Разработчик";

        [TrackChanges]
        public decimal Salary { get; set; } = 100000m;

        public string Department { get; set; } = "ИТ";
    }

    public class ChangeTracker<T> where T : class
    {
        private readonly T _target;
        private readonly Dictionary<string, object?> _originalValues = new Dictionary<string, object?>();

        public ChangeTracker(T target)
        {
            _target = target;
            Capture();
        }

        private void Capture()
        {
            _originalValues.Clear();
            foreach (PropertyInfo property in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (property.GetCustomAttribute<TrackChangesAttribute>() == null) continue;
                _originalValues[property.Name] = property.GetValue(_target);
            }
        }

        public List<string> GetChanges()
        {
            List<string> changes = new List<string>();
            foreach (PropertyInfo property in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (!_originalValues.TryGetValue(property.Name, out object? original)) continue;
                object? current = property.GetValue(_target);
                if (!Equals(original, current))
                {
                    changes.Add($"{property.Name}: {original} -> {current}");
                }
            }
            return changes;
        }

        public void AcceptChanges() => Capture();
    }

    public static void Run()
    {
        Employee employee = new Employee();
        ChangeTracker<Employee> tracker = new ChangeTracker<Employee>(employee);

        employee.Position = "Старший разработчик";
        employee.Salary = 150000m;
        employee.Department = "Финансы";

        Console.WriteLine("Изменения:");
        List<string> changes = tracker.GetChanges();
        if (changes.Count == 0) Console.WriteLine("  Изменений нет.");
        foreach (string change in changes) Console.WriteLine($"  {change}");

        tracker.AcceptChanges();
        Console.WriteLine("\nПосле AcceptChanges:");
        changes = tracker.GetChanges();
        if (changes.Count == 0) Console.WriteLine("  Изменений нет.");
        foreach (string change in changes) Console.WriteLine($"  {change}");
    }
}
