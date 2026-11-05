using System;
using System.Collections.Generic;
using System.Linq;

public static class Dop3
{
    private class Rule
    {
        public string Name { get; }
        public Predicate<string> Predicate { get; }
        public int Matches { get; set; }

        public Rule(string name, Predicate<string> predicate)
        {
            Name = name;
            Predicate = predicate;
        }
    }

    public class TextAnalyzer
    {
        private readonly List<string> _texts = new List<string>();
        private readonly List<Rule> _rules = new List<Rule>();

        public void AddText(string text) => _texts.Add(text);

        public void AddRule(string name, Predicate<string> rule)
        {
            _rules.Add(new Rule(name, rule));
        }

        public int CountSatisfying(string ruleName)
        {
            Rule rule = GetRule(ruleName);
            int count = _texts.Count(text => rule.Predicate(text));
            rule.Matches += count;
            return count;
        }

        public string? FindFirstSatisfying(string ruleName)
        {
            Rule rule = GetRule(ruleName);
            return _texts.FirstOrDefault(text => rule.Predicate(text));
        }

        public void Transform(Func<string, string> transformer)
        {
            for (int i = 0; i < _texts.Count; i++)
            {
                _texts[i] = transformer(_texts[i]);
            }
        }

        public void PrintStatistics()
        {
            foreach (Rule rule in _rules)
            {
                Console.WriteLine($"  Правило \"{rule.Name}\": совпадений {rule.Matches}");
            }
        }

        private Rule GetRule(string name)
        {
            Rule? rule = _rules.FirstOrDefault(r => r.Name == name);
            if (rule == null) throw new ArgumentException($"Правило \"{name}\" не найдено.");
            return rule;
        }

        public IEnumerable<string> Texts => _texts;
    }

    public static void Run()
    {
        TextAnalyzer analyzer = new TextAnalyzer();
        analyzer.AddText("Это длинное предложение для проверки.");
        analyzer.AddText("Короткое.");
        analyzer.AddText("Строка с буквой 'я' внутри.");
        analyzer.AddText("А роза упала на лапу Азора");
        analyzer.AddText("Ещё одна строка текста");

        analyzer.AddRule("Длина больше 10", text => text.Length > 10);
        analyzer.AddRule("Содержит 'я'", text => text.Contains('я'));
        analyzer.AddRule("Начинается с заглавной", text => text.Length > 0 && char.IsUpper(text[0]));
        analyzer.AddRule("Палиндром", text =>
        {
            string normalized = new string(text.Where(char.IsLetterOrDigit).ToArray()).ToLowerInvariant();
            return normalized.SequenceEqual(normalized.Reverse());
        });

        Console.WriteLine("Статистика по правилам:");
        foreach (string ruleName in new[] { "Длина больше 10", "Содержит 'я'", "Начинается с заглавной", "Палиндром" })
        {
            Console.WriteLine($"  {ruleName}: {analyzer.CountSatisfying(ruleName)}");
        }

        Console.WriteLine($"\nПервый палиндром: {analyzer.FindFirstSatisfying("Палиндром")}");

        analyzer.Transform(text => text.ToUpperInvariant());
        Console.WriteLine("\nТексты после преобразования в верхний регистр:");
        foreach (string text in analyzer.Texts)
        {
            Console.WriteLine($"  {text}");
        }
    }
}
