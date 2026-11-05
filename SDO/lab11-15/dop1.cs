using System;
using System.Linq;
using System.Reflection;

public static class Dop1
{
    public interface IWorker
    {
        void Work();
    }

    public class Student : IWorker
    {
        public string Name { get; set; } = "Иван";
        public int Age { get; set; } = 20;
        public double AverageGrade { get; set; } = 4.5;

        public Student() { }
        public Student(string name, int age) { Name = name; Age = age; }

        public void Work() => Console.WriteLine("Студент учится.");
        public string GetInfo() => $"{Name}, {Age} лет, средний балл {AverageGrade:F2}";
    }

    public static void Explore(Type type)
    {
        Console.WriteLine($"Имя: {type.Name}");
        Console.WriteLine($"Полное имя: {type.FullName}");
        Console.WriteLine($"Базовый тип: {type.BaseType?.Name}");
        Console.WriteLine($"Класс: {type.IsClass}, структура: {type.IsValueType}, интерфейс: {type.IsInterface}");

        Console.WriteLine("Интерфейсы: " + string.Join(", ", type.GetInterfaces().Select(i => i.Name)));

        Console.WriteLine("Свойства:");
        foreach (PropertyInfo property in type.GetProperties())
        {
            Console.WriteLine($"  {property.PropertyType.Name} {property.Name} (get: {property.CanRead}, set: {property.CanWrite})");
        }

        Console.WriteLine("Методы:");
        foreach (MethodInfo method in type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
        {
            Console.WriteLine($"  {method.ReturnType.Name} {method.Name}({string.Join(", ", method.GetParameters().Select(p => p.ParameterType.Name))})");
        }

        Console.WriteLine("Конструкторы:");
        foreach (ConstructorInfo constructor in type.GetConstructors())
        {
            Console.WriteLine($"  ({string.Join(", ", constructor.GetParameters().Select(p => p.ParameterType.Name))})");
        }
    }

    public static void Run()
    {
        Explore(typeof(Student));
        Console.WriteLine();
        Explore(typeof(IWorker));
    }
}
