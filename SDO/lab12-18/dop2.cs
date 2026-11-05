using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Xml.Serialization;

public static class Dop2
{
    public class Person
    {
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
    }

    public static class FormatConverter
    {
        private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions { WriteIndented = true };

        public static object? Read(string path, Type type)
        {
            string extension = Path.GetExtension(path).ToLowerInvariant();
            switch (extension)
            {
                case ".json":
                    return JsonSerializer.Deserialize(File.ReadAllText(path), type);
                case ".xml":
                    XmlSerializer xmlSerializer = new XmlSerializer(type);
                    using (StreamReader reader = new StreamReader(path))
                    {
                        return xmlSerializer.Deserialize(reader);
                    }
                case ".dat":
                    return JsonSerializer.Deserialize(File.ReadAllBytes(path), type);
                default:
                    throw new NotSupportedException($"Формат {extension} не поддерживается.");
            }
        }

        public static void Write(string path, object value, Type type)
        {
            string extension = Path.GetExtension(path).ToLowerInvariant();
            switch (extension)
            {
                case ".json":
                    File.WriteAllText(path, JsonSerializer.Serialize(value, type, JsonOptions));
                    break;
                case ".xml":
                    XmlSerializer xmlSerializer = new XmlSerializer(type);
                    using (StreamWriter writer = new StreamWriter(path))
                    {
                        xmlSerializer.Serialize(writer, value);
                    }
                    break;
                case ".dat":
                    File.WriteAllBytes(path, JsonSerializer.SerializeToUtf8Bytes(value, type, JsonOptions));
                    break;
                default:
                    throw new NotSupportedException($"Формат {extension} не поддерживается.");
            }
        }

        public static void Convert(string inputPath, string outputPath, Type type)
        {
            object? value = Read(inputPath, type);
            if (value == null) throw new InvalidDataException("Не удалось прочитать входной файл.");
            Write(outputPath, value, type);
        }

        public static void ConvertAll(IEnumerable<string> inputPaths, string outputExtension, Type type)
        {
            foreach (string inputPath in inputPaths)
            {
                string outputPath = Path.ChangeExtension(inputPath, outputExtension);
                Convert(inputPath, outputPath, type);
                Console.WriteLine($"  {Path.GetFileName(inputPath)} -> {Path.GetFileName(outputPath)}");
            }
        }
    }

    public static void Run()
    {
        string directory = Path.Combine(Path.GetTempPath(), "lab12_18_converter");
        Directory.CreateDirectory(directory);

        Person person = new Person
        {
            Name = "Иван Петров",
            Age = 30,
            Tags = new List<string> { "студент", "разработчик" }
        };

        string jsonPath = Path.Combine(directory, "person.json");
        string xmlPath = Path.Combine(directory, "person.xml");
        string datPath = Path.Combine(directory, "person.dat");

        FormatConverter.Write(jsonPath, person, typeof(Person));
        FormatConverter.Convert(jsonPath, xmlPath, typeof(Person));
        FormatConverter.Convert(xmlPath, datPath, typeof(Person));

        Console.WriteLine("Цепочка конвертаций:");
        Console.WriteLine($"  {Path.GetFileName(jsonPath)} -> {Path.GetFileName(xmlPath)} -> {Path.GetFileName(datPath)}");
        Console.WriteLine($"  Размеры: JSON {new FileInfo(jsonPath).Length} байт, XML {new FileInfo(xmlPath).Length} байт, DAT {new FileInfo(datPath).Length} байт");

        Person? restored = FormatConverter.Read(datPath, typeof(Person)) as Person;
        Console.WriteLine($"  Восстановлено: {restored?.Name}, {restored?.Age}, {string.Join(", ", restored?.Tags ?? new List<string>())}");

        Console.WriteLine("\nПакетная конвертация JSON -> XML:");
        FormatConverter.ConvertAll(new[] { jsonPath }, ".xml", typeof(Person));
    }
}
