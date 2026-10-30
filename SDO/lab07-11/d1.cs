using System;
using System.Collections.Generic;

public class Task3_Calc
{
    // Объявляем делегат для математической операции
    public delegate double CalcOperation(double current, double number);

    // Статические методы для операций 
    public static double Add(double cur, double num) => cur + num;
    public static double Subtract(double cur, double num) => cur - num;
    public static double Multiply(double cur, double num) => cur * num;
    public static double Divide(double cur, double num)
    {
        if (num == 0) { Console.WriteLine("  [Ошибка]: Деление на ноль!"); return cur; }
        return cur / num;
    }

    // Главный статический метод для запуска допа
    public static void Run()
    {
        Console.WriteLine("=== ДОП 1. Калькулятор с отменой операций ===\n");

        double result = 10; // Стартовое число в памяти
        Stack<double> history = new Stack<double>(); // Стек для отмены (хранит старые результаты)

        Console.WriteLine($"Начальное значение в памяти: {result}");

        // Функция выполнения шага (имитируем действия пользователя)
        // Передаем делегат операции и число
        Action<CalcOperation, double, string> execute = (op, num, name) =>
        {
            history.Push(result); // Сохраняем текущее состояние перед изменением
            result = op(result, num);
            Console.WriteLine($"Применили {name} {num} -> Текущий результат: {result}");
        };

        // Имитируем вычисления
        execute(Add, 5, "сложение");        // 10 + 5 = 15
        execute(Multiply, 2, "умножение"); // 15 * 2 = 30
        execute(Subtract, 4, "вычитание"); // 30 - 4 = 26

        // Имитируем отмену операций (Undo)
        Console.WriteLine("\n--- Отмена последнего действия (Undo) ---");
        if (history.Count > 0)
        {
            result = history.Pop();
            Console.WriteLine($"Откат назад! Результат вернулся к: {result}"); // Должен стать 30
        }

        Console.WriteLine("\n--- Отмена еще одного действия (Undo) ---");
        if (history.Count > 0)
        {
            result = history.Pop();
            Console.WriteLine($"Откат назад! Результат вернулся к: {result}"); // Должен стать 15
        }
    }
}
