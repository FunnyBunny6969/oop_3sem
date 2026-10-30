using System;
using System.Collections.Generic;

public class ProgramTask1
{
    public static void Run()
    {
        Console.WriteLine("=== Задание 1. Стандартные делегаты Action и Func ===\n");

        // Создаем исходный список чисел для обработки
        List<int> numbers = new List<int> { 2, 4, 6, 8 };
        Console.WriteLine($"Исходный список: {string.Join(", ", numbers)}");

        // 1. Использование Func<List<int>, int> для подсчета суммы
        // Делегат принимает список чисел и возвращает одно число (сумму)
        Func<List<int>, int> sumCalculator = CalculateSum;
        int sumResult = sumCalculator(numbers);
        Console.WriteLine($"Сумма чисел: {sumResult}");

        // 2. Использование Func<List<int>, int> для подсчета произведения
        // Передаем метод через лямбда-выражение для разнообразия синтаксиса
        Func<List<int>, int> productCalculator = list =>
        {
            if (list.Count == 0) return 0;
            int product = 1;
            foreach (int num in list)
            {
                product *= num;
            }
            return product;
        };
        int productResult = productCalculator(numbers);
        Console.WriteLine($"Произведение чисел: {productResult}");

        // 3. Использование Action<List<int>> для вывода элементов на экран
        // Делегат ничего не возвращает (void)
        Action<List<int>> listPrinter = PrintList;
        Console.Write("Красивый вывод списка: ");
        listPrinter(numbers);
    }

    // Метод для подсчета суммы (соответствует Func<List<int>, int>)
    static int CalculateSum(List<int> list)
    {
        int sum = 0;
        foreach (int num in list)
        {
            sum += num;
        }
        return sum;
    }

    // Метод для вывода списка на экран (соответствует Action<List<int>>)
    static void PrintList(List<int> list)
    {
        Console.Write("[ ");
        for (int i = 0; i < list.Count; i++)
        {
            Console.Write(list[i]);
            if (i < list.Count - 1) Console.Write(" | ");
        }
        Console.WriteLine(" ]");
    }
}
