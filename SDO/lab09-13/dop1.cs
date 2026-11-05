using System;
using System.Collections.Generic;
using System.Linq;

public static class Dop1
{
    public class Student
    {
        public string Name { get; set; } = string.Empty;
        public string Group { get; set; } = string.Empty;
        public List<int> Grades { get; set; } = new List<int>();

        public double AverageGrade => Grades.Count == 0 ? 0 : Grades.Average();
    }

    public class StudentManager
    {
        private readonly Dictionary<string, List<Student>> _groups = new Dictionary<string, List<Student>>();

        public void AddStudent(string group, Student student)
        {
            if (!_groups.ContainsKey(group)) _groups[group] = new List<Student>();
            _groups[group].Add(student);
        }

        public void RemoveStudent(string group, string name)
        {
            if (!_groups.TryGetValue(group, out List<Student>? students))
                throw new KeyNotFoundException($"Группа {group} не найдена.");
            students.RemoveAll(s => s.Name == name);
        }

        public List<Student> GetGroup(string group)
        {
            if (!_groups.TryGetValue(group, out List<Student>? students))
                throw new KeyNotFoundException($"Группа {group} не найдена.");
            return students;
        }

        public void PrintAll()
        {
            foreach (KeyValuePair<string, List<Student>> group in _groups)
            {
                Console.WriteLine($"Группа {group.Key}:");
                foreach (Student student in group.Value)
                {
                    Console.WriteLine($"  {student.Name}: средний балл {student.AverageGrade:F2}");
                }
            }
        }

        public void PrintGroupAverages()
        {
            foreach (KeyValuePair<string, List<Student>> group in _groups)
            {
                double average = group.Value.Average(s => s.AverageGrade);
                Console.WriteLine($"  {group.Key}: {group.Value.Count} студентов, средний балл {average:F2}");
            }
        }

        public List<Student> SortByAverage(string group) =>
            GetGroup(group).OrderByDescending(s => s.AverageGrade).ToList();
    }

    public static void Run()
    {
        StudentManager manager = new StudentManager();
        manager.AddStudent("ПИ-101", new Student { Name = "Иван Петров", Grades = new List<int> { 5, 4, 5 } });
        manager.AddStudent("ПИ-101", new Student { Name = "Анна Смирнова", Grades = new List<int> { 5, 5, 5 } });
        manager.AddStudent("ПИ-102", new Student { Name = "Петр Сидоров", Grades = new List<int> { 3, 4, 3 } });
        manager.AddStudent("ПИ-102", new Student { Name = "Мария Иванова", Grades = new List<int> { 4, 4, 5 } });

        manager.PrintAll();
        Console.WriteLine("\nСредние баллы по группам:");
        manager.PrintGroupAverages();

        Console.WriteLine("\nСортировка группы ПИ-101 по среднему баллу:");
        foreach (Student student in manager.SortByAverage("ПИ-101"))
        {
            Console.WriteLine($"  {student.Name}: {student.AverageGrade:F2}");
        }

        manager.RemoveStudent("ПИ-102", "Петр Сидоров");
        Console.WriteLine("\nПосле удаления Петра из ПИ-102:");
        manager.PrintAll();
    }
}
