using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Лабораторная 12: делегаты и лямбда-выражения ===");
        Console.WriteLine("--- Основное задание (вариант 3) ---");
        MainTask.Run();

        Section("Дополнительное задание 1: LambdaCalculator");
        Dop1.Run();

        Section("Дополнительное задание 2: PredicateComposer");
        Dop2.Run();

        Section("Дополнительное задание 3: TextAnalyzer");
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
    private static bool IsPalindrome(string text)
    {
        string normalized = new string(text.Where(char.IsLetterOrDigit).ToArray()).ToLowerInvariant();
        int left = 0;
        int right = normalized.Length - 1;
        while (left < right)
        {
            if (normalized[left] != normalized[right]) return false;
            left++;
            right--;
        }
        return true;
    }

    public static void Run()
    {
        string[] words =
        {
            "кот", "программирование", "шалаш", "делегаты",
            "А роза упала на лапу Азора", "лямбда", "довод"
        };

        Console.WriteLine("Исходный массив строк:");
        foreach (string word in words) Console.WriteLine($"  {word}");

        string[] longWords = Array.FindAll(words, word => word.Length > 5);
        Console.WriteLine("\nСтроки длиной больше 5 символов:");
        foreach (string word in longWords) Console.WriteLine($"  {word}");

        string[] upperWords = Array.ConvertAll(longWords, word => word.ToUpperInvariant());
        Array.Sort(upperWords);
        Console.WriteLine("\nПреобразованные в верхний регистр и отсортированные:");
        foreach (string word in upperWords) Console.WriteLine($"  {word}");

        Predicate<string> palindrome = text => IsPalindrome(text);
        Console.WriteLine("\nПроверка палиндромов:");
        foreach (string word in words)
        {
            Console.WriteLine($"  \"{word}\" — {(palindrome(word) ? "палиндром" : "не палиндром")}");
        }

        string? lastPalindrome = Array.FindLast(words, palindrome);
        Console.WriteLine($"\nПоследний палиндром в массиве: \"{lastPalindrome}\"");
    }
}
