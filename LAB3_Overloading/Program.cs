using System;
using System.Linq;

// Основной класс: одномерный массив
public class MyArray
{
    public int[] Values { get; private set; }

    // Вложенный класс Production
    public class Production
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public Production Prod { get; set; }

    // Вложенный класс Developer
    public class Developer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
    }
    public Developer Dev { get; set; }

    // Конструктор
    public MyArray(int[] values)
    {
        Values = values;
        Prod = new Production { Id = 1, Name = "Org1" };
        Dev = new Developer { Id = 101, Name = "Alice", Department = "IT" };
    }

    // Индексатор
    public int this[int index]
    {
        get => Values[index];
        set => Values[index] = value;
    }

    // Перегруженные операции
    public static MyArray operator -(MyArray arr, int scalar) =>
        new MyArray(arr.Values.Select(v => v - scalar).ToArray());

    public static bool operator >(MyArray arr, int element) =>
        arr.Values.Contains(element);

    public static bool operator <(MyArray arr, int element) =>
        arr.Values.Contains(element);

    public static bool operator !=(MyArray a, MyArray b) =>
        !a.Values.SequenceEqual(b.Values);

    public static bool operator ==(MyArray a, MyArray b) =>
        a.Values.SequenceEqual(b.Values);

    public static MyArray operator +(MyArray a, MyArray b) =>
        new MyArray(a.Values.Concat(b.Values).ToArray());

    // Переопределение Equals/GetHashCode для компилятора
    public override bool Equals(object obj) => base.Equals(obj);
    public override int GetHashCode() => base.GetHashCode();
}

// Статический класс для работы с массивом
public static class StatisticOperation
{
    public static int Sum(MyArray arr) => arr.Values.Sum();
    public static int MaxMinusMin(MyArray arr) => arr.Values.Max() - arr.Values.Min();
    public static int Count(MyArray arr) => arr.Values.Length;
}

// Методы расширения
public static class Extensions
{
    public static string RemoveVowels(this string str)
    {
        return new string(str.Where(c => !"aeiouAEIOU".Contains(c)).ToArray());
    }

    public static MyArray RemoveFirstFive(this MyArray arr)
    {
        if (arr.Values.Length <= 5) return new MyArray(new int[0]);
        return new MyArray(arr.Values.Skip(5).ToArray());
    }
}

// Тестирование
class Program
{
    static void Main()
    {
        var arr1 = new MyArray(new int[] { 1, 2, 3, 4, 5, 6 });
        var arr2 = new MyArray(new int[] { 4, 5, 6 });

        // Проверка перегрузки
        var arrMinus = arr1 - 2;
        Console.WriteLine(string.Join(", ", arrMinus.Values));

        Console.WriteLine(arr1 > 3); // true
        Console.WriteLine(arr1 != arr2); // true
        var arrUnion = arr1 + arr2;
        Console.WriteLine(string.Join(", ", arrUnion.Values));

        // Методы статического класса
        Console.WriteLine(StatisticOperation.Sum(arr1));
        Console.WriteLine(StatisticOperation.MaxMinusMin(arr1));
        Console.WriteLine(StatisticOperation.Count(arr1));

        // Методы расширения
        string text = "Hello World";
        Console.WriteLine(text.RemoveVowels());
        var arrRemoved = arr1.RemoveFirstFive();
        Console.WriteLine(string.Join(", ", arrRemoved.Values));
    }
}﻿
