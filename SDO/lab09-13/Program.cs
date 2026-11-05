using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Лабораторная 13: коллекции ===");
        Console.WriteLine("--- Основное задание (вариант 3) ---");
        MainTask.Run();

        Section("Дополнительное задание 1: система учёта студентов");
        Dop1.Run();

        Section("Дополнительное задание 2: анализ текста коллекциями");
        Dop2.Run();

        Section("Дополнительное задание 3: обработка заказов");
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
    public class Student
    {
        public string Name { get; set; } = string.Empty;
        public int Grade { get; set; }
    }

    public static void Run()
    {
        Stack<int> stack = new Stack<int>();
        for (int i = 1; i <= 10; i++) stack.Push(i);

        Console.WriteLine("Стек после добавления чисел 1..10:");
        Console.WriteLine($"  Верхний элемент: {stack.Peek()}, количество: {stack.Count}");

        Console.WriteLine("\nИзвлекаем 3 числа:");
        for (int i = 0; i < 3; i++)
        {
            Console.WriteLine($"  Извлечено: {stack.Pop()}");
        }

        Console.WriteLine("\nДобавляем числа 20, 30, 40:");
        stack.Push(20);
        stack.Push(30);
        stack.Push(40);

        Console.WriteLine("Содержимое стека (сверху вниз):");
        foreach (int value in stack) Console.WriteLine($"  {value}");

        Console.WriteLine();

        List<Student> students = new List<Student>
        {
            new Student { Name = "Иван Петров", Grade = 4 },
            new Student { Name = "Анна Смирнова", Grade = 5 },
            new Student { Name = "Петр Сидоров", Grade = 3 },
            new Student { Name = "Мария Иванова", Grade = 5 },
            new Student { Name = "Ольга Кузнецова", Grade = 4 }
        };

        var topStudents = students.OrderByDescending(s => s.Grade).Take(3);
        Console.WriteLine("Топ-3 студента по оценке:");
        foreach (Student student in topStudents)
        {
            Console.WriteLine($"  {student.Name}: {student.Grade}");
        }
    }
}
