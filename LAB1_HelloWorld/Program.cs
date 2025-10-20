using System;
using System.Text;

namespace BasicsCLR
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== 1. ТИПЫ ===");
            TypesDemo();
            
            Console.WriteLine("\n=== 2. СТРОКИ ===");
            StringsDemo();
            
            Console.WriteLine("\n=== 3. МАССИВЫ ===");
            ArraysDemo();
            
            Console.WriteLine("\n=== 4. КОРТЕЖИ ===");
            TuplesDemo();
            
            Console.WriteLine("\n=== 5. ЛОКАЛЬНАЯ ФУНКЦИЯ ===");
            LocalFunctionDemo();
            
            Console.WriteLine("\n=== 6. CHECKED/UNCHECKED ===");
            CheckedUncheckedDemo();
        }

        static void TypesDemo()
        {
            // 1a. Примитивные типы
            bool boolVar = true;
            byte byteVar = 255;
            sbyte sbyteVar = -128;
            char charVar = 'A';
            decimal decimalVar = 999.99m;
            double doubleVar = 3.14159;
            float floatVar = 2.718f;
            int intVar = 2147483647;
            uint uintVar = 4294967295;
            long longVar = 9223372036854775807;
            ulong ulongVar = 18446744073709551615;
            short shortVar = -32768;
            ushort ushortVar = 65535;

            // Ввод и вывод
            Console.Write("Введите целое число: ");
            int inputInt = int.Parse(Console.ReadLine());
            Console.WriteLine($"Вы ввели: {inputInt}");

            Console.Write("Введите строку: ");
            string inputString = Console.ReadLine();
            Console.WriteLine($"Вы ввели: {inputString}");

            // 1b. Приведения типов
            // Неявные приведения
            int intFromShort = shortVar;           // short → int
            long longFromInt = intVar;             // int → long
            double doubleFromFloat = floatVar;     // float → double
            decimal decimalFromInt = intVar;       // int → decimal
            double doubleFromInt = intVar;         // int → double

            // Явные приведения
            int intFromDouble = (int)doubleVar;    // double → int
            short shortFromInt = (short)intVar;    // int → short
            float floatFromDouble = (float)doubleVar; // double → float
            char charFromInt = (char)65;           // int → char ('A')
            byte byteFromInt = (byte)intVar;       // int → byte

            // Класс Convert
            string numberString = "123";
            int convertedInt = Convert.ToInt32(numberString);
            double convertedDouble = Convert.ToDouble("45.67");
            bool convertedBool = Convert.ToBoolean("true");

            // 1c. Упаковка и распаковка
            int valueType = 42;
            object boxed = valueType;              // Упаковка
            int unboxed = (int)boxed;              // Распаковка

            // 1d. Неявно типизированная переменная
            var implicitVar = "Это строка";
            var implicitNumber = 100;
            Console.WriteLine($"Неявно типизированная: {implicitVar}, {implicitNumber}");

            // 1e. Nullable переменная
            int? nullableInt = null;
            nullableInt = 10;
            Console.WriteLine($"Nullable int: {nullableInt ?? 0}");

            double? nullableDouble = 3.14;
            Console.WriteLine($"HasValue: {nullableDouble.HasValue}, Value: {nullableDouble.Value}");

            // 1f. Ошибка с var
            var varVariable = "Изначально строка";
            // varVariable = 42; // Ошибка компиляции: нельзя изменить тип
            Console.WriteLine("Ошибка с var закомментирована");
        }

        static void StringsDemo()
        {
            // 2a. Строковые литералы
            string literal1 = "Hello";
            string literal2 = "World";
            string literal3 = "Hello";

            Console.WriteLine($"literal1 == literal2: {literal1 == literal2}");
            Console.WriteLine($"literal1 == literal3: {literal1 == literal3}");
            Console.WriteLine($"ReferenceEquals(literal1, literal3): {ReferenceEquals(literal1, literal3)}");

            // 2b. Операции со строками
            string str1 = "C# ";
            string str2 = "Programming";
            string str3 = "Language";

            // Сцепление
            string concatenated = str1 + str2 + " " + str3;
            Console.WriteLine($"Сцепление: {concatenated}");

            // Копирование
            string copied = string.Copy(str1);
            Console.WriteLine($"Копия: {copied}");

            // Выделение подстроки
            string substring = str2.Substring(0, 7); // "Program"
            Console.WriteLine($"Подстрока: {substring}");

            // Разделение на слова
            string sentence = "C# is a great programming language";
            string[] words = sentence.Split(' ');
            Console.WriteLine("Разделенные слова:");
            foreach (string word in words)
            {
                Console.WriteLine($"  {word}");
            }

            // Вставка подстроки
            string inserted = str1.Insert(2, "Sharp ");
            Console.WriteLine($"После вставки: {inserted}");

            // Удаление подстроки
            string removed = inserted.Remove(2, 6);
            Console.WriteLine($"После удаления: {removed}");

            // Интерполирование строк
            string name = "Alice";
            int age = 25;
            string interpolated = $"Имя: {name}, Возраст: {age}";
            Console.WriteLine($"Интерполированная строка: {interpolated}");

            // 2c. Пустая и null строка
            string emptyString = "";
            string nullString = null;
            string whitespaceString = "   ";

            Console.WriteLine($"string.IsNullOrEmpty(emptyString): {string.IsNullOrEmpty(emptyString)}");
            Console.WriteLine($"string.IsNullOrEmpty(nullString): {string.IsNullOrEmpty(nullString)}");
            Console.WriteLine($"string.IsNullOrWhiteSpace(whitespaceString): {string.IsNullOrWhiteSpace(whitespaceString)}");

            // Дополнительные операции
            Console.WriteLine($"emptyString.Length: {emptyString?.Length ?? -1}");
            Console.WriteLine($"nullString?.Length: {nullString?.Length ?? -1}");

            // 2d. StringBuilder
            StringBuilder sb = new StringBuilder("Hello World");
            Console.WriteLine($"Исходный StringBuilder: {sb}");

            // Удаление символов
            sb.Remove(5, 6); // Удаляем " World"
            Console.WriteLine($"После удаления: {sb}");

            // Добавление в начало и конец
            sb.Insert(0, "Start: ");
            sb.Append(" :End");
            Console.WriteLine($"После добавления: {sb}");
        }

        static void ArraysDemo()
        {
            // 3a. Двумерный массив (матрица)
            int[,] matrix = {
                {1, 2, 3},
                {4, 5, 6},
                {7, 8, 9}
            };

            Console.WriteLine("Двумерный массив:");
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    Console.Write($"{matrix[i, j]}\t");
                }
                Console.WriteLine();
            }

            // 3b. Одномерный массив строк
            string[] stringArray = {"Apple", "Banana", "Cherry", "Date"};
            Console.WriteLine($"\nМассив строк. Длина: {stringArray.Length}");
            Console.WriteLine("Содержимое:");
            foreach (string item in stringArray)
            {
                Console.WriteLine($"  {item}");
            }

            // Изменение элемента
            Console.Write("Введите позицию для изменения (0-3): ");
            int position = int.Parse(Console.ReadLine());
            Console.Write("Введите новое значение: ");
            stringArray[position] = Console.ReadLine();

            Console.WriteLine("Обновленный массив:");
            foreach (string item in stringArray)
            {
                Console.WriteLine($"  {item}");
            }

            // 3c. Ступенчатый массив
            double[][] jaggedArray = new double[3][];
            jaggedArray[0] = new double[2];
            jaggedArray[1] = new double[3];
            jaggedArray[2] = new double[4];

            Console.WriteLine("\nВведите значения для ступенчатого массива:");
            for (int i = 0; i < jaggedArray.Length; i++)
            {
                for (int j = 0; j < jaggedArray[i].Length; j++)
                {
                    Console.Write($"Элемент [{i}][{j}]: ");
                    jaggedArray[i][j] = double.Parse(Console.ReadLine());
                }
            }

            Console.WriteLine("Ступенчатый массив:");
            for (int i = 0; i < jaggedArray.Length; i++)
            {
                Console.Write($"Строка {i}: ");
                for (int j = 0; j < jaggedArray[i].Length; j++)
                {
                    Console.Write($"{jaggedArray[i][j]} ");
                }
                Console.WriteLine();
            }

            // 3d. Неявно типизированные массивы
            var implicitArray = new[] {1, 2, 3, 4, 5};
            var implicitString = "Неявная строка";

            Console.WriteLine($"\nНеявно типизированный массив: {string.Join(", ", implicitArray)}");
            Console.WriteLine($"Неявно типизированная строка: {implicitString}");
        }

        static void TuplesDemo()
        {
            // 4a. Кортеж из 5 элементов
            (int, string, char, string, ulong) tuple = (42, "Hello", 'A', "World", 123456789UL);

            // 4b. Вывод кортежа
            Console.WriteLine($"Весь кортеж: {tuple}");
            Console.WriteLine($"Элементы 1, 3, 4: {tuple.Item1}, {tuple.Item3}, {tuple.Item4}");

            // 4c. Распаковка кортежа
            // Полная распаковка
            (int num, string str1, char ch, string str2, ulong ul) = tuple;
            Console.WriteLine($"Распаковано: {num}, {str1}, {ch}, {str2}, {ul}");

            // Частичная распаковка с использованием _
            (int first, _, char third, _, ulong fifth) = tuple;
            Console.WriteLine($"Частичная распаковка: {first}, {third}, {fifth}");

            // Распаковка с объявлением переменных отдельно
            int item1; string item2; char item3; string item4; ulong item5;
            (item1, item2, item3, item4, item5) = tuple;
            Console.WriteLine($"Раздельная распаковка: {item1}, {item2}, {item3}, {item4}, {item5}");

            // 4d. Сравнение кортежей
            var tuple1 = (1, "test", 'X');
            var tuple2 = (1, "test", 'X');
            var tuple3 = (2, "different", 'Y');

            Console.WriteLine($"tuple1 == tuple2: {tuple1 == tuple2}");
            Console.WriteLine($"tuple1 == tuple3: {tuple1 == tuple3}");
        }

        static void LocalFunctionDemo()
        {
            // 5. Локальная функция
            (int max, int min, int sum, char firstLetter) ProcessArrayAndString(int[] numbers, string text)
            {
                if (numbers == null || numbers.Length == 0)
                    throw new ArgumentException("Массив не может быть пустым");
                
                if (string.IsNullOrEmpty(text))
                    throw new ArgumentException("Строка не может быть пустой");

                int max = numbers[0];
                int min = numbers[0];
                int sum = 0;

                foreach (int number in numbers)
                {
                    if (number > max) max = number;
                    if (number < min) min = number;
                    sum += number;
                }

                char firstLetter = text[0];

                return (max, min, sum, firstLetter);
            }

            // Вызов локальной функции
            int[] testArray = { 5, 2, 8, 1, 9, 3 };
            string testString = "Programming";

            var result = ProcessArrayAndString(testArray, testString);
            Console.WriteLine($"Результат локальной функции:");
            Console.WriteLine($"  Максимум: {result.max}");
            Console.WriteLine($"  Минимум: {result.min}");
            Console.WriteLine($"  Сумма: {result.sum}");
            Console.WriteLine($"  Первая буква: {result.firstLetter}");
        }

        static void CheckedUncheckedDemo()
        {
            // 6a. Локальные функции с checked/unchecked
            
            void CheckedOperation()
            {
                Console.WriteLine("Checked блок:");
                try
                {
                    checked
                    {
                        int maxValue = int.MaxValue;
                        Console.WriteLine($"int.MaxValue = {maxValue}");
                        int overflow = maxValue + 1; // Вызовет OverflowException
                        Console.WriteLine($"maxValue + 1 = {overflow}");
                    }
                }
                catch (OverflowException ex)
                {
                    Console.WriteLine($"Ошибка переполнения: {ex.Message}");
                }
            }

            void UncheckedOperation()
            {
                Console.WriteLine("Unchecked блок:");
                unchecked
                {
                    int maxValue = int.MaxValue;
                    Console.WriteLine($"int.MaxValue = {maxValue}");
                    int overflow = maxValue + 1; // Не вызовет исключение
                    Console.WriteLine($"maxValue + 1 = {overflow} (переполнение)");
                }
            }

            // 6c. Вызов функций
            CheckedOperation();
            UncheckedOperation();
        }
    }
}
