using System;
using System.IO;
using System.Text;

public static class Dop2
{
    public static string ChatPath => Path.Combine(Path.GetTempPath(), "lab12_17_chat.txt");

    public static void AppendMessage(string author, string message)
    {
        string line = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {author}: {message}{Environment.NewLine}";
        File.AppendAllText(ChatPath, line, Encoding.UTF8);
    }

    public static void ReadChat()
    {
        if (!File.Exists(ChatPath))
        {
            Console.WriteLine("Файл чата не найден.");
            return;
        }

        foreach (string line in File.ReadAllLines(ChatPath))
        {
            Console.WriteLine($"  {line}");
        }
    }

    public static void Run()
    {
        if (File.Exists(ChatPath)) File.Delete(ChatPath);

        AppendMessage("Иван", "Привет! Как дела?");
        AppendMessage("Анна", "Привет! Всё хорошо, спасибо.");
        AppendMessage("Иван", "Отлично, до встречи.");

        Console.WriteLine($"Файл чата: {ChatPath}");
        Console.WriteLine("История сообщений:");
        ReadChat();
    }
}
