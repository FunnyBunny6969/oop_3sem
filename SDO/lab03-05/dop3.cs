using System;

public static class Dop3
{
    public class Vehicle
    {
        public string Brand { get; }
        public double Speed { get; }

        public Vehicle(string brand, double speed)
        {
            if (speed <= 0) throw new ArgumentException("Скорость должна быть положительной.");
            Brand = brand;
            Speed = speed;
        }

        public virtual double GetTravelTime(double distance, int hour)
        {
            if (distance <= 0) throw new ArgumentException("Расстояние должно быть положительным.");
            return distance / Speed;
        }

        public virtual string GetInfo() => $"{GetType().Name} {Brand}, скорость {Speed:F1} км/ч";
    }

    public class Car : Vehicle
    {
        public Car(string brand, double speed) : base(brand, speed) { }

        public override double GetTravelTime(double distance, int hour)
        {
            double speed = Speed;
            bool rushHour = (hour >= 8 && hour < 10) || (hour >= 18 && hour < 20);
            if (rushHour) speed *= 0.7;
            return distance / speed;
        }
    }

    public class Bicycle : Vehicle
    {
        public Bicycle(string brand, double speed) : base(brand, speed) { }

        public override double GetTravelTime(double distance, int hour)
        {
            if (distance <= 0) throw new ArgumentException("Расстояние должно быть положительным.");

            double speed = Speed;
            double remaining = distance;
            double time = 0;
            while (remaining > 0)
            {
                double segment = Math.Min(10, remaining);
                time += segment / speed;
                remaining -= segment;
                speed *= 0.9;
            }
            return time;
        }
    }

    public class PublicTransport : Vehicle
    {
        public PublicTransport(string brand, double speed) : base(brand, speed) { }

        public override double GetTravelTime(double distance, int hour)
        {
            if (hour < 6 || hour >= 23)
                throw new InvalidOperationException("Общественный транспорт не работает с 23 до 6 часов.");

            return distance / Speed + 10.0 / 60.0;
        }
    }

    public static void Run()
    {
        Vehicle[] vehicles =
        {
            new Car("Toyota", 80),
            new Bicycle("Trek", 20),
            new PublicTransport("Автобус", 40)
        };

        const double distance = 30;

        foreach (int hour in new[] { 9, 15, 4 })
        {
            Console.WriteLine($"Время в пути на {distance:F0} км в {hour}:00:");
            foreach (Vehicle vehicle in vehicles)
            {
                try
                {
                    double hours = vehicle.GetTravelTime(distance, hour);
                    Console.WriteLine($"  {vehicle.GetInfo()}: {hours:F2} ч ({hours * 60:F0} мин)");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"  {vehicle.GetInfo()}: {ex.Message}");
                }
            }
        }
    }
}
