using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Лабораторная 18: сериализация ===");
        Console.WriteLine("--- Основное задание (вариант 3) ---");
        MainTask.Run();

        Section("Дополнительное задание 1: универсальный сериализатор");
        Dop1.Run();

        Section("Дополнительное задание 2: конвертер форматов");
        Dop2.Run();

        Section("Дополнительное задание 3: сравнение производительности");
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

public class Student
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("full_name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("group")]
    public string Group { get; set; } = string.Empty;

    [JsonPropertyName("grades")]
    public List<int> Grades { get; set; } = new List<int>();
}

public static class MainTask
{
    public static void Run()
    {
        List<Student> students = new List<Student>
        {
            new Student { Id = 1, Name = "Иван Петров", Group = "ПИ-101", Grades = new List<int> { 5, 4, 5 } },
            new Student { Id = 2, Name = "Анна Смирнова", Group = "ПИ-101", Grades = new List<int> { 5, 5, 5 } },
            new Student { Id = 3, Name = "Петр Сидоров", Group = "ПИ-102", Grades = new List<int> { 3, 4, 4 } }
        };

        string path = Path.Combine(Path.GetTempPath(), "lab12_18_students.json");
        JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };

        string json = JsonSerializer.Serialize(students, options);
        File.WriteAllText(path, json);

        Console.WriteLine($"JSON сохранён в файл: {path}");
        Console.WriteLine(json);

        string loadedJson = File.ReadAllText(path);
        List<Student>? loaded = JsonSerializer.Deserialize<List<Student>>(loadedJson, options);
        Console.WriteLine("Десериализованные студенты:");
        if (loaded != null)
        {
            foreach (Student student in loaded)
            {
                Console.WriteLine($"  {student.Id}. {student.Name} ({student.Group}): {string.Join(", ", student.Grades)}");
            }
        }
    }
}
