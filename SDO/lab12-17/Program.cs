using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Лабораторная 17: работа с файлами ===");
        Console.WriteLine("--- Основное задание (вариант 3) ---");
        MainTask.Run();

        Section("Дополнительное задание 1: анализ текстового файла");
        Dop1.Run();

        Section("Дополнительное задание 2: файл чата");
        Dop2.Run();

        Section("Дополнительное задание 3: организация файлов");
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
    public static string WorkDirectory => Path.Combine(Path.GetTempPath(), "lab12_17_main");

    public static void Run()
    {
        Directory.CreateDirectory(WorkDirectory);
        string numbersPath = Path.Combine(WorkDirectory, "numbers.txt");
        string evenPath = Path.Combine(WorkDirectory, "even.txt");
        string oddPath = Path.Combine(WorkDirectory, "odd.txt");

        File.WriteAllLines(numbersPath, Enumerable.Range(1, 20).Select(i => i.ToString()));

        List<int> numbers = File.ReadAllLines(numbersPath)
            .Select(int.Parse)
            .ToList();

        File.WriteAllLines(evenPath, numbers.Where(n => n % 2 == 0).Select(n => n.ToString()));
        File.WriteAllLines(oddPath, numbers.Where(n => n % 2 != 0).Select(n => n.ToString()));

        Console.WriteLine($"Файл numbers.txt: {string.Join(", ", numbers)}");
        Console.WriteLine($"Чётные: {string.Join(", ", File.ReadAllLines(evenPath))}");
        Console.WriteLine($"Нечётные: {string.Join(", ", File.ReadAllLines(oddPath))}");

        string textPath = Path.Combine(WorkDirectory, "text.txt");
        File.WriteAllText(textPath, "Привет, мир! Это тестовый файл.\nВторая строка.");
        string text = File.ReadAllText(textPath);
        Console.WriteLine($"\nСимволов в text.txt: {text.Length}");
    }
}
