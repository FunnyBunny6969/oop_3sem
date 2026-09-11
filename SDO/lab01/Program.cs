using System;

class Program
{
    static void Main()
    {
        // === TASK1 ===
        bool flag = true;
        Console.WriteLine($"Исходное значение flag: {flag} (тип: {flag.GetType()})");
        object boxedFlag = flag;
        Console.WriteLine($"После упаковки boxedFlag: {boxedFlag} (тип: {boxedFlag.GetType()})");
        bool unboxedFlag = (bool)boxedFlag;
        Console.WriteLine($"После распаковки unboxedFlag: {unboxedFlag} (тип: {unboxedFlag.GetType()})");
        bool areEqual = (flag == unboxedFlag);
        Console.WriteLine($"\nЗначения совпадают: {areEqual}");

        // === TASK2 ===
        string text = "Привет";
        Console.WriteLine($"До вызова метода: text = \"{text}\"");
        AddExclamationMark(ref text);
        Console.WriteLine($"После вызова метода: text = \"{text}\"");
    }

    static void AddExclamationMark(ref string s)
    {
        s = s + "!";
        Console.WriteLine($"Внутри метода: s = \"{s}\"");
    }
}