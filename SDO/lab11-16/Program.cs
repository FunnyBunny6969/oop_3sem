using System;
using System.Linq;
using System.Reflection;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Лабораторная 16: атрибуты ===");
        Console.WriteLine("--- Основное задание (вариант 3) ---");
        MainTask.Run();

        Section("Дополнительное задание 1: TrackChanges");
        Dop1.Run();

        Section("Дополнительное задание 2: Mask");
        Dop2.Run();

        Section("Дополнительное задание 3: версии методов");
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

[AttributeUsage(AttributeTargets.Property)]
public class DisplayNameAttribute : Attribute
{
    public string Name { get; }
    public DisplayNameAttribute(string name) => Name = name;
}

[AttributeUsage(AttributeTargets.Method)]
public class RunOnStartupAttribute : Attribute
{
}

public static class MainTask
{
    public class Person
    {
        [DisplayName("Имя")]
        public string Name { get; set; } = "Иван Петров";

        [DisplayName("Возраст")]
        public int Age { get; set; } = 25;

        public string Address { get; set; } = "г. Москва";
    }

    public static class StartupTasks
    {
        [RunOnStartup]
        public static void InitializeDatabase()
        {
            Console.WriteLine("  [Startup] Инициализация базы данных выполнена.");
        }

        [RunOnStartup]
        public static void CheckConfiguration()
        {
            Console.WriteLine("  [Startup] Конфигурация проверена.");
        }

        public static void NotRun()
        {
            Console.WriteLine("  Этот метод не должен запускаться.");
        }
    }

    public static void PrintProperties(object obj)
    {
        foreach (PropertyInfo property in obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            DisplayNameAttribute? displayName = property.GetCustomAttribute<DisplayNameAttribute>();
            string label = displayName?.Name ?? property.Name;
            Console.WriteLine($"  {label}: {property.GetValue(obj)}");
        }
    }

    public static void RunStartupTasks()
    {
        foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
        {
            foreach (MethodInfo method in type.GetMethods(BindingFlags.Public | BindingFlags.Static))
            {
                if (method.GetCustomAttribute<RunOnStartupAttribute>() == null) continue;
                if (method.GetParameters().Length != 0) continue;
                method.Invoke(null, null);
            }
        }
    }

    public static void Run()
    {
        Person person = new Person();
        Console.WriteLine("Свойства объекта Person:");
        PrintProperties(person);

        Console.WriteLine("\nМетоды с RunOnStartup:");
        RunStartupTasks();
    }
}
