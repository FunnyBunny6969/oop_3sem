using System;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Лабораторная 09: исключения ===");
        Console.WriteLine("--- Основное задание (вариант 3) ---");
        MainTask.Run();

        Section("Дополнительное задание 1: калькулятор с обработкой ошибок");
        Dop1.Run();

        Section("Дополнительное задание 2: банковские переводы");
        Dop2.Run();

        Section("Дополнительное задание 3: валидация пользователя");
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
    public class EmptyStringException : Exception
    {
        public EmptyStringException(string message) : base(message) { }
    }

    public static void ValidateString(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            throw new EmptyStringException("Строка не может быть пустой.");
        Console.WriteLine($"Строка корректна: \"{text}\"");
    }

    public static void Run()
    {
        string[] inputs = { "123", "abc", "999999999999999999999", "" };

        foreach (string input in inputs)
        {
            Console.WriteLine($"\nВходная строка: \"{input}\"");
            try
            {
                int value = int.Parse(input);
                Console.WriteLine($"Успешно преобразовано: {value}");
            }
            catch (FormatException)
            {
                Console.WriteLine("Ошибка: строка имеет неверный формат числа.");
            }
            catch (OverflowException)
            {
                Console.WriteLine("Ошибка: число выходит за пределы диапазона int.");
            }
            finally
            {
                Console.WriteLine("Блок finally выполнен.");
            }
        }

        Console.WriteLine();
        foreach (string input in new[] { "Проверка", "", "   " })
        {
            try
            {
                ValidateString(input);
            }
            catch (EmptyStringException ex)
            {
                Console.WriteLine($"Ошибка валидации: {ex.Message}");
            }
        }
    }
}
