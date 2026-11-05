using System;
using System.Collections.Generic;
using System.Linq;

public static class Dop2
{
    public static void Run()
    {
        string text = "Это пример текста для анализа. Текст содержит слова, слова и ещё раз слова. " +
                      "Анализ текста показывает частоту слов и количество уникальных слов.";

        string[] words = text
            .ToLowerInvariant()
            .Split(new[] { ' ', '.', ',', '!', '?', ';', ':' }, StringSplitOptions.RemoveEmptyEntries);

        List<string> allWords = words.ToList();
        Dictionary<string, int> frequencies = new Dictionary<string, int>();
        foreach (string word in allWords)
        {
            frequencies[word] = frequencies.TryGetValue(word, out int count) ? count + 1 : 1;
        }

        HashSet<string> uniqueWords = new HashSet<string>(allWords);
        SortedDictionary<string, int> sorted = new SortedDictionary<string, int>(frequencies);

        Queue<string> lastTen = new Queue<string>();
        foreach (string word in allWords)
        {
            if (lastTen.Count == 10) lastTen.Dequeue();
            lastTen.Enqueue(word);
        }

        Console.WriteLine($"Всего слов: {allWords.Count}");
        Console.WriteLine($"Уникальных слов: {uniqueWords.Count}");
        Console.WriteLine($"Последние 10 слов: {string.Join(", ", lastTen)}");

        Console.WriteLine("\nТоп-10 самых частых слов:");
        foreach (KeyValuePair<string, int> pair in frequencies.OrderByDescending(p => p.Value).Take(10))
        {
            Console.WriteLine($"  {pair.Key}: {pair.Value}");
        }

        Console.WriteLine("\nСлова в алфавитном порядке (первые 10):");
        foreach (KeyValuePair<string, int> pair in sorted.Take(10))
        {
            Console.WriteLine($"  {pair.Key}: {pair.Value}");
        }
    }
}
