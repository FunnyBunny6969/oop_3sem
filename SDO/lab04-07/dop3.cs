using System;
using System.Collections.Generic;

public static class Dop3
{
    public class Student : IComparable<Student>
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public double Grade { get; set; }

        public Student(string name, int age, double grade)
        {
            Name = name;
            Age = age;
            Grade = grade;
        }

        public int CompareTo(Student? other)
        {
            if (other == null) return 1;
            return Grade.CompareTo(other.Grade);
        }

        public override string ToString() => $"{Name}, возраст {Age}, средний балл {Grade:F2}";
    }

    public class StudentComparer : IComparer<Student>
    {
        public int Compare(Student? x, Student? y)
        {
            if (x == null && y == null) return 0;
            if (x == null) return -1;
            if (y == null) return 1;
            return string.Compare(x.Name, y.Name, StringComparison.CurrentCulture);
        }
    }

    public static void Run()
    {
        List<Student> students = new List<Student>
        {
            new Student("Иван Петров", 20, 4.5),
            new Student("Анна Смирнова", 19, 4.9),
            new Student("Петр Сидоров", 21, 4.1),
            new Student("Мария Иванова", 18, 4.7)
        };

        Console.WriteLine("Исходный список:");
        foreach (Student student in students) Console.WriteLine($"  {student}");

        students.Sort();
        Console.WriteLine("\nСортировка по среднему баллу (IComparable):");
        foreach (Student student in students) Console.WriteLine($"  {student}");

        students.Sort(new StudentComparer());
        Console.WriteLine("\nСортировка по имени (IComparer):");
        foreach (Student student in students) Console.WriteLine($"  {student}");
    }
}
