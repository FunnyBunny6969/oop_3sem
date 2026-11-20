using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Xml.Serialization;

public static class Dop4
{
    public class SmartSerializer<T> where T : new()
    {
        public long XmlLimit { get; set; } = 1024;          // 1 КБ
        public long JsonLimit { get; set; } = 100 * 1024;   // 100 КБ

        public void SaveSmart(T obj, string baseName)
        {
            byte[] jsonBytes = JsonSerializer.SerializeToUtf8Bytes(obj);
            long size = jsonBytes.Length;

            if (size <= XmlLimit)
            {
                using var fs = File.Create(baseName + ".xml");
                new XmlSerializer(typeof(T)).Serialize(fs, obj);
                Console.WriteLine($"Сохранено в XML ({size} байт)");
            }
            else if (size <= JsonLimit)
            {
                File.WriteAllBytes(baseName + ".json", jsonBytes);
                Console.WriteLine($"Сохранено в JSON ({size} байт)");
            }
            else
            {
                using var bw = new BinaryWriter(File.Create(baseName + ".bin"));
                bw.Write(jsonBytes.Length);
                bw.Write(jsonBytes);
                Console.WriteLine($"Сохранено в BINARY ({size} байт)");
            }
        }

        public T LoadSmart(string baseName)
        {
            // Ищем любой файл с таким именем (хоть .xml, хоть .json, хоть без расширения)
            string file = File.Exists(baseName) 
                ? baseName 
                : Directory.GetFiles(".", baseName + ".*")[0];

            byte[] data = File.ReadAllBytes(file);
            string text = Encoding.UTF8.GetString(data).TrimStart();

            // Определяем формат строго по содержимому, а не по расширению
            if (text.StartsWith("<"))
            {
                Console.WriteLine($"Файл {file} определен как XML");
                using var ms = new MemoryStream(data);
                return (T)new XmlSerializer(typeof(T)).Deserialize(ms)!;
            }
            if (text.StartsWith("{") || text.StartsWith("["))
            {
                Console.WriteLine($"Файл {file} определен как JSON");
                return JsonSerializer.Deserialize<T>(data)!;
            }

            Console.WriteLine($"Файл {file} определен как BINARY");
            using var br = new BinaryReader(new MemoryStream(data));
            int len = br.ReadInt32();
            return JsonSerializer.Deserialize<T>(br.ReadBytes(len))!;
        }
    }

    public class DataModel
    {
        public string Name { get; set; } = "";
        public string Payload { get; set; } = "";
    }

    public static void Run()
    {
        var serializer = new SmartSerializer<DataModel>();

        // 1. Маленький (< 1 КБ) -> XML
        serializer.SaveSmart(new DataModel { Name = "Малый", Payload = "Привет" }, "test1");
        var res1 = serializer.LoadSmart("test1");

        // 2. Средний (1 - 100 КБ) -> JSON
        serializer.SaveSmart(new DataModel { Name = "Средний", Payload = new string('A', 5000) }, "test2");
        var res2 = serializer.LoadSmart("test2");

        // 3. Большой (> 100 КБ) -> Binary
        serializer.SaveSmart(new DataModel { Name = "Большой", Payload = new string('B', 150000) }, "test3");
        var res3 = serializer.LoadSmart("test3");

        // 4. Меняем порог: теперь 5000 байт тоже пойдут в XML
        serializer.XmlLimit = 10000;
        serializer.SaveSmart(new DataModel { Name = "Кастом", Payload = new string('C', 5000) }, "test4");
    }
}
