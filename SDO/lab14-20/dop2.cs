using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

public static class Dop2
{
    private class Statistics
    {
        public long Capitalized { get; set; }
        public long WithDigit { get; set; }
        public long Palindromes { get; set; }
        public long TotalLength { get; set; }
    }

    private static bool IsPalindrome(string word)
    {
        int left = 0;
        int right = word.Length - 1;
        while (left < right)
        {
            if (char.ToLowerInvariant(word[left]) != char.ToLowerInvariant(word[right])) return false;
            left++;
            right--;
        }
        return true;
    }

    private static bool HasDigit(string word)
    {
        foreach (char symbol in word)
        {
            if (char.IsDigit(symbol)) return true;
        }
        return false;
    }

    public static void Run()
    {
        const int wordCount = 1_000_000;
        string[] source =
        {
            "Привет", "мир", "level", "radar", "Anna", "test123", "42", "palindrome",
            "CSharp", "parallel", "data", "2026", "шалаш", "кот", "топот", "hello"
        };

        string[] words = new string[wordCount];
        Random random = new Random(42);
        for (int i = 0; i < wordCount; i++) words[i] = source[random.Next(source.Length)];

        Statistics total = new Statistics();
        object locker = new object();

        Stopwatch stopwatch = Stopwatch.StartNew();
        Parallel.For(0, wordCount,
            () => new Statistics(),
            (i, state, local) =>
            {
                string word = words[i];
                if (word.Length > 0 && char.IsUpper(word[0])) local.Capitalized++;
                if (HasDigit(word)) local.WithDigit++;
                if (IsPalindrome(word)) local.Palindromes++;
                local.TotalLength += word.Length;
                return local;
            },
            local =>
            {
                lock (locker)
                {
                    total.Capitalized += local.Capitalized;
                    total.WithDigit += local.WithDigit;
                    total.Palindromes += local.Palindromes;
                    total.TotalLength += local.TotalLength;
                }
            });
        stopwatch.Stop();

        Console.WriteLine($"Слов в массиве: {wordCount:N0}");
        Console.WriteLine($"  С заглавной буквы: {total.Capitalized:N0}");
        Console.WriteLine($"  Содержат цифру: {total.WithDigit:N0}");
        Console.WriteLine($"  Палиндромы: {total.Palindromes:N0}");
        Console.WriteLine($"  Общая длина: {total.TotalLength:N0}");
        Console.WriteLine($"  Время параллельного анализа: {stopwatch.ElapsedMilliseconds} мс");
    }
}
