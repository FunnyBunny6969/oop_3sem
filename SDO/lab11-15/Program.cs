using System;
using System.Linq;
using System.Reflection;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Лабораторная 15: рефлексия ===");
        Console.WriteLine("--- Основное задание (вариант 3) ---");
        MainTask.Run();

        Section("Дополнительное задание 1: исследователь типов");
        Dop1.Run();

        Section("Дополнительное задание 2: сравнение объектов");
        Dop2.Run();

        Section("Дополнительное задание 3: информация об объекте");
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

public static class MainTask
{
    public class Rectangle
    {
        public double Width { get; set; }
        public double Height { get; set; }

        public Rectangle(double width, double height)
        {
            Width = width;
            Height = height;
        }

        public double GetArea() => Width * Height;
        public double GetPerimeter() => 2 * (Width + Height);
    }

    public class Square : Rectangle
    {
        public Square(double side) : base(side, side) { }
    }

    public static void Run()
    {
        Type type = typeof(Rectangle);
        Console.WriteLine($"Тип: {type.Name}");
        Console.WriteLine("Методы:");
        foreach (MethodInfo method in type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
        {
            string parameters = string.Join(", ", method.GetParameters().Select(p => $"{p.ParameterType.Name} {p.Name}"));
            Console.WriteLine($"  {method.ReturnType.Name} {method.Name}({parameters})");
        }

        Rectangle rectangle = new Rectangle(4, 5);
        MethodInfo areaMethod = type.GetMethod("GetArea")!;
        MethodInfo perimeterMethod = type.GetMethod("GetPerimeter")!;

        Console.WriteLine($"\nВызов GetArea(): {areaMethod.Invoke(rectangle, null)}");
        Console.WriteLine($"Вызов GetPerimeter(): {perimeterMethod.Invoke(rectangle, null)}");

        Console.WriteLine("\nКлассы, наследующие Rectangle:");
        foreach (Type assemblyType in Assembly.GetExecutingAssembly().GetTypes())
        {
            if (assemblyType.IsSubclassOf(type))
            {
                Console.WriteLine($"  {assemblyType.Name}");
            }
        }
    }
}
