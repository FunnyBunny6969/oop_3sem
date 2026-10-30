using System;
using System.Collections.Generic;

public class ProgramTask1
{
    public static void Run()
    {
        Console.WriteLine("=== Задание 1. Стандартные делегаты Action и Func ===\n");

        List<int> numbers = new List<int> { 2, 4, 6, 8 };
        Console.WriteLine($"Исходный список: {string.Join(", ", numbers)}");

        Func<List<int>, int> sumCalculator = CalculateSum;
        int sumResult = sumCalculator(numbers);
        Console.WriteLine($"Сумма чисел: {sumResult}");

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

        Action<List<int>> listPrinter = PrintList;
        Console.Write("Красивый вывод списка: ");
        listPrinter(numbers);
    }

    static int CalculateSum(List<int> list)
    {
        int sum = 0;
        foreach (int num in list)
        {
            sum += num;
        }
        return sum;
    }

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
