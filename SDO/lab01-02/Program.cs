using System;

class Program
{
    static void Main()
    {
        task1();
        task2();

        Dop1();
        Dop2();
        Dop3();
    }


    static void task1()
    {
        Console.WriteLine("=== TASK1 ===");

        Random rnd = new Random();
        int[] arr = new int[12];

        // Заполнение массива случайными числами от -20 до 30
        for (int i = 0; i < arr.Length; i++)
        {
            arr[i] = rnd.Next(-20, 31);
        }

        // Вывод исходного массива
        Console.Write("Исходный массив: ");
        foreach (int x in arr)
            Console.Write(x + " ");
        Console.WriteLine();

        // Поиск первого и последнего отрицательных элементов
        int firstNeg = -1, lastNeg = -1;
        for (int i = 0; i < arr.Length; i++)
        {
            if (arr[i] < 0)
            {
                if (firstNeg == -1) firstNeg = i;
                lastNeg = i;
            }
        }

        // Проверка количества отрицательных элементов
        if (firstNeg == -1 || firstNeg == lastNeg)
        {
            Console.WriteLine("В массиве меньше двух отрицательных чисел.");
        }
        else
        {
            long product = 1;
            for (int i = firstNeg + 1; i < lastNeg; i++)
            {
                product *= arr[i];
            }
            Console.WriteLine($"Произведение элементов между первым и последним отрицательными: {product}");
        }
    }


    // Метод проверки числа на простоту
    static bool IsPrime(int n)
    {
        if (n < 2) return false;
        for (int i = 2; i * i <= n; i++)
        {
            if (n % i == 0) return false;
        }
        return true;
    }

    // Метод анализа массива
    static (int count, int sum, int? max) AnalyzePrimes(int[] array)
    {
        int count = 0;
        int sum = 0;
        int? max = null;

        foreach (int num in array)
        {
            if (IsPrime(num))
            {
                count++;
                sum += num;
                if (max == null || num > max)
                    max = num;
            }
        }

        return (count, sum, max);
    }

    static void task2()
    {
        Console.WriteLine("=== TASK2 ===");

        Random rnd = new Random();
        int[] numbers = new int[15];

        // Заполнение массива случайными числами от 1 до 50
        for (int i = 0; i < numbers.Length; i++)
        {
            numbers[i] = rnd.Next(1, 51);
        }

        // Вывод исходного массива
        Console.Write("Массив: ");
        foreach (int n in numbers)
            Console.Write(n + " ");
        Console.WriteLine();

        // Вызов метода и получение кортежа
        var result = AnalyzePrimes(numbers);

        // Вывод результатов
        Console.WriteLine($"Количество простых чисел: {result.count}");
        Console.WriteLine($"Сумма простых чисел: {result.sum}");
        Console.WriteLine($"Наибольшее простое число: {(result.max.HasValue ? result.max.ToString() : "null")}");

        // Деконструкция кортежа
        (int c, int s, int? m) = AnalyzePrimes(numbers);
        Console.WriteLine($"\nДеконструкция: count={c}, sum={s}, max={(m.HasValue ? m.ToString() : "null")}");
    }



    // ================= ЗАДАЧА 1: Матрицы =================
    static void Dop1()
    {
        Console.WriteLine("=== ЗАДАЧА 1 ===\n");

        int[,] a = { { 1, 2 }, { 3, 4 } };
        int[,] b = { { 5, 6 }, { 7, 8 } };

        Console.WriteLine("Матрица A:");
        Print(a);
        Console.WriteLine("Матрица B:");
        Print(b);

        Console.WriteLine("A + B:");
        Print(AddMatrices(a, b));

        Console.WriteLine("A * 3:");
        Print(MultiplyByScalar(a, 3));

        Console.WriteLine("A * B:");
        Print(MultiplyMatrices(a, b));

        var (sum, prod) = SumAndProduct(a);
        Console.WriteLine($"A: сумма={sum}, произведение={prod}\n");
    }

    static int[,] AddMatrices(int[,] a, int[,] b)
    {
        if (a.GetLength(0) != b.GetLength(0) || a.GetLength(1) != b.GetLength(1))
            throw new Exception("Размерности не совпадают");

        int[,] res = new int[a.GetLength(0), a.GetLength(1)];
        for (int i = 0; i < a.GetLength(0); i++)
            for (int j = 0; j < a.GetLength(1); j++)
                res[i, j] = a[i, j] + b[i, j];
        return res;
    }

    static int[,] MultiplyByScalar(int[,] m, int k)
    {
        int[,] res = new int[m.GetLength(0), m.GetLength(1)];
        for (int i = 0; i < m.GetLength(0); i++)
            for (int j = 0; j < m.GetLength(1); j++)
                res[i, j] = m[i, j] * k;
        return res;
    }

    static int[,] MultiplyMatrices(int[,] a, int[,] b)
    {
        if (a.GetLength(1) != b.GetLength(0))
            throw new Exception("Умножение невозможно");

        int[,] res = new int[a.GetLength(0), b.GetLength(1)];
        for (int i = 0; i < a.GetLength(0); i++)
            for (int j = 0; j < b.GetLength(1); j++)
                for (int t = 0; t < a.GetLength(1); t++)
                    res[i, j] += a[i, t] * b[t, j];
        return res;
    }

    static (long sum, long prod) SumAndProduct(int[,] m)
    {
        long s = 0, p = 1;
        foreach (int x in m) { s += x; p *= x; }
        return (s, p);
    }

    // ================= ЗАДАЧА 2: Анализ текста =================
    static void Dop2()
    {
        Console.WriteLine("=== ЗАДАЧА 2 ===\n");

        string text = "Привет, мир! Это тестовый Текст для анализа. Он содержит Разные слова.";
        var (count, longest, caps, stats) = AnalyzeText(text);

        Console.WriteLine($"Текст: {text}\n");
        Console.WriteLine($"Слов: {count}");
        Console.WriteLine($"Самое длинное: {longest}");
        Console.WriteLine($"С заглавной: [{string.Join(", ", caps)}]");
        Console.WriteLine("Статистика букв:");
        PrintDict(stats);
        Console.WriteLine();
    }

    static (int count, string longest, string[] caps, Dictionary<char, int> stats) AnalyzeText(string text)
    {
        char[] sep = { ' ', '.', ',', '!', '?', ';', ':', '-', '\n', '\r', '\t' };
        string[] words = text.Split(sep, StringSplitOptions.RemoveEmptyEntries);

        string longest = words.OrderByDescending(w => w.Length).First();
        string[] caps = words.Where(w => char.IsUpper(w[0])).ToArray();

        var stats = new Dictionary<char, int>();
        foreach (char c in text.ToLower())
            if (char.IsLetter(c))
                stats[c] = stats.ContainsKey(c) ? stats[c] + 1 : 1;

        return (words.Length, longest, caps, stats);
    }

    // ================= ЗАДАЧА 3: Магический квадрат =================
    static void Dop3()
    {
        Console.WriteLine("=== ЗАДАЧА 3 ===\n");

        int[,] m = { { 2, 7, 6 }, { 9, 5, 1 }, { 4, 3, 8 } };
        Print(m);

        var (isMagic, target, badRows, badCols) = CheckMagicSquare(m);

        if (isMagic)
            Console.WriteLine($"Магический квадрат! Сумма = {target}");
        else
            Console.WriteLine($"Не магический. Плохие строки: [{string.Join(",", badRows)}], столбцы: [{string.Join(",", badCols)}]");

        Console.WriteLine();
    }

    static (bool isMagic, long target, List<int> badRows, List<int> badCols) CheckMagicSquare(int[,] m)
    {
        int n = m.GetLength(0);
        if (n != m.GetLength(1))
            throw new Exception("Матрица должна быть квадратной");

        long target = 0;
        for (int j = 0; j < n; j++) target += m[0, j];

        var badRows = new List<int>();
        var badCols = new List<int>();
        bool ok = true;

        for (int i = 0; i < n; i++)
        {
            long rs = 0;
            for (int j = 0; j < n; j++) rs += m[i, j];
            if (rs != target) { badRows.Add(i); ok = false; }
        }

        for (int j = 0; j < n; j++)
        {
            long cs = 0;
            for (int i = 0; i < n; i++) cs += m[i, j];
            if (cs != target) { badCols.Add(j); ok = false; }
        }

        long d1 = 0, d2 = 0;
        for (int i = 0; i < n; i++) { d1 += m[i, i]; d2 += m[i, n - 1 - i]; }
        if (d1 != target || d2 != target) ok = false;

        return (ok, target, badRows, badCols);
    }

    // ================= Вспомогательные =================
    static void Print(int[,] m)
    {
        for (int i = 0; i < m.GetLength(0); i++)
        {
            for (int j = 0; j < m.GetLength(1); j++)
                Console.Write($"{m[i, j],5}");
            Console.WriteLine();
        }
        Console.WriteLine();
    }

    static void PrintDict(Dictionary<char, int> d)
    {
        foreach (var kv in d.OrderBy(x => x.Key))
            Console.WriteLine($"  '{kv.Key}': {kv.Value}");
    }
}
