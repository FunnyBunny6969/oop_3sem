using System;
using System.Diagnostics;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        Console.WriteLine("=== Лабораторная 21: асинхронное программирование ===");
        Console.WriteLine("--- Основное задание (вариант 3) ---");
        await MainTask.RunAsync();

        Section("Дополнительное задание 1: асинхронный калькулятор");
        await Dop1.RunAsync();

        Section("Дополнительное задание 2: обработчик задач с приоритетами");
        await Dop2.RunAsync();

        Section("Дополнительное задание 3: параллельное вычисление числа Пи");
        await Dop3.RunAsync();
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
    public static async Task<long> FactorialAsync(int number)
    {
        if (number < 0) throw new ArgumentException("Число не может быть отрицательным.");

        await Task.Delay(100);
        long result = 1;
        for (int i = 2; i <= number; i++) result *= i;
        return result;
    }

    public static async Task RunAsync()
    {
        Task<long>[] tasks =
        {
            FactorialAsync(5),
            FactorialAsync(7),
            FactorialAsync(10)
        };

        long[] results = await Task.WhenAll(tasks);
        Console.WriteLine($"Факториалы: 5! = {results[0]}, 7! = {results[1]}, 10! = {results[2]}");

        try
        {
            await FactorialAsync(-1);
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Обработано исключение: {ex.Message}");
        }
    }
}
