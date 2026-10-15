using System;
using System.Collections.Generic;
using System.Linq;

public class IntSet
{
    private List<int> _items = new List<int>();

    public void Add(int value)
    {
        if (!_items.Contains(value))
            _items.Add(value);
    }

    public static IntSet operator +(IntSet a, IntSet b)
    {
        IntSet result = new IntSet();
        foreach (int item in a._items) result.Add(item);
        foreach (int item in b._items) result.Add(item);
        return result;
    }

    public static IntSet operator *(IntSet a, IntSet b)
    {
        IntSet result = new IntSet();
        foreach (int item in a._items)
        {
            if (b._items.Contains(item))
                result.Add(item);
        }
        return result;
    }

    public static IntSet operator -(IntSet a, IntSet b)
    {
        IntSet result = new IntSet();
        foreach (int item in a._items)
        {
            if (!b._items.Contains(item))
                result.Add(item);
        }
        return result;
    }

    public static bool operator >(IntSet a, IntSet b)
    {
        foreach (int item in b._items)
        {
            if (!a._items.Contains(item))
                return false;
        }
        return a._items.Count > b._items.Count;
    }

    public static bool operator <(IntSet a, IntSet b)
    {
        foreach (int item in a._items)
        {
            if (!b._items.Contains(item))
                return false;
        }
        return a._items.Count < b._items.Count;
    }

    public override string ToString()
    {
        return "{" + string.Join(", ", _items) + "}";
    }
}

public static class ListExtensions
{
    public static void Shuffle(this List<int> list)
    {
        Random rng = new Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            int value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public static List<int> GetUnique(this List<int> list)
    {
        List<int> result = new List<int>();
        foreach (int item in list)
        {
            if (!result.Contains(item))
                result.Add(item);
        }
        return result;
    }

    public static double Average(this List<int> list)
    {
        if (list.Count == 0) return 0;
        double sum = 0;
        foreach (int item in list) sum += item;
        return sum / list.Count;
    }
}

class Program
{
    static void Main()
    {
        IntSet setA = new IntSet();
        setA.Add(1); setA.Add(2); setA.Add(3); setA.Add(4);

        IntSet setB = new IntSet();
        setB.Add(3); setB.Add(4); setB.Add(5); setB.Add(6);

        IntSet setC = new IntSet();
        setC.Add(1); setC.Add(2);

        Console.WriteLine($"Set A: {setA}");
        Console.WriteLine($"Set B: {setB}");
        Console.WriteLine($"Set C: {setC}");
        Console.WriteLine();

        Console.WriteLine($"Union (A + B): {setA + setB}");
        Console.WriteLine($"Intersection (A * B): {setA * setB}");
        Console.WriteLine($"Difference (A - B): {setA - setB}");
        Console.WriteLine();

        Console.WriteLine($"Is C a subset of A (C < A)? {setC < setA}");
        Console.WriteLine($"Is A a superset of C (A > C)? {setA > setC}");
        Console.WriteLine($"Is A a subset of B (A < B)? {setA < setB}");
        Console.WriteLine();

        List<int> numbers = new List<int> { 1, 2, 2, 3, 4, 4, 5 };
        Console.WriteLine($"Original list: {string.Join(", ", numbers)}");
        
        Console.WriteLine($"Average: {numbers.Average()}");
        
        List<int> unique = numbers.GetUnique();
        Console.WriteLine($"Unique items: {string.Join(", ", unique)}");
        
        unique.Shuffle();
        Console.WriteLine($"Shuffled unique items: {string.Join(", ", unique)}");



        Console.WriteLine("\n" + new string('=', 50));
        Console.WriteLine("=== DOP 1 ===");
        SmartArray sArr = new SmartArray(5);
        sArr[1] = 10; sArr[2] = 20; sArr[3] = 30; sArr[4] = 40; sArr[5] = 50;
        sArr.Print();
        (sArr + 5).Print();
        sArr.Shuffle();
        sArr.Print();

        Console.WriteLine("\n" + new string('=', 50));
        Console.WriteLine("=== DOP 2 ===");
        CyclicArray cArr = new CyclicArray(new int[] { 1, 2, 3, 4, 5 });
        cArr.Print();
        cArr++;
        cArr.Print();
        cArr--;
        cArr.Print();
        Console.WriteLine($"Index -1: {cArr[-1]}");
        Console.WriteLine($"Index 7: {cArr[7]}");

        Console.WriteLine("\n" + new string('=', 50));
        Console.WriteLine("=== DOP 3 ===");
        AlternatingArray alt1 = new AlternatingArray(new int[] { 1, 2, 3 });
        AlternatingArray alt2 = new AlternatingArray(new int[] { 4, 5, 6, 7, 8 });
        (alt1 + alt2).Print();
        (alt1 * 3).Print();

    }
}


public class SmartArray
{
    private int[] _array;

    public SmartArray(int size)
    {
        _array = new int[size];
    }

    public int this[int index]
    {
        get { return _array[index - 1]; }
        set { _array[index - 1] = value; }
    }

    public int Length => _array.Length;

    public static SmartArray operator +(SmartArray arr, int value)
    {
        SmartArray result = new SmartArray(arr.Length);
        for (int i = 1; i <= arr.Length; i++)
        {
            result[i] = arr[i] + value;
        }
        return result;
    }

    public static SmartArray operator -(SmartArray arr, int value)
    {
        SmartArray result = new SmartArray(arr.Length);
        for (int i = 1; i <= arr.Length; i++)
        {
            result[i] = arr[i] - value;
        }
        return result;
    }

    public void Print()
    {
        Console.WriteLine("[" + string.Join(", ", _array) + "]");
    }
}

public static class SmartArrayExtensions
{
    private static readonly Random _rng = new Random();

    public static void Shuffle(this SmartArray smartArr)
    {
        int n = smartArr.Length;
        while (n > 1)
        {
            n--;
            int k = _rng.Next(n + 1);
            int temp = smartArr[k + 1];
            smartArr[k + 1] = smartArr[n + 1];
            smartArr[n + 1] = temp;
        }
    }
}


public class CyclicArray
{
    private int[] _array;

    public CyclicArray(int[] initialArray)
    {
        _array = (int[])initialArray.Clone();
    }

    public int this[int index]
    {
        get
        {
            int validIndex = index % _array.Length;
            if (validIndex < 0) validIndex += _array.Length;
            return _array[validIndex];
        }
        set
        {
            int validIndex = index % _array.Length;
            if (validIndex < 0) validIndex += _array.Length;
            _array[validIndex] = value;
        }
    }

    public int Length => _array.Length;

    public static CyclicArray operator ++(CyclicArray arr)
    {
        int[] next = new int[arr.Length];
        next[0] = arr._array[arr.Length - 1];
        Array.Copy(arr._array, 0, next, 1, arr.Length - 1);
        arr._array = next;
        return arr;
    }

    public static CyclicArray operator --(CyclicArray arr)
    {
        int[] next = new int[arr.Length];
        next[arr.Length - 1] = arr._array[0];
        Array.Copy(arr._array, 1, next, 0, arr.Length - 1);
        arr._array = next;
        return arr;
    }

    public void Print()
    {
        Console.WriteLine("[" + string.Join(", ", _array) + "]");
    }
}


public class AlternatingArray
{
    private int[] _array;

    public AlternatingArray(int[] initialArray)
    {
        _array = (int[])initialArray.Clone();
    }

    public int Length => _array.Length;

    public static AlternatingArray operator +(AlternatingArray a, AlternatingArray b)
    {
        int totalLength = a.Length + b.Length;
        int[] result = new int[totalLength];
        int i = 0, j = 0, k = 0;

        while (i < a.Length && j < b.Length)
        {
            result[k++] = a._array[i++];
            result[k++] = b._array[j++];
        }

        while (i < a.Length) result[k++] = a._array[i++];
        while (j < b.Length) result[k++] = b._array[j++];

        return new AlternatingArray(result);
    }

    public static AlternatingArray operator *(AlternatingArray a, int times)
    {
        if (times <= 0) return new AlternatingArray(new int[0]);

        int[] result = new int[a.Length * times];
        for (int i = 0; i < times; i++)
        {
            Array.Copy(a._array, 0, result, i * a.Length, a.Length);
        }
        return new AlternatingArray(result);
    }

    public void Print()
    {
        Console.WriteLine("[" + string.Join(", ", _array) + "]");
    }
}