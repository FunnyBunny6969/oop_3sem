using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

public static class Dop3
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public double Price { get; set; }
    }

    public class Catalog
    {
        public string Title { get; set; } = string.Empty;
        public List<Item> Items { get; set; } = new List<Item>();
    }

    private class Result
    {
        public string Format { get; init; } = string.Empty;
        public double SerializeMs { get; init; }
        public double DeserializeMs { get; init; }
        public long Size { get; init; }
    }

    public static void Run()
    {
        Catalog catalog = new Catalog { Title = "Каталог товаров" };
        for (int i = 1; i <= 100; i++)
        {
            catalog.Items.Add(new Item { Id = i, Name = $"Товар {i}", Price = i * 99.5 });
        }

        string directory = Path.Combine(Path.GetTempPath(), "lab12_18_benchmark");
        Directory.CreateDirectory(directory);

        List<Result> results = new List<Result>();
        foreach (string extension in new[] { ".json", ".xml", ".dat" })
        {
            string path = Path.Combine(directory, "catalog" + extension);
            Stopwatch stopwatch = Stopwatch.StartNew();
            Dop1.UniversalSerializer<Catalog>.Serialize(catalog, path);
            stopwatch.Stop();
            double serializeMs = stopwatch.Elapsed.TotalMilliseconds;

            stopwatch.Restart();
            Catalog restored = Dop1.UniversalSerializer<Catalog>.Deserialize(path);
            stopwatch.Stop();
            double deserializeMs = stopwatch.Elapsed.TotalMilliseconds;

            results.Add(new Result
            {
                Format = extension,
                SerializeMs = serializeMs,
                DeserializeMs = deserializeMs,
                Size = new FileInfo(path).Length
            });

            Console.WriteLine($"{extension}: элементов после чтения {restored.Items.Count}");
        }

        Console.WriteLine();
        Console.WriteLine($"{"Формат",-8} {"Сериализация, мс",20} {"Десериализация, мс",22} {"Размер, байт",15}");
        foreach (Result result in results)
        {
            Console.WriteLine($"{result.Format,-8} {result.SerializeMs,20:F3} {result.DeserializeMs,22:F3} {result.Size,15}");
        }
    }
}
