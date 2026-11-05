using System;
using System.Reflection;

public static class Dop2
{
    [AttributeUsage(AttributeTargets.Property)]
    public class MaskAttribute : Attribute
    {
        public string Mask { get; }
        public MaskAttribute(string mask) => Mask = mask;
    }

    public class User
    {
        [Mask("**** *****")]
        public string Name { get; set; } = "Иван Петров";

        [Mask("****@****.***")]
        public string Email { get; set; } = "ivan.petrov@mail.ru";

        [Mask("+#-###-###-##-##")]
        public string Phone { get; set; } = "89991234567";

        public string City { get; set; } = "Москва";
    }

    public class DataMasker<T> where T : class, new()
    {
        private static string ApplyMask(string value, string mask)
        {
            int length = Math.Max(value.Length, mask.Length);
            char[] result = new char[length];
            for (int i = 0; i < length; i++)
            {
                char maskCharacter = i < mask.Length ? mask[i] : '*';
                if (i < value.Length)
                {
                    result[i] = maskCharacter switch
                    {
                        '*' => '*',
                        '#' => value[i],
                        _ => maskCharacter
                    };
                }
                else
                {
                    result[i] = maskCharacter == '#' ? '*' : maskCharacter;
                }
            }
            return new string(result);
        }

        public T Mask(T source)
        {
            T copy = new T();
            foreach (PropertyInfo property in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                object? value = property.GetValue(source);
                if (property.CanWrite)
                {
                    MaskAttribute? mask = property.GetCustomAttribute<MaskAttribute>();
                    if (mask != null && value is string text)
                    {
                        property.SetValue(copy, ApplyMask(text, mask.Mask));
                    }
                    else
                    {
                        property.SetValue(copy, value);
                    }
                }
            }
            return copy;
        }
    }

    public static void Run()
    {
        User user = new User();
        Console.WriteLine("Исходный объект:");
        Console.WriteLine($"  Name: {user.Name}");
        Console.WriteLine($"  Email: {user.Email}");
        Console.WriteLine($"  Phone: {user.Phone}");
        Console.WriteLine($"  City: {user.City}");

        User masked = new DataMasker<User>().Mask(user);
        Console.WriteLine("\nЗамаскированный объект:");
        Console.WriteLine($"  Name: {masked.Name}");
        Console.WriteLine($"  Email: {masked.Email}");
        Console.WriteLine($"  Phone: {masked.Phone}");
        Console.WriteLine($"  City: {masked.City}");
    }
}
