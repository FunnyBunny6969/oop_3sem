using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

public static class Dop1
{
    public class TextStatistics
    {
        public int Characters { get; set; }
        public int Letters { get; set; }
        public int Digits { get; set; }
        public int Punctuation { get; set; }
        public int Words { get; set; }
        public int Sentences { get; set; }
        public int Lines { get; set; }
        public double AverageWordLength { get; set; }
        public string LongestWord { get; set; } = string.Empty;
    }

    public static TextStatistics Analyze(string text)
    {
        MatchCollection wordMatches = Regex.Matches(text, @"[A-Za-zА-Яа-яЁё]+");
        string[] words = wordMatches.Cast<Match>().Select(m => m.Value).ToArray();
        int sentenceCount = Regex.Matches(text, @"[.!?]+").Count;

        return new TextStatistics
        {
            Characters = text.Length,
            Letters = text.Count(char.IsLetter),
            Digits = text.Count(char.IsDigit),
            Punctuation = text.Count(char.IsPunctuation),
            Words = words.Length,
            Sentences = sentenceCount,
            Lines = text.Split('\n').Length,
            AverageWordLength = words.Length == 0 ? 0 : words.Average(w => w.Length),
            LongestWord = words.Length == 0 ? string.Empty : words.OrderByDescending(w => w.Length).First()
        };
    }

    public static void Run()
    {
        string path = Path.Combine(Path.GetTempPath(), "lab12_17_text.txt");
        string text = "Это пример текстового файла. В нём есть слова, цифры 123 и знаки препинания! " +
                      "Анализ файла выполняется программно.\nВторая строка содержит ещё несколько слов.";
        File.WriteAllText(path, text);

        TextStatistics statistics = Analyze(File.ReadAllText(path));
        Console.WriteLine($"Файл: {path}");
        Console.WriteLine($"  Символов: {statistics.Characters}");
        Console.WriteLine($"  Букв: {statistics.Letters}");
        Console.WriteLine($"  Цифр: {statistics.Digits}");
        Console.WriteLine($"  Знаков препинания: {statistics.Punctuation}");
        Console.WriteLine($"  Слов: {statistics.Words}");
        Console.WriteLine($"  Предложений: {statistics.Sentences}");
        Console.WriteLine($"  Строк: {statistics.Lines}");
        Console.WriteLine($"  Средняя длина слова: {statistics.AverageWordLength:F2}");
        Console.WriteLine($"  Самое длинное слово: {statistics.LongestWord}");
    }
}
