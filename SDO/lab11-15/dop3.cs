using System;
using System.Linq;
using System.Reflection;

public static class Dop3
{
    [AttributeUsage(AttributeTargets.Class)]
    public class InfoAttribute : Attribute
    {
        public string Description { get; }
        public InfoAttribute(string description) => Description = description;
    }

    [Info("Класс для демонстрации возможностей рефлексии")]
    public class Person
    {
        public string Name { get; set; } = "Иван Петров";
        public int Age { get; set; } = 30;

        public Person() { }
        public Person(string name) => Name = name;

        public string GetGreeting() => $"Привет, {Name}!";
        public void CelebrateBirthday() => Age++;
    }

    public static void Inspect(object obj)
    {
        Type type = obj.GetType();
        Console.WriteLine($"Тип: {type.Name}");
        Console.WriteLine($"Полное имя: {type.FullName}");
        Console.WriteLine($"Сборка: {type.Assembly.GetName().Name}");

        InfoAttribute? info = type.GetCustomAttribute<InfoAttribute>();
        Console.WriteLine($"Атрибут Info: {(info != null ? info.Description : "нет")}");

        Console.WriteLine("Поля:");
        foreach (FieldInfo field in type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
        {
            Console.WriteLine($"  {field.FieldType.Name} {field.Name} = {field.GetValue(obj)}");
        }

        Console.WriteLine("Свойства:");
        foreach (PropertyInfo property in type.GetProperties())
        {
            Console.WriteLine($"  {property.PropertyType.Name} {property.Name} = {property.GetValue(obj)}");
        }

        Console.WriteLine("Методы:");
        foreach (MethodInfo method in type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
        {
            Console.WriteLine($"  {method.ReturnType.Name} {method.Name}({string.Join(", ", method.GetParameters().Select(p => p.ParameterType.Name))})");
        }

        Console.WriteLine("Конструкторы:");
        foreach (ConstructorInfo constructor in type.GetConstructors())
        {
            Console.WriteLine($"  ({string.Join(", ", constructor.GetParameters().Select(p => p.ParameterType.Name))})");
        }
    }

    public static void Run()
    {
        Person person = new Person();
        Inspect(person);

        MethodInfo greet = typeof(Person).GetMethod("GetGreeting")!;
        Console.WriteLine($"\nВызов GetGreeting(): {greet.Invoke(person, null)}");

        MethodInfo birthday = typeof(Person).GetMethod("CelebrateBirthday")!;
        birthday.Invoke(person, null);
        Console.WriteLine($"Возраст после CelebrateBirthday(): {person.Age}");
    }
}
