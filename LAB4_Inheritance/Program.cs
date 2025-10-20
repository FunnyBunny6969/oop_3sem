using System;
using System.Linq;
using System.Text;

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

    // Методы расширения для string
    public static int WordCount(this string str)
    {
        if (string.IsNullOrWhiteSpace(str))
            return 0;
        
        return str.Split(new char[] { ' ', '.', ',', '!', '?' }, 
                        StringSplitOptions.RemoveEmptyEntries).Length;
    }

    public static bool IsPalindrome(this string str)
    {
        if (string.IsNullOrEmpty(str))
            return false;
            
        string cleanStr = new string(str.Where(char.IsLetterOrDigit).ToArray()).ToLower();
        return cleanStr.SequenceEqual(cleanStr.Reverse());
    }

    public static string RemovePunctuation(this string str)
    {
        if (string.IsNullOrEmpty(str))
            return str;
            
        return new string(str.Where(c => !char.IsPunctuation(c)).ToArray());
    }

    // Методы расширения для MyArray
    public static int CountEvenNumbers(this MyArray arr)
    {
        return arr.Values.Count(x => x % 2 == 0);
    }

    public static double AverageValue(this MyArray arr)
    {
        if (arr.Values.Length == 0)
            return 0;
        return arr.Values.Average();
    }

    public static bool ContainsDuplicates(this MyArray arr)
    {
        return arr.Values.Length != arr.Values.Distinct().Count();
    }

    public static MyArray ReverseArray(this MyArray arr)
    {
        return new MyArray(arr.Values.Reverse().ToArray());
    }
}

// Старый класс методов расширения (оставляем для обратной совместимости)
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

        // Старые методы расширения
        string text = "Hello World";
        Console.WriteLine(text.RemoveVowels());
        var arrRemoved = arr1.RemoveFirstFive();
        Console.WriteLine(string.Join(", ", arrRemoved.Values));

        // Новые методы расширения для string
        Console.WriteLine("\n--- Методы расширения для string ---");
        string testString = "Hello, world! This is a test.";
        Console.WriteLine($"Количество слов: {testString.WordCount()}");
        Console.WriteLine($"Является палиндромом 'radar': {"radar".IsPalindrome()}");
        Console.WriteLine($"Без пунктуации: {testString.RemovePunctuation()}");

        // Новые методы расширения для MyArray
        Console.WriteLine("\n--- Методы расширения для MyArray ---");
        Console.WriteLine($"Четные числа: {arr1.CountEvenNumbers()}");
        Console.WriteLine($"Среднее значение: {arr1.AverageValue()}");
        Console.WriteLine($"Есть дубликаты: {arr1.ContainsDuplicates()}");
        
        var reversedArr = arr1.ReverseArray();
        Console.WriteLine($"Перевернутый массив: {string.Join(", ", reversedArr.Values)}");
    }
}