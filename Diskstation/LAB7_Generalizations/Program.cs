// ==================== ЛАБОРАТОРНАЯ 3 (ПРЕДЫДУЩАЯ) ====================
using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

// Основной класс: одномерный массив
public class MyArray
{
    public int[] Values { get; private set; }

    // Вложенный класс Production
    public class Production
    {
        public int Id { get; set; }
        public string? Name { get; set; } // Добавлено nullable
    }
    public Production Prod { get; set; }

    // Вложенный класс Developer
    public class Developer
    {
        public int Id { get; set; }
        public string? Name { get; set; } // Добавлено nullable
        public string? Department { get; set; } // Добавлено nullable
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
    public override bool Equals(object? obj) => base.Equals(obj); // Добавлено nullable
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

// ==================== ЛАБОРАТОРНАЯ 5 (НОВАЯ) ====================

// 1. Обобщенный интерфейс с операциями добавить, удалить, просмотреть
public interface ICollectionOperations<T>
{
    void Add(T item);
    bool Remove(T item);
    void Display();
    T? Find(Predicate<T> predicate); // Добавлено nullable
}

// 2. Обобщенный тип CollectionType<T> с наследованием интерфейса
// УБРАНО ОГРАНИЧЕНИЕ new() чтобы работало со string
public class CollectionType<T> : ICollectionOperations<T>
{
    private List<T> collection;

    public CollectionType()
    {
        collection = new List<T>();
    }

    public CollectionType(IEnumerable<T> initialData)
    {
        collection = new List<T>(initialData);
    }

    // Реализация интерфейса ICollectionOperations
    public void Add(T item)
    {
        try
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item), "Cannot add null element");
            
            collection.Add(item);
            Console.WriteLine($"Element added: {item}");
        }
        catch (ArgumentNullException ex)
        {
            Console.WriteLine($"Error adding element: {ex.Message}");
        }
        finally
        {
            Console.WriteLine("Add operation completed");
        }
    }

    public bool Remove(T item)
    {
        try
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item), "Cannot remove null element");
            
            bool removed = collection.Remove(item);
            Console.WriteLine(removed ? $"Element removed: {item}" : "Element not found for removal");
            return removed;
        }
        catch (ArgumentNullException ex)
        {
            Console.WriteLine($"Error removing element: {ex.Message}");
            return false;
        }
        finally
        {
            Console.WriteLine("Remove operation completed");
        }
    }

    public void Display()
    {
        try
        {
            if (collection.Count == 0)
            {
                Console.WriteLine("Collection is empty");
                return;
            }

            Console.WriteLine("Collection elements:");
            foreach (var item in collection)
            {
                Console.WriteLine($"  {item}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error displaying collection: {ex.Message}");
        }
        finally
        {
            Console.WriteLine("Display operation completed");
        }
    }

    public T? Find(Predicate<T> predicate) // Добавлено nullable
    {
        try
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate), "Predicate cannot be null");
            
            var result = collection.Find(predicate);
            Console.WriteLine(result != null ? $"Found element: {result}" : "Element not found");
            return result;
        }
        catch (ArgumentNullException ex)
        {
            Console.WriteLine($"Error finding element: {ex.Message}");
            return default(T); // Возвращает default для типа T (null для ссылочных типов)
        }
        finally
        {
            Console.WriteLine("Find operation completed");
        }
    }

    // Дополнительные полезные методы
    public int Count => collection.Count;
    
    public T? this[int index] // Добавлено nullable
    {
        get
        {
            try
            {
                return collection[index];
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine($"Index {index} is out of range");
                return default(T); // Возвращает default для типа T
            }
        }
        set
        {
            try
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));
                    
                collection[index] = value;
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine($"Index {index} is out of range");
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine($"Error setting value: {ex.Message}");
            }
        }
    }

    // 5. Методы сохранения и чтения объектов в JSON файл
    public void SaveToFile(string filePath)
    {
        try
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };
            
            string json = JsonSerializer.Serialize(collection, options);
            File.WriteAllText(filePath, json);
            Console.WriteLine($"Collection saved to {filePath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving to file: {ex.Message}");
        }
        finally
        {
            Console.WriteLine("Save operation completed");
        }
    }

    public void LoadFromFile(string filePath)
    {
        try
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"File {filePath} not found");

            string json = File.ReadAllText(filePath);
            var loadedCollection = JsonSerializer.Deserialize<List<T>>(json);
            
            if (loadedCollection != null)
            {
                collection = loadedCollection;
                Console.WriteLine($"Collection loaded from {filePath}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading from file: {ex.Message}");
        }
        finally
        {
            Console.WriteLine("Load operation completed");
        }
    }
}

// 4. Пользовательский класс для параметра обобщения (из лабы 4)
public class Student
{
    public string? Name { get; set; } // Добавлено nullable
    public int Age { get; set; }
    public double Grade { get; set; }

    public Student() { } // Конструктор по умолчанию

    public Student(string name, int age, double grade)
    {
        Name = name;
        Age = age;
        Grade = grade;
    }

    public override string ToString()
    {
        return $"Student: {Name}, Age: {Age}, Grade: {Grade}";
    }
}

// ==================== ТЕСТИРОВАНИЕ ВСЕГО ВМЕСТЕ ====================
class Program
{
    static void Main()
    {
        Console.WriteLine("=== ТЕСТИРОВАНИЕ ЛАБОРАТОРНОЙ 3 ===");
        
        var arr1 = new MyArray(new int[] { 1, 2, 3, 4, 5, 6 });
        var arr2 = new MyArray(new int[] { 4, 5, 6 });

        // Проверка перегрузки
        var arrMinus = arr1 - 2;
        Console.WriteLine("arr1 - 2: " + string.Join(", ", arrMinus.Values));

        Console.WriteLine("arr1 > 3: " + (arr1 > 3)); // true
        Console.WriteLine("arr1 != arr2: " + (arr1 != arr2)); // true
        var arrUnion = arr1 + arr2;
        Console.WriteLine("arr1 + arr2: " + string.Join(", ", arrUnion.Values));

        // Методы статического класса
        Console.WriteLine("Sum: " + StatisticOperation.Sum(arr1));
        Console.WriteLine("MaxMinusMin: " + StatisticOperation.MaxMinusMin(arr1));
        Console.WriteLine("Count: " + StatisticOperation.Count(arr1));

        // Старые методы расширения
        string text = "Hello World";
        Console.WriteLine("Remove vowels: " + text.RemoveVowels());
        var arrRemoved = arr1.RemoveFirstFive();
        Console.WriteLine("Remove first five: " + string.Join(", ", arrRemoved.Values));

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

        Console.WriteLine("\n\n=== ТЕСТИРОВАНИЕ ЛАБОРАТОРНОЙ 5 ===");
        
        // 3. Проверка использования для стандартных типов данных
        Console.WriteLine("\n--- Integer Collection ---");
        var intCollection = new CollectionType<int>();
        intCollection.Add(10);
        intCollection.Add(20);
        intCollection.Add(30);
        intCollection.Display();
        intCollection.Find(x => x > 15);
        intCollection.Remove(20);
        intCollection.Display();

        Console.WriteLine("\n--- String Collection ---");
        var stringCollection = new CollectionType<string>();
        stringCollection.Add("Hello");
        stringCollection.Add("World");
        stringCollection.Add("C#");
        stringCollection.Display();
        stringCollection.Find(s => s != null && s.StartsWith("W"));
        stringCollection.Remove("Hello");
        stringCollection.Display();

        Console.WriteLine("\n--- Double Collection ---");
        var doubleCollection = new CollectionType<double>();
        doubleCollection.Add(1.5);
        doubleCollection.Add(2.7);
        doubleCollection.Add(3.14);
        doubleCollection.Display();
        doubleCollection.Find(d => d < 2.0);

        // 4. Проверка с пользовательским классом
        Console.WriteLine("\n--- Student Collection ---");
        var studentCollection = new CollectionType<Student>();
        
        studentCollection.Add(new Student("Alice", 20, 4.5));
        studentCollection.Add(new Student("Bob", 22, 3.8));
        studentCollection.Add(new Student("Charlie", 21, 4.2));
        
        studentCollection.Display();
        
        // Поиск по предикату
        studentCollection.Find(s => s != null && s.Grade > 4.0);
        
        // 5. Сохранение в файл и загрузка
        studentCollection.SaveToFile("students.json");
        
        // Создание новой коллекции и загрузка из файла
        var loadedStudentCollection = new CollectionType<Student>();
        loadedStudentCollection.LoadFromFile("students.json");
        loadedStudentCollection.Display();

        // Тестирование обработки исключений
        Console.WriteLine("\n--- Exception Handling Test ---");
        var testCollection = new CollectionType<string>();
        
        // Попытка добавить null (будет исключение)
        testCollection.Add(null!); // ! для подавления предупреждения
        
        // Попытка удалить несуществующий элемент
        testCollection.Remove("Nonexistent");
        
        // Попытка загрузить несуществующий файл
        testCollection.LoadFromFile("nonexistent.json");
        
        Console.WriteLine("\nПрограмма завершена. Нажмите любую клавишу...");
        Console.ReadKey();
    }
}