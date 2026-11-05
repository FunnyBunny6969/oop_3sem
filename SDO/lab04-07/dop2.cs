using System;
using System.Collections.Generic;

public static class Dop2
{
    public interface IFlyable
    {
        void Fly();
    }

    public interface ISwimmable
    {
        void Swim();
    }

    public interface IRunnable
    {
        void Run();
    }

    public class Duck : IFlyable, ISwimmable, IRunnable
    {
        public void Fly() => Console.WriteLine("Утка летит.");
        public void Swim() => Console.WriteLine("Утка плывёт.");
        public void Run() => Console.WriteLine("Утка бежит.");
    }

    public class Fish : ISwimmable
    {
        public void Swim() => Console.WriteLine("Рыба плывёт.");
    }

    public class Dog : IRunnable, ISwimmable
    {
        public void Run() => Console.WriteLine("Собака бежит.");
        public void Swim() => Console.WriteLine("Собака плывёт.");
    }

    private static void Demonstrate(object animal)
    {
        Console.WriteLine($"Объект: {animal.GetType().Name}");

        if (animal is IFlyable flyable) flyable.Fly();
        if (animal is ISwimmable swimmable) swimmable.Swim();
        if (animal is IRunnable runnable) runnable.Run();

        int count = 0;
        if (animal is IFlyable) count++;
        if (animal is ISwimmable) count++;
        if (animal is IRunnable) count++;
        Console.WriteLine($"Количество реализованных интерфейсов: {count}");
    }

    public static void Run()
    {
        List<object> animals = new List<object> { new Duck(), new Fish(), new Dog() };
        foreach (object animal in animals)
        {
            Demonstrate(animal);
            Console.WriteLine();
        }
    }
}
