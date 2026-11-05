using System;
using System.IO;
using System.Text.Json;
using System.Xml.Serialization;

public static class Dop1
{
    public class Book
    {
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public int Year { get; set; }
    }

    public static class UniversalSerializer<T> where T : class, new()
    {
        private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions { WriteIndented = true };

        public static void Serialize(T value, string path)
        {
            string extension = Path.GetExtension(path).ToLowerInvariant();
            switch (extension)
            {
                case ".json":
                    File.WriteAllText(path, JsonSerializer.Serialize(value, JsonOptions));
                    break;
                case ".xml":
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                    using (StreamWriter writer = new StreamWriter(path))
                    {
                        xmlSerializer.Serialize(writer, value);
                    }
                    break;
                case ".dat":
                    File.WriteAllBytes(path, JsonSerializer.SerializeToUtf8Bytes(value, JsonOptions));
                    break;
                default:
                    throw new NotSupportedException($"Формат {extension} не поддерживается.");
            }
        }

        public static T Deserialize(string path)
        {
            string extension = Path.GetExtension(path).ToLowerInvariant();
            switch (extension)
            {
                case ".json":
                    return JsonSerializer.Deserialize<T>(File.ReadAllText(path), JsonOptions)
                           ?? throw new InvalidDataException("Не удалось прочитать JSON.");
                case ".xml":
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                    using (StreamReader reader = new StreamReader(path))
                    {
                        return (T)(xmlSerializer.Deserialize(reader)
                                  ?? throw new InvalidDataException("Не удалось прочитать XML."));
                    }
                case ".dat":
                    return JsonSerializer.Deserialize<T>(File.ReadAllBytes(path), JsonOptions)
                           ?? throw new InvalidDataException("Не удалось прочитать бинарный файл.");
                default:
                    throw new NotSupportedException($"Формат {extension} не поддерживается.");
            }
        }
    }

    public static void Run()
    {
        string directory = Path.Combine(Path.GetTempPath(), "lab12_18_universal");
        Directory.CreateDirectory(directory);

        Book book = new Book
        {
            Title = "CLR via C#",
            Author = "Джеффри Рихтер",
            Year = 2012
        };

        foreach (string extension in new[] { ".json", ".xml", ".dat" })
        {
            string path = Path.Combine(directory, "book" + extension);
            UniversalSerializer<Book>.Serialize(book, path);
            Book restored = UniversalSerializer<Book>.Deserialize(path);
            FileInfo info = new FileInfo(path);
            Console.WriteLine($"{extension}: {info.Length} байт, восстановлено: \"{restored.Title}\", {restored.Year}");
        }
    }
}
