using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Лабораторная 06: наследование и абстрактные классы ===");
        Console.WriteLine("--- Основное задание (вариант 3) ---");
        MainTask.Run();

        Section("Дополнительное задание 1: банковские счета");
        Dop1.Run();

        Section("Дополнительное задание 2: персонажи");
        Dop2.Run();

        Section("Дополнительное задание 3: продажа билетов");
        Dop3.Run();
    }

    static void Section(string title)
    {
        Console.WriteLine();
        Console.WriteLine(new string('=', 80));
        Console.WriteLine("=== " + title + " ===");
        Console.WriteLine(new string('=', 80));
    }
}

public static class MainTask
{
    public abstract class Dish
    {
        private string _name = string.Empty;
        private string _country = string.Empty;
        private int _cookingTime;

        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Название блюда не может быть пустым.");
                _name = value;
            }
        }

        public string Country
        {
            get => _country;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Страна происхождения не может быть пустой.");
                _country = value;
            }
        }

        public int CookingTime
        {
            get => _cookingTime;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Время приготовления должно быть положительным.");
                _cookingTime = value;
            }
        }

        protected Dish(string name, string country, int cookingTime)
        {
            Name = name;
            Country = country;
            CookingTime = cookingTime;
        }

        public abstract void Cook();

        public virtual string GetInfo() =>
            $"{Name} ({Country}), время приготовления: {CookingTime} мин.";
    }

    public class Soup : Dish
    {
        public string MainIngredient { get; set; }

        public Soup(string name, string country, int cookingTime, string mainIngredient)
            : base(name, country, cookingTime)
        {
            MainIngredient = mainIngredient;
        }

        public override void Cook() =>
            Console.WriteLine($"Варим суп \"{Name}\" с основным ингредиентом: {MainIngredient}.");

        public void AddSpices() => Console.WriteLine($"Добавляем специи в суп \"{Name}\".");
    }

    public class Salad : Dish
    {
        public string Dressing { get; set; }

        public Salad(string name, string country, int cookingTime, string dressing)
            : base(name, country, cookingTime)
        {
            Dressing = dressing;
        }

        public override void Cook() =>
            Console.WriteLine($"Готовим салат \"{Name}\" и заправляем: {Dressing}.");

        public void Mix() => Console.WriteLine($"Перемешиваем салат \"{Name}\".");
    }

    public class Dessert : Dish
    {
        public bool IsCold { get; set; }

        public Dessert(string name, string country, int cookingTime, bool isCold)
            : base(name, country, cookingTime)
        {
            IsCold = isCold;
        }

        public override void Cook() =>
            Console.WriteLine($"Готовим десерт \"{Name}\" ({(IsCold ? "холодный" : "горячий")}).");

        public void Serve() => Console.WriteLine($"Подаём десерт \"{Name}\" к столу.");
    }

    public static void Run()
    {
        List<Dish> dishes = new List<Dish>
        {
            new Soup("Борщ", "Украина", 90, "свёкла"),
            new Salad("Цезарь", "Италия", 20, "соус цезарь"),
            new Dessert("Тирамису", "Италия", 40, true)
        };

        foreach (Dish dish in dishes)
        {
            Console.WriteLine(dish.GetInfo());
            dish.Cook();
        }

        Console.WriteLine();

        foreach (Dish dish in dishes)
        {
            if (dish is Soup soup) soup.AddSpices();
            else if (dish is Salad salad) salad.Mix();
            else if (dish is Dessert dessert) dessert.Serve();
        }
    }
}
