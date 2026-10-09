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

        Task1();
        Task5();
        Task3();
    }

    static void AddExclamationMark(ref string s)
    {
        s = s + "!";
        Console.WriteLine($"Внутри метода: s = \"{s}\"");
    }

    // ================= ЗАДАЧА 1: Парадокс ссылочных типов =================
    static void Task1()
    {
        Console.WriteLine("=== ЗАДАЧА 1: Парадокс ссылочных типов ===\n");

        // --- string ---
        string original = "text";
        string copy = original;

        Console.WriteLine($"До изменения: original = \"{original}\", copy = \"{copy}\"");

        copy = copy + "!"; // создаётся НОВАЯ строка, copy теперь ссылается на неё

        Console.WriteLine($"После изменения copy: original = \"{original}\", copy = \"{copy}\"");
        Console.WriteLine("Вывод: original не изменилась, т.к. string неизменяема (immutable).");
        Console.WriteLine("Операция '+' создала новый объект в куче, а не изменила старый.\n");

        // --- object с числом ---
        object objOriginal = 42;
        object objCopy = objOriginal;

        Console.WriteLine($"До изменения: objOriginal = {objOriginal}, objCopy = {objCopy}");

        objCopy = 100; // objCopy теперь ссылается на НОВЫЙ упакованный объект

        Console.WriteLine($"После изменения objCopy: objOriginal = {objOriginal}, objCopy = {objCopy}");
        Console.WriteLine("Вывод: objOriginal не изменился, т.к. присваивание новой ссылки");
        Console.WriteLine("не меняет старый объект. Это та же логика, что и со string.\n");
    }

    // ================= ЗАДАЧА 5: По значению и по ссылке =================
    // Класс — ссылочный тип, внутри хранит значимый тип
    class Box
    {
        public int Value;
    }

    static void Task5()
    {
        Console.WriteLine("=== ЗАДАЧА 5 ===\n");


        // --- 1. Просто значимый тип по значению ---
        int number = 10;
        Console.WriteLine($"До вызова: number = {number}");
        ModifyInt(number);
        Console.WriteLine($"После вызова: number = {number}");
        Console.WriteLine("Вывод: int передан по значению — изменение копии не влияет на оригинал.\n");

        // --- 2. Объект, содержащий значимый тип ---
        Box box = new Box();
        box.Value = 10;
        Console.WriteLine($"До вызова: box.Value = {box.Value}");
        ModifyBox(box);
        Console.WriteLine($"После вызова: box.Value = {box.Value}");
        Console.WriteLine("Вывод: ссылка на объект скопирована, но оба указывают на один объект в куче.");
        Console.WriteLine("Изменение поля Value внутри метода видно снаружи.\n");

        // --- 3. Дополнительно: переприсваивание ссылки внутри метода ---
        Box box2 = new Box();
        box2.Value = 10;
        Console.WriteLine($"До вызова: box2.Value = {box2.Value}");
        ReassignBox(box2);
        Console.WriteLine($"После вызова: box2.Value = {box2.Value}");
        Console.WriteLine("Вывод: если внутри метода переприсвоить ссылку (box = new Box()),");
        Console.WriteLine("это не повлияет на оригинал, т.к. копия ссылки начала указывать на другой объект.\n");
    }

    // Принимает int по значению
    static void ModifyInt(int x)
    {
        x = 999;
    }

    // Принимает объект по значению (копируется ссылка)
    static void ModifyBox(Box box)
    {
        box.Value = 999; // меняем поле объекта — видно снаружи
    }

    // Переприсваивает ссылку внутри метода
    static void ReassignBox(Box box)
    {
        box = new Box(); // локальная копия ссылки теперь указывает на новый объект
        box.Value = 999; // это изменение не видно снаружи
    }

    // ================= ЗАДАЧА 3: Упаковка и копирование ссылки =================
    static void Task3()
    {
        Console.WriteLine("=== ЗАДАЧА 3: Упаковка и копирование ссылки ===\n");

        int x = 5;
        object obj1 = x;   // упаковка: значение 5 скопировано в кучу
        object obj2 = obj1; // копируется ссылка на тот же объект в куче

        Console.WriteLine($"До изменения x: x = {x}, obj1 = {obj1}, obj2 = {obj2}");

        x = 10; // меняем исходную переменную

        Console.WriteLine($"После изменения x: x = {x}, obj1 = {obj1}, obj2 = {obj2}");
        Console.WriteLine("Вывод: obj1 и obj2 остались равны 5, потому что упаковка");
        Console.WriteLine("создала отдельную копию значения в куче, независимую от x.\n");

        // Распаковка
        int y = (int)obj1; // распаковка: значение 5 скопировано из кучи в стек
        Console.WriteLine($"После распаковки: y = {y}, obj1 = {obj1}");

        y = 777; // меняем y

        Console.WriteLine($"После изменения y: y = {y}, obj1 = {obj1}");
        Console.WriteLine("Вывод: изменение y не влияет на obj1, т.к. распаковка");
        Console.WriteLine("создала независимую копию значения в стеке.\n");
    }
}